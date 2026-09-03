using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0006: Eine Datei hat höchstens 120 nicht-leere Zeilen. Gemeldet wird am Dateianfang.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FileLengthAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.FileTooLong];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxTreeAction(Analyze);
    }

    private static void Analyze(SyntaxTreeAnalysisContext context)
    {
        int count = context.Tree.GetText(context.CancellationToken).Lines.Count(HasContent);
        if (count > Rules.MaximumFileLines)
        {
            Location location = Location.Create(context.Tree, new TextSpan(0, 0));
            string actual = count.ToString(CultureInfo.InvariantCulture);
            string allowed = Rules.MaximumFileLines.ToString(CultureInfo.InvariantCulture);
            context.ReportDiagnostic(Diagnostic.Create(Rules.FileTooLong, location, actual, allowed));
        }
    }

    private static bool HasContent(TextLine line) => !string.IsNullOrWhiteSpace(line.ToString());
}
