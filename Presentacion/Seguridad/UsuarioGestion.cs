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

    public UsuarioGestion()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
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
        dgvUsuarios.Columns.Add("FechaRegistro", "Registro");

        dgvUsuarios.Columns["UsuarioId"]!.Width = 45;
        dgvUsuarios.Columns["UsuarioId"]!.Visible = false;
        dgvUsuarios.Columns["Nombres"]!.Width = 130;
        dgvUsuarios.Columns["Apellidos"]!.Width = 130;
        dgvUsuarios.Columns["Email"]!.Width = 185;
        dgvUsuarios.Columns["Roles"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dgvUsuarios.Columns["Activo"]!.Width = 70;
        dgvUsuarios.Columns["FechaRegistro"]!.Width = 90;

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

            dgvUsuarios.Rows[indice].Tag = usuario;
        }

        if (idSeleccionado.HasValue)
        {
            SeleccionarUsuario(idSeleccionado.Value);
        }

        ActualizarCasillasRoles();
    }

    private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
    {
        ActualizarCasillasRoles();
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

    private void btnGuardarRoles_Click(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow?.Tag is not Usuario usuario)
        {
            MessageBox.Show(
                "Selecciona un usuario para asignarle roles.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            var rolIds = _rolesCheckboxes.Where(c => c.Value.Checked).Select(c => c.Key).ToList();

            _usuarioLN.AsignarRoles(usuario.UsuarioId, rolIds);
            CargarUsuarios();
            SeleccionarUsuario(usuario.UsuarioId);

            MessageBox.Show(
                "Roles del usuario actualizados correctamente.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudieron guardar los roles: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        using var dialogo = new UsuarioNuevo();

        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            var usuario = new Usuario
            {
                Nombres = dialogo.Nombres,
                Apellidos = dialogo.Apellidos,
                Email = dialogo.Email,
                EsActivo = true
            };

            var nuevoId = _usuarioLN.CrearUsuario(usuario, dialogo.Password);
            CargarUsuarios();
            SeleccionarUsuario(nuevoId);

            MessageBox.Show(
                "Usuario creado correctamente. Asigna sus roles en el panel derecho.",
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

    private void btnEstado_Click(object sender, EventArgs e)
    {
        if (dgvUsuarios.CurrentRow?.Tag is not Usuario usuario)
        {
            MessageBox.Show(
                "Selecciona un usuario para cambiar su estado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            var nuevoEstado = !usuario.EsActivo;

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
                $"No se pudo cambiar el estado: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnRefrescar_Click(object sender, EventArgs e)
    {
        try
        {
            CargarRolesDisponibles();
            CargarUsuarios();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudieron refrescar los datos: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void SeleccionarUsuario(int usuarioId)
    {
        foreach (DataGridViewRow fila in dgvUsuarios.Rows)
        {
            if (fila.Tag is Usuario usuario && usuario.UsuarioId == usuarioId)
            {
                dgvUsuarios.CurrentCell = fila.Cells[0];
                return;
            }
        }
    }
}