# Preview Volume

> Windows 작업 표시줄의 Spotify 미리 보기 위에 볼륨 바를 띄워 줍니다.

[Español](README.md) · [English](README.en.md) · [Português](README.pt.md) · [Français](README.fr.md) · [Deutsch](README.de.md) · [Italiano](README.it.md) · [Polski](README.pl.md) · [Русский](README.ru.md) · **한국어** · [日本語](README.ja.md)

**Web:** https://divolandialabs.github.io/preview-volume/ · **다운로드:** [PreviewVolume-Setup.exe (최신 버전)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![미리 보기 위의 볼륨 바](docs/screenshots/preview-volume.png)

## 설명

작업 표시줄의 Spotify 아이콘에 마우스를 올리면 Windows가 이전 곡, 일시 정지, 다음 곡 버튼이 있는 미리 보기를 보여 주지만, 볼륨은 바꿀 수 없습니다. Preview Volume은 그 미리 보기 바로 위에 슬라이더, 퍼센트, 음소거 버튼이 있는 작은 바를 추가합니다.

Windows는 앱 자신만 미리 보기에 버튼을 넣을 수 있게 하는데, Spotify는 이미 그 자리를 쓰고 있습니다. 그래서 Preview Volume은 미리 보기에 붙은 작은 창을 직접 그리고, Spotify 안의 슬라이더가 아니라 **Windows 볼륨 믹서**에서 Spotify의 볼륨을 조절합니다(두 값이 함께 적용됩니다).

시계 옆에 머무는 아주 작은 프로그램(약 40KB)으로, 인터넷에 연결하지 않으며 자원도 거의 쓰지 않습니다.

## 기능

- 미리 보기와 같은 너비의 **볼륨 슬라이더**.
- 미리 보기나 바 위에서 **마우스 휠**: 2%씩 볼륨을 올리거나 내립니다.
- **스피커 클릭**으로 음소거 및 해제.
- 마우스를 치우면 저절로 사라지고, 미리 보기에서 바로 옮겨 가는 동안에는 그대로 남아 있습니다.
- **「소리가 나는 모든 앱에 표시」** 옵션: Chrome, Brave, 플레이어 등(기본값은 Spotify만).
- 시스템 트레이 아이콘과 선택 가능한 **Windows 시작 시 실행**.
- 여러 모니터와 모든 화면 배율(100%, 150%, 200% 등)에서 동작합니다.

![볼륨 바](docs/screenshots/barra.png)
![음소거](docs/screenshots/barra-silencio.png)

## 설치

1. [릴리스 페이지](https://github.com/DivolandiaLabs/preview-volume/releases/latest)에서 **`PreviewVolume-Setup.exe`**를 내려받습니다.
2. 실행한 뒤 **설치**를 누릅니다. 설치 프로그램은 Windows 언어로 열리며 왼쪽 아래에서 바꿀 수 있습니다(10개 언어). 현재 사용자에게만 `%LOCALAPPDATA%\Programs\Preview Volume`에 설치되며 관리자 권한이 필요 없습니다.
3. Windows와 함께 시작할지, 바탕 화면 바로 가기를 만들지 선택합니다.

![설치 프로그램](docs/screenshots/instalador.png)

프로그램에 디지털 서명이 없어서 처음 실행할 때 Windows SmartScreen 경고가 나올 수 있습니다. **추가 정보 → 실행**을 누르세요.

제거: **설정 → 앱 → 설치된 앱 → Preview Volume → 제거**.

자동 설치: `PreviewVolume-Setup.exe /silencioso` (옵션: `/dir "폴더"`, `/escritorio`, `/sininicio`, `/abrir`, `/idioma ko`).

## 사용법

- Spotify에서 음악을 재생하고 작업 표시줄 아이콘에 마우스를 올리면 미리 보기 위에 볼륨 바가 나타납니다.
- 슬라이더를 끌거나, 마우스 휠을 돌리거나, 스피커를 눌러 음소거하세요.
- 시계 옆 초록색 아이콘을 마우스 오른쪽 버튼으로 누르면 메뉴가 열립니다: 소리가 나는 모든 앱, Windows 시작 시 실행, 종료.
- 명령줄에서 종료: `PreviewVolume.exe /salir`.

## 요구 사항

- .NET Framework 4.8이 포함된 Windows 10 또는 11(Windows에 기본 포함).
- Windows 11의 클래식 작업 표시줄(Windows 10 스타일, 예: ExplorerPatcher 사용)에서 테스트했습니다. 새 Windows 11 작업 표시줄의 미리 보기는 다르게 동작할 수 있습니다.
- 설치 프로그램은 10개 언어를 지원하며, 볼륨 바와 프로그램 메뉴는 스페인어입니다.

## 소스에서 빌드

따로 설치할 것이 없습니다. Windows에 들어 있는 C# 컴파일러로 빌드합니다.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

`bin\PreviewVolume.exe`와 설치 프로그램 `bin\PreviewVolume-Setup.exe`가 만들어집니다. 아이콘은 프로그램이 직접 그립니다.

## 개인정보

Preview Volume은 인터넷에 연결하지 않으며 어떤 데이터도 수집하지 않습니다. 유일한 설정은 프로그램 옆의 `config.ini`에 저장됩니다.

## 라이선스

© 2026 Divolandia Labs. 모든 권리 보유. 프로그램은 무료로 사용할 수 있으며, 소스 코드는 참고용으로 공개합니다.

---
*개발: [DivolandiaLabs](https://divolandialabs.github.io/)*
