using Logica;
using Presentacion.Backlog;
using Presentacion.Cliente;
using Presentacion.Kanban;
using Presentacion.Proyectos;
using Presentacion.QA;
using Presentacion.Sprint;
using Presentacion.Reporte;
using Presentacion.Seguridad;

namespace Presentacion;

public partial class Principal : Form
{
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

        lblStatusUsuario.Text = $"Usuario: {usuario.NombreCompleto}";
        lblStatusRol.Text = "Rol: " + string.Join(", ", SesionContextoLN.RolesActuales.Select(r => r.NombreRol));

        try
        {
            var rutaLog = Path.Combine(AppContext.BaseDirectory, "permisos_debug.log");
            File.WriteAllText(rutaLog,
                $"[{DateTime.Now:O}] Usuario: {usuario.Email}\n" +
                PermisoLN.Diagnostico());
        }
        catch
        {
            // El log es solo diagnostico; si no se puede escribir, se ignora.
        }

        var rolesNoReconocidos = SesionContextoLN.RolesActuales
            .Select(r => r.NombreRol)
            .Where(nombre => PermisoLN.NormalizarRol(nombre) is null)
            .ToList();

        if (rolesNoReconocidos.Count > 0)
        {
            lblStatusRol.Text += "  (roles sin permiso mapeado: " + string.Join(", ", rolesNoReconocidos) + ")";
        }

        try
        {
            AplicarVisibilidad(backlogMenu, Modulo.EpicGestion, Modulo.ProductBacklogGestion, Modulo.UserStoryEdicion);
            AplicarVisibilidad(clienteToolStripMenuItem, Modulo.ClientePortal);
            AplicarVisibilidad(kanbanToolStripMenuItem, Modulo.TableroKanban, Modulo.TareaEdicion, Modulo.UcTarjetaKanban);
            AplicarVisibilidad(proyectosMenu, Modulo.ProyectoCreacion, Modulo.ProyectoMiembros);
            AplicarVisibilidad(qaGestionBug, Modulo.BugGestion, Modulo.BugReporte);
            AplicarVisibilidad(reporteToolStripMenuItem, Modulo.BurndownChartVista, Modulo.MetricasAgilesVista);
            AplicarVisibilidad(seguridadMenu, Modulo.RolGestion, Modulo.UsuarioGestion);
            AplicarVisibilidad(sprintsMenu, Modulo.SprintEjecucion, Modulo.SprintPlanificacion);

            Registrar(EstadoMenus());
        }
        catch (Exception ex)
        {
            try
            {
                Registrar("EXCEPCION en visibilidad: " + ex);
            }
            catch
            {
            }
        }

        if (SesionContextoLN.TieneRol("Cliente"))
        {
            AbrirPortalCliente();
        }
    }

    private void AplicarVisibilidad(ToolStripMenuItem menu, params Modulo[] modulos)
    {
        var visible = modulos.Any(PermisoLN.PuedeVer);
        menu.Visible = visible;
        Registrar($"  MENU {menu.Name} visible={visible}");
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

    private string EstadoMenus()
    {
        var sb = new System.Text.StringBuilder("Estado menús:");

        foreach (ToolStripMenuItem menu in menuStrip1.Items)
        {
            sb.Append($" | {menu.Text}({menu.Visible})[");
            var primero = true;

            foreach (ToolStripItem item in menu.DropDownItems)
            {
                if (!primero)
                {
                    sb.Append(", ");
                }

                sb.Append($"{item.Text}({item.Visible})");
                primero = false;
            }

            sb.Append(']');
        }

        return sb.ToString();
    }

    private void AbrirModulo<TForm>(Modulo modulo) where TForm : Form, new()
    {
        try
        {
            PermisoLN.ValidarAcceso(modulo, NivelAcceso.Lectura);

            var form = new TForm();
            form.ShowDialog();
        }
        catch (PermisoDenegadoException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Acceso denegado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private TResult? BuscarHijoAbierto<TResult>() where TResult : Form
    {
        foreach (Form hijo in MdiChildren)
        {
            if (hijo is TResult formAbierto)
            {
                formAbierto.Activate();
                return formAbierto;
            }
        }

        return null;
    }

    private void AbrirFormulario<TForm>() where TForm : Form, new()
    {
        if (BuscarHijoAbierto<TForm>() is not null)
        {
            return;
        }

        var formulario = new TForm
        {
            MdiParent = this,
            //WindowState = FormWindowState.Maximized
        };

        formulario.Show();
    }

    private void AbrirPortalCliente() => AbrirFormulario<ClientePortal>();

    private void mnuCerrarSesion_Click(object sender, EventArgs e)
    {
        SesionContextoLN.CerrarSesion();
        Close();
    }

    private void backlogEpic_Click(object sender, EventArgs e) =>
        AbrirModulo<EpicGestion>(Modulo.EpicGestion);

    private void backlogProducto_Click(object sender, EventArgs e) =>
        AbrirModulo<ProductBacklogGestion>(Modulo.ProductBacklogGestion);

    private void backlogHistoriaUsuarios_Click(object sender, EventArgs e) =>
        AbrirModulo<UserStoryEdicion>(Modulo.UserStoryEdicion);

    private void clientePortal_Click(object sender, EventArgs e) =>
        AbrirModulo<ClientePortal>(Modulo.ClientePortal);

    private void kanbanTablero_Click(object sender, EventArgs e) =>
        AbrirModulo<TableroKanban>(Modulo.TableroKanban);

    private void kanbanTarea_Click(object sender, EventArgs e) =>
        AbrirModulo<TareaEdicion>(Modulo.TareaEdicion);

    private void kanbanControlTarjetas_Click(object sender, EventArgs e) =>
        AbrirModulo<UcTarjetaKanban>(Modulo.UcTarjetaKanban);

    private void proyectoMiembros_Click(object sender, EventArgs e) =>
        AbrirModulo<ProyectoMiembros>(Modulo.ProyectoMiembros);

    private void proyectoNuevo_Click(object sender, EventArgs e) =>
        AbrirModulo<ProyectoCreacion>(Modulo.ProyectoCreacion);

    private void gestionBugToolStripMenuItem_Click(object sender, EventArgs e) =>
        AbrirModulo<BugGestion>(Modulo.BugGestion);

    private void qaReporteBug_Click(object sender, EventArgs e) =>
        AbrirModulo<BugReporte>(Modulo.BugReporte);

    private void reporteVistaGraficos_Click(object sender, EventArgs e) =>
        AbrirModulo<BurndownChartVista>(Modulo.BurndownChartVista);

    private void reporteMetricas_Click(object sender, EventArgs e) =>
        AbrirModulo<MetricasAgilesVista>(Modulo.MetricasAgilesVista);

    private void seguridadRol_Click(object sender, EventArgs e) =>
        AbrirModulo<RolGestion>(Modulo.RolGestion);

    private void seguridadUsuarios_Click(object sender, EventArgs e) =>
        AbrirModulo<UsuarioGestion>(Modulo.UsuarioGestion);

    private void sprintEjecucion_Click(object sender, EventArgs e) =>
        AbrirModulo<SprintEjecucion>(Modulo.SprintEjecucion);

    private void sprintPlanificaciones_Click(object sender, EventArgs e) =>
        AbrirModulo<SprintPlanificacion>(Modulo.SprintPlanificacion);
}