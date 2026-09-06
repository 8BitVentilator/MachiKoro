## Why

`Coins.Receive` addiert Einkommen derzeit ungeprüft. Überschreitet die Summe `int.MaxValue`, läuft der gespeicherte Betrag in den negativen Bereich über und verletzt die Zusage, dass `Coins` nie negativ werden kann.

## What Changes

- Münzbestände nehmen weiterhin jeden darstellbaren nichtnegativen Betrag exakt entgegen.
- `Coins.Receive` weist eine nicht darstellbare Summe mit einer `OverflowException` zurück, statt einen ungültigen negativen Münzbestand zu erzeugen.
- Die bestehende Repräsentation durch `int` und das Verhalten von `Of`, `Pay`, `CanAfford` und `ToString` bleiben unverändert.

## Capabilities

### New Capabilities

- `coin-balance`: Gültigkeit und arithmetisches Verhalten nichtnegativer Münzbeträge beim Empfangen von Einkommen.

### Modified Capabilities

Keine.

## Impact

Betroffen sind `Coins` in `src/MachiKoro.Domain/Economy/Coins.cs` und die zugehörigen Tests in `tests/MachiKoro.Domain.Tests/Economy/CoinsTests.cs`. Öffentliche Signaturen, externe Abhängigkeiten, persistierte Daten und andere Projekte ändern sich nicht.
