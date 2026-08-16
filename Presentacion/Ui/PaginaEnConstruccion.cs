/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Crea un UserControl marcador de posicion, con la paleta del
 *    sistema, que se muestre cuando el modulo destino todavia no tiene
 *    implementacion."
 * 3. Cambios del equipo: Se indica el nombre del formulario pendiente para
 *    que sirva de guia al equipo mientras se construyen los 13 modulos que
 *    siguen siendo plantillas.
 */

using System.Drawing.Drawing2D;

namespace Presentacion.Ui;

/// <summary>
/// Pagina que ocupa el host cuando el modulo elegido aun no esta construido.
/// Evita mostrar el formulario plantilla vacio de 800x450.
/// </summary>
public class PaginaEnConstruccion : Control
{
    private readonly string _modulo;
    private readonly string _archivo;

    public PaginaEnConstruccion(string modulo, string archivo)
    {
        _modulo = modulo;
        _archivo = archivo;

        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint,
            true);

        BackColor = Tema.Crema;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        using (var fondo = new SolidBrush(Tema.Crema))
        {
            g.FillRectangle(fondo, ClientRectangle);
        }

        const int anchoCaja = 420;
        var centroX = Width / 2;
        var y = Math.Max(80, Height / 2 - 130);

        using var centrado = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        // Circulo con el glifo de herramienta.
        var circulo = new Rectangle(centroX - 36, y, 72, 72);

        using (var fondoCirculo = new SolidBrush(Tema.SeleccionSuave))
        {
            g.FillEllipse(fondoCirculo, circulo);
        }

        if (Tema.HayIconos)
        {
            using var pincelGlifo = new SolidBrush(Tema.Teal);
            g.DrawString(Tema.Glifos.Construccion, Tema.IconoGrande, pincelGlifo, circulo, centrado);
        }

        using (var pincelEtiqueta = new SolidBrush(Tema.TextoTenue))
        {
            g.DrawString("MÓDULO PENDIENTE", Tema.Etiqueta, pincelEtiqueta,
                new RectangleF(centroX - anchoCaja / 2f, y + 96, anchoCaja, 20), centrado);
        }

        using (var pincelTitulo = new SolidBrush(Tema.TextoOscuro))
        {
            g.DrawString(_modulo, Tema.TituloSeccion, pincelTitulo,
                new RectangleF(centroX - anchoCaja / 2f, y + 120, anchoCaja, 40), centrado);
        }

        using (var pincelCuerpo = new SolidBrush(Tema.TextoTenue))
        {
            g.DrawString(
                $"Esta pantalla todavía no está implementada.\nSe construye en {_archivo}.",
                Tema.Cuerpo, pincelCuerpo,
                new RectangleF(centroX - anchoCaja / 2f, y + 168, anchoCaja, 60), centrado);
        }

        base.OnPaint(e);
    }
}
