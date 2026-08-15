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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
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

        if (SesionContextoLN.TieneRol("Cliente"))
        {
            proyectosMenu.Visible = false;
            backlogMenu.Visible = false;
            sprintsMenu.Visible = false;
            seguridadMenu.Visible = false;
            AbrirPortalCliente();
            return;
        }

        if (SesionContextoLN.TieneRol("Developer"))
        {
            proyectosMenu.Visible = false;
            seguridadUsuarios.Visible = false;
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

    private void backlogEpic_Click(object sender, EventArgs e)
    {
        var form = new EpicGestion();
        form.ShowDialog();
    }

    private void backlogProducto_Click(object sender, EventArgs e)
    {
        var form = new ProductBacklogGestion();
        form.ShowDialog();
    }

    private void backlogHistoriaUsuarios_Click(object sender, EventArgs e)
    {
        var form = new UserStoryEdicion();
        form.ShowDialog();
    }

    private void clientePortal_Click(object sender, EventArgs e)
    {
        var form = new ClientePortal();
        form.ShowDialog();
    }

    private void kanbanTablero_Click(object sender, EventArgs e)
    {
        var form = new TableroKanban();
        form.ShowDialog();
    }

    private void kanbanTarea_Click(object sender, EventArgs e)
    {
        var form = new TareaEdicion();
        form.ShowDialog();
    }

    private void kanbanControlTarjetas_Click(object sender, EventArgs e)
    {
        var form = new UcTarjetaKanban();
        form.ShowDialog();
    }

    private void proyectoMiembros_Click(object sender, EventArgs e)
    {
        var form = new ProyectoMiembros();
        form.ShowDialog();
    }

    private void proyectoNuevo_Click(object sender, EventArgs e)
    {
        var form = new ProyectoCreacion();
        form.ShowDialog();
    }

    private void gestionBugToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var form = new BugGestion();
        form.ShowDialog();
    }

    private void qaReporteBug_Click(object sender, EventArgs e)
    {
        var form = new BugReporte();
        form.ShowDialog();
    }

    private void reporteVistaGraficos_Click(object sender, EventArgs e)
    {
        var form = new BurndownChartVista();
        form.ShowDialog();
    }

    private void reporteMetricas_Click(object sender, EventArgs e)
    {
        var form = new MetricasAgilesVista();
        form.ShowDialog();
    }

    private void seguridadRol_Click(object sender, EventArgs e)
    {
        var form = new RolGestion();
        form.ShowDialog();
    }

    private void seguridadUsuarios_Click(object sender, EventArgs e)
    {
        var form = new UsuarioGestion();
        form.ShowDialog();
    }

    private void sprintEjecucion_Click(object sender, EventArgs e)
    {
        var form = new SprintEjecucion();
        form.ShowDialog();
    }

    private void sprintPlanificaciones_Click(object sender, EventArgs e)
    {
        var form = new SprintPlanificacion();
        form.ShowDialog();
    }
}