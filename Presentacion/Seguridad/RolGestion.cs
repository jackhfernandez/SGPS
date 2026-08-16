using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Seguridad;

public partial class RolGestion : Form
{
    private readonly RolLN _rolLN = new();

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
            dgvRoles.Columns.Add("FechaCreacion", "Creado");
            dgvRoles.Columns.Add("Usuarios", "Usuarios");

            dgvRoles.Columns["RolId"]!.Width = 45;
            dgvRoles.Columns["NombreRol"]!.Width = 140;
            dgvRoles.Columns["Descripcion"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvRoles.Columns["FechaCreacion"]!.Width = 90;
            dgvRoles.Columns["Usuarios"]!.Width = 70;

            foreach (var rol in roles)
            {
                var indice = dgvRoles.Rows.Add(
                    rol.RolId,
                    rol.NombreRol,
                    string.IsNullOrWhiteSpace(rol.Descripcion) ? "—" : rol.Descripcion,
                    rol.FechaCreacion.ToShortDateString(),
                    conteos.TryGetValue(rol.RolId, out var conteo) ? conteo : 0);

                dgvRoles.Rows[indice].Tag = rol;
            }

            LimpiarEditor();
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
        }
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarEditor();
        dgvRoles.ClearSelection();
        txtNombre.Focus();
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvRoles.CurrentRow?.Tag is Rol rolSeleccionado)
            {
                rolSeleccionado.NombreRol = txtNombre.Text.Trim();
                rolSeleccionado.Descripcion = txtDescripcion.Text.Trim();

                _rolLN.ActualizarRol(rolSeleccionado);
                MessageBox.Show("Rol actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var nuevo = new Rol
                {
                    NombreRol = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                _rolLN.CrearRol(nuevo);
                MessageBox.Show("Rol creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CargarRoles();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo guardar el rol: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dgvRoles.CurrentRow?.Tag is not Rol rol)
        {
            MessageBox.Show("Selecciona un rol para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmacion = MessageBox.Show(
            $"¿Eliminar el rol '{rol.NombreRol}'?",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _rolLN.EliminarRol(rol.RolId);
            MessageBox.Show("Rol eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarRoles();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo eliminar el rol: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnCancelar_Click(object sender, EventArgs e) => LimpiarEditor();

    private void LimpiarEditor()
    {
        txtNombre.Text = string.Empty;
        txtDescripcion.Text = string.Empty;
        dgvRoles.ClearSelection();
    }
}