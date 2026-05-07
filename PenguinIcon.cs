using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace RemandMe;

internal static class PenguinIcon
{
    public static Icon Create()
    {
        using var bitmap = new Bitmap(64, 64);
        using var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);

        using var black = new SolidBrush(Color.FromArgb(20, 28, 38));
        using var white = new SolidBrush(Color.White);
        using var yellow = new SolidBrush(Color.FromArgb(245, 188, 55));
        using var eye = new SolidBrush(Color.FromArgb(5, 8, 12));

        g.FillEllipse(black, 13, 6, 38, 50);
        g.FillEllipse(white, 20, 22, 24, 27);
        g.FillEllipse(eye, 22, 18, 6, 7);
        g.FillEllipse(eye, 36, 18, 6, 7);
        g.FillPolygon(yellow, new[] { new Point(31, 26), new Point(39, 30), new Point(29, 33) });
        g.FillEllipse(yellow, 16, 50, 14, 7);
        g.FillEllipse(yellow, 34, 50, 14, 7);

        var handle = bitmap.GetHicon();
        try
        {
            using var icon = Icon.FromHandle(handle);
            return (Icon)icon.Clone();
        }
        finally
        {
            DestroyIcon(handle);
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyIcon(IntPtr hIcon);
}
