# coin-balance Specification

## Purpose

Diese Capability stellt sicher, dass Münzbeträge nichtnegativ bleiben und Einkommen nur als exakt darstellbarer Münzbetrag entgegengenommen wird.

## Requirements

### Requirement: Gültiger Münzbetrag
Das System MUST Münzbeträge von 0 bis einschließlich `int.MaxValue` darstellen und negative Beträge zurückweisen.

#### Scenario: Kleinster gültiger Münzbetrag
- **WHEN** ein Münzbetrag von 0 erzeugt wird
- **THEN** akzeptiert das System den Betrag

#### Scenario: Größter gültiger Münzbetrag
- **WHEN** ein Münzbetrag von `int.MaxValue` erzeugt wird
- **THEN** akzeptiert das System den Betrag

#### Scenario: Negativer Münzbetrag
- **WHEN** ein Münzbetrag kleiner als 0 erzeugt wird
- **THEN** weist das System den Betrag zurück

### Requirement: Einkommen empfangen
Das System MUST einen Münzbestand und ein Einkommen exakt addieren, wenn die Summe höchstens `int.MaxValue` beträgt.

#### Scenario: Darstellbare Summe
- **WHEN** ein Münzbestand von 3 ein Einkommen von 2 empfängt
- **THEN** beträgt der resultierende Münzbestand 5

#### Scenario: Größte darstellbare Summe
- **WHEN** ein Münzbestand von `int.MaxValue - 1` ein Einkommen von 1 empfängt
- **THEN** beträgt der resultierende Münzbestand `int.MaxValue`

### Requirement: Überlauf beim Empfangen
Das System MUST das Empfangen von Einkommen mit einer `OverflowException` abbrechen, wenn die Summe `int.MaxValue` überschreitet.

#### Scenario: Summe überschreitet den darstellbaren Bereich
- **WHEN** ein Münzbestand von `int.MaxValue` ein Einkommen von 1 empfängt
- **THEN** löst das System eine `OverflowException` aus
