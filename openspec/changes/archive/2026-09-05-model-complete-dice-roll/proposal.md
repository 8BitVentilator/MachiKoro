## Why

`DiceRoll` speichert derzeit nur eine Summe und kann deshalb Würfe wie 5 + 1 und 3 + 3 nicht unterscheiden. Das Modell muss die einzelnen Würfelergebnisse erhalten, bevor darauf aufbauende Regeln wie Freizeitpark und Funkturm implementiert werden.

## What Changes

- `DiceRoll` repräsentiert das vollständige Ergebnis eines Wurfs mit einem oder zwei Würfeln.
- Ein eigener Werttyp repräsentiert das Ergebnis eines einzelnen sechsseitigen Würfels.
- Ein eigener Werttyp repräsentiert die Summe eines vollständigen Wurfs und übernimmt die bisherige Verwendung von `DiceRoll` in `ActivationNumbers`.
- `DiceRoll` kann fachlich beantworten, ob der Wurf ein Pasch war, und seine Summe für die Kartenaktivierung bereitstellen.
- **BREAKING**: Die Fabrik und Semantik von `DiceRoll` ändern sich; Aufrufer erzeugen keine Würfe mehr direkt aus einer Summe.
- Freizeitpark-, Funkturm-, `Turn`- und `Game`-Verhalten werden nicht implementiert.

## Capabilities

### New Capabilities

- `dice-roll`: Vollständige, gültige Würfelergebnisse sowie daraus abgeleitete Summe und Pascherkennung.

### Modified Capabilities

Keine.

## Impact

Betroffen sind die Würfel-Werttypen in `src/MachiKoro.Domain/Dice`, die Aktivierungswerte und Unternehmen unter `src/MachiKoro.Domain/Cards` sowie deren Tests in `tests/MachiKoro.Domain.Tests`. Externe Abhängigkeiten, persistierte Ereignisse und andere Projekte sind nicht betroffen.
