using Microsoft.CodeAnalysis;

namespace MachiKoro.Analyzers;

/// <summary>
/// Diagnosebeschreibungen und Grenzwerte der projekteigenen Regeln.
/// </summary>
internal static class Rules
{
    public const int MaximumInstanceState = 2;
    public const int MaximumMethods = 10;
    public const int MaximumNestingLevel = 1;
    public const int MaximumParameters = 3;
    public const int MaximumFileLines = 120;
    public const int MaximumLineLength = 120;
    public const int MaximumConditionalOperators = 2;

    private const string Category = "ObjectCalisthenics";

    public static readonly DiagnosticDescriptor TooMuchInstanceState = new(
        "MK0001",
        "Höchstens zwei Instanzvariablen pro Typ",
        "Typ '{0}' hat {1} Instanzvariablen (Felder und Auto-Properties), erlaubt sind {2}. "
            + "Fasse zusammengehörige Werte in einem eigenen Typ zusammen.",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor TooManyMethods = new(
        "MK0002",
        "Höchstens zehn Methoden pro Typ",
        "Typ '{0}' hat {1} Methoden, erlaubt sind {2}. Teile die Verantwortlichkeiten auf.",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ElseNotAllowed = new(
        "MK0003",
        "Kein else",
        "Ersetze 'else' durch Early Return, Guard Clause, Polymorphie oder einen switch-Ausdruck",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor TooDeeplyNested = new(
        "MK0004",
        "Eine Einrückungsebene pro Methode",
        "Kontrollfluss ist {0} Ebenen tief verschachtelt, erlaubt ist {1}. Extrahiere den inneren Block in eine Methode.",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor TooManyParameters = new(
        "MK0005",
        "Höchstens drei Parameter",
        "'{0}' hat {1} Parameter, erlaubt sind {2}. Fasse zusammengehörige Werte in einem eigenen Typ zusammen.",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor FileTooLong = new(
        "MK0006",
        "Kurze Dateien",
        "Datei hat {0} nicht-leere Zeilen, erlaubt sind {1}. Teile den Typ auf.",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor LineTooLong = new(
        "MK0007",
        "Zeilenlänge",
        "Zeile hat {0} Zeichen, erlaubt sind {1}",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ExpressionTooComplex = new(
        "MK0008",
        "Einfache Ausdrücke",
        "Ausdruck hat {0} Bedingungsoperatoren (&&, ||, ?:), erlaubt sind {1}. Benenne Teilbedingungen als Methode.",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor MissingTypedIdDocumentation = new(
        "MK0009",
        "Dokumentation für StronglyTypedId-Templates",
        "Extern sichtbares Mitglied benötigt einen XML-Dokumentationskommentar",
        "Documentation", DiagnosticSeverity.Warning, isEnabledByDefault: true);
}
