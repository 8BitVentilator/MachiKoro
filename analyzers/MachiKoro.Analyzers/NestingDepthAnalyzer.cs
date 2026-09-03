using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0004: Kontrollfluss (if, for, foreach, while, do, switch, try) darf innerhalb eines Methodenrumpfs
/// höchstens einmal verschachtelt sein. Gemeldet wird die erste Anweisung, die zu tief liegt.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NestingDepthAnalyzer : DiagnosticAnalyzer
{
    private static readonly ImmutableArray<SyntaxKind> ControlFlowKinds =
    [
        SyntaxKind.IfStatement, SyntaxKind.ForStatement, SyntaxKind.ForEachStatement, SyntaxKind.WhileStatement,
        SyntaxKind.DoStatement, SyntaxKind.SwitchStatement, SyntaxKind.TryStatement,
    ];

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.TooDeeplyNested];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(Analyze, ControlFlowKinds);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        int depth = context.Node.Ancestors().TakeWhile(IsInsideSameBody).Count(IsControlFlow);
        if (depth == Rules.MaximumNestingLevel)
        {
            Location location = context.Node.GetFirstToken().GetLocation();
            string actual = (depth + 1).ToString(CultureInfo.InvariantCulture);
            string allowed = Rules.MaximumNestingLevel.ToString(CultureInfo.InvariantCulture);
            context.ReportDiagnostic(Diagnostic.Create(Rules.TooDeeplyNested, location, actual, allowed));
        }
    }

    private static bool IsInsideSameBody(SyntaxNode node) =>
        node is not (MemberDeclarationSyntax or AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax);

    private static bool IsControlFlow(SyntaxNode node) => ControlFlowKinds.Contains(node.Kind());
}
