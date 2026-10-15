// Preview Volume: barra de volumen pegada a la miniatura que sale al pasar el raton por un boton de la barra de tareas.
// Las miniaturas solo admiten los botones que pone la propia app (Spotify ya pone los suyos), asi que dibujamos
// una ventanita encima de la miniatura y movemos el volumen de la app en el mezclador de Windows (sesiones WASAPI).
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Accessibility;
using Microsoft.Win32;

[assembly: System.Reflection.AssemblyTitle("Preview Volume")]
[assembly: System.Reflection.AssemblyProduct("Preview Volume")]
[assembly: System.Reflection.AssemblyCompany("Divolandia Labs")]
[assembly: System.Reflection.AssemblyCopyright("© 2026 Divolandia Labs")]
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.0.0")]

namespace PreviewVolume
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "/icono") { Icono.WriteIco(args[1]); return; }
            if (args.Length == 1 && args[0] == "/salir")
            {
                IntPtr h = W.FindWindow(null, Mensajero.Titulo);
                if (h != IntPtr.Zero) W.PostMessage(h, 0x0010, IntPtr.Zero, IntPtr.Zero);
                return;
            }
            bool nuevo;
            using (var mx = new Mutex(true, "PreviewVolume_unico", out nuevo))
            {
                if (!nuevo) return;
                Application.EnableVisualStyles();
                Application.Run(new Contexto());
            }
        }
    }

    // ---------------------------------------------------------------- Win32

    static class W
    {
        [StructLayout(LayoutKind.Sequential)] public struct RECT { public int L, T, R, B; }
        [StructLayout(LayoutKind.Sequential)] public struct POINT { public int X, Y; }
        [StructLayout(LayoutKind.Sequential)] public struct MSLL { public POINT pt; public int mouseData, flags, time; public IntPtr extra; }
        public delegate IntPtr HookProc(int code, IntPtr w, IntPtr l);
        public delegate bool EnumProc(IntPtr h, IntPtr l);

        [DllImport("user32")] public static extern bool EnumWindows(EnumProc f, IntPtr l);
        [DllImport("user32", CharSet = CharSet.Unicode)] public static extern int GetWindowText(IntPtr h, StringBuilder s, int n);
        [DllImport("user32")] public static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
        [DllImport("user32", CharSet = CharSet.Unicode)] public static extern IntPtr FindWindow(string c, string t);
        [DllImport("user32", CharSet = CharSet.Unicode)] public static extern IntPtr FindWindowEx(IntPtr p, IntPtr a, string c, string t);
        [DllImport("user32")] public static extern bool PostMessage(IntPtr h, int m, IntPtr w, IntPtr l);
        [DllImport("user32")] public static extern bool IsWindowVisible(IntPtr h);
        [DllImport("user32")] public static extern bool ShowWindow(IntPtr h, int cmd);
        [DllImport("user32")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
        [DllImport("user32")] public static extern bool GetCursorPos(out POINT p);
        [DllImport("user32")] public static extern uint GetDpiForWindow(IntPtr h);
        [DllImport("user32")] public static extern bool SetWindowPos(IntPtr h, IntPtr despues, int x, int y, int w, int hh, uint f);
        [DllImport("user32")] public static extern IntPtr SetWindowsHookEx(int id, HookProc f, IntPtr mod, uint hilo);
        [DllImport("user32")] public static extern bool UnhookWindowsHookEx(IntPtr h);
        [DllImport("user32")] public static extern IntPtr CallNextHookEx(IntPtr h, int code, IntPtr w, IntPtr l);
        [DllImport("kernel32", CharSet = CharSet.Unicode)] public static extern IntPtr GetModuleHandle(string n);
        [DllImport("dwmapi")] public static extern int DwmGetWindowAttribute(IntPtr h, int attr, out RECT r, int size);
        [DllImport("dwmapi")] public static extern int DwmGetWindowAttribute(IntPtr h, int attr, out int v, int size);
        [DllImport("oleacc")] public static extern int AccessibleObjectFromWindow(IntPtr h, uint id, ref Guid g, [MarshalAs(UnmanagedType.Interface)] out object o);
        [DllImport("oleacc")] public static extern int AccessibleChildren(IAccessible p, int inicio, int n, [Out] object[] hijos, out int obtenidos);

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const uint SWP_NOACTIVATE = 0x10, SWP_SHOWWINDOW = 0x40;

        public static bool Dentro(RECT r, int x, int y) { return x >= r.L && x < r.R && y >= r.T && y < r.B; }

        // Rectangulo visible de la ventana (sin el margen invisible de la sombra).
        public static RECT Marco(IntPtr h)
        {
            RECT r;
            if (DwmGetWindowAttribute(h, 9, out r, Marshal.SizeOf(typeof(RECT))) != 0) GetWindowRect(h, out r);
            return r;
        }
    }

    // ---------------------------------------------------------------- miniatura de la barra (MSAA)

    // Lo que se ve: el marco de la miniatura y los titulos de las ventanas que muestra.
    class Vista { public IntPtr Hwnd; public W.RECT Marco; public List<string> Titulos = new List<string>(); public string Clave; }

    static class Miniatura
    {
        static Guid IID = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");

        // Hay varias ventanas TaskListThumbnailWnd; solo la de la miniatura abierta esta visible.
        public static IntPtr Visible()
        {
            IntPtr h = IntPtr.Zero;
            while ((h = W.FindWindowEx(IntPtr.Zero, h, "TaskListThumbnailWnd", null)) != IntPtr.Zero)
                if (W.IsWindowVisible(h)) return h;
            return IntPtr.Zero;
        }

        // La ventana ocupa toda la franja de encima de la barra; lo que se ve son sus elementos de lista
        // (uno por ventana, con su titulo), que dan los bordes izquierdo y derecho del marco.
        public static Vista Leer(IntPtr h)
        {
            W.RECT rw;
            if (!W.GetWindowRect(h, out rw)) return null;
            var v = new Vista { Hwnd = h };
            int l = int.MaxValue, r = int.MinValue;
            object o = null;
            try
            {
                if (W.AccessibleObjectFromWindow(h, 0xFFFFFFFC, ref IID, out o) != 0 || o == null) return null;
                var acc = (IAccessible)o;
                int n = acc.accChildCount;
                var hijos = new object[n];
                int got;
                W.AccessibleChildren(acc, 0, n, hijos, out got);
                for (int i = 0; i < got; i++)
                {
                    object id = hijos[i];
                    if (id is IAccessible) { Marshal.ReleaseComObject(id); continue; }
                    if (Convert.ToInt32(acc.get_accRole(id)) != 34) continue; // ROLE_SYSTEM_LISTITEM
                    int x, y, w, hh;
                    acc.accLocation(out x, out y, out w, out hh, id);
                    if (w <= 0) continue;
                    l = Math.Min(l, x); r = Math.Max(r, x + w);
                    v.Titulos.Add(acc.get_accName(id) ?? "");
                }
            }
            catch { return null; }
            finally { if (o != null) Marshal.ReleaseComObject(o); }
            if (v.Titulos.Count == 0) return null;
            v.Marco = new W.RECT { L = l, R = r, T = rw.T, B = rw.B };
            v.Clave = l + "|" + r + "|" + rw.T + "|" + string.Join("\n", v.Titulos);
            return v;
        }

        // Procesos duenos de las ventanas con esos titulos.
        public static HashSet<uint> Procesos(List<string> titulos)
        {
            var pids = new HashSet<uint>();
            var sb = new StringBuilder(512);
            W.EnumWindows((h, l) =>
            {
                if (!W.IsWindowVisible(h)) return true;
                sb.Length = 0;
                W.GetWindowText(h, sb, sb.Capacity);
                if (titulos.Contains(sb.ToString())) { uint pid; W.GetWindowThreadProcessId(h, out pid); pids.Add(pid); }
                return true;
            }, IntPtr.Zero);
            return pids;
        }
    }

    // ---------------------------------------------------------------- volumen por aplicacion (Core Audio)

    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class MMDeviceEnumerator { }

    [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceEnumerator
    {
        [PreserveSig] int EnumAudioEndpoints(int flujo, int estados, out IMMDeviceCollection col);
    }

    [ComImport, Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceCollection
    {
        [PreserveSig] int GetCount(out int n);
        [PreserveSig] int Item(int i, out IMMDevice d);
    }

    [ComImport, Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDevice
    {
        [PreserveSig] int Activate(ref Guid iid, int ctx, IntPtr p, [MarshalAs(UnmanagedType.IUnknown)] out object o);
    }

    [ComImport, Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionManager2
    {
        [PreserveSig] int GetAudioSessionControl(IntPtr g, int f, out IntPtr c);
        [PreserveSig] int GetSimpleAudioVolume(IntPtr g, int f, out IntPtr v);
        [PreserveSig] int GetSessionEnumerator(out IAudioSessionEnumerator e);
    }

    [ComImport, Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionEnumerator
    {
        [PreserveSig] int GetCount(out int n);
        [PreserveSig] int GetSession(int i, out IAudioSessionControl2 s);
    }

    [ComImport, Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionControl2
    {
        [PreserveSig] int GetState(out int s);
        [PreserveSig] int GetDisplayName(out IntPtr n);
        [PreserveSig] int SetDisplayName(IntPtr n, IntPtr g);
        [PreserveSig] int GetIconPath(out IntPtr p);
        [PreserveSig] int SetIconPath(IntPtr p, IntPtr g);
        [PreserveSig] int GetGroupingParam(out Guid g);
        [PreserveSig] int SetGroupingParam(IntPtr g, IntPtr c);
        [PreserveSig] int RegisterAudioSessionNotification(IntPtr n);
        [PreserveSig] int UnregisterAudioSessionNotification(IntPtr n);
        [PreserveSig] int GetSessionIdentifier(out IntPtr s);
        [PreserveSig] int GetSessionInstanceIdentifier(out IntPtr s);
        [PreserveSig] int GetProcessId(out uint pid);
    }

    [ComImport, Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ISimpleAudioVolume
    {
        [PreserveSig] int SetMasterVolume(float v, ref Guid ctx);
        [PreserveSig] int GetMasterVolume(out float v);
        [PreserveSig] int SetMute(bool m, ref Guid ctx);
        [PreserveSig] int GetMute(out bool m);
    }

    // Todas las sesiones de sonido de una app (puede tener varias: un proceso por altavoz o varios procesos hijos).
    class AppSonido
    {
        public string Nombre;
        public List<ISimpleAudioVolume> Vols = new List<ISimpleAudioVolume>();
        static Guid ctx = Guid.Empty;

        public float Volumen
        {
            get { float max = 0; foreach (var v in Vols) { float x; if (v.GetMasterVolume(out x) == 0 && x > max) max = x; } return max; }
            set { float x = Math.Max(0f, Math.Min(1f, value)); foreach (var v in Vols) v.SetMasterVolume(x, ref ctx); }
        }

        public bool Silencio
        {
            get { foreach (var v in Vols) { bool m; if (v.GetMute(out m) == 0 && !m) return false; } return Vols.Count > 0; }
            set { foreach (var v in Vols) v.SetMute(value, ref ctx); }
        }

        public void Soltar() { foreach (var v in Vols) Marshal.ReleaseComObject(v); Vols.Clear(); }
    }

    static class Sonido
    {
        static Guid IID_Manager2 = new Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F");
        static readonly Dictionary<uint, string[]> nombres = new Dictionary<uint, string[]>();

        // Nombre del proceso y descripcion del ejecutable ("spotify", "Spotify"); se guardan por pid.
        static string[] NombresDe(uint pid)
        {
            string[] r;
            if (nombres.TryGetValue(pid, out r)) return r;
            r = new string[0];
            try
            {
                using (var p = Process.GetProcessById((int)pid))
                {
                    string desc = "";
                    try { desc = p.MainModule.FileVersionInfo.FileDescription ?? ""; } catch { }
                    r = new[] { p.ProcessName, desc };
                }
            }
            catch { }
            nombres[pid] = r;
            return r;
        }

        // Sesiones de sonido de los programas que muestran las ventanas de la miniatura.
        public static AppSonido Buscar(List<string> titulos, bool todas)
        {
            var exes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pid in Miniatura.Procesos(titulos))
            {
                var ns = NombresDe(pid);
                if (ns.Length > 0) exes.Add(ns[0]);
            }
            if (exes.Count == 0) return null;
            if (!todas && !exes.Contains("spotify")) return null;

            var app = new AppSonido { Nombre = string.Join(",", exes) };
            IMMDeviceEnumerator en = null; IMMDeviceCollection col = null;
            try
            {
                en = (IMMDeviceEnumerator)new MMDeviceEnumerator();
                if (en.EnumAudioEndpoints(0, 1, out col) != 0) return null;
                int nd; col.GetCount(out nd);
                for (int d = 0; d < nd; d++)
                {
                    IMMDevice dev; if (col.Item(d, out dev) != 0) continue;
                    object o;
                    if (dev.Activate(ref IID_Manager2, 23, IntPtr.Zero, out o) == 0)
                    {
                        var man = (IAudioSessionManager2)o;
                        IAudioSessionEnumerator se;
                        if (man.GetSessionEnumerator(out se) == 0)
                        {
                            int ns; se.GetCount(out ns);
                            for (int i = 0; i < ns; i++)
                            {
                                IAudioSessionControl2 s; if (se.GetSession(i, out s) != 0) continue;
                                uint pid; bool usada = false;
                                if (s.GetProcessId(out pid) == 0 && pid != 0 && NombresDe(pid).Length > 0 && exes.Contains(NombresDe(pid)[0]))
                                {
                                    var v = s as ISimpleAudioVolume;
                                    if (v != null) { app.Vols.Add(v); usada = true; }
                                }
                                if (!usada) Marshal.ReleaseComObject(s);
                            }
                            Marshal.ReleaseComObject(se);
                        }
                        Marshal.ReleaseComObject(man);
                    }
                    Marshal.ReleaseComObject(dev);
                }
            }
            catch { }
            finally
            {
                if (col != null) Marshal.ReleaseComObject(col);
                if (en != null) Marshal.ReleaseComObject(en);
            }
            if (app.Vols.Count == 0) return null;
            return app;
        }
    }

    // ---------------------------------------------------------------- configuracion (junto al exe)

    class Config
    {
        public bool Todas;
        static string Ruta { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"); } }

        public static Config Leer()
        {
            var c = new Config();
            try
            {
                foreach (var l in File.ReadAllLines(Ruta))
                    if (l.Trim() == "todas=1") c.Todas = true;
            }
            catch { }
            return c;
        }

        public void Guardar()
        {
            try { File.WriteAllText(Ruta, "todas=" + (Todas ? "1" : "0") + "\r\n"); } catch { }
        }

        const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public static bool Autoarranque
        {
            get { using (var k = Registry.CurrentUser.OpenSubKey(RunKey)) return k != null && k.GetValue("PreviewVolume") != null; }
            set
            {
                using (var k = Registry.CurrentUser.CreateSubKey(RunKey))
                    if (value) k.SetValue("PreviewVolume", "\"" + Application.ExecutablePath + "\"");
                    else k.DeleteValue("PreviewVolume", false);
            }
        }
    }

    // ---------------------------------------------------------------- panel de volumen

    class Panel : Form
    {
        public AppSonido App;
        public bool Arrastrando;
        float esc = 1f;
        bool sobreAltavoz;
        float volumen;
        bool silencio;

        public Panel()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(43, 43, 43);
            Text = "Preview Volume";
        }

        protected override bool ShowWithoutActivation { get { return true; } }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80 | 0x08000000 | 0x8; // TOOLWINDOW | NOACTIVATE | TOPMOST
                return cp;
            }
        }

        public void Colocar(W.RECT miniatura, float escala)
        {
            esc = escala;
            int h = (int)Math.Round(40 * esc), sep = (int)Math.Round(4 * esc);
            int y = miniatura.T - h - sep;
            var pantalla = Screen.FromPoint(new Point(miniatura.L, miniatura.T)).Bounds;
            if (y < pantalla.Top) y = miniatura.B + sep; // barra arriba: el panel va debajo
            // Mismo ancho que la miniatura, con un minimo para que la pista no quede enana.
            int w = Math.Max(miniatura.R - miniatura.L, (int)(150 * esc));
            int x = (miniatura.L + miniatura.R - w) / 2;
            x = Math.Max(pantalla.Left, Math.Min(x, pantalla.Right - w));
            W.SetWindowPos(Handle, W.HWND_TOPMOST, x, y, w, h, W.SWP_NOACTIVATE | W.SWP_SHOWWINDOW);
            Refrescar();
        }

        public void Refrescar()
        {
            if (App == null) return;
            float v = App.Volumen; bool m = App.Silencio;
            if (Math.Abs(v - volumen) > 0.001f || m != silencio) { volumen = v; silencio = m; Invalidate(); }
        }

        public void Sumar(float d)
        {
            if (App == null) return;
            if (silencio && d > 0) App.Silencio = false;
            App.Volumen = (float)Math.Round((App.Volumen + d) * 100) / 100f;
            Refrescar();
        }

        Rectangle ZonaAltavoz { get { return new Rectangle(0, 0, (int)(38 * esc), Height); } }
        int PistaIzq { get { return (int)(44 * esc); } }
        int PistaDer { get { return Width - (int)(46 * esc); } }

        void Arrastrar(int x)
        {
            float v = (x - PistaIzq) / (float)Math.Max(1, PistaDer - PistaIzq);
            if (silencio) App.Silencio = false;
            App.Volumen = v;
            Refrescar();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (App == null || e.Button != MouseButtons.Left) return;
            if (ZonaAltavoz.Contains(e.Location)) { App.Silencio = !silencio; Refrescar(); return; }
            Arrastrando = true;
            Capture = true;
            Arrastrar(e.X);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Arrastrando) { Arrastrar(e.X); return; }
            bool s = ZonaAltavoz.Contains(e.Location);
            if (s != sobreAltavoz) { sobreAltavoz = s; Cursor = s ? Cursors.Hand : Cursors.Default; Invalidate(); }
        }

        protected override void OnMouseUp(MouseEventArgs e) { Arrastrando = false; Capture = false; }

        protected override void OnMouseLeave(EventArgs e) { if (sobreAltavoz) { sobreAltavoz = false; Invalidate(); } }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = new Pen(Color.FromArgb(70, 70, 70))) g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);

            bool spotify = App != null && App.Nombre.IndexOf("spotify", StringComparison.OrdinalIgnoreCase) >= 0;
            Color acento = spotify ? Color.FromArgb(30, 215, 96) : Color.FromArgb(96, 205, 255);
            Color tinta = Color.FromArgb(235, 235, 235);
            float cy = Height / 2f;

            // altavoz (clic = silenciar)
            if (sobreAltavoz)
                using (var b = new SolidBrush(Color.FromArgb(62, 62, 62)))
                    g.FillRectangle(b, 3 * esc, 4 * esc, 33 * esc, Height - 8 * esc);
            Altavoz(g, 8 * esc, cy, esc, silencio ? -1 : volumen, tinta);

            // pista y tirador
            float x0 = PistaIzq, x1 = PistaDer, grueso = 4 * esc;
            float frac = silencio ? 0 : volumen;
            float xv = x0 + (x1 - x0) * frac;
            using (var p = new Pen(Color.FromArgb(95, 95, 95), grueso) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                g.DrawLine(p, x0, cy, x1, cy);
            if (xv > x0)
                using (var p = new Pen(acento, grueso) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                    g.DrawLine(p, x0, cy, xv, cy);
            float r = 7 * esc;
            using (var b = new SolidBrush(tinta)) g.FillEllipse(b, xv - r, cy - r, 2 * r, 2 * r);

            // porcentaje
            string t = silencio ? "mudo" : ((int)Math.Round(volumen * 100)).ToString();
            using (var f = new Font("Segoe UI", 12 * esc, GraphicsUnit.Pixel))
            using (var b = new SolidBrush(tinta))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString(t, f, b, new RectangleF(x1 + 6 * esc, 0, Width - x1 - 8 * esc, Height), sf);
            }
        }

        // Altavoz con ondas segun el nivel; nivel < 0 = silenciado (con una X).
        public static void Altavoz(Graphics g, float x, float cy, float e, float nivel, Color c)
        {
            using (var b = new SolidBrush(c))
            using (var p = new Pen(c, 1.6f * e) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.FillPolygon(b, new[] {
                    new PointF(x, cy - 3.5f * e), new PointF(x + 4 * e, cy - 3.5f * e), new PointF(x + 9 * e, cy - 8 * e),
                    new PointF(x + 9 * e, cy + 8 * e), new PointF(x + 4 * e, cy + 3.5f * e), new PointF(x, cy + 3.5f * e) });
                float ox = x + 9 * e;
                if (nivel < 0)
                {
                    g.DrawLine(p, ox + 4 * e, cy - 4 * e, ox + 11 * e, cy + 4 * e);
                    g.DrawLine(p, ox + 4 * e, cy + 4 * e, ox + 11 * e, cy - 4 * e);
                    return;
                }
                if (nivel > 0.01f) g.DrawArc(p, ox - 3 * e, cy - 4 * e, 8 * e, 8 * e, -55, 110);
                if (nivel > 0.34f) g.DrawArc(p, ox - 4 * e, cy - 8 * e, 15 * e, 16 * e, -55, 110);
                if (nivel > 0.67f) g.DrawArc(p, ox - 5 * e, cy - 12 * e, 22 * e, 24 * e, -55, 110);
            }
        }
    }

    // Ventana oculta con titulo fijo para que "/salir" pueda cerrar la copia abierta.
    class Mensajero : NativeWindow
    {
        public const string Titulo = "PreviewVolume_mensajero";
        readonly Action alCerrar;
        public Mensajero(Action alCerrar) { this.alCerrar = alCerrar; CreateHandle(new CreateParams { Caption = Titulo }); }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0010) { alCerrar(); return; } // WM_CLOSE
            base.WndProc(ref m);
        }
    }

    // ---------------------------------------------------------------- logica principal

    class Contexto : ApplicationContext
    {
        readonly Config cfg = Config.Leer();
        readonly Panel panel = new Panel();
        readonly Mensajero msj;
        readonly NotifyIcon icono = new NotifyIcon();
        readonly System.Windows.Forms.Timer reloj = new System.Windows.Forms.Timer { Interval = 40 };
        readonly W.HookProc hookProc;
        IntPtr hook;

        IntPtr miniatura;
        W.RECT rMini, rPanel;
        volatile bool activo;      // panel a la vista (lo lee el gancho del raton)
        string clave;              // miniatura que se esta mostrando (marco + titulos)
        bool conSonido;            // esa miniatura es de una app con sonido
        DateTime ultimaLectura, fuera = DateTime.MaxValue;

        public Contexto()
        {
            msj = new Mensajero(ExitThread);
            panel.CreateControl();
            var h = panel.Handle;

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini")))
            {
                cfg.Guardar();
                try { Config.Autoarranque = true; } catch { }
            }

            icono.Icon = Icono.Crear(32);
            icono.Text = "Preview Volume";
            icono.Visible = true;
            var menu = new ContextMenuStrip();
            var todas = new ToolStripMenuItem("Mostrar en todas las apps con sonido") { Checked = cfg.Todas };
            todas.Click += (s, e) => { cfg.Todas = !cfg.Todas; todas.Checked = cfg.Todas; cfg.Guardar(); };
            var inicio = new ToolStripMenuItem("Iniciar con Windows") { Checked = Config.Autoarranque };
            inicio.Click += (s, e) => { try { Config.Autoarranque = !Config.Autoarranque; } catch { } inicio.Checked = Config.Autoarranque; };
            menu.Items.Add(new ToolStripMenuItem("Preview Volume") { Enabled = false });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(todas);
            menu.Items.Add(inicio);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Salir", null, (s, e) => ExitThread());
            icono.ContextMenuStrip = menu;

            hookProc = Raton;
            hook = W.SetWindowsHookEx(14, hookProc, W.GetModuleHandle(null), 0);

            reloj.Tick += Tick;
            reloj.Start();
        }

        void Tick(object s, EventArgs e)
        {
            W.POINT c; W.GetCursorPos(out c);

            IntPtr h = Miniatura.Visible();
            Vista v = h != IntPtr.Zero ? Miniatura.Leer(h) : null;

            if (v != null)
            {
                if (v.Clave != clave)
                {
                    Ocultar();
                    clave = v.Clave; // aunque no tenga sonido, para no buscar otra vez en cada vuelta
                    conSonido = Mostrar(v, h);
                }
                else if (!activo && conSonido && W.Dentro(v.Marco, c.X, c.Y))
                    Mostrar(v, h); // se oculto al salir el raton y ha vuelto a la misma miniatura
            }
            else if (!activo) clave = null;

            if (activo)
            {
                // Sigue mientras el raton este en el panel (o arrastrando), o en la miniatura o la barra
                // con la miniatura abierta. Si no, se va enseguida aunque Windows deje la miniatura "visible".
                W.RECT rBarra;
                W.GetWindowRect(W.FindWindow("Shell_TrayWnd", null), out rBarra);
                // El hueco entre panel y miniatura cuenta como dentro, asi basta un margen minimo.
                var puente = new W.RECT { L = Math.Min(rPanel.L, rMini.L), R = Math.Max(rPanel.R, rMini.R),
                                          T = Math.Min(rPanel.T, rMini.T), B = Math.Max(rPanel.B, rMini.B) };
                bool dentro = panel.Arrastrando || W.Dentro(rPanel, c.X, c.Y)
                    || (v != null && (W.Dentro(puente, c.X, c.Y) || W.Dentro(rBarra, c.X, c.Y)));
                if (dentro) fuera = DateTime.MaxValue;
                else if (fuera == DateTime.MaxValue) fuera = DateTime.Now;
                else if ((DateTime.Now - fuera).TotalMilliseconds >= 60) Ocultar();
            }

            if (activo && !panel.Arrastrando && (DateTime.Now - ultimaLectura).TotalMilliseconds > 250)
            {
                ultimaLectura = DateTime.Now;
                try { panel.Refrescar(); } catch { Ocultar(); }
            }
        }

        bool Mostrar(Vista v, IntPtr h)
        {
            if (panel.App != null) { panel.App.Soltar(); panel.App = null; }
            panel.App = Sonido.Buscar(v.Titulos, cfg.Todas);
            if (panel.App == null) return false;
            miniatura = h;
            rMini = v.Marco;
            float esc = W.GetDpiForWindow(h) / 96f;
            if (esc <= 0) esc = 1;
            panel.Colocar(rMini, esc);
            W.GetWindowRect(panel.Handle, out rPanel);
            activo = true;
            fuera = DateTime.MaxValue;
            ultimaLectura = DateTime.Now;
            return true;
        }

        void Ocultar()
        {
            activo = false;
            fuera = DateTime.MaxValue;
            // El panel se muestra con SetWindowPos, asi que WinForms no sabe que esta visible: ocultar a mano.
            W.ShowWindow(panel.Handle, 0);
            if (panel.App != null) { panel.App.Soltar(); panel.App = null; }
        }

        // Rueda del raton sobre la miniatura o el panel = subir/bajar volumen.
        IntPtr Raton(int code, IntPtr w, IntPtr l)
        {
            if (code >= 0 && activo && (int)w == 0x020A)
            {
                var m = (W.MSLL)Marshal.PtrToStructure(l, typeof(W.MSLL));
                bool enMini = W.IsWindowVisible(miniatura) && W.Dentro(rMini, m.pt.X, m.pt.Y);
                if (enMini || W.Dentro(rPanel, m.pt.X, m.pt.Y))
                {
                    int delta = (short)((m.mouseData >> 16) & 0xFFFF);
                    panel.BeginInvoke(new Action(() => panel.Sumar(delta > 0 ? 0.02f : -0.02f)));
                    return (IntPtr)1;
                }
            }
            return W.CallNextHookEx(hook, code, w, l);
        }

        protected override void ExitThreadCore()
        {
            reloj.Stop();
            if (hook != IntPtr.Zero) W.UnhookWindowsHookEx(hook);
            icono.Visible = false;
            Ocultar();
            base.ExitThreadCore();
        }
    }

    // ---------------------------------------------------------------- icono

    static class Icono
    {
        public static void Pintar(Graphics g, int S)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using (var b = new SolidBrush(Color.FromArgb(30, 215, 96))) g.FillEllipse(b, 0.5f, 0.5f, S - 1, S - 1);
            float e = S / 32f;
            Panel.Altavoz(g, 6.5f * e, S / 2f, e, 1f, Color.FromArgb(20, 20, 20));
        }

        public static Icon Crear(int S)
        {
            using (var bmp = new Bitmap(S, S))
            {
                using (var g = Graphics.FromImage(bmp)) Pintar(g, S);
                return Icon.FromHandle(bmp.GetHicon());
            }
        }

        // Escribe un .ico con varias medidas (lo usa build.ps1).
        public static void WriteIco(string path)
        {
            int[] sizes = { 16, 20, 24, 32, 40, 48, 64, 256 };
            var frames = new byte[sizes.Length][];
            for (int i = 0; i < sizes.Length; i++)
                using (var bmp = new Bitmap(sizes[i], sizes[i], System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                using (var ms = new MemoryStream())
                {
                    using (var g = Graphics.FromImage(bmp)) Pintar(g, sizes[i]);
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    frames[i] = ms.ToArray();
                }
            using (var w = new BinaryWriter(File.Create(path)))
            {
                w.Write((ushort)0); w.Write((ushort)1); w.Write((ushort)sizes.Length);
                int off = 6 + 16 * sizes.Length;
                for (int i = 0; i < sizes.Length; i++)
                {
                    byte s = (byte)(sizes[i] >= 256 ? 0 : sizes[i]);
                    w.Write(s); w.Write(s); w.Write((byte)0); w.Write((byte)0);
                    w.Write((ushort)1); w.Write((ushort)32);
                    w.Write(frames[i].Length); w.Write(off);
                    off += frames[i].Length;
                }
                foreach (var f in frames) w.Write(f);
            }
        }
    }
}
