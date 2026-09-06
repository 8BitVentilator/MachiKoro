## Context

`DiceRoll` speichert inzwischen ein oder zwei Würfelergebnisse, die beteiligten Wertobjekte sind jedoch `readonly record struct`. Jeder dieser Typen besitzt deshalb zusätzlich zu seinen validierenden Fabriken einen öffentlichen `default`-Wert. Dieser umgeht die Wertebereiche und kann als ungültiges Würfelergebnis oder als ungültige Würfelsumme in weitere Fabriken gelangen.

Das Modell muss weiterhin die Object-Calisthenics-Regeln erfüllen: primitive Domänenwerte werden gewrappt, Wertobjekte sind unveränderlich, ein Typ hat höchstens zwei Instanzvariablen und Objekte stellen bevorzugt Verhalten statt Rohwerte bereit.

## Goals / Non-Goals

**Goals:**

- Ungültige Einzelwerte und Summen an ihren jeweiligen Fabrikgrenzen abweisen.
- Keine Objektinstanz eines Würfel-Werttyps kann einen ungültigen `default`-Zustand darstellen.
- Einen Wurf ohne Collection und mit höchstens zwei Instanzvariablen vollständig abbilden.
- Kartenaktivierung ermöglichen, ohne die interne Zahl eines Wertobjekts öffentlich herauszugeben.
- Die bestehende wertbasierte Gleichheit der Würfeltypen beibehalten.

**Non-Goals:**

- Es wird keine Zufallsquelle oder `IDice` eingeführt.
- Es wird kein öffentlicher `DiceCount`-Typ vorweggenommen; die Anzahl ist durch die Zusammensetzung des Wurfs erhalten.
- Es werden keine Landmark-Hooks, Ereignisse oder Zugzustände implementiert.

## Decisions

### Würfelwerte als `sealed record`

`DieResult`, `DiceTotal`, `DiceRoll` und `ActivationNumbers` sind `sealed record` mit privaten Konstruktoren. Der Standardwert eines Referenztyps ist `null` und damit keine ungültige Objektinstanz. Die wertbasierte Record-Gleichheit bleibt erhalten, und alle Typen erfüllen weiterhin die Vorgabe von höchstens zwei Instanzvariablen.

`DieResult.Of(int)` und `DiceTotal.Of(int)` validieren weiterhin ihre numerischen Wertebereiche. Fabriken, die diese Referenz-Wertobjekte entgegennehmen, weisen `null` mit `ArgumentNullException` ab. Dadurch führen auch explizit unterdrückte Nullable-Warnungen nicht zu verzögerten Nullreferenzfehlern oder unvollständigen Würfelwerten.

Nullbasiert kodierte Structs wurden verworfen. Sie könnten `default(DieResult)` und `default(DiceTotal)` als fachlichen Wert 1 interpretieren und damit jede Struct-Instanz gültig machen. Ein versehentlich nicht initialisierter Wurf oder später ein fehlendes serialisiertes Feld würde dadurch jedoch stillschweigend als tatsächlich gewürfelte Eins behandelt. Reine Prüfungen in konsumierenden Fabriken wurden ebenfalls verworfen, weil die ungültigen Struct-Instanzen selbst weiterhin öffentlich konstruierbar blieben.

### `DiceRoll` speichert ein oder zwei Ergebnisse

`DiceRoll` ist ein `sealed record` mit einem erforderlichen ersten `DieResult` und einem optionalen zweiten `DieResult`. Überladene Fabriken `Of(DieResult)` und `Of(DieResult, DieResult)` bilden die einzigen gültigen Objektinstanzen und weisen fehlende Ergebnisse ab. Die Reihenfolge der gelieferten Ergebnisse wird nicht normalisiert; Spielregeln verwenden ausschließlich Summe und Pascherkennung.

Eine Collection wurde verworfen, weil das Spiel genau ein oder zwei Würfel erlaubt und eine Collection zusätzliche Validierung sowie besondere Behandlung für unveränderliche Wertgleichheit erfordern würde. Ein Summenwert mit Pasch-Flag wurde verworfen, weil er die beobachteten Einzelergebnisse und damit das vollständige Wurfergebnis verlieren würde.

### Bisherige Summensemantik wird `DiceTotal`

Der Wertebereich und die Vergleichsoperationen für Summen liegen in einem `sealed record DiceTotal`. `ActivationNumbers` ist ebenfalls ein `sealed record`, speichert zwei gültige `DiceTotal`-Werte und weist fehlende Grenzen in seinen Fabriken ab. Karten deklarieren ihre Aktivierungszahlen mit `DiceTotal.Of(int)`.

`DiceRoll` bietet Verhalten zum Prüfen seiner abgeleiteten Summe gegen einen einzelnen `DiceTotal` beziehungsweise einen Bereich. `ActivationNumbers.Includes(DiceRoll)` delegiert den Vergleich an dieses Verhalten. So bleibt die interne Zahl gekapselt und `Establishment.IsActivatedBy(DiceRoll)` kann seine fachlich passende Signatur behalten.

Eine öffentliche numerische Summen-Property wurde verworfen, weil aktuelle Aufrufer nicht den Rohwert, sondern nur Gleichheit oder Bereichszugehörigkeit benötigen.

### Pasch ist abgeleitetes Verhalten

`DiceRoll.IsDoubles()` liefert nur dann wahr, wenn ein zweites Ergebnis vorhanden und gleich dem ersten ist. Ein einzelner Würfel ist unabhängig von seinem Wert niemals ein Pasch. Es wird kein separates gespeichertes Flag verwendet, sodass widersprüchliche Zustände nicht erzeugt werden können.

## Risks / Trade-offs

- [Die geänderte `DiceRoll`-Fabrik bricht alle aktuellen Karten- und Testaufrufer.] -> Die Migration erfolgt geschlossen innerhalb des noch alleinstehenden Domain-Projekts und wird durch Compilerfehler vollständig sichtbar.
- [Ein optionales zweites Wertobjekt führt intern einen Abwesenheitszustand ein.] -> Nur die beiden öffentlichen Fabriken erzeugen Instanzen; Abwesenheit bedeutet ausschließlich einen Wurf mit einem Würfel.
- [Die Reihenfolge zweier Ergebnisse beeinflusst die Record-Gleichheit, obwohl aktuelle Spielregeln sie nicht verwenden.] -> Es findet keine Sortierung statt; fachliche Auswertungen vergleichen Summe oder Pasch und hängen nicht von der Reihenfolge ab.
- [Referenz-Wertobjekte verursachen Allokationen und können von unvorsichtigen Aufrufern als `null` übergeben werden.] -> Die Anzahl der Würfelobjekte pro Zug ist vernachlässigbar; Nullable Reference Types und explizite Fabrikprüfungen machen fehlende Werte früh sichtbar.
- [Spätere Ereignisserialisierung kann eine öffentlich serialisierbare Darstellung benötigen.] -> Ereignisse existieren noch nicht; ihre Vertragsform wird zusammen mit dem Ereignismodell entschieden, ohne die Würfelsemantik erneut zu ändern.

## Migration Plan

1. Fehlende Referenzwerte an den Fabrikgrenzen durch zunächst fehlschlagende Tests abdecken.
2. Die vier Würfel-Werttypen geschlossen von Structs auf `sealed record` umstellen und `null` an ihren Fabrikgrenzen abweisen.
3. Nach erfolgreichem Build und vollständigem Testlauf verbleiben weder Struct-Deklarationen der vier Typen noch ungeschützte Fabrikgrenzen.

Eine Datenmigration oder Rollback-Strategie ist nicht erforderlich, weil noch keine Würfelereignisse persistiert oder als externe Verträge veröffentlicht werden.
