; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
MK0001  | ObjectCalisthenics | Warning | Höchstens zwei Instanzvariablen pro Typ
MK0002  | ObjectCalisthenics | Warning | Höchstens zehn Methoden pro Typ
MK0003  | ObjectCalisthenics | Warning | Kein else
MK0004  | ObjectCalisthenics | Warning | Eine Einrückungsebene pro Methode
MK0005  | ObjectCalisthenics | Warning | Höchstens drei Parameter
MK0006  | ObjectCalisthenics | Warning | Höchstens 120 nicht-leere Zeilen pro Datei
MK0007  | ObjectCalisthenics | Warning | Zeilenlänge höchstens 120 Zeichen
MK0008  | ObjectCalisthenics | Warning | Höchstens zwei Bedingungsoperatoren pro Ausdruck
MK0009  | Documentation | Warning | Extern sichtbare Member in .typedid benötigen XML-Dokumentation
