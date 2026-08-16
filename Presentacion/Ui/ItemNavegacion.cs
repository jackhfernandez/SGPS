/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Crea un UserControl para un item de barra lateral oscura en
 *    WinForms, con glifo opcional, texto, chevron de grupo y estados
 *    normal / hover / activo, dibujado por completo en OnPaint."
 * 3. Cambios del equipo: Se dibuja todo en OnPaint en lugar de componer
 *    Labels, para que el hover no parpadee y para que el control siga
 *    siendo una sola superficie clicable.
 */

using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Presentacion.Ui;

/// <summary>
/// Item de la barra lateral. Sirve tanto para un grupo (con glifo y chevron)
/// como para un sub-item (sin glifo y con sangria).
/// </summary>
public class ItemNavegacion : Control
{
    public const int AltoGrupo = 46;
    public const int AltoSubItem = 34;

    private bool _hover;
    private bool _activo;
    private bool _expandido;

    public ItemNavegacion(string texto, string? glifo, bool esGrupo, bool tieneHijos)
    {
        Texto = texto;
        Glifo = glifo;
        EsGrupo = esGrupo;
        TieneHijos = tieneHijos;

        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint,
            true);

        Height = esGrupo ? AltoGrupo : AltoSubItem;
        Cursor = Cursors.Hand;
        BackColor = Tema.VerdeProfundo;
        Font = esGrupo ? Tema.CuerpoSemi : Tema.Cuerpo;

        // El texto se dibuja a mano en OnPaint, pero se publica igualmente
        // para que el item tenga nombre en el arbol de accesibilidad.
        Text = texto;
        AccessibleName = texto;
        AccessibleRole = AccessibleRole.MenuItem;
    }

    public string Texto { get; }

    public string? Glifo { get; }

    public bool EsGrupo { get; }

    public bool TieneHijos { get; }

    /// <summary>Marca el item como la pantalla que se esta viendo.</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Activo
    {
        get => _activo;
        set { _activo = value; Invalidate(); }
    }

    /// <summary>Solo para grupos: cambia la direccion del chevron.</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Expandido
    {
        get => _expandido;
        set { _expandido = value; Invalidate(); }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var fondo = Tema.VerdeProfundo;

        if (_activo)
        {
            fondo = Tema.VerdeHover;
        }
        else if (_hover)
        {
            fondo = Color.FromArgb(19, 66, 58);
        }

        using (var pincel = new SolidBrush(fondo))
        {
            g.FillRectangle(pincel, ClientRectangle);
        }

        // Barra teal de 3px que marca el item activo.
        if (_activo)
        {
            using var acento = new SolidBrush(Tema.Teal);
            g.FillRectangle(acento, new Rectangle(0, 0, 3, Height));
        }

        var sangriaTexto = EsGrupo ? 52 : 54;
        var colorTexto = _activo
            ? Tema.Blanco
            : _hover
                ? Tema.Blanco
                : EsGrupo
                    ? Color.FromArgb(226, 232, 229)
                    : Tema.TealSuave;

        if (EsGrupo && Glifo is not null && Tema.HayIconos)
        {
            using var pincelGlifo = new SolidBrush(_activo || _hover ? Tema.Dorado : Tema.TealSuave);
            using var formato = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(Glifo, Tema.Icono, pincelGlifo,
                new RectangleF(20, 0, 24, Height), formato);
        }
        else if (EsGrupo && !Tema.HayIconos)
        {
            // Sin fuente de iconos: un punto sobrio en lugar de un cuadro vacio.
            using var punto = new SolidBrush(_activo || _hover ? Tema.Dorado : Tema.TealSuave);
            g.FillEllipse(punto, 28, Height / 2 - 3, 6, 6);
        }

        using (var pincelTexto = new SolidBrush(colorTexto))
        using (var formatoTexto = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        })
        {
            g.DrawString(Texto, Font, pincelTexto,
                new RectangleF(sangriaTexto, 0, Width - sangriaTexto - 34, Height), formatoTexto);
        }

        if (EsGrupo && TieneHijos && Tema.HayIconos)
        {
            using var pincelChevron = new SolidBrush(Tema.TealSuave);
            using var formatoChevron = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using var fuenteChevron = new Font(Tema.Icono.FontFamily, 8F);

            g.DrawString(
                _expandido ? Tema.Glifos.ChevronAbajo : Tema.Glifos.ChevronDerecha,
                fuenteChevron, pincelChevron,
                new RectangleF(Width - 34, 0, 24, Height), formatoChevron);
        }

        base.OnPaint(e);
    }
}
