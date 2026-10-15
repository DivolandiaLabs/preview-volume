# Compila bin\PreviewVolume.exe y el instalador bin\PreviewVolume-Setup.exe con el csc de .NET Framework (viene con Windows).
# El icono lo dibuja el propio programa: 1) copia temporal sin icono  2) escribe app.ico  3) version final.
$ErrorActionPreference = 'Stop'
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$common = @('/nologo', '/target:winexe', '/optimize+', '/platform:anycpu', '/codepage:65001',
            "/win32manifest:$here\app.manifest", '/r:System.Drawing.dll', '/r:System.Windows.Forms.dll', '/r:Accessibility.dll')

$tmp = Join-Path $here 'PreviewVolume_icono.exe'
& $csc @common "/out:$tmp" "$here\PreviewVolume.cs"
if ($LASTEXITCODE -ne 0) { throw "Error al compilar" }
Start-Process $tmp -ArgumentList '/icono', "`"$here\app.ico`"" -Wait
Remove-Item $tmp

New-Item -ItemType Directory -Force "$here\bin" | Out-Null
& $csc @common "/out:$here\bin\PreviewVolume.exe" "/win32icon:$here\app.ico" "$here\PreviewVolume.cs"
if ($LASTEXITCODE -ne 0) { throw "Error al compilar" }

# Instalador: lleva dentro PreviewVolume.exe
& $csc /nologo /target:winexe /optimize+ /platform:anycpu /codepage:65001 "/win32manifest:$here\instalador\instalador.manifest" "/win32icon:$here\app.ico" `
    /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll "/resource:$here\bin\PreviewVolume.exe,PreviewVolume.exe" `
    "/out:$here\bin\PreviewVolume-Setup.exe" "$here\instalador\Instalador.cs"
if ($LASTEXITCODE -ne 0) { throw "Error al compilar el instalador" }
Get-Item "$here\bin\*.exe" | Select-Object Name, Length
