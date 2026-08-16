/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Crea un Panel derivado para WinForms que pinte esquinas
 *    redondeadas y un borde de 1px, con propiedades de radio, color de
 *    borde y color de relleno, usando GraphicsPath y antialiasing."
 * 3. Cambios del equipo: Se dibuja el fondo del contenedor padre antes de
 *    la tarjeta para que el antialiasing de las esquinas no deje un halo
 *    gris, ya que WinForms no compone con transparencia real.
 */

using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Presentacion.Ui;

/// <summary>
/// Panel con esquinas redondeadas y borde. WinForms no ofrece esquinas
/// redondeadas de forma nativa, por eso el pintado propio.
/// </summary>
public class PanelTarjeta : Panel
{
    private int _radio = 10;
    private Color _colorBorde = Tema.Borde;
    private Color _colorRelleno = Tema.Blanco;
    private int _grosorBorde = 1;

    public PanelTarjeta()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint,
            true);

        BackColor = Color.Transparent;
    }

    // Estas tarjetas se construyen siempre desde codigo, nunca desde el
    // disenador, por eso ninguna propiedad se serializa.

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Radio
    {
        get => _radio;
        set { _radio = Math.Max(0, value); Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ColorBorde
    {
        get => _colorBorde;
        set { _colorBorde = value; Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ColorRelleno
    {
        get => _colorRelleno;
        set { _colorRelleno = value; Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int GrosorBorde
    {
        get => _grosorBorde;
        set { _grosorBorde = Math.Max(0, value); Invalidate(); }
    }

    /// <summary>Color con el que se limpia el area fuera de las esquinas redondeadas.</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ColorFondoDetras { get; set; } = Tema.Crema;

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // El area exterior a la curva se rellena con el fondo del contenedor
        // para evitar el halo que deja el antialiasing sobre negro.
        using (var fondo = new SolidBrush(ColorFondoDetras))
        {
            g.FillRectangle(fondo, ClientRectangle);
        }

        var borde = new Rectangle(0, 0, Width - 1, Height - 1);

        using var camino = CrearCamino(borde, _radio);
        using (var relleno = new SolidBrush(_colorRelleno))
        {
            g.FillPath(relleno, camino);
        }

        if (_grosorBorde > 0)
        {
            using var lapiz = new Pen(_colorBorde, _grosorBorde);
            g.DrawPath(lapiz, camino);
        }

        base.OnPaint(e);
    }

    internal static GraphicsPath CrearCamino(Rectangle rect, int radio)
    {
        var camino = new GraphicsPath();

        if (radio <= 0)
        {
            camino.AddRectangle(rect);
            return camino;
        }

        var diametro = Math.Min(radio * 2, Math.Min(rect.Width, rect.Height));

        camino.AddArc(rect.X, rect.Y, diametro, diametro, 180, 90);
        camino.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270, 90);
        camino.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
        camino.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90, 90);
        camino.CloseFigure();

        return camino;
    }
}
