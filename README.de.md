# Preview Volume

> Ein Lautstärkeregler über der Spotify-Vorschau in der Windows-Taskleiste.

[Español](README.md) · [English](README.en.md) · [Português](README.pt.md) · [Français](README.fr.md) · **Deutsch** · [Italiano](README.it.md) · [Polski](README.pl.md) · [Русский](README.ru.md) · [한국어](README.ko.md) · [日本語](README.ja.md)

**Download:** [PreviewVolume-Setup.exe (neueste Version)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![Der Lautstärkeregler über der Vorschau](docs/screenshots/preview-volume.png)

## Beschreibung

Wenn Sie mit der Maus über das Spotify-Symbol in der Taskleiste fahren, zeigt Windows eine Vorschau mit den Tasten Zurück, Pause und Weiter – die Lautstärke lässt sich dort aber nicht ändern. Preview Volume blendet direkt über dieser Vorschau eine kleine Leiste mit Schieberegler, Prozentanzeige und Stummschalter ein.

Windows erlaubt nur der App selbst, Schaltflächen in ihre Vorschau zu setzen, und Spotify belegt diese bereits. Deshalb zeichnet Preview Volume ein eigenes kleines Fenster direkt an der Vorschau und ändert die Lautstärke von Spotify im **Windows-Lautstärkemixer** – nicht den Regler in Spotify selbst (beide wirken zusammen).

Ein winziges Programm (rund 40 KB), das neben der Uhr sitzt, nie ins Internet geht und kaum Ressourcen braucht.

## Funktionen

- **Lautstärkeregler** über der Vorschau, genauso breit wie sie.
- **Mausrad** über der Vorschau oder der Leiste: Lautstärke in 2-%-Schritten lauter oder leiser.
- **Klick auf den Lautsprecher** schaltet stumm bzw. wieder laut.
- Verschwindet von selbst, wenn die Maus weggeht, und bleibt stehen, während Sie von der Vorschau zur Leiste fahren.
- Option **„Für alle Apps mit Ton anzeigen“**: Chrome, Brave, Mediaplayer … (standardmäßig nur Spotify).
- Symbol im Infobereich mit optionalem **Start mit Windows**.
- Funktioniert mit mehreren Monitoren und jeder Anzeigeskalierung (100 %, 150 %, 200 % …).

![Lautstärkeleiste](docs/screenshots/barra.png)
![Stummgeschaltet](docs/screenshots/barra-silencio.png)

## Installation

1. Laden Sie **`PreviewVolume-Setup.exe`** von der [Release-Seite](https://github.com/DivolandiaLabs/preview-volume/releases/latest) herunter.
2. Starten Sie die Datei und klicken Sie auf **Instalar** (Installieren). Installiert wird nur für Ihr Benutzerkonto, in `%LOCALAPPDATA%\Programs\Preview Volume`, ohne Administratorrechte.
3. Wählen Sie, ob das Programm mit Windows starten soll und ob Sie eine Desktopverknüpfung möchten.

![Installationsprogramm](docs/screenshots/instalador.png)

Das Programm ist nicht signiert, daher kann Windows SmartScreen beim ersten Mal warnen: Klicken Sie auf **Weitere Informationen → Trotzdem ausführen**.

Deinstallieren: **Einstellungen → Apps → Installierte Apps → Preview Volume → Deinstallieren**.

Stille Installation: `PreviewVolume-Setup.exe /silencioso` (Optionen: `/dir "Ordner"`, `/escritorio`, `/sininicio`, `/abrir`).

## Verwendung

- Spielen Sie Musik in Spotify ab und fahren Sie über das Symbol in der Taskleiste: Über der Vorschau erscheint der Lautstärkeregler.
- Ziehen Sie den Regler, nutzen Sie das Mausrad oder klicken Sie auf den Lautsprecher zum Stummschalten.
- Rechtsklick auf das grüne Symbol neben der Uhr öffnet das Menü: alle Apps mit Ton, mit Windows starten und beenden.
- Beenden über die Kommandozeile: `PreviewVolume.exe /salir`.

## Voraussetzungen

- Windows 10 oder 11 mit .NET Framework 4.8 (bereits in Windows enthalten).
- Getestet unter Windows 11 mit der klassischen Taskleiste (im Stil von Windows 10, z. B. mit ExplorerPatcher). Die Vorschau der neuen Windows-11-Taskleiste kann sich anders verhalten.
- Die Oberfläche ist auf Spanisch.

## Aus dem Quellcode bauen

Nichts zu installieren: Gebaut wird mit dem C#-Compiler, der in Windows enthalten ist.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

Ergebnis: `bin\PreviewVolume.exe` und das Installationsprogramm `bin\PreviewVolume-Setup.exe`. Das Symbol zeichnet das Programm selbst.

## Datenschutz

Preview Volume verbindet sich nie mit dem Internet und sammelt keine Daten. Die einzige Einstellung liegt in `config.ini` neben dem Programm.

## Lizenz

© 2026 Divolandia Labs. Alle Rechte vorbehalten. Das Programm ist kostenlos nutzbar; der Quellcode wird zur Einsicht veröffentlicht.

---
*Entwickelt von [DivolandiaLabs](https://divolandialabs.github.io/)*
