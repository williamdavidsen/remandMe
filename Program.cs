using Microsoft.Win32;

namespace RemandMe;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Contains("--install-startup", StringComparer.OrdinalIgnoreCase))
        {
            StartupManager.Install();
            return;
        }

        if (args.Contains("--uninstall-startup", StringComparer.OrdinalIgnoreCase))
        {
            StartupManager.Uninstall();
            return;
        }

        var installStartup = !args.Contains("--no-startup", StringComparer.OrdinalIgnoreCase);
        var showImmediately = args.Contains("--show-now", StringComparer.OrdinalIgnoreCase);

        Application.Run(new ReminderContext(installStartup, showImmediately));
    }
}

internal sealed class ReminderContext : ApplicationContext
{
    private readonly System.Windows.Forms.Timer _timer = new();
    private readonly NotifyIcon _trayIcon;
    private readonly TimeSpan _interval;
    private DateTime _nextAlertAt;
    private AlertForm? _alertForm;

    public ReminderContext(bool installStartup, bool showImmediately)
    {
        _interval = ReadInterval();
        _trayIcon = BuildTrayIcon();
        _trayIcon.Visible = true;

        if (installStartup)
        {
            StartupManager.Install();
        }

        SystemEvents.PowerModeChanged += OnPowerModeChanged;

        _timer.Interval = 1_000;
        _timer.Tick += OnTick;
        _timer.Start();

        ResetTimer();

        if (showImmediately)
        {
            ShowAlert();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            SystemEvents.PowerModeChanged -= OnPowerModeChanged;
            _timer.Dispose();
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            _alertForm?.Dispose();
        }

        base.Dispose(disposing);
    }

    private static TimeSpan ReadInterval()
    {
        var secondsText = Environment.GetEnvironmentVariable("REMANDME_INTERVAL_SECONDS");
        return int.TryParse(secondsText, out var seconds) && seconds > 0
            ? TimeSpan.FromSeconds(seconds)
            : TimeSpan.FromMinutes(20);
    }

    private NotifyIcon BuildTrayIcon()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Uyariyi simdi goster", null, (_, _) => ShowAlert());
        menu.Items.Add("20 dakikayi yeniden baslat", null, (_, _) => ResetTimer());
        menu.Items.Add("Windows baslangicindan kaldir", null, (_, _) => StartupManager.Uninstall());
        menu.Items.Add("Cikis", null, (_, _) => ExitThread());

        return new NotifyIcon
        {
            Text = "RemandMe - 20 dakikada bir kalk",
            Icon = PenguinIcon.Create(),
            ContextMenuStrip = menu
        };
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (DateTime.Now >= _nextAlertAt)
        {
            ShowAlert();
        }
    }

    private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
        if (e.Mode == PowerModes.Resume)
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        _nextAlertAt = DateTime.Now.Add(_interval);
    }

    private void ShowAlert()
    {
        ResetTimer();

        if (_alertForm is { IsDisposed: false })
        {
            _alertForm.Activate();
            return;
        }

        _alertForm = new AlertForm();
        _alertForm.FormClosed += (_, _) => _alertForm = null;
        _alertForm.Show();
        _alertForm.Activate();
    }
}

internal static class StartupManager
{
    private const string AppName = "RemandMe";
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public static void Install()
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
        key.SetValue(AppName, $"\"{Application.ExecutablePath}\"");
    }

    public static void Uninstall()
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
        key?.DeleteValue(AppName, throwOnMissingValue: false);
    }
}
