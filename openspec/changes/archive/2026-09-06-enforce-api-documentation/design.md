## Context

`GenerateDocumentationFile` ist zentral aktiviert, damit unter anderem `IDE0005` im Build ausgewertet wird.
Gleichzeitig unterdrückt `Directory.Build.props` die dadurch aktivierte Compilerdiagnose `CS1591` für die gesamte
Solution. Die projektspezifischen Analyzer werden dagegen über `src/Directory.Build.props` ausschließlich in
Produktionsprojekte eingebunden.

Der Domain-Code enthält normalen C#-Quelltext und zwei StronglyTypedId-Deklarationen, deren öffentliche Member
aus `Identifiers/guid-domain.typedid` erzeugt werden. StronglyTypedId markiert seinen Output als generiert und
deaktiviert darin `CS1591`; die Dokumentationspflicht kann deshalb nicht allein durch den Compiler abgedeckt
werden. Siehe `proposal.md` für die Motivation und `specs/api-documentation/spec.md` für den Verhaltensvertrag.

## Goals / Non-Goals

**Goals:**

- Fehlende XML-Dokumentation in normalem Produktionscode und projektgesteuerten `.typedid`-Templates im Build
  an der verantwortlichen Quelldeklaration melden.
- Die globale Prüfung unbenutzter `using`-Direktiven beibehalten.
- Vererbte Verträge ohne kopierte Texte dokumentieren.
- Den aktuellen Bestand ohne Änderungen an Signaturen oder Laufzeitverhalten migrieren.

**Non-Goals:**

- Öffentliche Typen und Methoden von Tests oder dem Analyzer-Projekt dokumentieren.
- Compiler-synthetisierte Standardmember ohne eigene Quelldeklaration dokumentieren.
- Eine Dokumentationswebsite erzeugen oder veröffentlichen.
- Die Sichtbarkeit bestehender Domain-Typen im Rahmen dieser Änderung neu gestalten.
- Englische Sprache oder fachliche Qualität der Texte automatisch analysieren.

## Decisions

### Compilerdiagnose nach Verzeichnis begrenzen

Die globale `NoWarn`-Ergänzung für `CS1591` wird entfernt. Die Root-`.editorconfig` setzt die Diagnose außerhalb
des Produktionsbereichs auf `none` und für C#-Dateien unter `src/` auf `error`. Dadurch bleibt
`GenerateDocumentationFile` für alle Projekte aktiv, ohne Test- und Analyzer-APIs in den Dokumentationsvertrag
aufzunehmen.

Die Bereichsausnahme wird direkt an der Diagnostic-Konfiguration begründet. Alternativ erwogen wurde,
`GenerateDocumentationFile` außerhalb von `src/` abzuschalten; das würde jedoch die bestehende Build-Prüfung
von `IDE0005` schwächen. Eine globale Aktivierung von `CS1591` wurde wegen redundanter Kommentare an
beschreibend benannten Tests verworfen.

### Eigene Regel für projektgesteuerte Templates

Eine neue Diagnostic `MK0009` prüft als Additional Files eingebundene `.typedid`-Templates. Sie analysiert deren
C#-Syntax und meldet jede explizite öffentliche oder geschützte Memberdeklaration ohne XML-Dokumentation an der
Position im Template. Compiler-synthetisierte Member sind nicht Teil dieser Eingabe und benötigen keine
Sonderbehandlung.

Die Prüfung des Templates ist einer Analyse des generierten Syntaxbaums vorzuziehen: Die Meldung verweist damit
auf die Datei, die der Entwickler tatsächlich ändern kann, und ein Kommentar dokumentiert alle daraus erzeugten
ID-Typen zentral. Ein Reflection-Test gegen die erzeugte XML-Datei wurde verworfen, weil er XML-Member-IDs
rekonstruieren und compiler-synthetisierte API nachträglich von projektierten Membern unterscheiden müsste.

`MK0009` wird wie die bestehenden MK-Regeln in `Rules`, `.editorconfig` und dem Analyzer-Release-Tracking
registriert. Die Regel läuft nur in Produktionsprojekten, weil diese als einzige den projektspezifischen Analyzer
referenzieren.

### Dokumentationsform

Eigenständige API-Deklarationen erhalten englische `<summary>`-Texte sowie die für ihren Vertrag relevanten
`<param>`, `<returns>` und `<exception>`-Elemente. Abstrakte Basismember und Interface-Mitglieder tragen den
vollständigen Vertrag; Overrides und Implementierungen verwenden `<inheritdoc/>`. Positions-Records
dokumentieren ihre fachlichen Properties über `<param>` am Record. Objekt-Overrides übernehmen vorhandene
Verträge ebenfalls mit `<inheritdoc/>`.

Die Build-Regeln prüfen die Existenz eines XML-Kommentars, nicht dessen sprachliche oder fachliche Qualität.
Diese bleibt Teil der Review-Checkliste, da eine automatische Sprach- oder Bedeutungsprüfung unverhältnismäßig
und fehleranfällig wäre.

### Migration als rotes Quality Gate

Die Durchsetzung wird zunächst gegen den undokumentierten Bestand aktiviert und muss aus dem erwarteten Grund
fehlschlagen. Anschließend werden Template und Domain-API dokumentiert, bis Build und Tests wieder grün sind.
Damit dient das Quality Gate selbst als fehlschlagender Test der Konfigurationsänderung; das Laufzeitverhalten
benötigt keine neuen fachlichen Tests.

## Risks / Trade-offs

- [XML-Kommentare erhöhen die Dateilänge und können `MK0006` auslösen] -> Kommentare knapp halten und die
  bestehenden Grenzwerte während der Migration kontinuierlich prüfen.
- [Wiederholte Karteneigenschaften erzeugen Dokumentationsrauschen] -> Gemeinsame Verträge an Basismembern
  beschreiben und konkrete Overrides mit `<inheritdoc/>` versehen.
- [Die Template-Regel könnte nicht öffentliche Hilfsdeklarationen erfassen] -> Nur explizit extern sichtbare
  Member prüfen und die Regel zuerst am bestehenden Template rot und grün verifizieren.
- [Englische oder fachlich leere Kommentare passieren die technische Prüfung] -> Konkrete Qualitätskriterien in
  den Coding-Guidelines und der Review-Checkliste festhalten.
- [Verzeichnisbasierter Scope erfasst künftig falsch abgelegte Projekte nicht] -> Die Architektur schreibt
  Produktionsprojekte unter `src/` vor; Abweichungen sind ein separates Architekturproblem.

## Migration Plan

1. `CS1591` für `src/` und `MK0009` für `.typedid`-Templates aktivieren und die erwarteten Build-Fehler
   protokollieren.
2. Basistypen, Interfaces und Generator-Template dokumentieren, damit vererbte Verträge verfügbar sind.
3. Konkrete Domain-Typen, Konstruktoren, Felder, Enum-Werte und Record-Parameter dokumentieren.
4. Coding- und Build-Guidelines aktualisieren und das vollständige Quality Gate ausführen.

Die Änderung benötigt keine Laufzeitmigration oder Deployment-Reihenfolge. Ein Rollback besteht aus dem
Zurücknehmen der Diagnostic-Aktivierung und der neuen Template-Regel; die hinzugefügten XML-Kommentare sind
laufzeitneutral und können bestehen bleiben.
