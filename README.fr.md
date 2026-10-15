# Preview Volume

> Une barre de volume au-dessus de l'aperçu de Spotify dans la barre des tâches de Windows.

[Español](README.md) · [English](README.en.md) · [Português](README.pt.md) · **Français** · [Deutsch](README.de.md) · [Italiano](README.it.md) · [Polski](README.pl.md) · [Русский](README.ru.md) · [한국어](README.ko.md) · [日本語](README.ja.md)

**Web:** https://divolandialabs.github.io/preview-volume/ · **Téléchargement :** [PreviewVolume-Setup.exe (dernière version)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![La barre de volume au-dessus de l'aperçu](docs/screenshots/preview-volume.png)

## Description

Quand vous survolez l'icône de Spotify dans la barre des tâches, Windows affiche un aperçu avec les boutons précédent, pause et suivant — mais impossible d'y régler le volume. Preview Volume ajoute juste au-dessus de cet aperçu une petite barre avec un curseur, le pourcentage et un bouton muet.

Windows ne laisse que l'application elle-même placer des boutons sur son aperçu, et Spotify utilise déjà les siens. Preview Volume dessine donc sa propre petite fenêtre collée à l'aperçu et règle le volume de Spotify dans le **mélangeur de volume de Windows**, et non le curseur interne de Spotify (les deux se combinent).

C'est un tout petit programme (environ 40 Ko) qui se loge près de l'horloge, ne se connecte jamais à Internet et ne consomme presque rien.

## Fonctionnalités

- **Curseur de volume** au-dessus de l'aperçu, de la même largeur.
- **Molette de la souris** sur l'aperçu ou sur la barre : volume plus ou moins fort par pas de 2 %.
- **Clic sur le haut-parleur** pour couper ou rétablir le son.
- Se masque toute seule quand la souris s'éloigne, et reste affichée pendant que vous passez de l'aperçu à la barre.
- Option **« Afficher pour toutes les applis avec du son »** : Chrome, Brave, lecteurs… (Spotify uniquement par défaut).
- Icône dans la zone de notification avec **démarrage avec Windows** en option.
- Compatible plusieurs écrans et toutes les mises à l'échelle (100 %, 150 %, 200 %…).

![Barre de volume](docs/screenshots/barra.png)
![Son coupé](docs/screenshots/barra-silencio.png)

## Installation

1. Téléchargez **`PreviewVolume-Setup.exe`** depuis la [page des versions](https://github.com/DivolandiaLabs/preview-volume/releases/latest).
2. Lancez-le et cliquez sur **Installer**. Le programme d'installation s'ouvre dans la langue de Windows, modifiable en bas à gauche (10 langues). L'installation se fait pour votre utilisateur uniquement, dans `%LOCALAPPDATA%\Programs\Preview Volume`, sans droits d'administrateur.
3. Choisissez s'il doit démarrer avec Windows et si vous voulez un raccourci sur le bureau.

![Programme d'installation](docs/screenshots/instalador.png)

Le programme n'est pas signé : la première fois, Windows SmartScreen peut afficher un avertissement. Cliquez sur **Informations complémentaires → Exécuter quand même**.

Pour le désinstaller : **Paramètres → Applications → Applications installées → Preview Volume → Désinstaller**.

Installation silencieuse : `PreviewVolume-Setup.exe /silencioso` (options : `/dir "dossier"`, `/escritorio`, `/sininicio`, `/abrir`, `/idioma fr`).

## Utilisation

- Lancez de la musique dans Spotify et survolez son icône dans la barre des tâches : la barre de volume apparaît au-dessus de l'aperçu.
- Faites glisser le curseur, utilisez la molette ou cliquez sur le haut-parleur pour couper le son.
- Clic droit sur l'icône verte près de l'horloge pour le menu : toutes les applis avec du son, démarrer avec Windows et quitter.
- Pour le fermer en ligne de commande : `PreviewVolume.exe /salir`.

## Configuration requise

- Windows 10 ou 11 avec .NET Framework 4.8 (déjà inclus dans Windows).
- Testé sous Windows 11 avec la barre des tâches classique (style Windows 10, par exemple avec ExplorerPatcher). L'aperçu de la nouvelle barre de Windows 11 peut fonctionner autrement.
- Le programme d'installation existe en 10 langues ; la barre et le menu du programme sont en espagnol.

## Compiler depuis le code

Rien à installer : la compilation utilise le compilateur C# fourni avec Windows.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

Cela produit `bin\PreviewVolume.exe` et le programme d'installation `bin\PreviewVolume-Setup.exe`. L'icône est dessinée par le programme lui-même.

## Confidentialité

Preview Volume ne se connecte jamais à Internet et ne collecte aucune donnée. Son unique réglage est enregistré dans `config.ini`, à côté du programme.

## Licence

© 2026 Divolandia Labs. Tous droits réservés. Le programme est gratuit ; le code source est publié à titre de référence.

---
*Développé par [DivolandiaLabs](https://divolandialabs.github.io/)*
