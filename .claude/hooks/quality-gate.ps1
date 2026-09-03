#Requires -Version 7
<#
.SYNOPSIS
    Stop-Hook: Quality Gate. Läuft nur, wenn seit dem letzten grünen Lauf C#-Dateien geändert wurden.

.DESCRIPTION
    Schritte in dieser Reihenfolge, Abbruch beim ersten Fehler:
      1. dotnet format   – Whitespace, Style und Analyzer-Fixes anwenden
      2. dotnet build    – Warnungen sind Fehler (Directory.Build.props), inkl. .NET-Analyzer, Meziantou, Roslynator, MK-Regeln
      3. dotnet test     – alle Tests der Solution

    Bei Fehler wird der Stop blockiert (decision=block) und Claude erhält die Fehlerausgabe als Auftrag.
    Nach MaxAttempts vergeblichen Versuchen in Folge wird der Stop durchgelassen, damit keine Endlosschleife entsteht;
    der Marker bleibt, das Gate läuft beim nächsten Stop erneut.
#>
$ErrorActionPreference = 'Continue'
[Console]::OutputEncoding = [System.Text.UTF8Encoding]::new($false)   # Claude Code liest stdout als UTF-8
$MaxAttempts = 3

$raw = [Console]::In.ReadToEnd()
$hookInput = $null
try { $hookInput = $raw | ConvertFrom-Json } catch { $hookInput = $null }

$root = $env:CLAUDE_PROJECT_DIR
if (-not $root) { $root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot) }
$solution = Join-Path $root 'MachiKoro.slnx'
$marker = Join-Path $root '.claude/.quality-gate-pending'

if (-not (Test-Path $marker) -or -not (Test-Path $solution)) { exit 0 }

$sessionId = if ($hookInput -and $hookInput.session_id) { $hookInput.session_id } else { 'default' }
$counterFile = Join-Path ([System.IO.Path]::GetTempPath()) "machikoro-quality-gate-$sessionId.count"
$attempts = 0
if (Test-Path $counterFile) { $attempts = [int](Get-Content $counterFile -Raw) }

function Invoke-Step {
    param([string]$Name, [string[]]$Arguments)
    $output = & dotnet @Arguments 2>&1 | ForEach-Object { $_.ToString() }
    return [pscustomobject]@{ Name = $Name; ExitCode = $LASTEXITCODE; Output = $output }
}

function Format-Failure {
    param([pscustomobject]$Step)
    $relevant = $Step.Output | Where-Object { $_ -match 'error|warn|fail|Failed|Fehler' } | Select-Object -Unique -First 40
    if (-not $relevant) { $relevant = $Step.Output | Select-Object -Last 30 }
    return ($relevant -join "`n")
}

Push-Location $root
try {
    $steps = @(
        @{ Name = 'dotnet format'; Arguments = @('format', $solution, '--no-restore', '--verbosity', 'quiet') },
        @{ Name = 'dotnet build';  Arguments = @('build', $solution, '--nologo', '-consoleLoggerParameters:NoSummary;ErrorsOnly') },
        # Kein --nologo: der Microsoft.Testing.Platform-Runner findet damit keine Tests (Exit 5).
        @{ Name = 'dotnet test';   Arguments = @('test', $solution, '--no-build', '--verbosity', 'quiet') }
    )

    foreach ($definition in $steps) {
        $step = Invoke-Step -Name $definition.Name -Arguments $definition.Arguments
        if ($step.ExitCode -eq 0) { continue }

        $attempts++
        Set-Content -Path $counterFile -Value $attempts -NoNewline
        $details = Format-Failure -Step $step

        if ($attempts -ge $MaxAttempts) {
            Remove-Item $counterFile -Force -ErrorAction SilentlyContinue
            $message = "Quality Gate nach $MaxAttempts Versuchen weiterhin rot ($($step.Name)). Stop wird durchgelassen, Gate läuft beim nächsten Stop erneut.`n$details"
            @{ systemMessage = $message } | ConvertTo-Json -Compress
            exit 0
        }

        $reason = @"
QUALITY GATE FEHLGESCHLAGEN – Schritt '$($step.Name)' (Versuch $attempts von $MaxAttempts).
Behebe die Ursachen, halte dabei docs/coding-guidelines.md ein (SOLID, Object Calisthenics) und beende dann erneut.
Analyzer-Regeln nicht unterdrücken (#pragma, SuppressMessage, .editorconfig), sondern den Code umbauen.

$details
"@
        @{ decision = 'block'; reason = $reason } | ConvertTo-Json -Compress
        exit 0
    }

    # Alles grün.
    Remove-Item $marker -Force -ErrorAction SilentlyContinue
    Remove-Item $counterFile -Force -ErrorAction SilentlyContinue
    @{ systemMessage = 'Quality Gate grün: dotnet format, build (Analyzer) und Tests erfolgreich.' } | ConvertTo-Json -Compress
    exit 0
}
finally {
    Pop-Location
}
