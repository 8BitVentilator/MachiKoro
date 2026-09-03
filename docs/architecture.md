# Architektur

Stand: 2026-09-03. Zielbild für die erste lauffähige Version (Basisspiel, Netzwerk, Konsolen-Client). Beschreibt Struktur und Entscheidungen, keinen Code. Fachliche Grundlage ist [spielregeln.md](spielregeln.md), Code-Regeln stehen in [coding-guidelines.md](coding-guidelines.md).

## 1. Anforderungen, die die Architektur bestimmen

| Anforderung | Entscheidung |
|---|---|
| Menschen spielen gegeneinander über das Netzwerk, keine Computergegner | Client-Server. Der Server ist die einzige Instanz, die Regeln ausführt. |
| Ein Server verwaltet mehrere Partien, Beitritt per Spielcode | Lobby mit Spielcodes, Ereignisse pro Partie. |
| Partien speichern und nachvollziehen | Der Ereignisstrom einer Partie ist zugleich Speicherform und Protokoll. |
| Zufall reproduzierbar | Würfel-Seed ist Teil des Startereignisses. |
| Erste UI ist die Konsole, spätere UIs offen | Client kennt nur Nachrichten, keine Regeln. Jede UI spricht dasselbe Protokoll. |
| Basisspiel zuerst, Variante und Erweiterungen später möglich | Karteneffekte und Auslage als austauschbare Typen. |
| Code Englisch, Oberfläche Deutsch, Englisch vorbereitet | Domäne arbeitet mit Bezeichnern, Client übersetzt über Ressourcen. |
| Object Calisthenics und SOLID werden vom Build erzwungen | Kleine Typen, Slices statt Dienste, First-Class Collections. |

## 2. Architekturstil

**Reiche Domäne, Vertical Slices in der Anwendungsschicht, ereignisbasiertes Spiel.**

- Die Domäne ist ein zusammenhängendes Regelmodell und wird nicht in Slices geschnitten, weil die Regeln eines Brettspiels sich gegenseitig durchdringen (Einkaufszentrum wirkt auf Bäckerei, Freizeitpark auf Würfeln).
- Die Anwendungsschicht ist in Slices pro Spieleraktion geschnitten. Jeder Slice ist ein Ordner mit Kommando, Handler und Ergebnis und kennt keine anderen Slices.
- Der Zustand einer Partie entsteht ausschließlich durch Anwenden ihrer Ereignisse. Laden, Nachvollziehen und Verteilen an Clients nutzen denselben Ereignisstrom.

Verworfen: klassische Schichten mit Zustandsschnappschuss (zwei Wahrheiten, großer `GameService`) und durchgehende Vertical Slices bis in die Domäne (Regeln würden dupliziert).

## 3. Projekte und Abhängigkeiten

```
MachiKoro.Client  ──►  MachiKoro.Contracts  ◄──  MachiKoro.Server
                                                       │
                                                       ▼
                                               MachiKoro.Application
                                                       │
                                                       ▼
                                                 MachiKoro.Domain
```

| Projekt | Verantwortung | Darf referenzieren |
|---|---|---|
| `MachiKoro.Domain` | Spielregeln. `Game`-Aggregat, Werte-Objekte, Kommandos, Ereignisse, Kartenkatalog. Nur BCL. | nichts |
| `MachiKoro.Application` | Anwendungsfälle als Vertical Slices. Schnittstellen für Ereignisspeicher und Seed-Quelle. | `Domain` |
| `MachiKoro.Contracts` | Nachrichten zwischen Client und Server: Kommando-Records, Ereignis-Records, Fehlercodes. Reine Daten. | nichts |
| `MachiKoro.Server` | ASP.NET-Core-Host mit SignalR. Lobby, Verbindungen, Übersetzung Contracts ↔ Application, Dateispeicher. | `Application`, `Contracts` |
| `MachiKoro.Client` | Konsolen-Client. Baut Anzeige aus Ereignissen, sammelt Eingaben, sendet Kommandos. Lokalisierung. | `Contracts` |

`Contracts` trennt Client und Domäne. Ohne dieses Projekt müsste der Client `Application` und damit die gesamte Domäne referenzieren und könnte Regeln lokal ausführen. Contracts-Records sind von Domänen-Ereignissen getrennt, damit sich die Domäne ändern kann, ohne alte Clients zu brechen.

Testprojekte: je eines pro Projekt unter `tests/`, dazu `MachiKoro.ArchitectureTests`. Das vorhandene Analyzer-Projekt `analyzers/MachiKoro.Analyzers` wird über `src/Directory.Build.props` in alle fünf Produktionsprojekte eingebunden.

## 4. Domäne

### Aggregat `Game`

Einzige Einstiegsstelle für Regeln. Nimmt ein Kommando an und gibt entweder eine Liste von Ereignissen oder einen Regelverstoß (`Rejection` mit Fehlercode) zurück. `Game` verändert sich nur, indem es seine eigenen Ereignisse auf sich anwendet (`Apply`). Kein Typ außer `Game` ruft `Apply` auf. „Partie laden“ bedeutet: leeres `Game`, alle Ereignisse anwenden.

Zustand von `Game`: `Players` (First-Class Collection) und `Table`. `Table` hält die Auslage `Supply` und den laufenden `Turn`. Ein `Player` hält `City` und `Coins`. `City` hält `Establishments` und `Landmarks`. Damit hat kein Typ mehr als zwei Instanzvariablen.

### Kommandos

Was ein Spieler will. Ein Kommando pro Aktion:

`StartGame`, `RollDice` (1 oder 2 Würfel), `RerollDice` (Funkturm), `BuildEstablishment`, `BuildLandmark`, `SkipBuilding`, `ChooseTelevisionTarget`, `ChooseOfficeSwap`, `EndTurn`.

### Ereignisse

Was geschehen ist. Unveränderliche `sealed record`, Vergangenheitsform, nur Fakten:

`GameCreated`, `PlayerJoined`, `GameStarted` (Spielreihenfolge, Würfel-Seed, Spielmodus), `DiceRolled`, `CoinsPaidToPlayer`, `CoinsReceivedFromBank`, `EstablishmentBuilt`, `LandmarkBuilt`, `EstablishmentsSwapped`, `ExtraTurnGranted`, `TurnEnded`, `GameWon`.

Jedes gespeicherte Ereignis trägt eine Versionsnummer, damit spätere Formatänderungen alte Partien lesbar lassen.

### Zufall

`Game` würfelt nie selbst. Ein `IDice` liefert Würfe. Die Produktivimplementierung `SeededDice` arbeitet deterministisch aus dem Seed in `GameStarted`; beim Abspielen entstehen dieselben Würfe, weil Seed und Reihenfolge gleich sind. Tests nutzen `ScriptedDice` mit fester Folge.

### Karten und Effekte

Jedes der 15 Unternehmen ist ein Eintrag im Kartenkatalog: `EstablishmentId`, Würfelzahlen, Farbe, Branche, Kosten, `IncomeEffect`. Effekt-Typen:

| Effekt | Karten |
|---|---|
| `FixedIncomeFromBank` | Weizenfeld, Bauernhof, Bäckerei, Mini-Markt, Wald, Bergwerk, Apfelplantage |
| `IncomePerIndustry` | Molkerei (Kuh), Möbelfabrik (Zahnrad), Markthalle (Weizen) |
| `TakeFromActivePlayer` | Café, Familien-Restaurant |
| `TakeFromEveryone` | Stadion |
| `TakeFromChosenPlayer` | Fernsehsender |
| `SwapEstablishment` | Bürohaus |

Großprojekte sind Typen mit Hooks, die der Zugablauf befragt: `ModifyDiceChoice` (Bahnhof), `ModifyIncome` (Einkaufszentrum), `AfterRoll` (Freizeitpark, Funkturm). Neue Karten einer Erweiterung sind neue Katalogeinträge, meist mit vorhandenen Effekten.

### Zugablauf

`Turn` ist ein Zustandsautomat mit Phasen `AwaitingRoll`, `AwaitingReroll`, `ResolvingIncome`, `AwaitingChoice` (Fernsehsender, Bürohaus), `AwaitingBuild`, `Ended`. Jedes Kommando ist nur in bestimmten Phasen erlaubt. Einkommensreihenfolge nach [spielregeln.md](spielregeln.md): Rot (Zahlungen gegen den Uhrzeigersinn), Blau, Grün, Violett.

## 5. Application

### Slices

```
Application/
  CreateGame/               CreateGameCommand, CreateGameHandler, CreateGameResult
  JoinGame/
  StartGame/
  RollDice/
  RerollDice/
  BuildEstablishment/
  BuildLandmark/
  SkipBuilding/
  ChooseTelevisionTarget/
  ChooseOfficeSwap/
  ReplayGame/               liefert den Ereignisstrom einer Partie für „Nachvollziehen“
  Shared/                   IEventStore, IDiceSeedSource, GameLoader
```

Jeder Handler läuft in vier Schritten: Ereignisse laden, `Game` aufbauen, Kommando ausführen, neue Ereignisse anhängen und zurückgeben. Handler referenzieren nur `Shared` und `Domain`, nie andere Slices.

### Ereignisspeicher

`IEventStore` mit `LoadAsync(gameId)` und `AppendAsync(gameId, expectedVersion, events)`. Die erwartete Version verhindert, dass zwei gleichzeitige Kommandos derselben Partie sich überschreiben. Die Implementierung liegt im Server.

### Lobby

`CreateGame` vergibt einen sechsstelligen Spielcode und schreibt `GameCreated`. `JoinGame` prüft Kapazität (2 bis 4) und Namensdopplung, schreibt `PlayerJoined`. `StartGame` darf nur der Ersteller ab zwei Spielern auslösen; hier wird der Seed gezogen und in `GameStarted` festgeschrieben.

### Lesen

Keine eigene Lesedatenbank. Beim Verbinden erhält ein Client alle bisherigen Ereignisse seiner Partie und baut seine Anzeige daraus auf. Für Basisspiel-Partien sind das einige hundert Ereignisse.

## 6. Server und Protokoll

- **Transport:** SignalR über ASP.NET Core. Ein Hub `GameHub`, eine Gruppe pro Spielcode. Gründe: Server-Push an alle Teilnehmer, Wiederverbindung, fertiger .NET-Client.
- **Nachrichten (`Contracts`):** Client → Server ein Record pro Kommando (z. B. `RollDiceMessage` mit `GameCode`, `PlayerId`, `DiceCount`). Server → Client ein Record pro Ereignis sowie `CommandRejected` (Fehlercode plus Parameter, kein fertiger Text) und `ServerError`.
- **Identität:** Kein Login. Beim Beitritt erhält der Spieler eine `PlayerId` (GUID), die der Client lokal speichert und bei Wiederverbindung mitschickt. Der Server prüft Zugehörigkeit zur Partie und Zugrecht bei jedem Kommando.
- **Persistenz:** Ordner `games/` neben der Server-Anwendung, eine Datei `<GameCode>.jsonl` pro Partie, ein Ereignis pro Zeile mit Typname, Version und Nutzdaten. Beendete Partien bleiben als Archiv. Partien werden erst beim ersten Zugriff geladen.
- **Nebenläufigkeit:** Kommandos an dieselbe Partie werden sequenziell verarbeitet (eine Warteschlange pro Spielcode). Zusammen mit der erwarteten Version im Speicher ist doppelte Verarbeitung ausgeschlossen.

## 7. Client

- **Struktur:** Schleife aus Ereignis empfangen, `GameView` aktualisieren, in eigener Zugphase Eingabe erfragen, Kommando senden. `GameView` (Städte, Münzen, Auslage, aktueller Zug) entsteht ausschließlich aus Ereignis-Nachrichten. Regeln werden nicht nachgerechnet.
- **Lokalisierung:** `IStringLocalizer` mit `Texts.de.resx`, später `Texts.en.resx`. Kartennamen und Fehlercodes werden im Client übersetzt; die Domäne kennt nur Bezeichner wie `WheatField`. Sprache per Kommandozeilenoption, Standard Deutsch. Ein Test stellt sicher, dass jede Sprache alle Schlüssel hat.
- **Eingabe:** Textbefehle mit Nummern: `w 2` (würfeln mit 2 Würfeln), `b 7` (bauen, Karte 7 der Auslage), `g` (Großprojekt), `p` (passen). Vor jeder Eingabe zeigt der Client die in der aktuellen Phase erlaubten Befehle.

## 8. Fehlerbehandlung

| Klasse | Beispiel | Behandlung |
|---|---|---|
| Regelverstoß | Bauen ohne Deckung, falsche Phase, nicht an der Reihe | Kein Ausnahmefall. `Game` gibt `Rejection` mit Fehlercode zurück (`NotYourTurn`, `InsufficientCoins`, `PurpleAlreadyOwned`). Server sendet `CommandRejected` nur an den Absender. Kein Ereignis wird geschrieben. |
| Ungültige Eingabe an der Grenze | Unbekannter Spielcode, `DiceCount` 7, leerer Name | Validierung im Server vor dem Handler. Antwort `CommandRejected`. |
| Technischer Fehler | Datei nicht schreibbar, Versionskonflikt, beschädigte Zeile | Ausnahme. Versionskonflikt: Kommando einmal auf dem aktuellen Stand wiederholen. Sonst protokollieren, Absender erhält `ServerError` ohne Details. Partie bleibt konsistent, weil Ereignisse erst nach erfolgreicher Regelprüfung angehängt werden. |

Verbindungsabbruch: Der Client verbindet sich mit derselben `PlayerId` neu und erhält den gesamten Ereignisstrom. Ist er an der Reihe, bleibt die Phase offen, bis er zurück ist. Kein Zeitlimit im ersten Umfang.

Ein regelwidriger Zustand ist konstruktiv ausgeschlossen: Zustand entsteht nur aus Ereignissen, Ereignisse nur nach bestandener Regelprüfung. Die einzige verbleibende Fehlerquelle ist `Apply`; deshalb prüft ein Test pro Ereignis, dass Anwenden und erneutes Laden aus dem Strom denselben Zustand ergeben.

## 9. Tests

| Ebene | Projekt | Inhalt | Werkzeug |
|---|---|---|---|
| Domäne | `Domain.Tests` | Jede Regel aus `spielregeln.md`: jede Karte, jedes Großprojekt, Einkommensreihenfolge, Zahlungsausfall, Siegbedingung. `ScriptedDice`. | xUnit v3, Shouldly |
| Application | `Application.Tests` | Pro Slice ein Handler-Test mit `InMemoryEventStore`. Versionskonflikt, Lobby-Grenzen. | xUnit v3 |
| Server | `Server.Tests` | Hub mit `WebApplicationFactory`: zwei Clients, Kommandos, Ereignisverteilung, Wiederverbindung. Dateispeicher inkl. beschädigter Zeile. | xUnit v3, `Microsoft.AspNetCore.Mvc.Testing` |
| Client | `Client.Tests` | `GameView` aus Ereignisfolgen, Eingabe-Parser, Vollständigkeit der Ressourcen je Sprache. | xUnit v3 |
| Ende-zu-Ende | `Server.Tests` | Komplette Partie aus aufgezeichneter Kommandofolge mit festem Seed bis `GameWon`. Regressionsanker für Abspielbarkeit gespeicherter Partien. | xUnit v3 |
| Architektur | `ArchitectureTests` | Regeln unten. | ArchUnitNET |

Architekturregeln als Tests:

1. `Domain` referenziert keine Projekt-Assembly und keine Pakete.
2. `Application` referenziert nur `Domain`.
3. `Client` referenziert nur `Contracts`.
4. `Contracts` referenziert nichts.
5. Typen in `Contracts` haben keine Methoden außer Konstruktor und Properties.
6. Jeder Slice-Ordner enthält genau einen Handler; Handler referenzieren keine Typen anderer Slice-Ordner.
7. Alle Ereignis-Typen in `Domain` sind `sealed record`.
8. Kein Typ außer `Game` ruft `Apply` auf.

## 10. Erweiterbarkeit

- **Neue Karten (Großstadt, Hafen):** Kartenkatalog erweitern, bei Bedarf ein neuer `IncomeEffect` oder Großprojekt-Hook. Bestehende Karten und Handler bleiben unberührt.
- **Variante „Komme, was wolle“:** `Supply` bekommt neben `FullSupply` eine zweite Strategie `DrawPileSupply` (Nachziehstapel, 10-Sorten-Regel). Der Modus steht in `GameStarted`, alte Partien bleiben abspielbar.
- **Andere Clients:** Eine Web- oder Desktop-UI referenziert `Contracts` und spricht denselben Hub. Server unverändert.
- **Ereignisversionierung:** Versionsnummer wird von Anfang an geschrieben. Upcaster im Server kommen erst, wenn ein Ereignis-Typ sich tatsächlich ändert.

Bewusst nicht vorgesehen: Computergegner, Login, Datenbank, Zeitlimits, Zuschauer, Chat. Alles davon wäre eine Ergänzung an benannter Stelle (neue Slices, neue Nachrichten), keine Umstrukturierung.

## 11. Umsetzungsreihenfolge (Vorschlag für die Planung)

1. Domäne: Werte-Objekte, Kartenkatalog, `Game` mit Ereignissen, alle Regeln testgetrieben.
2. Application: Slices mit `InMemoryEventStore`, `ReplayGame`.
3. Contracts und Server: Hub, Dateispeicher, Lobby, Ende-zu-Ende-Test.
4. Client: `GameView`, Eingabe, Lokalisierung Deutsch.
5. Architekturtests begleitend ab Schritt 2, sobald zwei Projekte existieren.

Jeder Schritt endet mit grünem Quality Gate. Die Detailplanung erfolgt gesondert.
