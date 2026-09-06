## Why

Die Coding-Guidelines verlangen XML-Kommentare für öffentliche Methoden und Properties, während die zentrale
Build-Konfiguration `CS1591` global unterdrückt. Dadurch ist ein großer Teil der öffentlichen Domain-API
undokumentiert und neue Verstöße passieren trotz Quality Gate unbemerkt.

## What Changes

- Die Dokumentationspflicht wird auf die gesamte extern sichtbare API aller Produktionsprojekte unter `src/`
  erweitert.
- XML-API-Dokumentation wird ein englischsprachiger, im Build geprüfter Vertrag.
- `CS1591` wird für normalen C#-Produktionscode als Fehler aktiviert, während Tests und Analyzer ausdrücklich
  außerhalb des Geltungsbereichs bleiben.
- Eine projektspezifische Analyzer-Regel prüft öffentliche Mitglieder in `.typedid`-Templates, weil
  `StronglyTypedId` die Compilerdiagnose in generiertem Code unterdrückt.
- Die bestehende öffentliche Domain-API und das StronglyTypedId-Template werden vollständig dokumentiert.
- Coding- und Build-Guidelines werden an den durchgesetzten Geltungsbereich angeglichen.

## Capabilities

### New Capabilities

- `api-documentation`: Definiert Umfang, Sprache und Build-Durchsetzung der XML-Dokumentation für extern
  sichtbare Produktions-API einschließlich projektgesteuerter Codegenerierung.

### Modified Capabilities

Keine.

## Impact

Betroffen sind die zentrale Diagnostic-Konfiguration, die projektspezifischen Analyzer, alle aktuell öffentlichen
Typen und Mitglieder in `MachiKoro.Domain`, das StronglyTypedId-Template sowie die Coding- und Build-Guidelines.
Laufzeitverhalten und öffentliche Signaturen ändern sich nicht. Es werden keine neuen Laufzeitabhängigkeiten
eingeführt.
