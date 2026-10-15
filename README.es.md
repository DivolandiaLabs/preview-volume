# Preview Volume

> Una barra de volumen encima de la vista previa de Spotify en la barra de tareas de Windows.

**Español** · [English](README.en.md) · [Português](README.pt.md) · [Français](README.fr.md) · [Deutsch](README.de.md) · [Italiano](README.it.md) · [Polski](README.pl.md) · [Русский](README.ru.md) · [한국어](README.ko.md) · [日本語](README.ja.md)

**Web:** https://divolandialabs.github.io/preview-volume/ · **Descarga:** [PreviewVolume-Setup.exe (última versión)](https://github.com/DivolandiaLabs/preview-volume/releases/latest)

![La barra de volumen encima de la vista previa](docs/screenshots/preview-volume.png)

## Descripción

Al pasar el ratón por el icono de Spotify en la barra de tareas, Windows muestra una vista previa con los botones de anterior, pausa y siguiente, pero no hay forma de cambiar el volumen. Preview Volume añade justo encima de esa vista previa una pequeña barra con un deslizador, el porcentaje y un botón de silencio.

Windows solo deja poner botones en la vista previa a la propia aplicación, y Spotify ya pone los suyos. Por eso Preview Volume dibuja su propia ventanita pegada a la vista previa y cambia el volumen de Spotify en el **mezclador de volumen de Windows**, no el deslizador que Spotify tiene por dentro (los dos se suman).

Es un programa pequeño (unos 40 KB) que se queda junto al reloj, no se conecta a internet y apenas gasta recursos.

## Características

- **Deslizador de volumen** encima de la vista previa, del mismo ancho que ella.
- **Rueda del ratón** sobre la vista previa o sobre la barra: sube o baja el volumen de 2 en 2.
- **Clic en el altavoz** para silenciar o quitar el silencio.
- Se oculta sola al apartar el ratón y no desaparece mientras vas de la vista previa a la barra.
- Opción **«Mostrar en todas las apps con sonido»**: Chrome, Brave, reproductores… (por defecto, solo Spotify).
- Icono en la bandeja del sistema con **inicio con Windows** opcional.
- Funciona con varios monitores y con cualquier escala de pantalla (100 %, 150 %, 200 %…).

![Barra de volumen](docs/screenshots/barra.png)
![Barra silenciada](docs/screenshots/barra-silencio.png)

## Instalación

1. Descarga **`PreviewVolume-Setup.exe`** de la [página de versiones](https://github.com/DivolandiaLabs/preview-volume/releases/latest).
2. Ábrelo y pulsa **Instalar**. El instalador aparece en el idioma de Windows y se puede cambiar abajo a la izquierda (10 idiomas). Se instala solo para tu usuario, en `%LOCALAPPDATA%\Programs\Preview Volume`, sin pedir permisos de administrador.
3. Elige si quieres que arranque con Windows y si quieres un acceso en el escritorio.

![Instalador](docs/screenshots/instalador.png)

El programa no está firmado, así que la primera vez Windows SmartScreen puede avisar: pulsa **Más información → Ejecutar de todas formas**.

Para desinstalarlo: **Configuración → Aplicaciones → Aplicaciones instaladas → Preview Volume → Desinstalar**.

Instalación silenciosa: `PreviewVolume-Setup.exe /silencioso` (opciones: `/dir "carpeta"`, `/escritorio`, `/sininicio`, `/abrir`, `/idioma en`).

## Uso

- Pon música en Spotify y pasa el ratón por su icono de la barra de tareas: encima de la vista previa aparece la barra de volumen.
- Arrastra el deslizador, usa la rueda del ratón o haz clic en el altavoz para silenciar.
- Clic derecho en el icono verde junto al reloj para el menú: todas las apps con sonido, iniciar con Windows y salir.
- Para cerrarlo desde la línea de comandos: `PreviewVolume.exe /salir`.

## Requisitos

- Windows 10 u 11 con .NET Framework 4.8 (ya viene con Windows).
- Probado en Windows 11 con la barra de tareas clásica (estilo Windows 10, por ejemplo con ExplorerPatcher). La vista previa de la barra nueva de Windows 11 puede funcionar de otra forma.
- El instalador está en 10 idiomas; la barra y el menú del programa, en español.

## Compilar desde el código

No hace falta instalar nada: se compila con el compilador de C# que trae Windows.

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

Genera `bin\PreviewVolume.exe` y el instalador `bin\PreviewVolume-Setup.exe`. El icono lo dibuja el propio programa.

## Privacidad

Preview Volume no se conecta a internet ni recoge ningún dato. Su único ajuste se guarda en `config.ini`, junto al programa.

## Licencia

© 2026 Divolandia Labs. Todos los derechos reservados. Puedes usar el programa gratis; el código se publica para consulta.

---
*Desarrollado por [DivolandiaLabs](https://divolandialabs.github.io/)*
