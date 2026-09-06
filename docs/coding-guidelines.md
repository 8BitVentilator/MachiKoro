# Coding-Guidelines

Gilt für allen Code unter `src/`. Für Tests gelten die Lockerungen in `tests/.editorconfig` und die [Testing-Guidelines](testing-guidelines.md).

Drei Schichten setzen die Regeln durch:

| Schicht | Was sie prüft | Wo konfiguriert |
|---|---|---|
| `dotnet format` | Formatierung, Style, automatisch behebbare Analyzer-Fixes | `.editorconfig`, Hooks in `.claude/settings.json` |
| Analyzer im Build | Metriken, Unveränderlichkeit, Dokumentation, tote Member, .NET-Regeln | `Directory.Build.props`, `.editorconfig`, `CodeMetricsConfig.txt`, `analyzers/MachiKoro.Analyzers` |
| Review (Mensch oder Agent) | Alles, was ein Analyzer nicht messen kann | Checkliste unten |

Analyzer-Befunde werden **nie unterdrückt**, sondern durch Umbau des Codes gelöst.

## Object Calisthenics

Neun Regeln nach Jeff Bay. Die Spalte „Durchsetzung“ nennt die Analyzer-Regel, die das Verhalten misst. Wo „Review“ steht, prüft niemand automatisch, dort ist die Disziplin beim Schreiben entscheidend.

| # | Regel | Bedeutung im Code | Durchsetzung |
|---|---|---|---|
| 1 | Eine Einrückungsebene pro Methode | Kein `if` in einer Schleife, keine Schleife in einem `if`. Innere Blöcke werden zu benannten Methoden extrahiert. | MK0004 (eigener Analyzer) |
| 2 | Kein `else` | Early Return, Guard Clauses, Polymorphie oder Strategy statt Verzweigung. Ein `switch`-Ausdruck über einen geschlossenen Typ (Enum, Record-Hierarchie) ist erlaubt. | MK0003 (eigener Analyzer) |
| 3 | Primitive und Strings wrappen | Kein `int coins`, sondern `Coins`. Kein `string name`, sondern `PlayerName`. Werte-Objekte sind `readonly record struct` oder `sealed record`. | MK0005 (höchstens 3 Parameter) zwingt indirekt, Review |
| 4 | First-Class Collections | Eine Klasse, die eine Collection enthält, enthält nichts anderes. `Hand`, `Supply`, `Establishments` statt `List<Establishment>` im Player. | Review |
| 5 | Ein Punkt pro Zeile | Kein `player.Hand.Cards.First().Cost`. Frag das Objekt, statt durch es hindurchzugreifen (Law of Demeter). Fluent APIs und LINQ auf eigenen Werten sind ausgenommen. | MK0008 (Ausdruckskomplexität), Review |
| 6 | Nicht abkürzen | `Establishment` statt `Est`, `diceRoll` statt `dr`. Ein Name, der lang wird, deutet auf eine Klasse mit zu viel Verantwortung. | IDE1006 (Form), Review (Bedeutung) |
| 7 | Kleine Einheiten | Methoden ≤ 15 Zeilen, Dateien ≤ 120 nicht-leere Zeilen, ≤ 10 Methoden pro Klasse. | Meziantou MA0051; MK0006; MK0002 |
| 8 | Höchstens zwei Instanzvariablen | Zwei Felder oder Auto-Properties pro Klasse. Mehr bedeutet: Es fehlt ein Zwischenobjekt, das zwei davon zusammenfasst. | MK0001 |
| 9 | Keine Getter/Setter | Objekte tun etwas, statt Zustand herauszugeben. `coins.CanAfford(cost)` statt `coins.Amount >= cost.Amount`. Öffentliche Setter gibt es nicht. Read-only-Properties sind erlaubt, wenn ein anderer Typ den Wert wirklich braucht (Anzeige, Persistenz). | Roslynator RCS1170, CA1051, Review |

Ergänzend gemessen: zyklomatische Komplexität ≤ 5 (CA1502), Wartbarkeitsindex ≥ 20 (CA1505), Klassenkopplung ≤ 12 Typen (CA1506), Zeilenlänge ≤ 120 (MK0007).

### Die MK-Regeln

Die projekteigenen Regeln liegen im Projekt `analyzers/MachiKoro.Analyzers` und werden über
`src/Directory.Build.props` in jedes Produktionsprojekt eingebunden. MK0001 bis MK0008 messen
Object Calisthenics; MK0009 ergänzt die Dokumentationsprüfung für Generator-Templates. Grenzwerte stehen als
Konstanten in `Rules.cs`. Alle Abhängigkeiten des Projekts sind MIT- oder Apache-lizenziert.

| ID | Regel | Grenze |
|---|---|---|
| MK0001 | Instanzvariablen pro Typ (Felder und Auto-Properties) | 2 |
| MK0002 | Methoden pro Typ | 10 |
| MK0003 | `else` | verboten |
| MK0004 | Verschachtelung von Kontrollfluss innerhalb einer Methode | 1 Ebene |
| MK0005 | Parameter pro Methode oder Konstruktor | 3 |
| MK0006 | Nicht-leere Zeilen pro Datei | 120 |
| MK0007 | Zeichen pro Zeile | 120 |
| MK0008 | Bedingungsoperatoren (`&&`, `\|\|`, `?:`) pro Ausdruck | 2 |
| MK0009 | XML-Dokumentation extern sichtbarer Member in `.typedid`-Templates | vollständig |

### Beispiel

```csharp
// Verstößt gegen 1, 2, 3, 9
public int Collect(Player p, int roll)
{
    int sum = 0;
    foreach (var c in p.Cards)
    {
        if (c.Activation == roll)
        {
            sum += c.Income;
        }
        else
        {
            continue;
        }
    }
    return sum;
}

// Konform
public Coins IncomeFor(DiceRoll roll) => _establishments.ActivatedBy(roll).TotalIncome();
```

## SOLID

| Prinzip | Regel für dieses Projekt | Durchsetzung |
|---|---|---|
| **S**ingle Responsibility | Eine Klasse hat genau einen Grund, sich zu ändern. Regeln, Zustand, Zufall und Ein-/Ausgabe liegen in getrennten Typen. | CA1506, MK0001, MK0002, Review |
| **O**pen/Closed | Neue Gebäude oder Effekte kommen als neue Typen dazu, nicht als neuer `case` in einer bestehenden Methode. Effekte sind polymorph (`IIncomeEffect`), nicht per Enum-Switch. | Review |
| **L**iskov Substitution | Ableitungen verschärfen keine Vorbedingungen und werfen keine Ausnahmen, die die Basis nicht kennt. Bevorzugt `sealed` und Komposition statt Vererbung. | CA1052/CA1812, Review |
| **I**nterface Segregation | Interfaces haben 1 bis 3 Methoden und sind aus Sicht des Aufrufers benannt (`IDice`, `IIncomeEffect`), nicht aus Sicht der Implementierung. | Review |
| **D**ependency Inversion | Die Domäne kennt keine Infrastruktur. Zufall (`IDice`), Zeit und Ausgabe werden als Interface hineingereicht. Konstruktor-Injektion, keine statischen Zugriffe. | Review, später Architekturtests |

## Weitere Konventionen

- `sealed` ist Standard für Klassen. Vererbung braucht eine Begründung.
- Unveränderlichkeit ist Standard: `readonly` Felder, `init`-Properties nur an Grenzen, Werte-Objekte als `readonly record struct`.
- IDs werden mit dem NuGet-Paket `StronglyTypedId` erzeugt. Keine nackten `Guid`-Werte und keine handgeschriebenen ID-Wrapper.
- Fehler in der Domäne sind Domänenausnahmen (`InvalidMoveException`) oder Ergebnistypen, keine `ArgumentException` aus der Tiefe.
- Defensive Prüfungen (`ArgumentOutOfRangeException.ThrowIfNegative`) stehen an Systemgrenzen und in Fabrikmethoden, nicht in jeder Methode.
- Kommentare erklären Spielregeln oder Entscheidungen, nie den Code selbst.
- Die gesamte extern sichtbare Produktions-API ist nach den Regeln im folgenden Abschnitt dokumentiert.
- File-scoped Namespaces, `using` außerhalb des Namespaces, ein Typ pro Datei, Dateiname = Typname.

## API-Dokumentation

Jeder explizit deklarierte öffentliche Typ und Member unter `src/` besitzt einen englischen
XML-Dokumentationskommentar. Das gilt ebenso für geschützte Member eines extern sichtbaren Typs, also auch für
Konstruktoren, Felder, Enum-Werte und geschützte Overrides. Eigenständige Dokumentation beschreibt den fachlichen
Zweck in `<summary>` sowie alle Parameter. Rückgabewerte und ausdrücklich ausgelöste Ausnahmen werden mit
`<returns>` beziehungsweise `<exception>` dokumentiert, wenn sie Teil des Vertrags sind.

Overrides und Interface-Implementierungen verwenden `<inheritdoc/>`, wenn der geerbte Vertrag vollständig
dokumentiert ist. Fachliche Positionsparameter eines Records werden mit `<param>` am Record dokumentiert;
ausschließlich vom Compiler synthetisierte Standardmember benötigen keinen eigenen Kommentar.

`CS1591` prüft normalen C#-Produktionscode. Projektgesteuerte `.typedid`-Templates werden zusätzlich durch MK0009
geprüft, weil StronglyTypedId die Compilerdiagnose im generierten Code unterdrückt. Test- und Analyzer-Projekte sind
von der Dokumentationspflicht ausgenommen, ihre übrigen Build-Regeln bleiben aktiv. Die Analyzer prüfen nur, ob ein
Kommentar existiert. Englische Sprache, fachlicher Gehalt und Vollständigkeit der Vertragselemente bleiben
Review-Aufgaben.

## Review-Checkliste

Die Liste enthält nur, was kein Analyzer misst. Die Regeln 1, 2, 7 und 8 sowie Parameteranzahl, Zeilen- und
Dateilänge, Ausdruckskomplexität und das Vorhandensein von API-Kommentaren fehlen hier absichtlich: Sie brechen den
Build (MK0001 bis MK0009, CS1591, MA0051) und brauchen kein Review.

Vor dem Abschluss jeder Aufgabe für jeden geänderten Typ prüfen:

- [ ] Kein nackter `int`, `string`, `bool` als Domänenwert in Signaturen (Regel 3)
- [ ] Keine Collection neben anderem Zustand (Regel 4)
- [ ] Kein Durchgriff über zwei Objekte hinweg (Regel 5)
- [ ] Namen ausgeschrieben und aus der Domänensprache (Regel 6)
- [ ] Keine öffentlichen Setter, kein Getter, der nur Zustand herausreicht (Regel 9)
- [ ] Ein Änderungsgrund pro Klasse (SRP)
- [ ] Neues Verhalten als neuer Typ statt als neuer Zweig (OCP)
- [ ] Domäne referenziert keine Infrastruktur (DIP)
- [ ] API-Kommentare sind englisch, fachlich aussagekräftig und dokumentieren relevante Parameter, Rückgaben und Ausnahmen
