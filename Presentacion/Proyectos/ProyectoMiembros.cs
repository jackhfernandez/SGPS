using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Proyectos
{
    public partial class ProyectoMiembros : Form
    {
        private const string RolPorDefecto = "Developer";

        private readonly ProyectoMiembroLN _proyectoMiembroLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private bool _sincronizandoSeleccion;

        public ProyectoMiembros()
        {
            InitializeComponent();
            EstablecerRolDefecto();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.ProyectoMiembros);
            }
            catch (PermisoDenegadoException ex)
            {
                MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            base.OnLoad(e);
            CargarProyectos();
        }

        private Proyecto? ProyectoSeleccionado =>
            (cboProyectos.SelectedItem as ProyectoComboItem)?.Proyecto;

        private void CargarProyectos()
        {
            try
            {
                var proyectos = _proyectoLN.ListarProyectos();
                var idSeleccionado = (cboProyectos.SelectedItem as ProyectoComboItem)?.Proyecto.ProyectoId;

                cboProyectos.Items.Clear();
                foreach (var proyecto in proyectos)
                {
                    cboProyectos.Items.Add(new ProyectoComboItem(proyecto));
                }

                if (idSeleccionado.HasValue)
                {
                    foreach (var item in cboProyectos.Items)
                    {
                        if (item is ProyectoComboItem it && it.Proyecto.ProyectoId == idSeleccionado.Value)
                        {
                            cboProyectos.SelectedItem = item;
                            break;
                        }
                    }
                }
                else if (cboProyectos.Items.Count > 0)
                {
                    cboProyectos.SelectedIndex = 0;
                }

                if (cboProyectos.SelectedItem is null)
                {
                    LimpiarGrillas();
                    ActualizarEstadoBotones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los proyectos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cboProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMiembros();
            CargarDisponibles();
            ActualizarEstadoBotones();
        }

        private void CargarMiembros()
        {
            dgvMiembros.DataSource = null;
            dgvMiembros.Columns.Clear();
            EstilizarGrilla(dgvMiembros);

            dgvMiembros.Columns.Add("Nombre", "Miembro");
            dgvMiembros.Columns.Add("Rol", "Rol");
            dgvMiembros.Columns.Add("Desde", "Desde");
            dgvMiembros.Columns["Nombre"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMiembros.Columns["Rol"]!.Width = 90;
            dgvMiembros.Columns["Desde"]!.Width = 100;

            var proyecto = ProyectoSeleccionado;
            if (proyecto is null)
            {
                lblTituloMiembros.Text = "MIEMBROS DEL PROYECTO";
                return;
            }

            try
            {
                var miembros = _proyectoMiembroLN.ListarPorProyecto(proyecto.ProyectoId);

                foreach (var miembro in miembros)
                {
                    var indice = dgvMiembros.Rows.Add(
                        miembro.NombreCompleto,
                        miembro.RolEnProyecto,
                        miembro.FechaAsignacion.ToShortDateString());

                    dgvMiembros.Rows[indice].Tag = miembro;
                }

                lblTituloMiembros.Text = $"MIEMBROS DEL PROYECTO ({miembros.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los miembros: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarDisponibles()
        {
            dgvDisponibles.DataSource = null;
            dgvDisponibles.Columns.Clear();
            EstilizarGrilla(dgvDisponibles);

            dgvDisponibles.Columns.Add("Nombre", "Usuario");
            dgvDisponibles.Columns.Add("Email", "Correo");
            dgvDisponibles.Columns["Nombre"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDisponibles.Columns["Email"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var proyecto = ProyectoSeleccionado;
            if (proyecto is null)
            {
                lblTituloDisponibles.Text = "EQUIPO DISPONIBLE";
                return;
            }

            try
            {
                var disponibles = _proyectoMiembroLN.ListarDisponibles(proyecto.ProyectoId);

                foreach (var usuario in disponibles)
                {
                    var indice = dgvDisponibles.Rows.Add(usuario.NombreCompleto, usuario.Email);

                    dgvDisponibles.Rows[indice].Tag = usuario;
                }

                lblTituloDisponibles.Text = $"EQUIPO DISPONIBLE ({disponibles.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los usuarios disponibles: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvMiembros_SelectionChanged(object sender, EventArgs e)
        {
            if (_sincronizandoSeleccion)
            {
                return;
            }

            _sincronizandoSeleccion = true;
            try
            {
                LimpiarSeleccion(dgvDisponibles);

                if (dgvMiembros.CurrentRow?.Tag is ProyectoMiembro miembro)
                {
                    cboRol.Text = miembro.RolEnProyecto;
                }
            }
            finally
            {
                _sincronizandoSeleccion = false;
            }

            ActualizarEstadoBotones();
        }

        private void dgvDisponibles_SelectionChanged(object sender, EventArgs e)
        {
            if (_sincronizandoSeleccion)
            {
                return;
            }

            _sincronizandoSeleccion = true;
            try
            {
                LimpiarSeleccion(dgvMiembros);

                if (dgvDisponibles.CurrentRow?.Tag is Usuario)
                {
                    EstablecerRolDefecto();
                }
            }
            finally
            {
                _sincronizandoSeleccion = false;
            }

            ActualizarEstadoBotones();
        }

        private static void LimpiarSeleccion(DataGridView dgv)
        {
            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }

        private void EstablecerRolDefecto()
        {
            if (cboRol.Items.Contains(RolPorDefecto))
            {
                cboRol.SelectedItem = RolPorDefecto;
            }
            else if (cboRol.Items.Count > 0)
            {
                cboRol.SelectedIndex = 0;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            cboProyectos.SelectedIndex = -1;
            EstablecerRolDefecto();
            LimpiarGrillas();
            ActualizarEstadoBotones();
            cboProyectos.Focus();
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            var proyecto = ProyectoSeleccionado;
            if (proyecto is null)
            {
                MessageBox.Show("Selecciona un proyecto para asignar miembros.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvDisponibles.CurrentRow?.Tag is not Usuario usuario)
            {
                MessageBox.Show("Selecciona un usuario del equipo disponible.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboRol.SelectedIndex < 0)
            {
                MessageBox.Show("Elige el rol que tendrá el nuevo miembro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string rolAsignado = cboRol.Text;
                _proyectoMiembroLN.Asignar(proyecto.ProyectoId, usuario.UsuarioId, rolAsignado);

                CargarMiembros();
                CargarDisponibles();
                ActualizarEstadoBotones();

                MessageBox.Show(
                    $"'{usuario.NombreCompleto}' fue asignado al proyecto '{proyecto.NombreProyecto}' como {rolAsignado}.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                dgvDisponibles.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo asignar al miembro: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            var proyecto = ProyectoSeleccionado;
            if (proyecto is null)
            {
                MessageBox.Show("Selecciona un proyecto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvMiembros.CurrentRow?.Tag is not ProyectoMiembro miembro)
            {
                MessageBox.Show("Selecciona un miembro del proyecto para cambiar su rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboRol.SelectedIndex < 0)
            {
                MessageBox.Show("Elige el nuevo rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string nuevoRol = cboRol.Text;
                _proyectoMiembroLN.CambiarRol(proyecto.ProyectoId, miembro.UsuarioId, nuevoRol);

                CargarMiembros();
                ActualizarEstadoBotones();
                SeleccionarMiembro(miembro.UsuarioId);

                MessageBox.Show(
                    $"El rol de '{miembro.NombreCompleto}' fue actualizado a {nuevoRol}.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cambiar el rol: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var proyecto = ProyectoSeleccionado;
            if (proyecto is null)
            {
                MessageBox.Show("Selecciona un proyecto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvMiembros.CurrentRow?.Tag is not ProyectoMiembro miembro)
            {
                MessageBox.Show("Selecciona un miembro del proyecto para quitarlo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Quitar a '{miembro.NombreCompleto}' del proyecto '{proyecto.NombreProyecto}'?",
                "Confirmar baja de miembro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _proyectoMiembroLN.Quitar(proyecto.ProyectoId, miembro.UsuarioId);

                CargarMiembros();
                CargarDisponibles();
                ActualizarEstadoBotones();

                MessageBox.Show(
                    $"'{miembro.NombreCompleto}' fue quitado del proyecto.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                dgvDisponibles.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo quitar al miembro: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            EstablecerRolDefecto();
            dgvMiembros.ClearSelection();
            dgvDisponibles.ClearSelection();
            ActualizarEstadoBotones();
            dgvDisponibles.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        private void LimpiarGrillas()
        {
            dgvMiembros.DataSource = null;
            dgvMiembros.Columns.Clear();
            dgvDisponibles.DataSource = null;
            dgvDisponibles.Columns.Clear();
            lblTituloMiembros.Text = "MIEMBROS DEL PROYECTO";
            lblTituloDisponibles.Text = "EQUIPO DISPONIBLE";
        }

        private void SeleccionarMiembro(int usuarioId)
        {
            foreach (DataGridViewRow fila in dgvMiembros.Rows)
            {
                if (fila.Tag is ProyectoMiembro miembro && miembro.UsuarioId == usuarioId)
                {
                    for (var indice = 0; indice < fila.Cells.Count; indice++)
                    {
                        if (fila.Cells[indice].Visible)
                        {
                            dgvMiembros.CurrentCell = fila.Cells[indice];
                            return;
                        }
                    }
                }
            }
        }

        private static void EstilizarGrilla(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 30;
        }

        private void ActualizarEstadoBotones()
        {
            bool proyectoOk = ProyectoSeleccionado != null;

            btnAsignar.Enabled = proyectoOk && dgvDisponibles.CurrentRow?.Tag is Usuario;
            btnModificar.Enabled = proyectoOk && dgvMiembros.CurrentRow?.Tag is ProyectoMiembro;
            btnQuitar.Enabled = proyectoOk && dgvMiembros.CurrentRow?.Tag is ProyectoMiembro;
        }

        private sealed class ProyectoComboItem
        {
            public Proyecto Proyecto { get; }

            public ProyectoComboItem(Proyecto proyecto)
            {
                Proyecto = proyecto;
            }

            public override string ToString() => $"{Proyecto.ClaveProyecto} — {Proyecto.NombreProyecto}";
        }
    }
}
