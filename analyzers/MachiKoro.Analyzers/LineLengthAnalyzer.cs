using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0007: Eine Zeile hat höchstens 120 Zeichen.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class LineLengthAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.LineTooLong];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxTreeAction(Analyze);
    }

    private static void Analyze(SyntaxTreeAnalysisContext context)
    {
        TextLineCollection lines = context.Tree.GetText(context.CancellationToken).Lines;
        foreach (TextLine line in lines.Where(IsTooLong))
        {
            Location location = Location.Create(context.Tree, line.Span);
            string actual = line.Span.Length.ToString(CultureInfo.InvariantCulture);
            string allowed = Rules.MaximumLineLength.ToString(CultureInfo.InvariantCulture);
            context.ReportDiagnostic(Diagnostic.Create(Rules.LineTooLong, location, actual, allowed));
        }
    }

    private static bool IsTooLong(TextLine line) => line.Span.Length > Rules.MaximumLineLength;
}
