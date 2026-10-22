# Preview Volume

> Una barra del volume sopra l'anteprima di Spotify nella barra delle applicazioni di Windows.

[Español](README.md) · [English](README.en.md) · [Português](README.pt.md) · [Français](README.fr.md) · [Deutsch](README.de.md) · **Italiano** · [Polski](README.pl.md) · [Русский](README.ru.md) · [한국어](README.ko.md) · [日本語](README.ja.md)

**Web:** https://divolandialabs.github.io/preview-volume/ · **Download:** [PreviewVolume-Setup.exe (ultima versione)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![La barra del volume sopra l'anteprima](docs/screenshots/preview-volume.png)

## Descrizione

Quando passi il mouse sull'icona di Spotify nella barra delle applicazioni, Windows mostra un'anteprima con i pulsanti precedente, pausa e successivo, ma non c'è modo di cambiare il volume. Preview Volume aggiunge proprio sopra quell'anteprima una piccola barra con un cursore, la percentuale e un pulsante per disattivare l'audio.

Windows permette solo all'app stessa di mettere pulsanti nella propria anteprima, e Spotify usa già i suoi. Per questo Preview Volume disegna una sua piccola finestra attaccata all'anteprima e cambia il volume di Spotify nel **mixer volume di Windows**, non il cursore interno di Spotify (i due si sommano).

È un programma minuscolo (circa 40 KB) che resta accanto all'orologio, non si collega mai a Internet e consuma pochissime risorse.

## Caratteristiche

- **Cursore del volume** sopra l'anteprima, della stessa larghezza.
- **Rotellina del mouse** sull'anteprima o sulla barra: alza o abbassa il volume del 2 % alla volta.
- **Clic sull'altoparlante** per disattivare o riattivare l'audio.
- Si nasconde da sola quando allontani il mouse e resta visibile mentre passi dall'anteprima alla barra.
- Opzione **«Mostra per tutte le app con audio»**: Chrome, Brave, lettori… (per impostazione predefinita solo Spotify).
- Icona nell'area di notifica con **avvio con Windows** facoltativo.
- Funziona con più monitor e con qualsiasi ridimensionamento dello schermo (100 %, 150 %, 200 %…).
- **Barra classica o di Windows 11**: con la barra in stile Windows 10 (o Windows 11 con ExplorerPatcher) la barra si attacca all'anteprima; con la nuova barra di Windows 11 viene posizionata sopra partendo dal pulsante della barra delle applicazioni. Si sceglie durante l'installazione o dal menu dell'icona.

![Barra del volume](docs/screenshots/barra.png)
![Audio disattivato](docs/screenshots/barra-silencio.png)

## Installazione

1. Scarica **`PreviewVolume-Setup.exe`** dalla [pagina delle versioni](https://github.com/DivolandiaLabs/preview-volume/releases/latest).
2. Aprilo e premi **Installa**. Il programma di installazione si apre nella lingua di Windows e si può cambiare in basso a sinistra (10 lingue). Si installa solo per il tuo utente, in `%LOCALAPPDATA%\Programs\Preview Volume`, senza permessi di amministratore.
3. Scegli se avviarlo con Windows, se vuoi un collegamento sul desktop e quale **barra delle applicazioni** usi: Windows 10 (o ExplorerPatcher) o Windows 11. Il programma di installazione propone quella che rileva.

![Programma di installazione](docs/screenshots/instalador.png)

Il programma non è firmato, quindi la prima volta Windows SmartScreen potrebbe avvisarti: fai clic su **Ulteriori informazioni → Esegui comunque**.

Per disinstallarlo: **Impostazioni → App → App installate → Preview Volume → Disinstalla**.

Installazione silenziosa: `PreviewVolume-Setup.exe /silencioso` (opzioni: `/dir "cartella"`, `/escritorio`, `/sininicio`, `/abrir`, `/idioma it`, `/barra10`, `/barra11`).

## Utilizzo

- Riproduci musica su Spotify e passa il mouse sulla sua icona nella barra delle applicazioni: sopra l'anteprima compare la barra del volume.
- Trascina il cursore, usa la rotellina o fai clic sull'altoparlante per disattivare l'audio.
- Clic destro sull'icona verde accanto all'orologio per il menu: tutte le app con audio, avvio con Windows ed esci.
- Per chiuderlo dalla riga di comando: `PreviewVolume.exe /salir`.

## Requisiti

- Windows 10 o 11 con .NET Framework 4.8 (già incluso in Windows).
- Funziona con la barra classica (Windows 10, o Windows 11 con ExplorerPatcher) e con la nuova barra di Windows 11. Nella nuova Windows non permette di leggere l'anteprima, quindi la posizione della barra è approssimativa; con quella classica è attaccata esattamente.
- Il programma di installazione è in 10 lingue; la barra e il menu del programma sono in spagnolo.

## Compilare dal codice

Non serve installare nulla: si compila con il compilatore C# incluso in Windows.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

Crea `bin\PreviewVolume.exe` e il programma di installazione `bin\PreviewVolume-Setup.exe`. L'icona la disegna il programma stesso.

## Privacy

Preview Volume non si collega mai a Internet e non raccoglie alcun dato. La sua unica impostazione è salvata in `config.ini`, accanto al programma.

## Licenza

© 2026 Divolandia Labs. Tutti i diritti riservati. Il programma è gratuito; il codice sorgente è pubblicato a scopo di consultazione.

---
*Sviluppato da [DivolandiaLabs](https://divolandialabs.github.io/)*
