using System;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MachiKoro.Analyzers;

/// <summary>
/// Gemeinsame Fragen an Typsymbole und Meldungsaufbau für Zählregeln.
/// </summary>
internal static class TypeShape
{
    public static int CountMembers<TMember>(INamedTypeSymbol type, Func<TMember, bool> predicate)
        where TMember : ISymbol =>
        IsClassOrStruct(type) ? type.GetMembers().OfType<TMember>().Count(predicate) : 0;

    public static Diagnostic Exceeded(
        DiagnosticDescriptor rule,
        INamedTypeSymbol type,
        (int Actual, int Allowed) limit) =>
        Diagnostic.Create(rule, LocationOf(type), type.Name, Invariant(limit.Actual), Invariant(limit.Allowed));

    private static bool IsClassOrStruct(INamedTypeSymbol type) =>
        type.TypeKind is TypeKind.Class or TypeKind.Struct && !type.IsImplicitlyDeclared;

    private static Location LocationOf(ISymbol symbol) => symbol.Locations.FirstOrDefault() ?? Location.None;

    private static string Invariant(int value) => value.ToString(CultureInfo.InvariantCulture);
}
