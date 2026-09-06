## 1. Einzelne Würfelergebnisse

- [x] 1.1 Grenzwerttests für `DieResult.Of(int)` mit 0, 1, 6 und 7 ergänzen und durch den gezielten Testlauf den erwarteten roten Zustand nachweisen
- [x] 1.2 `DieResult` als unveränderliches Wertobjekt für 1 bis 6 implementieren und mit `dotnet test MachiKoro.slnx --filter "FullyQualifiedName~DieResult"` den grünen Zustand prüfen

## 2. Würfelsummen

- [x] 2.1 Grenzwert- und Vergleichstests für `DiceTotal` aus der bisherigen Summensemantik von `DiceRollTests` ableiten und durch den gezielten Testlauf den erwarteten roten Zustand nachweisen
- [x] 2.2 `DiceTotal` als unveränderliches Wertobjekt für 1 bis 12 mit den benötigten Gleichheits- und Bereichsoperationen implementieren und mit `dotnet test MachiKoro.slnx --filter "FullyQualifiedName~DiceTotal"` prüfen

## 3. Vollständiger Würfelwurf

- [x] 3.1 Tests für Würfe mit einem und zwei Ergebnissen, unterschiedliche Zusammensetzungen gleicher Summe, Summenprüfung sowie positive und negative Pascherkennung ergänzen und durch den gezielten Testlauf den erwarteten roten Zustand nachweisen

## 4. Kartenaktivierung migrieren

- [x] 4.1 Aktivierungstests und bestehende Kartentests auf `DiceTotal` beziehungsweise vollständige `DiceRoll`-Werte umstellen; dabei einen Test für identische Aktivierung durch 5 + 1 und 3 + 3 ergänzen und den erwarteten roten Zustand nachweisen
- [x] 4.2 `DiceRoll` auf ein erforderliches und ein optionales `DieResult` mit den beiden `Of`-Fabriken, Summenverhalten und `IsDoubles()` umstellen sowie `ActivationNumbers` und alle Kartenkatalogwerte auf `DiceTotal` migrieren; anschließend mit `dotnet test MachiKoro.slnx --filter "FullyQualifiedName~DiceRoll"` prüfen
- [x] 4.3 Die migrierte Kartenaktivierung mit `dotnet test MachiKoro.slnx --filter "FullyQualifiedName~Cards"` vollständig prüfen

## 5. Qualitätsprüfung

- [x] 5.1 Im gesamten Repository prüfen, dass kein Aufrufer mehr `DiceRoll.Of(int)` verwendet, und alle neuen öffentlichen APIs mit XML-Dokumentation versehen
- [x] 5.2 `dotnet build MachiKoro.slnx` und `dotnet test MachiKoro.slnx` ausführen und sicherstellen, dass Build, Analyzer und vollständige Testsuite ohne Befund bleiben

## 6. Gültige Standardzustände

- [x] 6.1 Tests ergänzen, die fehlende `DieResult`-Werte in beiden `DiceRoll`-Fabriken und fehlende `DiceTotal`-Werte in beiden `ActivationNumbers`-Fabriken zurückweisen, und durch gezielte Testläufe den erwarteten roten Zustand nachweisen
- [x] 6.2 `DieResult`, `DiceTotal`, `DiceRoll` und `ActivationNumbers` auf `sealed record` umstellen, fehlende Referenzwerte an den Fabrikgrenzen abweisen und die gezielten Würfel- und Aktivierungstests grün ausführen
- [x] 6.3 Mit `dotnet build MachiKoro.slnx` und `dotnet test MachiKoro.slnx` sicherstellen, dass Analyzer und vollständige Testsuite ohne Befund bleiben
