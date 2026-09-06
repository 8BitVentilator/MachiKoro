## 1. Dokumentationsdiagnosen

- [x] 1.1 `MK0009` als Analyzer für undokumentierte extern sichtbare Member in `.typedid`-Additional-Files
  implementieren, Descriptor, Schweregrad und Release-Tracking registrieren und mit
  `dotnet build src/MachiKoro.Domain/MachiKoro.Domain.csproj` verifizieren, dass der Build am bestehenden
  `guid-domain.typedid` mit `MK0009` fehlschlägt.
- [x] 1.2 Die öffentlichen Member in `guid-domain.typedid` vollständig auf Englisch dokumentieren und mit
  `dotnet build src/MachiKoro.Domain/MachiKoro.Domain.csproj` verifizieren, dass alle `MK0009`-Diagnosen behoben
  sind.
- [x] 1.3 Die globale `CS1591`-Unterdrückung entfernen und die Diagnostic in `.editorconfig` ausschließlich für
  C#-Produktionscode unter `src/` als Fehler aktivieren; mit `dotnet build MachiKoro.slnx` verifizieren, dass der
  Build wegen undokumentierter Domain-API fehlschlägt und keine `CS1591`-Diagnosen aus `tests/` oder `analyzers/`
  enthält.

## 2. Bestehende Produktions-API

- [x] 2.1 `Coins`, die Würfel-Wertobjekte und `ActivationNumbers` vollständig und konsistent auf Englisch
  dokumentieren, einschließlich der Übersetzung bestehender deutscher Kommentare; mit einem Domain-Build
  verifizieren, dass diese Dateien keine Dokumentationsdiagnosen mehr erzeugen.
- [x] 2.2 Die ID-Deklarationen, Enums, Karten-Collections sowie die abstrakten Verträge `Establishment` und
  `Landmark` dokumentieren; mit einem Domain-Build verifizieren, dass diese Dateien keine
  Dokumentationsdiagnosen mehr erzeugen.
- [x] 2.3 Alle konkreten Unternehmen und Großprojekte dokumentieren, für geerbte Member `<inheritdoc/>`
  verwenden und mit einem Domain-Build verifizieren, dass die Kartenimplementierungen keine
  Dokumentationsdiagnosen mehr erzeugen.
- [x] 2.4 Alle Income-Interfaces, Effekte und Claim-Records dokumentieren, fachliche Positionsparameter über
  `<param>` beschreiben und mit `dotnet build src/MachiKoro.Domain/MachiKoro.Domain.csproj` verifizieren, dass
  das vollständige Produktionsprojekt ohne Dokumentationsdiagnosen baut.

## 3. Verbindliche Richtlinien

- [x] 3.1 `docs/coding-guidelines.md` auf die vollständige extern sichtbare Produktions-API, englische
  Dokumentationssprache, `<inheritdoc/>`, Generator-Templates und compiler-synthetisierte Ausnahmen aktualisieren
  und durch Abgleich mit `specs/api-documentation/spec.md` verifizieren.
- [x] 3.2 `docs/building.md`, `AGENTS.md`, Analyzer-Beschreibungen und Konfigurationskommentare auf `MK0009`, den
  produktionsbezogenen `CS1591`-Scope und die begründete Ausnahme für Tests und Analyzer aktualisieren und
  verifizieren, dass keine Beschreibung mehr ausschließlich von `MK0001` bis `MK0008` als vollständigem
  Regelbestand spricht.

## 4. Quality Gate

- [x] 4.1 `dotnet format MachiKoro.slnx --verify-no-changes` ausführen und alle Formatierungs- oder
  Analyzerbefunde beheben, bis die Prüfung erfolgreich ist.
- [x] 4.2 `dotnet build MachiKoro.slnx` ausführen und verifizieren, dass alle Projekte ohne Warnungen oder Fehler
  bauen.
- [x] 4.3 `dotnet test MachiKoro.slnx` ausführen und verifizieren, dass die vollständige Testsuite erfolgreich
  bleibt.
