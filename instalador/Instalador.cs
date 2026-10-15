using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

[assembly: System.Reflection.AssemblyTitle("Instalador de Preview Volume")]
[assembly: System.Reflection.AssemblyProduct("Preview Volume")]
[assembly: System.Reflection.AssemblyCompany("Divolandia Labs")]
[assembly: System.Reflection.AssemblyCopyright("© 2026 Divolandia Labs")]
[assembly: System.Reflection.AssemblyVersion("1.1.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.1.0.0")]

namespace PreviewVolumeSetup
{
    // Instalador y desinstalador de Preview Volume (un solo .exe con el programa dentro)
    static class Programa
    {
        public const string Nombre = "Preview Volume";
        public const string Version = "1.1.0";
        public const string Exe = "PreviewVolume.exe";
        public const string ClaveDesinstalar = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\PreviewVolume";
        public const string ClaveInicio = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public const string ValorInicio = "PreviewVolume";

        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool desinstalar = false, silencioso = false, escritorio = false, inicio = true, abrir = false;
            string dir = null;
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i].ToLowerInvariant();
                if (a == "/desinstalar" || a == "/uninstall") desinstalar = true;
                else if (a == "/silencioso" || a == "/s" || a == "/silent") silencioso = true;
                else if (a == "/escritorio") escritorio = true;
                else if (a == "/sininicio") inicio = false;
                else if (a == "/abrir") abrir = true;
                else if (a == "/dir" && i + 1 < args.Length) dir = args[++i];
                else if (a == "/idioma" && i + 1 < args.Length) { int n = Array.IndexOf(L.Codigos, args[++i].ToLowerInvariant()); if (n >= 0) L.Actual = n; }
            }
            if (desinstalar)
            {
                if (dir == null) dir = Path.GetDirectoryName(Application.ExecutablePath);
                // el desinstalador vive dentro de la carpeta que va a borrar: se copia a TEMP y sigue desde allí
                string copia = Path.Combine(Path.GetTempPath(), "PreviewVolume-desinstalar.exe");
                if (Application.ExecutablePath.StartsWith(dir.TrimEnd('\\') + "\\", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        File.Copy(Application.ExecutablePath, copia, true);
                        Process.Start(copia, "/desinstalar /dir \"" + dir + "\"" + (silencioso ? " /silencioso" : ""));
                        return 0;
                    }
                    catch { }
                }
                if (!silencioso) { using (var v = new VentanaDesinstalar(dir)) v.ShowDialog(); }
                else Instalacion.Desinstalar(dir, null);
                // la copia temporal se borra sola al cerrarse
                if (string.Equals(Application.ExecutablePath, copia, StringComparison.OrdinalIgnoreCase))
                    try { Process.Start(new ProcessStartInfo("cmd.exe", "/c ping -n 3 127.0.0.1 >nul & del \"" + copia + "\"") { CreateNoWindow = true, UseShellExecute = false }); } catch { }
                return 0;
            }
            if (dir == null) dir = Instalacion.CarpetaInstalada() ?? Instalacion.CarpetaPorDefecto;
            if (silencioso)
            {
                try { Instalacion.Instalar(dir, escritorio, inicio, null); if (abrir) Process.Start(Path.Combine(dir, Exe)); return 0; }
                catch (Exception ex) { Console.Error.WriteLine(ex.Message); return 1; }
            }
            Application.Run(new VentanaInstalar(dir));
            return 0;
        }

        public static Icon IconoApp()
        {
            try { return Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { return SystemIcons.Application; }
        }
    }

    static class Instalacion
    {
        public static string CarpetaPorDefecto
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", Programa.Nombre); }
        }

        public static string CarpetaInstalada()
        {
            try
            {
                using (var k = Registry.CurrentUser.OpenSubKey(Programa.ClaveDesinstalar))
                {
                    string d = k == null ? null : k.GetValue("InstallLocation") as string;
                    return !string.IsNullOrEmpty(d) && Directory.Exists(d) ? d : null;
                }
            }
            catch { return null; }
        }

        static string AccesoMenuInicio { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), Programa.Nombre + ".lnk"); } }
        static string AccesoEscritorio { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Programa.Nombre + ".lnk"); } }

        // Preview Volume no tiene ventana principal: se le pide salir con "/salir" y, si no, se cierra a la fuerza.
        // Se cierra cualquier copia (tambien una abierta desde otra carpeta), porque solo puede haber una a la vez.
        static void CerrarPrograma()
        {
            var abiertos = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Programa.Exe));
            if (abiertos.Length == 0) return;
            try
            {
                string ruta = abiertos[0].MainModule.FileName;
                var p = Process.Start(new ProcessStartInfo(ruta, "/salir") { UseShellExecute = false });
                if (p != null) p.WaitForExit(3000);
            }
            catch { }
            foreach (var p in abiertos)
            {
                try { if (!p.WaitForExit(3000)) { p.Kill(); p.WaitForExit(3000); } }
                catch { }
            }
        }

        public static void Instalar(string dir, bool escritorio, bool inicio, Action<string, int> progreso)
        {
            Action<string, int> P = (t, n) => { if (progreso != null) progreso(t, n); };
            P(L.T("Preparando la carpeta…"), 5);
            Directory.CreateDirectory(dir);
            P(L.F("Cerrando {0} si está abierto…", Programa.Nombre), 12);
            CerrarPrograma();
            P(L.F("Copiando {0}…", Programa.Exe), 25);
            string exe = Path.Combine(dir, Programa.Exe);
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(Programa.Exe))
            {
                if (s == null) throw new InvalidOperationException(L.F("El instalador está dañado: no contiene {0}", Programa.Exe));
                string tmp = exe + ".nuevo";
                using (var f = File.Create(tmp)) s.CopyTo(f);
                if (File.Exists(exe)) File.Delete(exe);
                File.Move(tmp, exe);
            }
            // con config.ini presente el programa no toca el arranque con Windows: lo decide el instalador
            string cfg = Path.Combine(dir, "config.ini");
            if (!File.Exists(cfg)) File.WriteAllText(cfg, "todas=0\r\n");

            P(L.T("Creando el desinstalador…"), 45);
            string desinst = Path.Combine(dir, "Desinstalar.exe");
            if (!string.Equals(Application.ExecutablePath, desinst, StringComparison.OrdinalIgnoreCase)) File.Copy(Application.ExecutablePath, desinst, true);

            P(L.T("Creando accesos directos…"), 60);
            string desc = L.T("Barra de volumen en la vista previa de la barra de tareas");
            Acceso(AccesoMenuInicio, exe, desc);
            if (escritorio) Acceso(AccesoEscritorio, exe, desc);
            else if (File.Exists(AccesoEscritorio)) File.Delete(AccesoEscritorio);

            P(L.T("Registrando en Windows…"), 80);
            using (var k = Registry.CurrentUser.CreateSubKey(Programa.ClaveInicio))
                if (inicio) k.SetValue(Programa.ValorInicio, "\"" + exe + "\"");
                else k.DeleteValue(Programa.ValorInicio, false);
            using (var k = Registry.CurrentUser.CreateSubKey(Programa.ClaveDesinstalar))
            {
                k.SetValue("DisplayName", Programa.Nombre);
                k.SetValue("DisplayVersion", Programa.Version);
                k.SetValue("Publisher", "Divolandia Labs");
                k.SetValue("HelpLink", "https://github.com/DivolandiaLabs");
                k.SetValue("DisplayIcon", "\"" + exe + "\",0");
                k.SetValue("InstallLocation", dir);
                k.SetValue("UninstallString", "\"" + desinst + "\" /desinstalar");
                k.SetValue("QuietUninstallString", "\"" + desinst + "\" /desinstalar /silencioso");
                k.SetValue("NoModify", 1, RegistryValueKind.DWord);
                k.SetValue("NoRepair", 1, RegistryValueKind.DWord);
                k.SetValue("EstimatedSize", (int)((new FileInfo(exe).Length + new FileInfo(desinst).Length) / 1024), RegistryValueKind.DWord);
                k.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
            }
            P(L.T("Actualizando los iconos de Windows…"), 95);
            try { SHChangeNotify(0x08000000, 0, IntPtr.Zero, IntPtr.Zero); } catch { }
            P(L.T("Listo"), 100);
        }

        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        static extern void SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        public static void Desinstalar(string dir, Action<string, int> progreso)
        {
            Action<string, int> P = (t, n) => { if (progreso != null) progreso(t, n); };
            P(L.F("Cerrando {0}…", Programa.Nombre), 10);
            CerrarPrograma();
            P(L.T("Quitando accesos directos…"), 30);
            try { if (File.Exists(AccesoMenuInicio)) File.Delete(AccesoMenuInicio); } catch { }
            try { if (File.Exists(AccesoEscritorio)) File.Delete(AccesoEscritorio); } catch { }
            P(L.T("Quitando el registro…"), 50);
            try { Registry.CurrentUser.DeleteSubKeyTree(Programa.ClaveDesinstalar, false); } catch { }
            try
            {
                using (var k = Registry.CurrentUser.OpenSubKey(Programa.ClaveInicio, true))
                    if (k != null) k.DeleteValue(Programa.ValorInicio, false);
            }
            catch { }
            P(L.T("Borrando archivos…"), 70);
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    foreach (string f in new[] { Programa.Exe, "Desinstalar.exe", "config.ini" })
                    {
                        string r = Path.Combine(dir, f);
                        if (File.Exists(r)) File.Delete(r);
                    }
                    if (Directory.Exists(dir) && Directory.GetFileSystemEntries(dir).Length == 0) Directory.Delete(dir);
                    break;
                }
                catch { Thread.Sleep(500); }
            }
            P(L.T("Listo"), 100);
        }

        static void Acceso(string lnk, string destino, string descripcion)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(lnk));
            var tipo = Type.GetTypeFromProgID("WScript.Shell");
            object sh = Activator.CreateInstance(tipo);
            object a = tipo.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, sh, new object[] { lnk });
            var t = a.GetType();
            t.InvokeMember("TargetPath", BindingFlags.SetProperty, null, a, new object[] { destino });
            t.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, a, new object[] { Path.GetDirectoryName(destino) });
            t.InvokeMember("Description", BindingFlags.SetProperty, null, a, new object[] { descripcion });
            t.InvokeMember("IconLocation", BindingFlags.SetProperty, null, a, new object[] { destino + ",0" });
            t.InvokeMember("Save", BindingFlags.InvokeMethod, null, a, null);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(a);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(sh);
        }
    }

    // ---------------- Ventanas ----------------
    class Base : Form
    {
        protected float E;
        protected Panel cabecera;
        protected Label titulo, subtitulo;
        protected readonly Color acento = Color.FromArgb(22, 160, 75);

        protected Base(string t, string sub)
        {
            using (var g = CreateGraphics()) E = g.DpiX / 96f;
            Text = t;
            Font = new Font("Segoe UI", 9.5f);
            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(S(560), S(380));
            BackColor = Color.White;
            Icon = Programa.IconoApp();
            cabecera = new Panel { Dock = DockStyle.Top, Height = S(96) };
            cabecera.Paint += (s, e) =>
            {
                using (var b = new LinearGradientBrush(cabecera.ClientRectangle, Color.FromArgb(18, 18, 20), Color.FromArgb(24, 70, 44), 0f))
                    e.Graphics.FillRectangle(b, cabecera.ClientRectangle);
                var ic = Programa.IconoApp();
                using (var bmp = new Icon(ic, S(48), S(48)).ToBitmap()) e.Graphics.DrawImage(bmp, S(22), S(24), S(48), S(48));
            };
            titulo = new Label { Text = t, ForeColor = Color.White, BackColor = Color.Transparent, Font = new Font("Segoe UI Semibold", 15f), Location = new Point(S(84), S(20)), AutoSize = true };
            subtitulo = new Label { Text = sub, ForeColor = Color.FromArgb(150, 230, 180), BackColor = Color.Transparent, Location = new Point(S(86), S(56)), AutoSize = true };
            cabecera.Controls.Add(titulo);
            cabecera.Controls.Add(subtitulo);
            Controls.Add(cabecera);
        }

        protected int S(int v) { return (int)Math.Round(v * E); }

        protected Button Boton(string t, int x, int y, bool principal)
        {
            var b = new Button { Text = t, Location = new Point(S(x), S(y)), Size = new Size(S(110), S(32)), FlatStyle = FlatStyle.Flat };
            b.FlatAppearance.BorderColor = principal ? acento : Color.FromArgb(200, 200, 200);
            if (principal) { b.BackColor = acento; b.ForeColor = Color.White; }
            Controls.Add(b);
            return b;
        }
    }

    class VentanaInstalar : Base
    {
        TextBox carpeta;
        CheckBox escritorio, inicio, abrir;
        ProgressBar barra;
        Label aviso, estado, etiquetaIdioma;
        ComboBox idioma;
        Button instalar, cancelar, examinar;
        readonly bool actualizar;
        bool terminado, ocupado;

        public VentanaInstalar(string dir) : base("", "")
        {
            actualizar = Instalacion.CarpetaInstalada() != null;
            aviso = new Label { Location = new Point(S(24), S(116)), Size = new Size(S(512), S(22)) };
            carpeta = new TextBox { Text = dir, Location = new Point(S(24), S(142)), Width = S(400) };
            examinar = new Button { Location = new Point(S(432), S(140)), Size = new Size(S(104), S(28)), FlatStyle = FlatStyle.Flat };
            examinar.Click += (s, e) =>
            {
                using (var d = new FolderBrowserDialog { SelectedPath = carpeta.Text })
                    if (d.ShowDialog(this) == DialogResult.OK)
                        carpeta.Text = d.SelectedPath.EndsWith(Programa.Nombre, StringComparison.OrdinalIgnoreCase) ? d.SelectedPath : Path.Combine(d.SelectedPath, Programa.Nombre);
            };
            inicio = new CheckBox { Checked = true, Location = new Point(S(24), S(186)), AutoSize = true };
            escritorio = new CheckBox { Checked = false, Location = new Point(S(24), S(214)), AutoSize = true };
            abrir = new CheckBox { Checked = true, Location = new Point(S(24), S(242)), AutoSize = true };
            barra = new ProgressBar { Location = new Point(S(24), S(280)), Size = new Size(S(512), S(10)), Visible = false };
            estado = new Label { Location = new Point(S(24), S(294)), Size = new Size(S(512), S(36)), ForeColor = Color.DimGray };
            etiquetaIdioma = new Label { Location = new Point(S(24), S(341)), AutoSize = true, ForeColor = Color.DimGray };
            idioma = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(S(96), S(338)), Width = S(130) };
            idioma.Items.AddRange(L.Nombres);
            idioma.SelectedIndex = L.Actual;
            idioma.SelectedIndexChanged += (s, e) => { L.Actual = idioma.SelectedIndex; Textos(); };
            Controls.AddRange(new Control[] { aviso, carpeta, examinar, inicio, escritorio, abrir, barra, estado, etiquetaIdioma, idioma });
            instalar = Boton("", 308, 334, true);
            cancelar = Boton("", 426, 334, false);
            instalar.Click += (s, e) => Instalar();
            cancelar.Click += (s, e) => Close();
            AcceptButton = instalar;
            Textos();
        }

        // Pone todos los textos en el idioma elegido (se llama otra vez al cambiar de idioma).
        void Textos()
        {
            Text = titulo.Text = L.F("Instalar {0}", Programa.Nombre);
            subtitulo.Text = L.F("Versión {0} · Divolandia Labs", Programa.Version);
            aviso.Text = actualizar ? L.T("Ya está instalado: se actualizará en la misma carpeta.") : L.T("Carpeta de instalación (solo para tu usuario, sin permisos de administrador):");
            examinar.Text = L.T("Examinar…");
            inicio.Text = L.F("Iniciar {0} con Windows", Programa.Nombre);
            escritorio.Text = L.T("Crear acceso directo en el escritorio");
            abrir.Text = L.F("Abrir {0} al terminar", Programa.Nombre);
            etiquetaIdioma.Text = L.T("Idioma");
            instalar.Text = terminado ? L.T("Finalizar") : actualizar ? L.T("Actualizar") : L.T("Instalar");
            cancelar.Text = L.T("Cancelar");
            if (terminado) estado.Text = L.T("Instalado. Está junto al reloj: pasa el ratón por el icono de Spotify.");
        }

        void Instalar()
        {
            if (terminado)
            {
                if (abrir.Checked) try { Process.Start(Path.Combine(carpeta.Text, Programa.Exe)); } catch { }
                Close();
                return;
            }
            if (ocupado) return;
            string dir = carpeta.Text.Trim();
            if (dir.Length < 4) { MessageBox.Show(this, L.T("Elige una carpeta válida."), Text); return; }
            ocupado = true;
            instalar.Enabled = cancelar.Enabled = examinar.Enabled = carpeta.Enabled = idioma.Enabled = false;
            barra.Visible = true;
            bool esc = escritorio.Checked, ini = inicio.Checked;
            var hilo = new Thread(() =>
            {
                string error = null;
                try
                {
                    Instalacion.Instalar(dir, esc, ini, (t, n) => BeginInvoke((Action)(() => { estado.Text = t; barra.Value = n; })));
                }
                catch (UnauthorizedAccessException) { error = L.T("No hay permiso para escribir en esa carpeta. Elige otra (por ejemplo, la propuesta)."); }
                catch (Exception ex) { error = ex.Message; }
                BeginInvoke((Action)(() =>
                {
                    ocupado = false;
                    if (error != null)
                    {
                        MessageBox.Show(this, L.T("No se pudo instalar:") + "\n\n" + error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        instalar.Enabled = cancelar.Enabled = examinar.Enabled = carpeta.Enabled = idioma.Enabled = true;
                        barra.Visible = false; estado.Text = "";
                        return;
                    }
                    terminado = true;
                    estado.ForeColor = Color.FromArgb(20, 130, 50);
                    Textos();
                    instalar.Enabled = idioma.Enabled = true;
                }));
            });
            hilo.IsBackground = true;
            hilo.Start();
        }
    }

    class VentanaDesinstalar : Base
    {
        public VentanaDesinstalar(string dir) : base(L.F("Desinstalar {0}", Programa.Nombre), L.T("Se quitará el programa, sus accesos directos y su registro"))
        {
            var l = new Label { Text = L.F("Carpeta: {0}", dir), Location = new Point(S(24), S(120)), Size = new Size(S(512), S(40)) };
            var barra = new ProgressBar { Location = new Point(S(24), S(220)), Size = new Size(S(512), S(10)), Visible = false };
            var estado = new Label { Location = new Point(S(24), S(238)), Size = new Size(S(512), S(22)), ForeColor = Color.DimGray };
            Controls.AddRange(new Control[] { l, barra, estado });
            var si = Boton(L.T("Desinstalar"), 308, 334, true);
            var no = Boton(L.T("Cancelar"), 426, 334, false);
            bool hecho = false;
            no.Click += (s, e) => Close();
            si.Click += (s, e) =>
            {
                if (hecho) { Close(); return; }
                si.Enabled = no.Enabled = false;
                barra.Visible = true;
                var hilo = new Thread(() =>
                {
                    Instalacion.Desinstalar(dir, (t, n) => BeginInvoke((Action)(() => { estado.Text = t; barra.Value = n; })));
                    BeginInvoke((Action)(() =>
                    {
                        hecho = true;
                        estado.Text = L.F("{0} se ha desinstalado.", Programa.Nombre);
                        si.Text = L.T("Cerrar"); si.Enabled = true;
                    }));
                });
                hilo.IsBackground = true;
                hilo.Start();
            };
        }
    }
}
