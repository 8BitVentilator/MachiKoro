## Purpose

Diese Capability bewahrt die einzelnen Ergebnisse eines Würfelwurfs und stellt daraus die für Spielregeln benötigte Summe und Pascherkennung bereit.

## ADDED Requirements

### Requirement: Gültiges einzelnes Würfelergebnis
Das System MUST ein einzelnes Würfelergebnis auf die Werte 1 bis einschließlich 6 begrenzen.

#### Scenario: Kleinster gültiger Wert
- **WHEN** ein einzelnes Würfelergebnis mit dem Wert 1 erzeugt wird
- **THEN** akzeptiert das System das Ergebnis

#### Scenario: Größter gültiger Wert
- **WHEN** ein einzelnes Würfelergebnis mit dem Wert 6 erzeugt wird
- **THEN** akzeptiert das System das Ergebnis

#### Scenario: Wert außerhalb des Würfels
- **WHEN** ein einzelnes Würfelergebnis mit einem Wert kleiner als 1 oder größer als 6 erzeugt wird
- **THEN** weist das System den Wert zurück

### Requirement: Vollständiger Würfelwurf
Das System MUST einen Würfelwurf aus genau einem oder genau zwei gültigen einzelnen Würfelergebnissen bilden und deren Zusammensetzung erhalten.

#### Scenario: Wurf mit einem Würfel
- **WHEN** ein Würfelwurf aus einem einzelnen Würfelergebnis gebildet wird
- **THEN** repräsentiert er einen Wurf mit genau einem Würfel

#### Scenario: Wurf mit zwei Würfeln
- **WHEN** ein Würfelwurf aus zwei einzelnen Würfelergebnissen gebildet wird
- **THEN** repräsentiert er einen Wurf mit genau zwei Würfeln und erhält beide Ergebnisse

#### Scenario: Fehlendes Würfelergebnis
- **WHEN** ein Würfelwurf mit einem fehlenden einzelnen Würfelergebnis gebildet werden soll
- **THEN** weist das System den unvollständigen Wurf zurück

#### Scenario: Unterschiedliche Zusammensetzung bei gleicher Summe
- **WHEN** die Würfe 5 + 1 und 3 + 3 gebildet werden
- **THEN** bleiben sie trotz gleicher Summe als unterschiedlich zusammengesetzte Würfe unterscheidbar

### Requirement: Würfelsumme
Das System MUST aus einem vollständigen Würfelwurf dessen Summe ableiten und eine Würfelsumme auf die Werte 1 bis einschließlich 12 begrenzen.

#### Scenario: Summe eines einzelnen Würfels
- **WHEN** ein Würfelwurf aus dem einzelnen Ergebnis 5 besteht
- **THEN** beträgt seine Würfelsumme 5

#### Scenario: Summe zweier Würfel
- **WHEN** ein Würfelwurf aus den Ergebnissen 5 und 1 besteht
- **THEN** beträgt seine Würfelsumme 6

#### Scenario: Ungültige Würfelsumme
- **WHEN** eine Würfelsumme mit einem Wert kleiner als 1 oder größer als 12 erzeugt wird
- **THEN** weist das System den Wert zurück

### Requirement: Pascherkennung
Das System MUST einen Würfelwurf genau dann als Pasch erkennen, wenn er aus zwei gleichen einzelnen Würfelergebnissen besteht.

#### Scenario: Zwei gleiche Würfel
- **WHEN** ein Würfelwurf aus den Ergebnissen 3 und 3 besteht
- **THEN** erkennt das System einen Pasch

#### Scenario: Zwei unterschiedliche Würfel
- **WHEN** ein Würfelwurf aus den Ergebnissen 5 und 1 besteht
- **THEN** erkennt das System keinen Pasch

#### Scenario: Einzelner Würfel
- **WHEN** ein Würfelwurf aus dem einzelnen Ergebnis 3 besteht
- **THEN** erkennt das System keinen Pasch

### Requirement: Kartenaktivierung durch die Würfelsumme
Das System MUST Aktivierungszahlen aus vorhandenen gültigen Würfelsummen bilden und Unternehmen weiterhin ausschließlich anhand der Summe des vollständigen Würfelwurfs aktivieren.

#### Scenario: Fehlende Würfelsumme
- **WHEN** Aktivierungszahlen mit einer fehlenden Würfelsumme gebildet werden sollen
- **THEN** weist das System die unvollständigen Aktivierungszahlen zurück

#### Scenario: Gleiche Summe aktiviert dieselben Unternehmen
- **WHEN** die Würfe 5 + 1 und 3 + 3 jeweils gegen dieselben Aktivierungszahlen ausgewertet werden
- **THEN** aktivieren beide Würfe dieselben Unternehmen
