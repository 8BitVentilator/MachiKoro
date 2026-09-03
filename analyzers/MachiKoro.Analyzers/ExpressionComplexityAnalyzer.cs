using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0008: Ein Ausdruck enthält höchstens zwei Bedingungsoperatoren (&amp;&amp;, ||, ?:).
/// Gezählt wird am äußersten Operator einer zusammenhängenden Kette.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ExpressionComplexityAnalyzer : DiagnosticAnalyzer
{
    private static readonly ImmutableArray<SyntaxKind> ConditionalKinds =
    [
        SyntaxKind.LogicalAndExpression, SyntaxKind.LogicalOrExpression, SyntaxKind.ConditionalExpression,
    ];

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.ExpressionTooComplex];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(Analyze, ConditionalKinds);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        if (IsConditional(SkipParentheses(context.Node.Parent)))
        {
            return;   // Nur die äußerste Kette wird gezählt und gemeldet.
        }

        int count = context.Node.DescendantNodesAndSelf().Count(IsConditional);
        if (count > Rules.MaximumConditionalOperators)
        {
            string actual = count.ToString(CultureInfo.InvariantCulture);
            string allowed = Rules.MaximumConditionalOperators.ToString(CultureInfo.InvariantCulture);
            context.ReportDiagnostic(Diagnostic.Create(Rules.ExpressionTooComplex, context.Node.GetLocation(), actual, allowed));
        }
    }

    private static SyntaxNode? SkipParentheses(SyntaxNode? node) =>
        node is ParenthesizedExpressionSyntax parenthesized ? SkipParentheses(parenthesized.Parent) : node;

    private static bool IsConditional(SyntaxNode? node) => node is not null && ConditionalKinds.Contains(node.Kind());
}
