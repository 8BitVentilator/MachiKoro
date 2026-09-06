# MachiKoro

Digitale Umsetzung des Brettspiels **Machi Koro** in C# 14 auf .NET 10. Solution: `MachiKoro.slnx`.

## Struktur

| Pfad | Inhalt |
|---|---|
| `analyzers/MachiKoro.Analyzers` | Eigene Roslyn-Regeln MK0001 bis MK0009 für Object Calisthenics und `.typedid`-Dokumentation. Wird in alle `src/`-Projekte eingebunden. |
| `src/MachiKoro.Domain` | Spielregeln und Domänenmodell. Keine externen Abhängigkeiten. |
| `tests/MachiKoro.Domain.Tests` | xUnit v3 + Shouldly. Ein Testprojekt pro Quellprojekt, gleiche Ordnerstruktur. |
| `docs/` | Coding-, Test- und Build-Richtlinien (verbindlich) sowie die Spielregel-Referenz. |
| `docs/assets/spielregeln/` | Originalanleitung (PDF) und Kartenfotos. Quelle für `docs/spielregeln.md`, nicht direkt lesen. |

## Verbindliche Richtlinien

- [docs/coding-guidelines.md](docs/coding-guidelines.md): SOLID und Object Calisthenics, mit Zuordnung zu den Analyzer-Regeln.
- [docs/testing-guidelines.md](docs/testing-guidelines.md): TDD, Testaufbau, Benennung.
- [docs/building.md](docs/building.md): Build, Tests, Quality Gate und Hooks.
- [docs/architecture.md](docs/architecture.md): Zielarchitektur (Projekte, Abhängigkeiten, Ereignismodell, Slices, Protokoll, Architekturregeln). Neue Projekte und Typen müssen sich dort einordnen.

## Harte Regeln

1. **Test zuerst.** Kein Produktionscode ohne vorher fehlschlagenden Test.
2. **Analyzer sind Gesetz.** Warnungen sind Build-Fehler. Keine Regel unterdrücken (`#pragma`, `SuppressMessage`, `.editorconfig`), stattdessen den Code umbauen. Ausnahme nur nach ausdrücklicher Freigabe durch den Menschen, mit Begründung als Kommentar.
3. **Object Calisthenics gelten für jeden Typ in `src/`.** Details in den Coding-Guidelines. Insbesondere: keine `else`, keine Getter/Setter, Primitive und Collections in eigene Typen wrappen, höchstens zwei Instanzvariablen.
4. **Domänensprache.** Bezeichner heißen wie im Spiel: `Coins`, `Establishment`, `Landmark`, `DiceRoll`, `Player`, `Turn`. Keine Abkürzungen.
5. **IDs sind stark typisiert.** Für ID-Typen wird das NuGet-Paket `StronglyTypedId` verwendet; keine nackten `Guid`-Werte oder handgeschriebenen ID-Wrapper.
6. **Produktions-API ist englisch dokumentiert.** `CS1591` prüft C# unter `src/`, MK0009 die `.typedid`-Templates. Tests und Analyzer sind nur von dieser Dokumentationspflicht ausgenommen.
7. **Vor dem Abschluss** einer Aufgabe müssen `dotnet build` und `dotnet test` grün sein. Der Stop-Hook prüft das und blockiert sonst.

## Arbeitsablauf pro Aufgabe

1. Test schreiben, der das gewünschte Verhalten beschreibt und fehlschlägt.
2. Kleinste Implementierung, die den Test grün macht.
3. Refactoring gegen die Coding-Guidelines, bis Build und Analyzer ohne Befund sind.
4. Erst dann die nächste Aufgabe.

## Spielregeln

**[docs/spielregeln.md](docs/spielregeln.md) ist die fachliche Wahrheit.** Dort stehen alle 15 Unternehmen und 4 Großprojekte mit Zahlen, Kosten und Effekten, die Einkommensreihenfolge, das Glossar Regelbegriff → Code-Bezeichner und die bewusst getroffenen Auslegungen offener Punkte. Bevor eine Spielregel implementiert wird, ist die entsprechende Stelle dort zu lesen. Das Original ist `docs/assets/spielregeln/rules-German.pdf`; bei Widersprüchen gilt das PDF und die Markdown-Datei wird korrigiert.

Kurzfassung: 2 bis 4 Spieler, Start mit 3 Münzen, Weizenfeld und Bäckerei. Pro Zug: würfeln (1 Würfel, mit Bahnhof wahlweise 2), Einkommen abwickeln (erst Zahlungen an Mitspieler für Rot, gegen den Uhrzeigersinn; dann Blau für alle, Grün und Violett für den aktiven Spieler), optional 1 Unternehmen oder 1 Großprojekt bauen. Sieg: alle vier Großprojekte gebaut.
