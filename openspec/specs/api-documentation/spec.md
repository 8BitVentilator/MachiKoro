# API Documentation Specification

## Purpose

Diese Capability stellt sicher, dass die gesamte extern sichtbare Produktions-API vollständig, konsistent und
englisch dokumentiert ist und fehlende Dokumentation das Quality Gate zuverlässig stoppt.

## Requirements

### Requirement: Vollständige Dokumentation der Produktions-API
Jeder explizit deklarierte extern sichtbare Typ und jedes explizit deklarierte extern sichtbare Mitglied in einem
Produktionsprojekt unter `src/` MUST einen XML-Dokumentationskommentar besitzen. Extern sichtbar umfasst
öffentliche API sowie geschützte API eines extern sichtbaren Typs.

#### Scenario: Undokumentierte öffentliche API
- **WHEN** ein öffentlicher Typ oder ein öffentliches Mitglied ohne XML-Dokumentationskommentar zu einem
  Produktionsprojekt hinzugefügt wird
- **THEN** schlägt der Build mit einer Dokumentationsdiagnose fehl

#### Scenario: Undokumentierte geschützte API
- **WHEN** ein geschütztes Mitglied eines extern sichtbaren Typs keinen XML-Dokumentationskommentar besitzt
- **THEN** schlägt der Build mit einer Dokumentationsdiagnose fehl

#### Scenario: Nicht extern sichtbare Implementierung
- **WHEN** ein ausschließlich intern oder privat sichtbarer Typ oder ein solches Mitglied keinen
  XML-Dokumentationskommentar besitzt
- **THEN** erzeugt die Dokumentationspflicht keinen Build-Fehler

### Requirement: Englische und aussagekräftige Dokumentation
XML-API-Dokumentation MUST in englischer Sprache verfasst sein und den fachlichen Zweck der API beschreiben.
Eigenständig dokumentierte Deklarationen MUST eine Zusammenfassung und Dokumentation ihrer Parameter
besitzen; Rückgabewerte und ausdrücklich ausgelöste Ausnahmen MUST dokumentiert werden, wenn sie Teil des
Vertrags der Deklaration sind.

#### Scenario: Eigenständig dokumentierte Operation
- **WHEN** eine extern sichtbare Operation Parameter, einen Rückgabewert oder ausdrücklich ausgelöste Ausnahmen
  besitzt
- **THEN** beschreibt ihr englischer XML-Kommentar den Zweck und alle für ihren Vertrag relevanten Bestandteile

#### Scenario: Bestehender nicht englischer Kommentar
- **WHEN** ein nicht englischer XML-Kommentar zur extern sichtbaren Produktions-API gehört
- **THEN** wird der Kommentar durch eine fachlich gleichwertige englische Dokumentation ersetzt

### Requirement: Geerbte Dokumentation
Der Dokumentationsvertrag MUST Overrides und Implementierungen von Interface-Mitgliedern erlauben, ihre
Vertragsdokumentation mit `<inheritdoc/>` zu übernehmen, sofern das überschriebene oder implementierte Mitglied
vollständig dokumentiert ist.

#### Scenario: Dokumentiertes Override
- **WHEN** ein Override mit `<inheritdoc/>` auf ein vollständig dokumentiertes Basismitglied verweist
- **THEN** erfüllt das Override die Dokumentationspflicht ohne duplizierte Beschreibung

#### Scenario: Dokumentierte Interface-Implementierung
- **WHEN** eine Implementierung mit `<inheritdoc/>` auf ein vollständig dokumentiertes Interface-Mitglied verweist
- **THEN** erfüllt die Implementierung die Dokumentationspflicht ohne duplizierte Beschreibung

### Requirement: Dokumentation projektgesteuerter Codegenerierung
Extern sichtbare API, die aus einem projektgesteuerten Generator-Template entsteht, MUST denselben
Dokumentationsanforderungen wie handgeschriebene Produktions-API unterliegen. Die Prüfung MUST unabhängig von
einer Unterdrückung der Compilerdiagnose im erzeugten Code wirksam bleiben.

#### Scenario: Undokumentiertes öffentliches Template-Mitglied
- **WHEN** ein projektgesteuertes Generator-Template ein öffentliches Mitglied ohne XML-Dokumentation deklariert
- **THEN** schlägt der Build mit einer projektspezifischen Dokumentationsdiagnose am Template fehl

#### Scenario: Zentral dokumentiertes Template-Mitglied
- **WHEN** ein öffentliches Template-Mitglied eine vollständige englische XML-Dokumentation besitzt
- **THEN** übernimmt jede daraus erzeugte ID-API dieselbe Dokumentation

### Requirement: Begrenzter Geltungsbereich
Die Dokumentationspflicht MUST für Produktionsprojekte unter `src/` gelten. Test- und Analyzer-Projekte MUST
von dieser Pflicht ausgenommen bleiben, ohne andere bestehende Build-Prüfungen zu deaktivieren.

#### Scenario: Öffentliche Test-API
- **WHEN** eine öffentliche Testklasse oder Testmethode keinen XML-Dokumentationskommentar besitzt
- **THEN** bleibt der Build hinsichtlich der Produktions-API-Dokumentationspflicht erfolgreich

#### Scenario: Andere Build-Prüfungen außerhalb der Produktion
- **WHEN** Test- oder Analyzer-Code gegen eine andere aktivierte Build-Regel verstößt
- **THEN** meldet der Build den Verstoß weiterhin unabhängig von der Dokumentationsausnahme

### Requirement: Ausschluss compiler-synthetisierter Standardmember
Mitglieder, die ausschließlich vom C#-Compiler synthetisiert werden und keine eigenständige Deklaration im
Produktionscode oder in einem projektgesteuerten Template besitzen, MUST von der Dokumentationspflicht
ausgenommen sein.

#### Scenario: Synthetisierte Record-Gleichheit
- **WHEN** der Compiler Standardmember für die Wertgleichheit eines Records synthetisiert
- **THEN** verlangt das Quality Gate keinen eigenständigen XML-Kommentar für diese Member

#### Scenario: Fachlicher Record-Positionsparameter
- **WHEN** ein Record-Positionsparameter eine fachliche öffentliche Property definiert
- **THEN** beschreibt die XML-Dokumentation des Records diesen Parameter
