## 1. Geprüftes Empfangen von Einkommen

- [x] 1.1 Grenzwerttests für die größte darstellbare Summe und eine `int.MaxValue` überschreitende Summe in `CoinsTests` ergänzen und mit `dotnet test MachiKoro.slnx --filter "FullyQualifiedName~Coins"` den erwarteten roten Zustand des Überlauftests nachweisen
- [x] 1.2 Die Addition in `Coins.Receive` auf geprüfte Arithmetik umstellen und mit `dotnet test MachiKoro.slnx --filter "FullyQualifiedName~Coins"` verifizieren, dass darstellbare Summen exakt bleiben und eine nicht darstellbare Summe eine `OverflowException` auslöst

## 2. Qualitätsprüfung

- [x] 2.1 `dotnet build MachiKoro.slnx` und `dotnet test MachiKoro.slnx` ausführen und sicherstellen, dass Build, Analyzer und vollständige Testsuite ohne Befund bleiben
