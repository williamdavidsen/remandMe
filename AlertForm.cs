using System.Drawing.Drawing2D;

namespace RemandMe;

internal sealed class AlertForm : Form
{
    private readonly Button _okButton = new();
    private readonly Button _snoozeButton = new();
    private Rectangle _panelBounds;
    private Rectangle _penguinBounds;
    private Rectangle _contentBounds;

    public AlertForm()
    {
        Text = "RemandMe";
        StartPosition = FormStartPosition.Manual;
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        DoubleBuffered = true;
        BackColor = Color.FromArgb(16, 20, 28);
        ForeColor = Color.White;
        KeyPreview = true;
        MinimumSize = new Size(620, 420);

        var screen = Screen.FromPoint(Cursor.Position).WorkingArea;
        Width = Math.Max(MinimumSize.Width, (int)(screen.Width * 0.70));
        Height = Math.Max(MinimumSize.Height, (int)(screen.Height * 0.60));
        Left = screen.Left + (screen.Width - Width) / 2;
        Top = screen.Top + (screen.Height - Height) / 2;

        BuildButtons();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        BringToFront();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
        {
            Close();
        }

        base.OnKeyDown(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        CalculateLayout();
        PositionButtons();
        Region = new Region(RoundedRect(ClientRectangle, 28));
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        using var background = new LinearGradientBrush(ClientRectangle, Color.FromArgb(22, 26, 38), Color.FromArgb(26, 92, 117), 35f);
        g.FillRectangle(background, ClientRectangle);

        DrawPanel(g);
        DrawDecorations(g);
        DrawPenguin(g);
        DrawText(g);
    }

    private void BuildButtons()
    {
        ConfigureButton(_okButton, "Tamam, kalkiyorum", Color.FromArgb(247, 196, 65));
        ConfigureButton(_snoozeButton, "Bir tur daha", Color.FromArgb(236, 244, 248));

        _okButton.Click += (_, _) => Close();
        _snoozeButton.Click += (_, _) => Close();

        Controls.Add(_okButton);
        Controls.Add(_snoozeButton);
        CalculateLayout();
        PositionButtons();
    }

    private static void ConfigureButton(Button button, string text, Color backColor)
    {
        button.Text = text;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = backColor;
        button.ForeColor = Color.FromArgb(20, 24, 32);
        button.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
        button.Height = 46;
        button.Width = 190;
        button.TabStop = false;
    }

    private void CalculateLayout()
    {
        var outerPadX = Math.Max(28, ClientSize.Width / 34);
        var outerPadY = Math.Max(26, ClientSize.Height / 26);
        _panelBounds = new Rectangle(
            outerPadX,
            outerPadY,
            ClientSize.Width - outerPadX * 2,
            ClientSize.Height - outerPadY * 2);

        var innerPad = Math.Max(28, _panelBounds.Width / 32);
        var contentGap = Math.Max(34, _panelBounds.Width / 36);
        var leftWidth = (int)(_panelBounds.Width * 0.34f);

        _penguinBounds = new Rectangle(
            _panelBounds.X + innerPad,
            _panelBounds.Y + innerPad,
            leftWidth,
            _panelBounds.Height - innerPad * 2);

        _contentBounds = new Rectangle(
            _penguinBounds.Right + contentGap,
            _panelBounds.Y + innerPad + 8,
            _panelBounds.Right - (_penguinBounds.Right + contentGap) - innerPad,
            _panelBounds.Height - innerPad * 2 - 8);
    }

    private void PositionButtons()
    {
        if (_contentBounds == Rectangle.Empty)
        {
            CalculateLayout();
        }

        var gap = 12;
        var totalWidth = _okButton.Width + _snoozeButton.Width + gap;
        var x = _contentBounds.X + Math.Max(0, (_contentBounds.Width - totalWidth) / 2);
        var y = _contentBounds.Bottom - _okButton.Height - 10;
        _okButton.Location = new Point(x, y);
        _snoozeButton.Location = new Point(_okButton.Right + gap, y);
    }

    private void DrawPanel(Graphics g)
    {
        using var shadow = new SolidBrush(Color.FromArgb(65, 0, 0, 0));
        using var fill = new SolidBrush(Color.FromArgb(244, 248, 252));
        using var path = RoundedRect(new Rectangle(_panelBounds.X + 8, _panelBounds.Y + 10, _panelBounds.Width, _panelBounds.Height), 30);
        g.FillPath(shadow, path);

        using var panelPath = RoundedRect(_panelBounds, 30);
        g.FillPath(fill, panelPath);
    }

    private void DrawDecorations(Graphics g)
    {
        using var line = new Pen(Color.FromArgb(26, 92, 117), 3f);
        using var pale = new SolidBrush(Color.FromArgb(36, 26, 92, 117));

        g.DrawLine(line, _contentBounds.X, _contentBounds.Y + 84, _contentBounds.Right - 8, _contentBounds.Y + 84);
        g.FillEllipse(pale, _contentBounds.Right - 120, _contentBounds.Bottom - 150, 92, 92);
        g.FillEllipse(pale, _contentBounds.X + 24, _contentBounds.Bottom - 84, 44, 44);
    }

    private void DrawText(Graphics g)
    {
        var titleRect = new Rectangle(_contentBounds.X, _contentBounds.Y + 8, _contentBounds.Width, 72);
        var textRect = new Rectangle(_contentBounds.X, titleRect.Bottom + 30, _contentBounds.Width, 105);
        var bubbleRect = new Rectangle(_contentBounds.X, textRect.Bottom + 24, _contentBounds.Width, 70);

        using var titleFont = new Font("Segoe UI Black", ScaleFont(32f), FontStyle.Bold);
        using var bodyFont = new Font("Segoe UI Semibold", ScaleFont(16f), FontStyle.Bold);
        using var smallFont = new Font("Segoe UI", ScaleFont(12.5f), FontStyle.Regular);
        using var titleBrush = new SolidBrush(Color.FromArgb(20, 28, 36));
        using var bodyBrush = new SolidBrush(Color.FromArgb(53, 65, 75));
        using var bubbleFill = new SolidBrush(Color.FromArgb(255, 247, 222));
        using var bubbleBorder = new Pen(Color.FromArgb(250, 220, 139), 1.5f);

        var titleFormat = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };
        var bodyFormat = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near,
            Trimming = StringTrimming.Word
        };

        g.DrawString("20 dakika doldu!", titleFont, titleBrush, titleRect, titleFormat);
        g.DrawString("Ayaga kalk, omuzlarini gevset, bir bardak su ic. Sandalye seni ozledi ama belin bu iliskiyi onaylamiyor.", bodyFont, bodyBrush, textRect, bodyFormat);

        using var bubblePath = RoundedRect(bubbleRect, 22);
        g.FillPath(bubbleFill, bubblePath);
        g.DrawPath(bubbleBorder, bubblePath);
        g.DrawString("Penguen raporu: hareket yoksa buz pistinde kayma yok.", smallFont, bodyBrush, Rectangle.Inflate(bubbleRect, -20, -13), bodyFormat);
    }

    private void DrawPenguin(Graphics g)
    {
        var size = Math.Min(_penguinBounds.Width * 0.92f, _penguinBounds.Height * 0.78f);
        var x = _penguinBounds.X + (_penguinBounds.Width - size * 0.86f) / 2f;
        var y = _penguinBounds.Y + (_penguinBounds.Height - size * 1.24f) / 2f + size * 0.05f;
        var body = new RectangleF(x, y + size * 0.16f, size * 0.86f, size * 1.05f);
        var head = new RectangleF(x + size * 0.07f, y, size * 0.72f, size * 0.64f);

        using var black = new SolidBrush(Color.FromArgb(23, 31, 42));
        using var white = new SolidBrush(Color.FromArgb(250, 252, 255));
        using var yellow = new SolidBrush(Color.FromArgb(245, 188, 55));
        using var beakShadow = new SolidBrush(Color.FromArgb(223, 151, 40));
        using var blush = new SolidBrush(Color.FromArgb(244, 117, 126));
        using var eyeWhite = new SolidBrush(Color.FromArgb(255, 255, 255));
        using var eyeDark = new SolidBrush(Color.FromArgb(8, 12, 18));
        using var eyeSpark = new SolidBrush(Color.FromArgb(255, 255, 255));
        using var wing = new Pen(Color.FromArgb(23, 31, 42), Math.Max(10, size * 0.08f));

        g.FillEllipse(black, body);
        g.FillEllipse(black, head);
        g.FillEllipse(white, x + size * 0.15f, y + size * 0.31f, size * 0.55f, size * 0.78f);

        wing.StartCap = LineCap.Round;
        wing.EndCap = LineCap.Round;
        g.DrawCurve(wing, new[]
        {
            new PointF(x + size * 0.05f, y + size * 0.58f),
            new PointF(x - size * 0.07f, y + size * 0.72f),
            new PointF(x + size * 0.02f, y + size * 0.92f)
        });
        g.DrawCurve(wing, new[]
        {
            new PointF(x + size * 0.78f, y + size * 0.55f),
            new PointF(x + size * 0.98f, y + size * 0.45f),
            new PointF(x + size * 0.94f, y + size * 0.24f)
        });

        var leftEye = new RectangleF(x + size * 0.23f, y + size * 0.19f, size * 0.105f, size * 0.125f);
        var rightEye = new RectangleF(x + size * 0.515f, y + size * 0.19f, size * 0.105f, size * 0.125f);
        g.FillEllipse(eyeWhite, leftEye);
        g.FillEllipse(eyeWhite, rightEye);
        g.FillEllipse(eyeDark, x + size * 0.265f, y + size * 0.225f, size * 0.045f, size * 0.060f);
        g.FillEllipse(eyeDark, x + size * 0.550f, y + size * 0.225f, size * 0.045f, size * 0.060f);
        g.FillEllipse(eyeSpark, x + size * 0.278f, y + size * 0.232f, size * 0.014f, size * 0.014f);
        g.FillEllipse(eyeSpark, x + size * 0.563f, y + size * 0.232f, size * 0.014f, size * 0.014f);
        g.FillEllipse(blush, x + size * 0.13f, y + size * 0.34f, size * 0.12f, size * 0.07f);
        g.FillEllipse(blush, x + size * 0.59f, y + size * 0.34f, size * 0.12f, size * 0.07f);

        var upperBeak = new[]
        {
            new PointF(x + size * 0.350f, y + size * 0.315f),
            new PointF(x + size * 0.430f, y + size * 0.345f),
            new PointF(x + size * 0.500f, y + size * 0.315f),
            new PointF(x + size * 0.457f, y + size * 0.385f),
            new PointF(x + size * 0.393f, y + size * 0.385f)
        };

        var lowerBeak = new[]
        {
            new PointF(x + size * 0.393f, y + size * 0.382f),
            new PointF(x + size * 0.457f, y + size * 0.382f),
            new PointF(x + size * 0.425f, y + size * 0.430f)
        };

        g.FillPolygon(yellow, upperBeak);
        g.FillPolygon(beakShadow, lowerBeak);

        using var smilePen = new Pen(Color.FromArgb(8, 12, 18), Math.Max(2f, size * 0.012f));
        smilePen.StartCap = LineCap.Round;
        smilePen.EndCap = LineCap.Round;
        g.DrawArc(
            smilePen,
            x + size * 0.303f,
            y + size * 0.405f,
            size * 0.245f,
            size * 0.145f,
            18,
            144);

        g.FillEllipse(yellow, x + size * 0.16f, y + size * 1.15f, size * 0.25f, size * 0.11f);
        g.FillEllipse(yellow, x + size * 0.49f, y + size * 1.15f, size * 0.25f, size * 0.11f);

        using var signFont = new Font("Segoe UI Black", ScaleFont(11.5f), FontStyle.Bold);
        using var signBrush = new SolidBrush(Color.FromArgb(25, 31, 39));
        var sign = new RectangleF(x + size * 0.46f, y + size * 0.01f, size * 0.34f, size * 0.14f);
        using var signPath = RoundedRect(Rectangle.Round(sign), 9);
        g.FillPath(yellow, signPath);
        g.DrawString("KALK!", signFont, signBrush, sign, new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        });
    }

    private float ScaleFont(float baseSize)
    {
        return Math.Max(baseSize * 0.72f, Math.Min(baseSize, _panelBounds.Width / 42f));
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}
