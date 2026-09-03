# Testing-Guidelines

## Stack

- **xUnit v3** (`xunit.v3`), Testprojekte sind ausführbar (`OutputType=Exe`).
- **Shouldly** für Assertions: `result.ShouldBe(expected)`, `Should.Throw<T>(() => ...)`.
- Keine Mocking-Bibliothek, solange handgeschriebene Fakes reichen (`FixedDice`, `SequenceDice`). Die Domäne ist klein genug.

## TDD ist Pflicht

Jede Änderung an `src/` beginnt mit einem fehlschlagenden Test.

1. **Rot:** Test schreiben, ausführen, Fehlschlag sehen. Der Fehlschlag muss aus dem richtigen Grund kommen (fehlendes Verhalten, nicht Compile-Fehler an falscher Stelle).
2. **Grün:** Kleinste Implementierung, die den Test bestehen lässt.
3. **Refactor:** Gegen die [Coding-Guidelines](coding-guidelines.md) umbauen, Tests bleiben grün.

## Aufbau

- Ein Testprojekt pro Quellprojekt: `tests/<Projekt>.Tests`. Namespace und Ordner spiegeln das Quellprojekt (`Economy/Coins.cs` → `Economy/CoinsTests.cs`).
- Eine Testklasse pro Typ: `CoinsTests`. Wächst sie über etwa 15 Tests, wird sie nach Verhalten geteilt (`CoinsPaymentTests`).
- Testmethoden heißen `Methode_Szenario_Erwartung`: `Pay_InsufficientCoins_LeavesZero`. Unterstriche sind in Tests erlaubt.
- Struktur Arrange / Act / Assert ohne Kommentare, getrennt durch Leerzeilen. Bei Einzeilern entfällt die Trennung.
- `[Theory]` mit `[InlineData]` für Grenzwerte und Wertetabellen. `[MemberData]` nur, wenn Daten Objekte sind.
- Jeder Test hat genau eine fachliche Aussage und mindestens eine Assertion. Das prüft kein Analyzer, sondern das Review.

## Was getestet wird

- **Spielregeln** vollständig: jedes Gebäude, jede Aktivierung, jede Kaufregel, Siegbedingung.
- **Grenzwerte:** 0 Münzen, letztes Gebäude im Vorrat, Würfelsumme 12, ein Spieler kann nicht zahlen.
- **Negativfälle:** ungültiger Zug, Kauf ohne Deckung, Würfeln außer der Reihe.
- **Zufall** wird über `IDice` ersetzt und nie echt gewürfelt.

## Was nicht getestet wird

- Konstruktoren ohne Logik, triviale `ToString`-Ausgaben (außer sie sind Teil der Fachlichkeit).
- Analyzer-Verhalten oder Formatierung. Das prüft der Build.

## Ausführen

```bash
dotnet test MachiKoro.slnx
dotnet test MachiKoro.slnx --filter "FullyQualifiedName~Coins"
```

Der Stop-Hook führt alle Tests automatisch aus, sobald C#-Dateien geändert wurden. Details in [building.md](building.md).
