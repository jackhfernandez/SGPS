/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Convierte Principal.cs de contenedor MDI con MenuStrip a un
 *    shell con barra lateral: navegacion generada desde MapaNavegacion y
 *    filtrada por PermisoLN, acordeon de expansion unica, y modulos
 *    incrustados en un panel de contenido en vez de ventanas flotantes."
 * 3. Cambios del equipo: Se conservo el volcado de diagnostico a
 *    permisos_debug.log y la doble validacion de permisos al navegar
 *    (defensa en profundidad), que ya existian en la version con menus.
 */

using System.Drawing.Drawing2D;
using Logica;
using Presentacion.Cliente;
using Presentacion.Ui;

namespace Presentacion;

public partial class Principal : Form
{
    private readonly Dictionary<GrupoNav, List<ItemNavegacion>> _subItems = [];
    private readonly Dictionary<GrupoNav, ItemNavegacion> _cabecerasGrupo = [];
    private readonly List<ItemNavegacion> _todosLosItems = [];

    private GrupoNav? _grupoExpandido;
    private Control? _contenidoActual;

    public Principal()
    {
        InitializeComponent();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        AplicarMenuSegunRol();
    }

    private void AplicarMenuSegunRol()
    {
        var usuario = SesionContextoLN.UsuarioActual;

        if (usuario is null)
        {
            return;
        }

        RegistrarDiagnostico(usuario.Email);
        MostrarRolesNoReconocidos();

        try
        {
            ConstruirNavegacion();
        }
        catch (Exception ex)
        {
            Registrar("EXCEPCION construyendo la navegacion: " + ex);
        }

        // El Cliente entra directo a su portal restringido, como antes.
        if (SesionContextoLN.TieneRol("Cliente"))
        {
            var portal = MapaNavegacion.Grupos
                .FirstOrDefault(g => g.Texto == "Cliente");

            if (portal is not null && MapaNavegacion.EsVisible(portal))
            {
                AbrirGrupo(portal);
                return;
            }
        }

        MostrarResumen();
    }

    // ------------------------------------------------------------ Navegacion

    private void ConstruirNavegacion()
    {
        flowNav.SuspendLayout();
        flowAdmin.SuspendLayout();

        AgregarItemResumen();

        foreach (var grupo in MapaNavegacion.Grupos.Where(MapaNavegacion.EsVisible))
        {
            AgregarGrupo(grupo, flowNav);
        }

        if (MapaNavegacion.EsVisible(MapaNavegacion.Administracion))
        {
            AgregarGrupo(MapaNavegacion.Administracion, flowAdmin);
        }

        flowAdmin.ResumeLayout();
        flowNav.ResumeLayout();

        Registrar(EstadoNavegacion());
    }

    private void AgregarItemResumen()
    {
        var item = new ItemNavegacion("Resumen", Tema.Glifos.Inicio, esGrupo: true, tieneHijos: false)
        {
            Width = flowNav.ClientSize.Width,
            Margin = Padding.Empty
        };

        item.Click += (_, _) =>
        {
            ContraerTodo();
            MostrarResumen();
        };

        flowNav.Controls.Add(item);
        _todosLosItems.Add(item);
    }

    private void AgregarGrupo(GrupoNav grupo, FlowLayoutPanel contenedor)
    {
        var itemsVisibles = MapaNavegacion.ItemsVisibles(grupo).ToList();

        var cabecera = new ItemNavegacion(grupo.Texto, grupo.Glifo, esGrupo: true, tieneHijos: itemsVisibles.Count > 1)
        {
            Width = contenedor.ClientSize.Width,
            Margin = Padding.Empty
        };

        contenedor.Controls.Add(cabecera);
        _cabecerasGrupo[grupo] = cabecera;
        _todosLosItems.Add(cabecera);

        var hijos = new List<ItemNavegacion>();

        foreach (var item in itemsVisibles)
        {
            var subItem = new ItemNavegacion(item.Texto, null, esGrupo: false, tieneHijos: false)
            {
                Width = contenedor.ClientSize.Width,
                Margin = Padding.Empty,
                Visible = false,
                Tag = item
            };

            subItem.Click += (_, _) => Navegar(grupo, item);

            contenedor.Controls.Add(subItem);
            hijos.Add(subItem);
            _todosLosItems.Add(subItem);
        }

        _subItems[grupo] = hijos;

        // Un solo modulo visible: el grupo navega directo, sin desplegar. La
        // cabecera lleva el Tag del item para que MarcarActivo la resalte a
        // ella, ya que el sub-item nunca se muestra.
        if (itemsVisibles.Count == 1)
        {
            cabecera.Tag = itemsVisibles[0];
            cabecera.Click += (_, _) =>
            {
                ContraerTodo();
                Navegar(grupo, itemsVisibles[0]);
            };
        }
        else
        {
            cabecera.Click += (_, _) => AlternarGrupo(grupo);
        }
    }

    /// <summary>Acordeon de expansion unica: al abrir un grupo se cierran los demas.</summary>
    private void AlternarGrupo(GrupoNav grupo)
    {
        var yaAbierto = ReferenceEquals(_grupoExpandido, grupo);

        ContraerTodo();

        if (yaAbierto)
        {
            return;
        }

        _grupoExpandido = grupo;

        if (_cabecerasGrupo.TryGetValue(grupo, out var cabecera))
        {
            cabecera.Expandido = true;
        }

        if (_subItems.TryGetValue(grupo, out var hijos))
        {
            foreach (var hijo in hijos)
            {
                hijo.Visible = true;
            }
        }
    }

    private void ContraerTodo()
    {
        _grupoExpandido = null;

        foreach (var cabecera in _cabecerasGrupo.Values)
        {
            cabecera.Expandido = false;
        }

        foreach (var hijo in _subItems.Values.SelectMany(h => h))
        {
            hijo.Visible = false;
        }
    }

    /// <summary>Abre el primer modulo visible de un grupo (usado por las tarjetas del Resumen).</summary>
    private void AbrirGrupo(GrupoNav grupo)
    {
        var item = MapaNavegacion.ItemsVisibles(grupo).FirstOrDefault(i => i.Construido)
                   ?? MapaNavegacion.ItemsVisibles(grupo).FirstOrDefault();

        if (item is null)
        {
            return;
        }

        if (MapaNavegacion.ItemsVisibles(grupo).Count() > 1)
        {
            AlternarGrupo(grupo);
        }

        Navegar(grupo, item);
    }

    private void Navegar(GrupoNav grupo, ItemNav item)
    {
        try
        {
            // Defensa en profundidad: el modulo se revalida aunque el item
            // solo se muestre cuando hay permiso.
            PermisoLN.ValidarAcceso(item.Modulo, NivelAcceso.Lectura);
        }
        catch (PermisoDenegadoException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Acceso denegado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        Control contenido;

        if (item.Construido)
        {
            try
            {
                contenido = PrepararIncrustado(item.Crear());
            }
            catch (PermisoDenegadoException ex)
            {
                MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo abrir '{item.Texto}'.\n\n{ex.Message}",
                    "Error al abrir el módulo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }
        else
        {
            contenido = new PaginaEnConstruccion(item.Texto, $"{item.Modulo}.cs");
        }

        MostrarEnHost(contenido);
        MarcarActivo(item);
        lblBreadcrumb.Text = $"SGPS  /  {grupo.Texto.ToUpperInvariant()}  /  {item.Texto.ToUpperInvariant()}";
    }

    private void MostrarResumen()
    {
        var resumen = new PaginaResumen();
        resumen.GrupoElegido += (_, grupo) => AbrirGrupo(grupo);

        MostrarEnHost(resumen);
        MarcarActivo(null);

        if (_todosLosItems.Count > 0)
        {
            _todosLosItems[0].Activo = true;
        }

        lblBreadcrumb.Text = "SGPS  /  RESUMEN";
    }

    /// <summary>
    /// Deja un Form listo para vivir dentro del panel de contenido. El orden
    /// importa: TopLevel debe pasar a false antes de agregarlo a Controls.
    /// </summary>
    private Control PrepararIncrustado(Form formulario)
    {
        formulario.TopLevel = false;
        formulario.FormBorderStyle = FormBorderStyle.None;
        formulario.ControlBox = false;
        formulario.AutoScroll = false;
        formulario.Dock = DockStyle.Fill;

        // Los modulos se escribieron como dialogos y su boton Cancelar llama
        // a Close(). Incrustados eso dejaria el panel vacio, asi que el shell
        // vuelve al Resumen en lugar de tocar cada formulario.
        formulario.FormClosed += FormularioIncrustado_FormClosed;

        return formulario;
    }

    private void FormularioIncrustado_FormClosed(object? sender, FormClosedEventArgs e)
    {
        if (!ReferenceEquals(sender, _contenidoActual) || _contenidoActual is null)
        {
            return;
        }

        var cerrado = _contenidoActual;
        _contenidoActual = null;

        // Diferido: el formulario todavia esta procesando su propio cierre.
        BeginInvoke(() =>
        {
            panelContenido.Controls.Remove(cerrado);
            cerrado.Dispose();
            MostrarResumen();
        });
    }

    private void MostrarEnHost(Control contenido)
    {
        panelContenido.SuspendLayout();

        if (_contenidoActual is not null)
        {
            if (_contenidoActual is Form anterior)
            {
                anterior.FormClosed -= FormularioIncrustado_FormClosed;
            }

            panelContenido.Controls.Remove(_contenidoActual);
            _contenidoActual.Dispose();
            _contenidoActual = null;
        }

        contenido.Dock = DockStyle.Fill;

        // Se asigna antes de agregarlo para que FormClosed, que puede
        // dispararse durante el Load del modulo, encuentre la referencia.
        _contenidoActual = contenido;
        panelContenido.Controls.Add(contenido);

        // Un Form incrustado necesita Show() explicito para hacerse visible.
        if (contenido is Form formulario)
        {
            formulario.Show();
        }

        panelContenido.ResumeLayout();
    }

    private void MarcarActivo(ItemNav? item)
    {
        foreach (var control in _todosLosItems)
        {
            control.Activo = item is not null && ReferenceEquals(control.Tag, item);
        }
    }

    // -------------------------------------------------------------- Pintado

    private void panelLogo_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var circulo = new Rectangle(24, 32, 34, 34);

        using (var fondo = new SolidBrush(Tema.Teal))
        {
            g.FillEllipse(fondo, circulo);
        }

        using var centro = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        using (var pincelInicial = new SolidBrush(Tema.Blanco))
        using (var fuenteInicial = new Font("Georgia", 13F, FontStyle.Bold))
        {
            g.DrawString("S", fuenteInicial, pincelInicial, circulo, centro);
        }

        using (var pincelMarca = new SolidBrush(Tema.Blanco))
        {
            g.DrawString("SGPS", Tema.Marca, pincelMarca, new PointF(66, 38));
        }

        using (var pincelPunto = new SolidBrush(Tema.Dorado))
        {
            g.FillEllipse(pincelPunto, 132, 56, 6, 6);
        }
    }

    private void panelUsuario_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // Separador superior.
        using (var separador = new Pen(Color.FromArgb(28, 82, 72)))
        {
            g.DrawLine(separador, 20, 0, panelUsuario.Width - 20, 0);
        }

        var usuario = SesionContextoLN.UsuarioActual;

        if (usuario is null)
        {
            return;
        }

        var circulo = new Rectangle(22, 26, 36, 36);
        var rolPrincipal = SesionContextoLN.RolesActuales
            .Select(r => PermisoLN.NormalizarRol(r.NombreRol))
            .FirstOrDefault(r => r is not null);

        var colorAvatar = rolPrincipal is null
            ? Tema.TealSuave
            : Tema.ColorDeRolSobreOscuro(rolPrincipal.Value);

        using (var fondo = new SolidBrush(colorAvatar))
        {
            g.FillEllipse(fondo, circulo);
        }

        using var centro = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        using (var pincelIniciales = new SolidBrush(Tema.ContrasteSobre(colorAvatar)))
        using (var fuenteIniciales = new Font("Segoe UI", 10F, FontStyle.Bold))
        {
            g.DrawString(Tema.Iniciales(usuario.NombreCompleto), fuenteIniciales, pincelIniciales, circulo, centro);
        }

        using var recorte = new StringFormat
        {
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        using (var pincelNombre = new SolidBrush(Tema.Blanco))
        {
            g.DrawString(usuario.NombreCompleto, Tema.CuerpoSemi, pincelNombre,
                new RectangleF(68, 28, panelUsuario.Width - 110, 20), recorte);
        }

        var textoRoles = string.Join(", ", SesionContextoLN.RolesActuales.Select(r => r.NombreRol));

        using (var pincelRol = new SolidBrush(Tema.TealSuave))
        {
            g.DrawString(string.IsNullOrWhiteSpace(textoRoles) ? "Sin rol" : textoRoles,
                Tema.Cuerpo, pincelRol,
                new RectangleF(68, 47, panelUsuario.Width - 110, 20), recorte);
        }
    }

    private void panelSuperior_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Linea inferior que separa la barra superior del contenido.
        using var borde = new Pen(Tema.Borde);
        g.DrawLine(borde, 32, panelSuperior.Height - 1, panelSuperior.Width - 32, panelSuperior.Height - 1);
    }

    // --------------------------------------------------------- Diagnostico

    private void MostrarRolesNoReconocidos()
    {
        var rolesNoReconocidos = SesionContextoLN.RolesActuales
            .Select(r => r.NombreRol)
            .Where(nombre => PermisoLN.NormalizarRol(nombre) is null)
            .ToList();

        if (rolesNoReconocidos.Count == 0)
        {
            return;
        }

        lblAviso.Text = "Roles sin permiso mapeado: " + string.Join(", ", rolesNoReconocidos);
        lblAviso.Visible = true;
    }

    private void RegistrarDiagnostico(string email)
    {
        try
        {
            var rutaLog = Path.Combine(AppContext.BaseDirectory, "permisos_debug.log");
            File.WriteAllText(rutaLog,
                $"[{DateTime.Now:O}] Usuario: {email}\n" +
                PermisoLN.Diagnostico());
        }
        catch
        {
            // El log es solo diagnostico; si no se puede escribir, se ignora.
        }
    }

    private void Registrar(string texto)
    {
        try
        {
            var rutaLog = Path.Combine(AppContext.BaseDirectory, "permisos_debug.log");
            File.AppendAllText(rutaLog, texto + Environment.NewLine);
        }
        catch
        {
        }
    }

    private string EstadoNavegacion()
    {
        var sb = new System.Text.StringBuilder("Estado navegación:");

        foreach (var grupo in MapaNavegacion.TodosLosGrupos)
        {
            var visible = MapaNavegacion.EsVisible(grupo);
            sb.Append($" | {grupo.Texto}({visible})[");
            sb.Append(string.Join(", ", MapaNavegacion.ItemsVisibles(grupo).Select(i => i.Texto)));
            sb.Append(']');
        }

        return sb.ToString();
    }

    // -------------------------------------------------------------- Eventos

    private void btnMenuUsuario_Click(object sender, EventArgs e) =>
        menuUsuario.Show(btnMenuUsuario, new Point(0, btnMenuUsuario.Height));

    private void mnuCerrarSesion_Click(object sender, EventArgs e)
    {
        SesionContextoLN.CerrarSesion();
        Close();
    }
}
