using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0001: Zählt Instanzfelder und Auto-Properties (über ihre Backing-Fields) eines Typs.
/// Berechnete Properties und statische Member zählen nicht.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class InstanceStateAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.TooMuchInstanceState];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        var type = (INamedTypeSymbol)context.Symbol;
        int count = TypeShape.CountMembers<IFieldSymbol>(type, IsInstanceState);
        if (count > Rules.MaximumInstanceState)
        {
            (int Actual, int Allowed) limit = (count, Rules.MaximumInstanceState);
            context.ReportDiagnostic(TypeShape.Exceeded(Rules.TooMuchInstanceState, type, limit));
        }
    }

    private static bool IsInstanceState(IFieldSymbol field) =>
        !field.IsStatic && !field.IsConst && IsDeclaredOrBackingField(field);

    private static bool IsDeclaredOrBackingField(IFieldSymbol field) =>
        !field.IsImplicitlyDeclared || field.AssociatedSymbol is IPropertySymbol;
}
