# Preview Volume

> Suwak głośności nad podglądem Spotify na pasku zadań Windows.

[Español](README.md) · [English](README.en.md) · [Português](README.pt.md) · [Français](README.fr.md) · [Deutsch](README.de.md) · [Italiano](README.it.md) · **Polski** · [Русский](README.ru.md) · [한국어](README.ko.md) · [日本語](README.ja.md)

**Web:** https://divolandialabs.github.io/preview-volume/ · **Pobierz:** [PreviewVolume-Setup.exe (najnowsza wersja)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![Pasek głośności nad podglądem](docs/screenshots/preview-volume.png)

## Opis

Gdy najedziesz myszą na ikonę Spotify na pasku zadań, Windows pokazuje podgląd z przyciskami „poprzedni”, „pauza” i „następny” — ale nie da się tam zmienić głośności. Preview Volume dodaje tuż nad tym podglądem mały pasek z suwakiem, wartością procentową i przyciskiem wyciszenia.

Windows pozwala umieszczać przyciski w podglądzie tylko samej aplikacji, a Spotify już z nich korzysta. Dlatego Preview Volume rysuje własne małe okienko przyklejone do podglądu i zmienia głośność Spotify w **mikserze głośności Windows**, a nie suwak wewnątrz Spotify (oba działają razem).

To malutki program (ok. 40 KB), który siedzi obok zegara, nigdy nie łączy się z internetem i prawie nie zużywa zasobów.

## Funkcje

- **Suwak głośności** nad podglądem, tej samej szerokości.
- **Kółko myszy** nad podglądem lub paskiem: głośniej lub ciszej co 2 %.
- **Kliknięcie głośnika** wycisza lub przywraca dźwięk.
- Sam się chowa, gdy odsuniesz mysz, i nie znika, gdy przechodzisz z podglądu na pasek.
- Opcja **„Pokazuj dla wszystkich aplikacji z dźwiękiem”**: Chrome, Brave, odtwarzacze… (domyślnie tylko Spotify).
- Ikona w zasobniku systemowym z opcjonalnym **uruchamianiem z Windows**.
- Działa z wieloma monitorami i każdym skalowaniem ekranu (100 %, 150 %, 200 %…).

![Pasek głośności](docs/screenshots/barra.png)
![Wyciszony](docs/screenshots/barra-silencio.png)

## Instalacja

1. Pobierz **`PreviewVolume-Setup.exe`** ze [strony wydań](https://github.com/DivolandiaLabs/preview-volume/releases/latest).
2. Uruchom go i kliknij **Instaluj**. Instalator otwiera się w języku Windows, a język można zmienić w lewym dolnym rogu (10 języków). Instaluje się tylko dla Twojego użytkownika, w `%LOCALAPPDATA%\Programs\Preview Volume`, bez uprawnień administratora.
3. Wybierz, czy ma się uruchamiać z Windows i czy chcesz skrót na pulpicie.

![Instalator](docs/screenshots/instalador.png)

Program nie jest podpisany cyfrowo, więc przy pierwszym uruchomieniu Windows SmartScreen może wyświetlić ostrzeżenie: kliknij **Więcej informacji → Uruchom mimo to**.

Odinstalowanie: **Ustawienia → Aplikacje → Zainstalowane aplikacje → Preview Volume → Odinstaluj**.

Instalacja cicha: `PreviewVolume-Setup.exe /silencioso` (opcje: `/dir "folder"`, `/escritorio`, `/sininicio`, `/abrir`, `/idioma pl`).

## Użycie

- Włącz muzykę w Spotify i najedź na jego ikonę na pasku zadań: nad podglądem pojawi się pasek głośności.
- Przeciągnij suwak, użyj kółka myszy albo kliknij głośnik, aby wyciszyć.
- Kliknij prawym przyciskiem zieloną ikonę obok zegara, aby otworzyć menu: wszystkie aplikacje z dźwiękiem, uruchamianie z Windows i zamknięcie.
- Zamknięcie z wiersza poleceń: `PreviewVolume.exe /salir`.

## Wymagania

- Windows 10 lub 11 z .NET Framework 4.8 (jest już w systemie).
- Przetestowano w Windows 11 z klasycznym paskiem zadań (w stylu Windows 10, np. z ExplorerPatcher). Podgląd nowego paska Windows 11 może działać inaczej.
- Instalator jest dostępny w 10 językach; pasek i menu programu są po hiszpańsku.

## Kompilacja ze źródeł

Nie trzeba nic instalować: kompiluje się kompilatorem C# dołączonym do Windows.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

Powstają `bin\PreviewVolume.exe` i instalator `bin\PreviewVolume-Setup.exe`. Ikonę rysuje sam program.

## Prywatność

Preview Volume nigdy nie łączy się z internetem i nie zbiera żadnych danych. Jedyne ustawienie zapisuje w pliku `config.ini` obok programu.

## Licencja

© 2026 Divolandia Labs. Wszelkie prawa zastrzeżone. Program jest darmowy; kod źródłowy udostępniono do wglądu.

---
*Stworzone przez [DivolandiaLabs](https://divolandialabs.github.io/)*
