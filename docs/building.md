# Build und Quality Gate

## Voraussetzungen

- .NET SDK 10.0.400 oder neuer (`dotnet --version`)
- PowerShell 7 (`pwsh`) für die Hooks. PowerShell 7 ist plattformübergreifend, die Hook-Skripte laufen unverändert unter Windows, Linux und macOS.

```bash
# Ubuntu / Debian
sudo apt-get install -y powershell        # ggf. zuvor Microsoft-Paketquelle einrichten: https://aka.ms/install-powershell
# oder distributionsunabhängig
sudo snap install powershell --classic
# macOS
brew install powershell/tap/powershell
# Windows
winget install Microsoft.PowerShell
```

Die Hooks werden ohne Shell gestartet (Exec-Form in `.claude/settings.json`, relative Pfade, Arbeitsverzeichnis ist das Projekt). Damit spielt es keine Rolle, ob auf dem Rechner Bash, Git Bash oder nur PowerShell vorhanden ist.

## Befehle

```bash
dotnet restore MachiKoro.slnx
dotnet build   MachiKoro.slnx          # Warnungen sind Fehler
dotnet test    MachiKoro.slnx
dotnet format  MachiKoro.slnx          # Formatierung, Style und Analyzer-Fixes anwenden
dotnet format  MachiKoro.slnx --verify-no-changes   # nur prüfen (CI)
```

## Zentrale Konfiguration

| Datei | Zweck |
|---|---|
| `Directory.Build.props` | Zielframework, C# 14, `TreatWarningsAsErrors`, `AnalysisLevel=latest-all`, `EnforceCodeStyleInBuild`, Analyzer-Pakete |
| `Directory.Packages.props` | Zentrale Paketversionen (Central Package Management) |
| `.editorconfig` | Formatierung, Namensregeln, Schweregrad jeder Analyzer-Regel |
| `tests/.editorconfig` | Lockerungen für Testprojekte |
| `CodeMetricsConfig.txt` | Grenzwerte der .NET-Metrikregeln CA1502, CA1505, CA1506 |
| `analyzers/MachiKoro.Analyzers/Rules.cs` | Grenzwerte der Object-Calisthenics-Regeln MK0001 bis MK0008 |
| `src/Directory.Build.props` | Bindet `analyzers/MachiKoro.Analyzers` in jedes Produktionsprojekt ein |
| `global.json` | SDK-Version und `dotnet test` auf Microsoft.Testing.Platform |
| `nuget.config` | Nur nuget.org als Quelle (Central Package Management verlangt eindeutige Quelle) |

Analyzer: `Microsoft.CodeAnalysis.NetAnalyzers` (im SDK, MIT), `Meziantou.Analyzer` (MIT), `Roslynator.Analyzers` (Apache-2.0), `MachiKoro.Analyzers` (eigen, Regeln MK0001 bis MK0008 siehe [coding-guidelines.md](coding-guidelines.md)). SonarAnalyzer.CSharp wird bewusst nicht eingesetzt: Die Sonar Source-Available License v1.0 schließt die Verarbeitung der Analyzer-Ausgabe durch KI-Werkzeuge aus, was dem Stop-Hook widerspricht.

## Testprojekt (`dotnet test`)

xUnit v3 läuft auf Microsoft.Testing.Platform. Die Testprojekte sind ausführbar, `dotnet test` ist über `global.json` auf den neuen Runner gestellt. Alternativ startet `dotnet run --project tests/MachiKoro.Domain.Tests` die Tests direkt.

## Quality Gate über Claude-Code-Hooks

Konfiguriert in `.claude/settings.json`, Skripte in `.claude/hooks/`.

### PostToolUse (`format-file.ps1`)

Läuft nach jedem `Write` oder `Edit` einer `.cs`-Datei:

1. formatiert die Datei mit `dotnet format whitespace --include <Datei>`,
2. legt den Marker `.claude/.quality-gate-pending` an.

Still, blockiert nie. Die vollständige Formatierung mit Style- und Analyzer-Fixes braucht einen Build und passiert im Stop-Hook.

### Stop (`quality-gate.ps1`)

Läuft, wenn Claude einen Turn beenden will, aber nur wenn der Marker existiert. Reihenfolge:

1. `dotnet format` (alle Fixes anwenden)
2. `dotnet build` (Analyzer als Fehler)
3. `dotnet test`

Schlägt ein Schritt fehl, wird der Stop blockiert und Claude erhält die Fehlerausgabe mit dem Auftrag, den Code umzubauen statt Regeln zu unterdrücken. Nach 3 vergeblichen Versuchen in Folge wird der Stop durchgelassen, damit keine Endlosschleife entsteht. Der Marker bleibt, das Gate läuft beim nächsten Stop wieder.

Nach einem grünen Lauf wird der Marker gelöscht, bis zur nächsten C#-Änderung läuft das Gate nicht.

### Hooks prüfen oder ändern

- `/hooks` in Claude Code zeigt die aktiven Hooks.
- Manuell testen:

```bash
echo '{"tool_name":"Edit","tool_input":{"file_path":"src/MachiKoro.Domain/Economy/Coins.cs"}}' | pwsh -NoProfile -File .claude/hooks/format-file.ps1
echo '{"session_id":"manual"}' | pwsh -NoProfile -File .claude/hooks/quality-gate.ps1
```

Beide Skripte lesen das Hook-JSON von stdin und ermitteln das Projektverzeichnis aus `CLAUDE_PROJECT_DIR` oder, falls nicht gesetzt, aus ihrem eigenen Speicherort.

## CI

Für eine Pipeline reichen dieselben drei Schritte mit `dotnet format --verify-no-changes` statt `dotnet format`.
