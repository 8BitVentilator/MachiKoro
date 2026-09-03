using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0002: Zählt gewöhnliche Methoden eines Typs. Konstruktoren, Accessoren und Operatoren zählen nicht.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MethodCountAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.TooManyMethods];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        var type = (INamedTypeSymbol)context.Symbol;
        int count = TypeShape.CountMembers<IMethodSymbol>(type, IsOrdinaryMethod);
        if (count > Rules.MaximumMethods)
        {
            context.ReportDiagnostic(TypeShape.Exceeded(Rules.TooManyMethods, type, (count, Rules.MaximumMethods)));
        }
    }

    private static bool IsOrdinaryMethod(IMethodSymbol method) =>
        method.MethodKind == MethodKind.Ordinary && !method.IsImplicitlyDeclared;
}
