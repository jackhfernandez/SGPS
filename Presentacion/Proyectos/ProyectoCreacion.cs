using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Proyectos
{
    public partial class ProyectoCreacion : Form
    {
        private readonly ProyectoLN _proyectoLN = new();
        private bool _modoAlta;

        public ProyectoCreacion()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.ProyectoCreacion);
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

        private void CargarProyectos()
        {
            try
            {
                var proyectos = _proyectoLN.ListarProyectos();
                var idSeleccionado = (dgvProyectos.CurrentRow?.Tag as Proyecto)?.ProyectoId;

                dgvProyectos.DataSource = null;
                dgvProyectos.Columns.Clear();

                dgvProyectos.EnableHeadersVisualStyles = false;
                dgvProyectos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
                dgvProyectos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvProyectos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dgvProyectos.ColumnHeadersHeight = 30;

                dgvProyectos.Columns.Add("ProyectoId", "Id");
                dgvProyectos.Columns.Add("ClaveProyecto", "Clave");
                dgvProyectos.Columns.Add("NombreProyecto", "Nombre del proyecto");
                dgvProyectos.Columns.Add("Metodologia", "Metodología");
                dgvProyectos.Columns.Add("Inicio", "Inicio");
                dgvProyectos.Columns.Add("Activo", "Activo");

                dgvProyectos.Columns["ProyectoId"]!.Width = 45;
                dgvProyectos.Columns["ClaveProyecto"]!.Width = 90;
                dgvProyectos.Columns["NombreProyecto"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvProyectos.Columns["Metodologia"]!.Width = 90;
                dgvProyectos.Columns["Inicio"]!.Width = 90;
                dgvProyectos.Columns["Activo"]!.Width = 60;

                foreach (var proyecto in proyectos)
                {
                    var indice = dgvProyectos.Rows.Add(
                        proyecto.ProyectoId,
                        proyecto.ClaveProyecto,
                        proyecto.NombreProyecto,
                        proyecto.Metodologia,
                        proyecto.FechaInicio.ToShortDateString(),
                        proyecto.EsActivo ? "Sí" : "No");

                    if (!proyecto.EsActivo)
                    {
                        dgvProyectos.Rows[indice].DefaultCellStyle.ForeColor = Color.FromArgb(150, 150, 150);
                    }

                    dgvProyectos.Rows[indice].Tag = proyecto;
                }

                if (idSeleccionado.HasValue)
                {
                    SeleccionarProyecto(idSeleccionado.Value);
                }
                else
                {
                    LimpiarEditor();
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

        private void dgvProyectos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow?.Tag is Proyecto proyecto)
            {
                txtClave.Text = proyecto.ClaveProyecto;
                txtNombre.Text = proyecto.NombreProyecto;
                txtDescripcion.Text = proyecto.Descripcion ?? string.Empty;
                cboMetodologia.Text = proyecto.Metodologia;
                dtpInicio.Value = proyecto.FechaInicio;

                if (proyecto.FechaFinEstimada.HasValue)
                {
                    dtpFinEstimada.Checked = true;
                    dtpFinEstimada.Value = proyecto.FechaFinEstimada.Value;
                }
                else
                {
                    dtpFinEstimada.Checked = false;
                }

                _modoAlta = false;
                ActualizarEstadoBotones();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarEditor();
            _modoAlta = true;
            ActualizarEstadoBotones();
            txtClave.Focus();
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
                var nuevo = ObtenerProyectoDelEditor();
                var nuevoId = _proyectoLN.CrearProyecto(nuevo);

                _modoAlta = false;
                CargarProyectos();
                SeleccionarProyecto(nuevoId);

                MessageBox.Show(
                    "Proyecto creado correctamente. El usuario en sesión fue registrado como Product Owner.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo crear el proyecto: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow?.Tag is not Proyecto proyecto)
            {
                MessageBox.Show(
                    "Selecciona un proyecto para modificar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                var modificado = ObtenerProyectoDelEditor();
                modificado.ProyectoId = proyecto.ProyectoId;
                modificado.EsActivo = proyecto.EsActivo;

                _proyectoLN.ActualizarProyecto(modificado);

                CargarProyectos();
                SeleccionarProyecto(modificado.ProyectoId);

                MessageBox.Show("Proyecto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo modificar el proyecto: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow?.Tag is not Proyecto proyecto)
            {
                MessageBox.Show(
                    "Selecciona un proyecto para activar o desactivar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var nuevoEstado = !proyecto.EsActivo;
            var confirmacion = MessageBox.Show(
                $"¿{(nuevoEstado ? "Activar" : "Desactivar")} el proyecto '{proyecto.NombreProyecto}'?",
                "Confirmar cambio de estado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _proyectoLN.CambiarEstadoActivo(proyecto.ProyectoId, nuevoEstado);

                CargarProyectos();
                SeleccionarProyecto(proyecto.ProyectoId);

                MessageBox.Show(
                    $"Proyecto {(nuevoEstado ? "activado" : "desactivado")} correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cambiar el estado del proyecto: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarEditor();
            ActualizarEstadoBotones();
            txtClave.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        private Proyecto ObtenerProyectoDelEditor()
        {
            return new Proyecto
            {
                ClaveProyecto = txtClave.Text.Trim().ToUpperInvariant(),
                NombreProyecto = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Metodologia = cboMetodologia.Text,
                FechaInicio = dtpInicio.Value.Date,
                FechaFinEstimada = dtpFinEstimada.Checked ? dtpFinEstimada.Value.Date : null
            };
        }

        private void LimpiarEditor()
        {
            txtClave.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            cboMetodologia.SelectedIndex = -1;
            dtpInicio.Value = DateTime.Today;
            dtpFinEstimada.Checked = false;
            dtpFinEstimada.Value = DateTime.Today;
            _modoAlta = false;
            dgvProyectos.ClearSelection();
            ActualizarEstadoBotones();
        }

        private void SeleccionarProyecto(int proyectoId)
        {
            foreach (DataGridViewRow fila in dgvProyectos.Rows)
            {
                if (fila.Tag is Proyecto proyecto && proyecto.ProyectoId == proyectoId)
                {
                    for (var indice = 0; indice < fila.Cells.Count; indice++)
                    {
                        if (fila.Cells[indice].Visible)
                        {
                            dgvProyectos.CurrentCell = fila.Cells[indice];
                            return;
                        }
                    }
                }
            }
        }

        private void ActualizarEstadoBotones()
        {
            btnAgregar.Enabled = _modoAlta;
            btnModificar.Enabled = dgvProyectos.CurrentRow?.Tag is Proyecto;
            btnDesactivar.Enabled = dgvProyectos.CurrentRow?.Tag is Proyecto;
        }
    }
}
