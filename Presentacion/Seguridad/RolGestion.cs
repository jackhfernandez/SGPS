using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Seguridad;

public partial class RolGestion : Form
{
    private readonly RolLN _rolLN = new();
    private bool _modoAlta;

    public RolGestion()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        try
        {
            PermisoLN.ValidarLectura(Modulo.RolGestion);
        }
        catch (PermisoDenegadoException ex)
        {
            MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        base.OnLoad(e);
        CargarRoles();
    }

    private void CargarRoles()
    {
        try
        {
            var roles = _rolLN.ListarRoles();
            var conteos = _rolLN.ObtenerConteoUsuariosPorRol();

            var idSeleccionado = (dgvRoles.CurrentRow?.Tag as Rol)?.RolId;

            dgvRoles.DataSource = null;
            dgvRoles.Columns.Clear();

            dgvRoles.EnableHeadersVisualStyles = false;
            dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
            dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRoles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvRoles.ColumnHeadersHeight = 30;

            dgvRoles.Columns.Add("RolId", "Id");
            dgvRoles.Columns.Add("NombreRol", "Rol");
            dgvRoles.Columns.Add("Descripcion", "Descripción");
            dgvRoles.Columns.Add("Creado", "Creado");
            dgvRoles.Columns.Add("Usuarios", "Usuarios");
            dgvRoles.Columns.Add("Activo", "Activo");

            dgvRoles.Columns["RolId"]!.Width = 45;
            dgvRoles.Columns["NombreRol"]!.Width = 140;
            dgvRoles.Columns["Descripcion"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvRoles.Columns["Creado"]!.Width = 90;
            dgvRoles.Columns["Usuarios"]!.Width = 70;
            dgvRoles.Columns["Activo"]!.Width = 60;

            foreach (var rol in roles)
            {
                var indice = dgvRoles.Rows.Add(
                    rol.RolId,
                    rol.NombreRol,
                    string.IsNullOrWhiteSpace(rol.Descripcion) ? "—" : rol.Descripcion,
                    rol.FechaCreacion.ToShortDateString(),
                    conteos.TryGetValue(rol.RolId, out var conteo) ? conteo : 0,
                    rol.EsActivo ? "Sí" : "No");

                if (!rol.EsActivo)
                {
                    dgvRoles.Rows[indice].DefaultCellStyle.ForeColor = Color.FromArgb(150, 150, 150);
                }

                dgvRoles.Rows[indice].Tag = rol;
            }

            if (idSeleccionado.HasValue)
            {
                SeleccionarRol(idSeleccionado.Value);
            }
            else
            {
                LimpiarEditor();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudieron cargar los roles: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void dgvRoles_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvRoles.CurrentRow?.Tag is Rol rol)
        {
            txtNombre.Text = rol.NombreRol;
            txtDescripcion.Text = rol.Descripcion;
            _modoAlta = false;
            ActualizarEstadoBotones();
        }
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarEditor();
        _modoAlta = true;
        ActualizarEstadoBotones();
        txtNombre.Focus();
    }

    private void btnAgregar_Click(object sender, EventArgs e)
    {
        if (!_modoAlta)
        {
            MessageBox.Show(
                "Presiona 'Nuevo' para preparar el formulario y luego 'Agregar'.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            var nuevo = new Rol
            {
                NombreRol = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim()
            };

            var nuevoId = _rolLN.CrearRol(nuevo);

            _modoAlta = false;
            CargarRoles();
            SeleccionarRol(nuevoId);

            MessageBox.Show("Rol creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo crear el rol: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (dgvRoles.CurrentRow?.Tag is not Rol rol)
        {
            MessageBox.Show(
                "Selecciona un rol para modificar.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            rol.NombreRol = txtNombre.Text.Trim();
            rol.Descripcion = txtDescripcion.Text.Trim();

            _rolLN.ActualizarRol(rol);

            CargarRoles();
            SeleccionarRol(rol.RolId);

            MessageBox.Show("Rol actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo modificar el rol: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnDesactivar_Click(object sender, EventArgs e)
    {
        if (dgvRoles.CurrentRow?.Tag is not Rol rol)
        {
            MessageBox.Show(
                "Selecciona un rol para activar o desactivar.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var nuevoEstado = !rol.EsActivo;
        var confirmacion = MessageBox.Show(
            $"¿{(nuevoEstado ? "Activar" : "Desactivar")} el rol '{rol.NombreRol}'?",
            "Confirmar cambio de estado",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _rolLN.CambiarEstado(rol.RolId, nuevoEstado);

            CargarRoles();
            SeleccionarRol(rol.RolId);

            MessageBox.Show(
                $"Rol {(nuevoEstado ? "activado" : "desactivado")} correctamente.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo cambiar el estado del rol: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarEditor();
        ActualizarEstadoBotones();
        txtNombre.Focus();
    }

    private void btnCancelar_Click(object sender, EventArgs e) => Close();

    private void LimpiarEditor()
    {
        txtNombre.Text = string.Empty;
        txtDescripcion.Text = string.Empty;
        _modoAlta = false;
        dgvRoles.ClearSelection();
        ActualizarEstadoBotones();
    }

    private void SeleccionarRol(int rolId)
    {
        foreach (DataGridViewRow fila in dgvRoles.Rows)
        {
            if (fila.Tag is Rol rol && rol.RolId == rolId)
            {
                dgvRoles.CurrentCell = fila.Cells[0];
                return;
            }
        }
    }

    private void ActualizarEstadoBotones()
    {
        btnAgregar.Enabled = _modoAlta;
        btnModificar.Enabled = dgvRoles.CurrentRow?.Tag is Rol;
        btnDesactivar.Enabled = dgvRoles.CurrentRow?.Tag is Rol;
    }
}
