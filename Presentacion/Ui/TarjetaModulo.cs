/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Crea una tarjeta grande clicable para la pagina de inicio, que
 *    muestre glifo, titulo, descripcion y un chip con el nivel de acceso,
 *    con estado hover y estado atenuado para modulos aun no construidos."
 * 3. Cambios del equipo: El estado atenuado no se pinta en gris plano sino
 *    mezclando cada color con el crema del fondo, para que la tarjeta siga
 *    perteneciendo a la paleta en lugar de parecer deshabilitada por error.
 */

using System.Drawing.Drawing2D;
using Logica;

namespace Presentacion.Ui;

/// <summary>
/// El "boton grande" de la pagina de Resumen: un grupo de la navegacion
/// presentado como tarjeta. Si el grupo aun no tiene modulos implementados
/// se dibuja atenuado y no navega.
/// </summary>
public class TarjetaModulo : PanelTarjeta
{
    private bool _hover;

    public TarjetaModulo(GrupoNav grupo, NivelAcceso nivel, bool construido)
    {
        Grupo = grupo;
        Nivel = nivel;
        Construido = construido;

        Radio = 12;
        ColorRelleno = construido ? Tema.Blanco : Color.FromArgb(250, 248, 243);
        ColorBorde = Tema.Borde;
        Cursor = construido ? Cursors.Hand : Cursors.Default;
        Margin = new Padding(0, 0, 16, 16);

        AccessibleName = grupo.Texto;
        AccessibleRole = AccessibleRole.PushButton;
    }

    public GrupoNav Grupo { get; }

    public NivelAcceso Nivel { get; }

    public bool Construido { get; }

    protected override void OnMouseEnter(EventArgs e)
    {
        if (Construido)
        {
            _hover = true;
            ColorBorde = Tema.Teal;
            Invalidate();
        }

        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        ColorBorde = Tema.Borde;
        Invalidate();
        base.OnMouseLeave(e);
    }

    /// <summary>Mezcla un color con el crema del fondo para el estado atenuado.</summary>
    private Color Atenuar(Color color, float mezcla = 0.55f)
    {
        if (Construido)
        {
            return color;
        }

        return Color.FromArgb(
            (int)(color.R + (Tema.Crema.R - color.R) * mezcla),
            (int)(color.G + (Tema.Crema.G - color.G) * mezcla),
            (int)(color.B + (Tema.Crema.B - color.B) * mezcla));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Nada de asignar propiedades del control aqui: los setters llaman a
        // Invalidate() y eso encadena un repintado infinito.
        base.OnPaint(e);

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        const int margen = 20;

        // Glifo dentro de un cuadro teal suave.
        var cajaGlifo = new Rectangle(margen, margen, 44, 44);

        using (var camino = CrearCamino(cajaGlifo, 10))
        using (var fondoGlifo = new SolidBrush(Atenuar(_hover ? Tema.Teal : Tema.SeleccionSuave)))
        {
            g.FillPath(fondoGlifo, camino);
        }

        if (Tema.HayIconos)
        {
            using var pincelGlifo = new SolidBrush(Atenuar(_hover ? Tema.Blanco : Tema.Teal));
            using var centro = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(Grupo.Glifo, Tema.IconoGrande, pincelGlifo, cajaGlifo, centro);
        }

        // Titulo. Se reduce la fuente hasta que quepa, para que un nombre de
        // modulo largo no salga cortado a la mitad.
        var anchoTitulo = Width - margen * 2;

        using (var fuenteTitulo = FuenteQueQuepa(g, Grupo.Texto, Tema.TituloTarjeta, anchoTitulo))
        using (var pincelTitulo = new SolidBrush(Atenuar(Tema.TextoOscuro)))
        using (var formatoTitulo = new StringFormat
        {
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        })
        {
            g.DrawString(Grupo.Texto, fuenteTitulo, pincelTitulo,
                new RectangleF(margen, cajaGlifo.Bottom + 14, anchoTitulo, 26), formatoTitulo);
        }

        // Descripcion.
        using (var pincelDesc = new SolidBrush(Atenuar(Tema.TextoTenue)))
        using (var formatoDesc = new StringFormat { Trimming = StringTrimming.EllipsisWord })
        {
            g.DrawString(Grupo.Descripcion, Tema.Cuerpo, pincelDesc,
                new RectangleF(margen, cajaGlifo.Bottom + 42, Width - margen * 2, 40), formatoDesc);
        }

        DibujarChip(g, margen);
    }

    /// <summary>
    /// Devuelve la fuente base, o una copia reducida hasta que el texto quepa
    /// en <paramref name="anchoDisponible"/>. El llamador debe liberarla.
    /// </summary>
    private static Font FuenteQueQuepa(Graphics g, string texto, Font baseFuente, int anchoDisponible)
    {
        var tamano = baseFuente.Size;
        var fuente = new Font(baseFuente.FontFamily, tamano, baseFuente.Style);

        while (tamano > 9F && g.MeasureString(texto, fuente).Width > anchoDisponible)
        {
            fuente.Dispose();
            tamano -= 0.5F;
            fuente = new Font(baseFuente.FontFamily, tamano, baseFuente.Style);
        }

        return fuente;
    }

    private void DibujarChip(Graphics g, int margen)
    {
        var texto = Construido ? Tema.TextoNivel(Nivel) : "En construcción";
        var colorTexto = Construido ? Tema.Teal : Tema.TextoTenue;
        var colorFondo = Construido ? Tema.SeleccionSuave : Tema.Crema;

        var ancho = (int)g.MeasureString(texto, Tema.Etiqueta).Width + 18;
        var chip = new Rectangle(margen, Height - margen - 22, ancho, 22);

        using (var camino = CrearCamino(chip, 11))
        using (var fondo = new SolidBrush(colorFondo))
        {
            g.FillPath(fondo, camino);
        }

        using var pincel = new SolidBrush(colorTexto);
        using var centro = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        g.DrawString(texto, Tema.Etiqueta, pincel, chip, centro);
    }
}
