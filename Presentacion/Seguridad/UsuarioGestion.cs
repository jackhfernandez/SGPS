using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Seguridad;

public partial class UsuarioGestion : Form
{
    private readonly UsuarioLN _usuarioLN = new();
    private readonly RolLN _rolLN = new();

    private readonly Dictionary<int, CheckBox> _rolesCheckboxes = new();
    private Dictionary<int, List<Rol>> _rolesPorUsuario = new();
    private bool _modoAlta;

    public UsuarioGestion()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        try
        {
            PermisoLN.ValidarLectura(Modulo.UsuarioGestion);
        }
        catch (PermisoDenegadoException ex)
        {
            MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        base.OnLoad(e);

        try
        {
            CargarRolesDisponibles();
            CargarUsuarios();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo cargar la configuración: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CargarRolesDisponibles()
    {
        _rolesCheckboxes.Clear();
        flowLayoutRoles.Controls.Clear();

        foreach (var rol in _rolLN.ListarRoles())
        {
            var casilla = new CheckBox
            {
                Text = rol.NombreRol,
                Tag = rol.RolId,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(44, 51, 47),
                Margin = new Padding(3, 6, 3, 6)
            };

            _rolesCheckboxes[rol.RolId] = casilla;
            flowLayoutRoles.Controls.Add(casilla);
        }
    }

    private void CargarUsuarios()
    {
        var usuarios = _usuarioLN.ListarUsuarios();
        _rolesPorUsuario = _usuarioLN.ObtenerRolesDeTodosLosUsuarios();

        var resumenRoles = _rolesPorUsuario.ToDictionary(
            kvp => kvp.Key,
            kvp => string.Join(", ", kvp.Value.OrderBy(r => r.NombreRol).Select(r => r.NombreRol)));

        var idSeleccionado = (dgvUsuarios.CurrentRow?.Tag as Usuario)?.UsuarioId;

        dgvUsuarios.DataSource = null;
        dgvUsuarios.Columns.Clear();

        dgvUsuarios.EnableHeadersVisualStyles = false;
        dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
        dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgvUsuarios.ColumnHeadersHeight = 30;

        dgvUsuarios.Columns.Add("UsuarioId", "Id");
        dgvUsuarios.Columns.Add("Nombres", "Nombres");
        dgvUsuarios.Columns.Add("Apellidos", "Apellidos");
        dgvUsuarios.Columns.Add("Email", "Correo electrónico");
        dgvUsuarios.Columns.Add("Roles", "Roles");
        dgvUsuarios.Columns.Add("Activo", "Activo");
        dgvUsuarios.Columns.Add("Registro", "Registro");

        dgvUsuarios.Columns["UsuarioId"]!.Width = 45;
        dgvUsuarios.Columns["UsuarioId"]!.Visible = false;
        dgvUsuarios.Columns["Nombres"]!.Width = 130;
        dgvUsuarios.Columns["Apellidos"]!.Width = 130;
        dgvUsuarios.Columns["Email"]!.Width = 185;
        dgvUsuarios.Columns["Roles"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dgvUsuarios.Columns["Activo"]!.Width = 70;
        dgvUsuarios.Columns["Registro"]!.Width = 90;

        foreach (var usuario in usuarios)
        {
            var rol = resumenRoles.TryGetValue(usuario.UsuarioId, out var nombre)
                ? nombre
                : "—";

            var indice = dgvUsuarios.Rows.Add(
                usuario.UsuarioId,
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Email,
                rol,
                usuario.EsActivo ? "Sí" : "No",
                usuario.FechaRegistro.ToShortDateString());

            if (!usuario.EsActivo)
            {
                dgvUsuarios.Rows[indice].DefaultCellStyle.ForeColor = Color.FromArgb(150, 150, 150);
            }

            dgvUsuarios.Rows[indice].Tag = usuario;
        }

        if (idSeleccionado.HasValue)
        {
            SeleccionarUsuario(idSeleccionado.Value);
        }
        else
        {
            LimpiarEditor();
        }
    }

    private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow?.Tag is Usuario usuario)
        {
            txtNombres.Text = usuario.Nombres;
            txtApellidos.Text = usuario.Apellidos;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = string.Empty;
            _modoAlta = false;
            ActualizarCasillasRoles();
            ActualizarEstadoBotones();
        }
    }

    private void ActualizarCasillasRoles()
    {
        if (dgvUsuarios.CurrentRow?.Tag is not Usuario usuario)
        {
            return;
        }

        var asignados = new HashSet<int>(
            _rolesPorUsuario.TryGetValue(usuario.UsuarioId, out var roles)
                ? roles.Select(r => r.RolId)
                : Enumerable.Empty<int>());

        foreach (var casilla in _rolesCheckboxes.Values)
        {
            casilla.Checked = asignados.Contains((int)casilla.Tag!);
        }
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarEditor();
        _modoAlta = true;
        ActualizarEstadoBotones();
        txtNombres.Focus();
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
            var usuario = new Usuario
            {
                Nombres = txtNombres.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                EsActivo = true
            };

            var nuevoId = _usuarioLN.CrearUsuario(usuario, txtPassword.Text);
            _usuarioLN.AsignarRoles(nuevoId, ObtenerRolIdsMarcados());

            _modoAlta = false;
            CargarUsuarios();
            SeleccionarUsuario(nuevoId);

            MessageBox.Show(
                "Usuario creado correctamente con sus roles asignados.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo crear el usuario: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow?.Tag is not Usuario usuario)
        {
            MessageBox.Show(
                "Selecciona un usuario para modificar.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            usuario.Nombres = txtNombres.Text.Trim();
            usuario.Apellidos = txtApellidos.Text.Trim();
            usuario.Email = txtEmail.Text.Trim();

            _usuarioLN.ActualizarUsuario(usuario);

            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                _usuarioLN.RestablecerPassword(usuario.UsuarioId, txtPassword.Text);
            }

            _usuarioLN.AsignarRoles(usuario.UsuarioId, ObtenerRolIdsMarcados());

            CargarUsuarios();
            SeleccionarUsuario(usuario.UsuarioId);

            MessageBox.Show(
                "Usuario actualizado correctamente con sus roles asignados.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo modificar el usuario: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnDesactivar_Click(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow?.Tag is not Usuario usuario)
        {
            MessageBox.Show(
                "Selecciona un usuario para activar o desactivar.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var nuevoEstado = !usuario.EsActivo;
        var confirmacion = MessageBox.Show(
            $"¿{(nuevoEstado ? "Activar" : "Desactivar")} al usuario '{usuario.NombreCompleto}'?",
            "Confirmar cambio de estado",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _usuarioLN.CambiarEstadoActivo(usuario.UsuarioId, nuevoEstado);

            CargarUsuarios();
            SeleccionarUsuario(usuario.UsuarioId);

            MessageBox.Show(
                $"Usuario {(nuevoEstado ? "activado" : "desactivado")} correctamente.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo cambiar el estado del usuario: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarEditor();
        ActualizarEstadoBotones();
        txtNombres.Focus();
    }

    private void btnCancelar_Click(object sender, EventArgs e) => Close();

    private List<int> ObtenerRolIdsMarcados() =>
        _rolesCheckboxes.Where(c => c.Value.Checked).Select(c => c.Key).ToList();

    private void LimpiarEditor()
    {
        txtNombres.Text = string.Empty;
        txtApellidos.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPassword.Text = string.Empty;
        _modoAlta = false;

        foreach (var casilla in _rolesCheckboxes.Values)
        {
            casilla.Checked = false;
        }

        dgvUsuarios.ClearSelection();
        ActualizarEstadoBotones();
    }

    private void SeleccionarUsuario(int usuarioId)
    {
        foreach (DataGridViewRow fila in dgvUsuarios.Rows)
        {
            if (fila.Tag is Usuario usuario && usuario.UsuarioId == usuarioId)
            {
                for (var indice = 0; indice < fila.Cells.Count; indice++)
                {
                    if (fila.Cells[indice].Visible)
                    {
                        dgvUsuarios.CurrentCell = fila.Cells[indice];
                        return;
                    }
                }
            }
        }
    }

    private void ActualizarEstadoBotones()
    {
        btnAgregar.Enabled = _modoAlta;
        btnModificar.Enabled = dgvUsuarios.CurrentRow?.Tag is Usuario;
        btnDesactivar.Enabled = dgvUsuarios.CurrentRow?.Tag is Usuario;
    }
}
