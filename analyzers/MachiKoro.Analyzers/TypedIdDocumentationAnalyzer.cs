using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MachiKoro.Analyzers;

/// <summary>
/// MK0009: Meldet undokumentierte extern sichtbare Member in StronglyTypedId-Templates.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class TypedIdDocumentationAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = [Rules.MissingTypedIdDocumentation];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterAdditionalFileAction(Analyze);
    }

    private static void Analyze(AdditionalFileAnalysisContext context)
    {
        AdditionalText file = context.AdditionalFile;
        if (!file.Path.EndsWith(".typedid", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        SourceText? text = file.GetText(context.CancellationToken);
        if (text is null)
        {
            return;
        }

        AnalyzeDeclarations(context, text);
    }

    private static void AnalyzeDeclarations(AdditionalFileAnalysisContext context, SourceText text)
    {
        string path = context.AdditionalFile.Path;
        SyntaxNode root = CSharpSyntaxTree.ParseText(text, path: path).GetRoot(context.CancellationToken);
        foreach (MemberDeclarationSyntax declaration in root.DescendantNodes().OfType<MemberDeclarationSyntax>())
        {
            ReportMissingDocumentation(context, text, declaration);
        }
    }

    private static void ReportMissingDocumentation(
        AdditionalFileAnalysisContext context,
        SourceText text,
        MemberDeclarationSyntax declaration)
    {
        SyntaxToken accessibility = AccessibilityModifier(declaration);
        if (accessibility == default || HasDocumentation(declaration))
        {
            return;
        }

        TextSpan span = accessibility.Span;
        Location location = Location.Create(context.AdditionalFile.Path, span, text.Lines.GetLinePositionSpan(span));
        context.ReportDiagnostic(Diagnostic.Create(Rules.MissingTypedIdDocumentation, location));
    }

    private static SyntaxToken AccessibilityModifier(MemberDeclarationSyntax declaration)
    {
        SyntaxToken[] tokens = [.. declaration.ChildTokens()];
        if (tokens.Any(token => token.IsKind(SyntaxKind.PrivateKeyword))
            && tokens.Any(token => token.IsKind(SyntaxKind.ProtectedKeyword)))
        {
            return default;
        }

        return tokens.FirstOrDefault(IsExternallyVisible);
    }

    private static bool IsExternallyVisible(SyntaxToken token) =>
        token.IsKind(SyntaxKind.PublicKeyword) || token.IsKind(SyntaxKind.ProtectedKeyword);

    private static bool HasDocumentation(MemberDeclarationSyntax declaration) =>
        declaration.GetLeadingTrivia().Any(IsDocumentation);

    private static bool IsDocumentation(SyntaxTrivia trivia) =>
        trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
        || trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia);
}
