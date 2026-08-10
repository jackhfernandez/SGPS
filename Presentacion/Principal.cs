using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Logica;
using Presentacion.Modulos;
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
            mnuUsuarios.Visible = false;
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
            WindowState = FormWindowState.Maximized
        };

        formulario.Show();
    }

    private void AbrirPortalCliente() => AbrirFormulario<ClientePortal>();

    private void mnuCerrarSesion_Click(object sender, EventArgs e)
    {
        SesionContextoLN.CerrarSesion();
        Close();
    }

    private void mnuProyectoNuevo_Click(object sender, EventArgs e) => AbrirFormulario<ProyectoCreacion>();

    private void backlogMenu_Click(object sender, EventArgs e) => AbrirFormulario<ProductBacklogGestion>();

    private void sprintsMenu_Click(object sender, EventArgs e) => AbrirFormulario<SprintPlanificacion>();

    private void portalMenu_Click(object sender, EventArgs e) => AbrirPortalCliente();

    private void mnuUsuarios_Click(object sender, EventArgs e) => AbrirFormulario<UsuarioGestion>();

    private void mnuRoles_Click(object sender, EventArgs e) => AbrirFormulario<RolGestion>();

    private void mnuVentanaMosaicoH_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);

    private void mnuVentanaMosaicoV_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileVertical);

    private void mnuVentanaCascada_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);

    private void mnuVentanaCerrarTodos_Click(object sender, EventArgs e)
    {
        foreach (Form hijo in MdiChildren)
        {
            hijo.Close();
        }
    }
}