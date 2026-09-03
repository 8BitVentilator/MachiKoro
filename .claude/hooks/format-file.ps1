#Requires -Version 7
<#
.SYNOPSIS
    PostToolUse-Hook (Write|Edit): formatiert eine geänderte C#-Datei sofort mit `dotnet format whitespace`
    und merkt sich, dass das Quality Gate beim nächsten Stop laufen muss.

.NOTES
    Liest das Hook-JSON von stdin. Läuft still: keine Ausgabe, Exit-Code immer 0.
    Die vollständige Formatierung (Style + Analyzer-Fixes) übernimmt quality-gate.ps1, weil sie einen Build braucht.
#>
$ErrorActionPreference = 'Continue'
[Console]::OutputEncoding = [System.Text.UTF8Encoding]::new($false)

$raw = [Console]::In.ReadToEnd()
if ([string]::IsNullOrWhiteSpace($raw)) { exit 0 }

try { $hookInput = $raw | ConvertFrom-Json } catch { exit 0 }

$file = $hookInput.tool_input.file_path
if (-not $file) { $file = $hookInput.tool_response.filePath }
if (-not $file -or $file -notmatch '\.cs$') { exit 0 }

$root = $env:CLAUDE_PROJECT_DIR
if (-not $root) { $root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot) }
$solution = Join-Path $root 'MachiKoro.slnx'
if (-not (Test-Path $solution)) { exit 0 }

$fullPath = [System.IO.Path]::GetFullPath($file)
$relative = [System.IO.Path]::GetRelativePath($root, $fullPath)
if ($relative.StartsWith('..')) { exit 0 }   # Datei liegt außerhalb des Projekts

# Marker: Quality Gate ist fällig.
$marker = Join-Path $root '.claude/.quality-gate-pending'
Set-Content -Path $marker -Value (Get-Date -Format 'o') -NoNewline

Push-Location $root
try {
    & dotnet format $solution whitespace --include $relative --no-restore 2>&1 | Out-Null
}
finally {
    Pop-Location
}
exit 0
