# Preview Volume

> Windows のタスクバーに出る Spotify のプレビューの上に、音量バーを表示します。

[Español](README.md) · [English](README.en.md) · [Português](README.pt.md) · [Français](README.fr.md) · [Deutsch](README.de.md) · [Italiano](README.it.md) · [Polski](README.pl.md) · [Русский](README.ru.md) · [한국어](README.ko.md) · **日本語**

**Web:** https://divolandialabs.github.io/preview-volume/ · **ダウンロード:** [PreviewVolume-Setup.exe（最新版）](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![プレビューの上の音量バー](docs/screenshots/preview-volume.png)

## 概要

タスクバーの Spotify アイコンにマウスを合わせると、Windows は「前へ」「一時停止」「次へ」のボタン付きプレビューを表示しますが、音量は変えられません。Preview Volume は、そのプレビューのすぐ上にスライダー・パーセント表示・ミュートボタンを備えた小さなバーを追加します。

Windows では、プレビューにボタンを置けるのはそのアプリ自身だけで、Spotify はすでに自分のボタンを使っています。そこで Preview Volume はプレビューに寄り添う小さなウィンドウを自前で描き、Spotify 内部のスライダーではなく **Windows の音量ミキサー**で Spotify の音量を変えます（両方の設定が合わさって効きます）。

時計の横に常駐するとても小さなプログラム（約 40 KB）で、インターネットには一切接続せず、リソースもほとんど使いません。

## 機能

- プレビューと同じ幅の**音量スライダー**。
- プレビューやバーの上で**マウスホイール**：2 % ずつ音量を上げ下げ。
- **スピーカーをクリック**でミュート／解除。
- マウスを離すと自動で消え、プレビューからバーへ移動する間は表示されたままです。
- **「音が出るすべてのアプリで表示」**オプション：Chrome、Brave、各種プレーヤーなど（既定では Spotify のみ）。
- 通知領域のアイコンと、任意の **Windows 起動時に開始**。
- 複数モニターと、あらゆる表示スケール（100 %、150 %、200 % など）に対応。
- **クラシックまたは Windows 11 のタスクバー**：Windows 10 スタイルのタスクバー（または ExplorerPatcher を使った Windows 11）ではプレビューにぴったり付き、新しい Windows 11 のタスクバーではタスクバーのボタンを基準にプレビューの上に表示されます。インストール時かアイコンのメニューで選べます。

![音量バー](docs/screenshots/barra.png)
![ミュート](docs/screenshots/barra-silencio.png)

## インストール

1. [リリースページ](https://github.com/DivolandiaLabs/preview-volume/releases/latest)から **`PreviewVolume-Setup.exe`** をダウンロードします。
2. 起動して **インストール** を押します。インストーラーは Windows の言語で開き、左下で切り替えられます（10 言語）。現在のユーザーだけに `%LOCALAPPDATA%\Programs\Preview Volume` へインストールされ、管理者権限は不要です。
3. Windows と一緒に起動するか、デスクトップにショートカットを作るか、使っている**タスクバー**（Windows 10 または ExplorerPatcher / Windows 11）を選びます。インストーラーが検出したものを提案します。

![インストーラー](docs/screenshots/instalador.png)

プログラムにはデジタル署名がないため、初回は Windows SmartScreen の警告が出ることがあります。**詳細情報 → 実行**を押してください。

アンインストール：**設定 → アプリ → インストールされているアプリ → Preview Volume → アンインストール**。

サイレントインストール：`PreviewVolume-Setup.exe /silencioso`（オプション：`/dir "フォルダー"`、`/escritorio`、`/sininicio`、`/abrir`、`/idioma ja`、`/barra10`、`/barra11`）。

## 使い方

- Spotify で音楽を再生し、タスクバーのアイコンにマウスを合わせると、プレビューの上に音量バーが現れます。
- スライダーをドラッグするか、マウスホイールを回すか、スピーカーをクリックしてミュートします。
- 時計の横の緑のアイコンを右クリックするとメニューが開きます：音が出るすべてのアプリ、Windows 起動時に開始、終了。
- コマンドラインから終了：`PreviewVolume.exe /salir`。

## 動作環境

- .NET Framework 4.8 を含む Windows 10 または 11（Windows に標準搭載）。
- クラシックなタスクバー（Windows 10、または ExplorerPatcher を使った Windows 11）と新しい Windows 11 のタスクバーの両方で動作します。新しいタスクバーではプレビューを読み取れないため位置はおおよそで、クラシックでは正確に付きます。
- インストーラーは 10 言語に対応。音量バーとプログラムのメニューはスペイン語です。

## ソースからのビルド

追加のインストールは不要です。Windows に含まれる C# コンパイラーでビルドできます。

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

`bin\PreviewVolume.exe` とインストーラー `bin\PreviewVolume-Setup.exe` が作られます。アイコンはプログラム自身が描画します。

## プライバシー

Preview Volume はインターネットに接続せず、データも一切収集しません。唯一の設定は、プログラムと同じ場所の `config.ini` に保存されます。

## ライセンス

© 2026 Divolandia Labs. All rights reserved. プログラムは無料で使えます。ソースコードは参照用として公開しています。

---
*開発：[DivolandiaLabs](https://divolandialabs.github.io/)*
