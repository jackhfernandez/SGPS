/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Crea la pagina de inicio del shell: saludo con el nombre del
 *    usuario, rejilla de tarjetas grandes por grupo de navegacion filtradas
 *    por permisos, y una franja inferior con cifras reales de la capa de
 *    logica."
 * 3. Cambios del equipo: La franja inferior solo muestra cifras que la capa
 *    Logica ya sabe entregar hoy (ListarProyectos, ListarUsuarios,
 *    ListarRoles). No se replican los KPIs de capacidad y velocidad del
 *    mockup porque no hay consultas que los alimenten y poner numeros
 *    inventados en la pantalla de inicio seria enganoso.
 */

using System.Drawing.Drawing2D;
using Logica;

namespace Presentacion.Ui;

/// <summary>
/// Pagina de inicio del shell. Presenta cada grupo de la navegacion como una
/// tarjeta grande, filtrada por los permisos del usuario.
/// </summary>
public class PaginaResumen : UserControl
{
    private const int Columnas = 4;

    /// <summary>Se dispara cuando el usuario pulsa una tarjeta de modulo construido.</summary>
    public event EventHandler<GrupoNav>? GrupoElegido;

    public PaginaResumen()
    {
        BackColor = Tema.Crema;
        AutoScroll = false;
        Padding = new Padding(0);

        var raiz = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Tema.Crema,
            ColumnCount = 1,
            RowCount = 3,
            AutoScroll = false
        };

        raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));

        raiz.Controls.Add(CrearCabecera(), 0, 0);
        raiz.Controls.Add(CrearRejillaTarjetas(), 0, 1);
        raiz.Controls.Add(CrearFranjaCifras(), 0, 2);

        Controls.Add(raiz);
    }

    // -------------------------------------------------------------- Cabecera

    private static Control CrearCabecera()
    {
        var cabecera = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Crema };

        var usuario = SesionContextoLN.UsuarioActual;
        var nombre = usuario?.Nombres?.Split(' ').FirstOrDefault() ?? "usuario";

        var roles = SesionContextoLN.RolesActuales.Select(r => r.NombreRol).ToList();
        var textoRoles = roles.Count == 0 ? "SIN ROL" : string.Join("  ·  ", roles).ToUpperInvariant();

        var rolPrincipal = SesionContextoLN.RolesActuales
            .Select(r => PermisoLN.NormalizarRol(r.NombreRol))
            .FirstOrDefault(r => r is not null);

        var lblEtiqueta = new Label
        {
            Text = textoRoles,
            Font = Tema.Etiqueta,
            ForeColor = rolPrincipal is null ? Tema.TextoTenue : Tema.ColorDeRol(rolPrincipal.Value),
            Location = new Point(0, 4),
            Size = new Size(700, 20),
            BackColor = Color.Transparent
        };

        var lblSaludo = new Label
        {
            Text = $"{Saludo()}, {nombre}.",
            Font = Tema.TituloHero,
            ForeColor = Tema.TextoOscuro,
            Location = new Point(-4, 26),
            Size = new Size(900, 54),
            BackColor = Color.Transparent,
            UseCompatibleTextRendering = true
        };

        var lblSub = new Label
        {
            Text = "Estos son los módulos a los que tienes acceso.",
            Font = Tema.Cuerpo,
            ForeColor = Tema.TextoTenue,
            Location = new Point(0, 86),
            Size = new Size(900, 22),
            BackColor = Color.Transparent
        };

        cabecera.Controls.AddRange([lblEtiqueta, lblSaludo, lblSub]);

        return cabecera;
    }

    private static string Saludo() => DateTime.Now.Hour switch
    {
        < 12 => "Buen día",
        < 19 => "Buenas tardes",
        _ => "Buenas noches"
    };

    // -------------------------------------------------------------- Tarjetas

    private Control CrearRejillaTarjetas()
    {
        var visibles = MapaNavegacion.TodosLosGrupos
            .Where(MapaNavegacion.EsVisible)
            .ToList();

        var filas = Math.Max(1, (int)Math.Ceiling(visibles.Count / (double)Columnas));

        var rejilla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Tema.Crema,
            ColumnCount = Columnas,
            RowCount = filas,
            AutoScroll = false,
            Padding = new Padding(0, 8, 0, 8)
        };

        for (var c = 0; c < Columnas; c++)
        {
            rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / Columnas));
        }

        for (var f = 0; f < filas; f++)
        {
            rejilla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / filas));
        }

        for (var i = 0; i < visibles.Count; i++)
        {
            var grupo = visibles[i];
            var construido = MapaNavegacion.GrupoConstruido(grupo);

            var tarjeta = new TarjetaModulo(grupo, MapaNavegacion.NivelDelGrupo(grupo), construido)
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 16)
            };

            if (construido)
            {
                tarjeta.Click += (_, _) => GrupoElegido?.Invoke(this, grupo);
            }

            rejilla.Controls.Add(tarjeta, i % Columnas, i / Columnas);
        }

        return rejilla;
    }

    // ---------------------------------------------------------------- Cifras

    private static Control CrearFranjaCifras()
    {
        var contenedor = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Tema.Crema,
            Padding = new Padding(0, 0, 16, 0)
        };

        var tarjeta = new TarjetaTira(LeerCifras())
        {
            Dock = DockStyle.Fill,
            Radio = 12,
            ColorRelleno = Tema.VerdeProfundo,
            ColorBorde = Tema.VerdeProfundo,
            ColorFondoDetras = Tema.Crema
        };

        contenedor.Controls.Add(tarjeta);

        return contenedor;
    }

    /// <summary>
    /// Cifras que la capa Logica ya expone hoy. Si la base de datos no
    /// responde se muestran guiones en lugar de propagar la excepcion a la
    /// pantalla de inicio.
    /// </summary>
    private static (string Etiqueta, string Valor)[] LeerCifras()
    {
        var cifras = new List<(string, string)>();

        try
        {
            var proyectos = new ProyectoLN().ListarProyectos();
            cifras.Add(("PROYECTOS ACTIVOS", proyectos.Count(p => p.EsActivo).ToString()));
            cifras.Add(("PROYECTOS TOTALES", proyectos.Count.ToString()));
        }
        catch (Exception)
        {
            cifras.Add(("PROYECTOS ACTIVOS", "—"));
            cifras.Add(("PROYECTOS TOTALES", "—"));
        }

        if (PermisoLN.PuedeVer(Modulo.UsuarioGestion))
        {
            try
            {
                cifras.Add(("USUARIOS", new UsuarioLN().ListarUsuarios().Count.ToString()));
                cifras.Add(("ROLES", new RolLN().ListarRoles().Count.ToString()));
            }
            catch (Exception)
            {
                cifras.Add(("USUARIOS", "—"));
                cifras.Add(("ROLES", "—"));
            }
        }
        else
        {
            var visibles = MapaNavegacion.TodosLosGrupos.Count(MapaNavegacion.EsVisible);
            cifras.Add(("MÓDULOS DISPONIBLES", visibles.ToString()));
        }

        return [.. cifras];
    }

    /// <summary>Tarjeta oscura con cifras en columnas, al pie de la pagina.</summary>
    private sealed class TarjetaTira(( string Etiqueta, string Valor)[] cifras) : PanelTarjeta
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (cifras.Length == 0)
            {
                return;
            }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var anchoColumna = Width / cifras.Length;

            for (var i = 0; i < cifras.Length; i++)
            {
                var x = i * anchoColumna;
                var (etiqueta, valor) = cifras[i];

                if (i > 0)
                {
                    using var separador = new Pen(Tema.TealSuave);
                    g.DrawLine(separador, x, 26, x, Height - 26);
                }

                using (var pincelEtiqueta = new SolidBrush(Tema.TealSuave))
                {
                    g.DrawString(etiqueta, Tema.Etiqueta, pincelEtiqueta,
                        new RectangleF(x + 28, 26, anchoColumna - 40, 20));
                }

                using (var pincelValor = new SolidBrush(Tema.Blanco))
                {
                    g.DrawString(valor, Tema.Numero, pincelValor,
                        new RectangleF(x + 24, 46, anchoColumna - 40, 48));
                }
            }
        }
    }
}
