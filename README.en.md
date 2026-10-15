# Preview Volume

> A volume bar on top of Spotify's taskbar preview in Windows.

[Español](README.md) · **English** · [Português](README.pt.md) · [Français](README.fr.md) · [Deutsch](README.de.md) · [Italiano](README.it.md) · [Polski](README.pl.md) · [Русский](README.ru.md) · [한국어](README.ko.md) · [日本語](README.ja.md)

**Download:** [PreviewVolume-Setup.exe (latest release)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![The volume bar on top of the preview](docs/screenshots/preview-volume.png)

## Description

When you hover over the Spotify icon on the taskbar, Windows shows a preview with previous, pause and next buttons — but there is no way to change the volume. Preview Volume adds a small bar right above that preview, with a slider, the percentage and a mute button.

Windows only lets an app put buttons on its own preview, and Spotify already uses them. So Preview Volume draws its own little window attached to the preview and changes Spotify's volume in the **Windows volume mixer**, not the slider inside Spotify (the two add up).

It is a tiny program (about 40 KB) that sits next to the clock, never goes online and barely uses any resources.

## Features

- **Volume slider** above the preview, matching its width.
- **Mouse wheel** over the preview or the bar: volume up or down in 2 % steps.
- **Click the speaker** to mute or unmute.
- Hides by itself when you move the mouse away, and stays put while you move from the preview to the bar.
- **"Show for all apps with sound"** option: Chrome, Brave, media players… (Spotify only by default).
- System tray icon with optional **start with Windows**.
- Works with multiple monitors and any display scaling (100 %, 150 %, 200 %…).

![Volume bar](docs/screenshots/barra.png)
![Muted](docs/screenshots/barra-silencio.png)

## Installation

1. Download **`PreviewVolume-Setup.exe`** from the [releases page](https://github.com/DivolandiaLabs/preview-volume/releases/latest).
2. Run it and click **Instalar** (Install). It installs for your user only, in `%LOCALAPPDATA%\Programs\Preview Volume`, with no administrator rights needed.
3. Choose whether it should start with Windows and whether you want a desktop shortcut.

![Installer](docs/screenshots/instalador.png)

The program is not code-signed, so the first time Windows SmartScreen may warn you: click **More info → Run anyway**.

To uninstall: **Settings → Apps → Installed apps → Preview Volume → Uninstall**.

Silent install: `PreviewVolume-Setup.exe /silencioso` (options: `/dir "folder"`, `/escritorio`, `/sininicio`, `/abrir`).

## Usage

- Play music in Spotify and hover over its taskbar icon: the volume bar appears above the preview.
- Drag the slider, use the mouse wheel, or click the speaker to mute.
- Right-click the green icon next to the clock for the menu: all apps with sound, start with Windows, and exit.
- To close it from the command line: `PreviewVolume.exe /salir`.

## Requirements

- Windows 10 or 11 with .NET Framework 4.8 (already part of Windows).
- Tested on Windows 11 with the classic taskbar (Windows 10 style, e.g. with ExplorerPatcher). The preview of the new Windows 11 taskbar may behave differently.
- The interface is in Spanish.

## Building from source

Nothing to install: it builds with the C# compiler that ships with Windows.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

This produces `bin\PreviewVolume.exe` and the installer `bin\PreviewVolume-Setup.exe`. The icon is drawn by the program itself.

## Privacy

Preview Volume never connects to the internet and collects no data. Its only setting is stored in `config.ini`, next to the program.

## License

© 2026 Divolandia Labs. All rights reserved. The program is free to use; the source code is published for reference.

---
*Developed by [DivolandiaLabs](https://divolandialabs.github.io/)*
