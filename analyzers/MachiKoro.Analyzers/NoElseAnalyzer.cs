using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0003: Meldet jede else-Klausel, auch else-if.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NoElseAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.ElseNotAllowed];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ElseClause);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        var elseClause = (ElseClauseSyntax)context.Node;
        context.ReportDiagnostic(Diagnostic.Create(Rules.ElseNotAllowed, elseClause.ElseKeyword.GetLocation()));
    }
}
