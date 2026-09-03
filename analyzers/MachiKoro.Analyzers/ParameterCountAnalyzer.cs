using System.Collections.Immutable;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0005: Methoden und Konstruktoren haben höchstens drei Parameter.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ParameterCountAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.TooManyParameters];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSymbolAction(Analyze, SymbolKind.Method);
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        var method = (IMethodSymbol)context.Symbol;
        if (!IsRelevant(method) || method.Parameters.Length <= Rules.MaximumParameters)
        {
            return;
        }

        string actual = method.Parameters.Length.ToString(CultureInfo.InvariantCulture);
        string allowed = Rules.MaximumParameters.ToString(CultureInfo.InvariantCulture);
        Location location = method.Locations.Length > 0 ? method.Locations[0] : Location.None;
        context.ReportDiagnostic(Diagnostic.Create(Rules.TooManyParameters, location, method.Name, actual, allowed));
    }

    private static bool IsRelevant(IMethodSymbol method) =>
        method.MethodKind is MethodKind.Ordinary or MethodKind.Constructor && !method.IsImplicitlyDeclared;
}
