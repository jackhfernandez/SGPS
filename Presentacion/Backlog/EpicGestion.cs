using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Backlog
{
    public partial class EpicGestion : Form
    {
        private readonly EpicLN _epicLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private bool _modoAlta;
        private bool _cargando;

        public EpicGestion()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.EpicGestion);
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
                _cargando = true;

                var proyectos = _proyectoLN.ListarProyectos()
                    .Where(p => p.EsActivo)
                    .OrderBy(p => p.NombreProyecto)
                    .ToList();

                cboProyecto.DisplayMember = nameof(Proyecto.NombreProyecto);
                cboProyecto.ValueMember = nameof(Proyecto.ProyectoId);
                cboProyecto.DataSource = proyectos;
                cboProyecto.SelectedIndex = proyectos.Count > 0 ? 0 : -1;

                _cargando = false;

                if (proyectos.Count == 0)
                {
                    MessageBox.Show(
                        "No hay proyectos activos registrados. Crea un proyecto antes de definir epics.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                CargarEpics();
            }
            catch (Exception ex)
            {
                _cargando = false;

                MessageBox.Show(
                    $"No se pudieron cargar los proyectos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarEpics()
        {
            if (ProyectoSeleccionadoId() is not int proyectoId)
            {
                dgvEpics.Rows.Clear();
                LimpiarEditor();
                return;
            }

            try
            {
                var epics = _epicLN.ListarPorProyecto(proyectoId);
                var idSeleccionado = (dgvEpics.CurrentRow?.Tag as Epic)?.EpicId;

                dgvEpics.DataSource = null;
                dgvEpics.Columns.Clear();

                dgvEpics.EnableHeadersVisualStyles = false;
                dgvEpics.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
                dgvEpics.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvEpics.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dgvEpics.ColumnHeadersHeight = 30;

                dgvEpics.Columns.Add("EpicId", "Id");
                dgvEpics.Columns.Add("Titulo", "Título");
                dgvEpics.Columns.Add("Descripcion", "Descripción");
                dgvEpics.Columns.Add("ColorHex", "Color");
                dgvEpics.Columns.Add("Creacion", "Creación");

                dgvEpics.Columns["EpicId"]!.Width = 45;
                dgvEpics.Columns["Titulo"]!.Width = 240;
                dgvEpics.Columns["Descripcion"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvEpics.Columns["ColorHex"]!.Width = 80;
                dgvEpics.Columns["Creacion"]!.Width = 90;

                foreach (var epic in epics)
                {
                    var indice = dgvEpics.Rows.Add(
                        epic.EpicId,
                        epic.Titulo,
                        epic.Descripcion ?? string.Empty,
                        epic.ColorHex,
                        epic.FechaCreacion.ToShortDateString());

                    dgvEpics.Rows[indice].Cells["ColorHex"].Style.BackColor = ConvertirColor(epic.ColorHex);
                    dgvEpics.Rows[indice].Cells["ColorHex"].Style.ForeColor = Color.White;
                    dgvEpics.Rows[indice].Tag = epic;
                }

                if (idSeleccionado.HasValue)
                {
                    SeleccionarEpic(idSeleccionado.Value);
                }
                else
                {
                    LimpiarEditor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los epics: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarEpics();
        }

        private void dgvEpics_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEpics.CurrentRow?.Tag is Epic epic)
            {
                txtTitulo.Text = epic.Titulo;
                txtDescripcion.Text = epic.Descripcion ?? string.Empty;
                txtColor.Text = epic.ColorHex;

                _modoAlta = false;
                ActualizarEstadoBotones();
            }
        }

        private void txtColor_TextChanged(object sender, EventArgs e)
        {
            pnlColor.BackColor = ConvertirColor(txtColor.Text);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (ProyectoSeleccionadoId() is null)
            {
                MessageBox.Show(
                    "Selecciona un proyecto antes de crear un epic.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            LimpiarEditor();
            txtColor.Text = "#3182CE";
            _modoAlta = true;
            ActualizarEstadoBotones();
            txtTitulo.Focus();
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
                var nuevo = ObtenerEpicDelEditor();
                var nuevoId = _epicLN.CrearEpic(nuevo);

                _modoAlta = false;
                CargarEpics();
                SeleccionarEpic(nuevoId);

                MessageBox.Show("Epic creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo crear el epic: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvEpics.CurrentRow?.Tag is not Epic epic)
            {
                MessageBox.Show(
                    "Selecciona un epic para modificar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                var modificado = ObtenerEpicDelEditor();
                modificado.EpicId = epic.EpicId;
                modificado.ProyectoId = epic.ProyectoId;

                _epicLN.ActualizarEpic(modificado);

                CargarEpics();
                SeleccionarEpic(modificado.EpicId);

                MessageBox.Show("Epic actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo modificar el epic: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEpics.CurrentRow?.Tag is not Epic epic)
            {
                MessageBox.Show(
                    "Selecciona un epic para eliminar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar el epic '{epic.Titulo}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _epicLN.EliminarEpic(epic.EpicId);

                CargarEpics();
                LimpiarEditor();

                MessageBox.Show("Epic eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo eliminar el epic: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarEditor();
            ActualizarEstadoBotones();
            txtTitulo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        private Epic ObtenerEpicDelEditor()
        {
            return new Epic
            {
                ProyectoId = ProyectoSeleccionadoId() ?? 0,
                Titulo = txtTitulo.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                ColorHex = txtColor.Text.Trim()
            };
        }

        private void LimpiarEditor()
        {
            txtTitulo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtColor.Text = string.Empty;
            _modoAlta = false;
            dgvEpics.ClearSelection();
            ActualizarEstadoBotones();
        }

        private void SeleccionarEpic(int epicId)
        {
            foreach (DataGridViewRow fila in dgvEpics.Rows)
            {
                if (fila.Tag is Epic epic && epic.EpicId == epicId)
                {
                    for (var indice = 0; indice < fila.Cells.Count; indice++)
                    {
                        if (fila.Cells[indice].Visible)
                        {
                            dgvEpics.CurrentCell = fila.Cells[indice];
                            return;
                        }
                    }
                }
            }
        }

        private int? ProyectoSeleccionadoId() =>
            cboProyecto.SelectedItem is Proyecto proyecto ? proyecto.ProyectoId : null;

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.EpicGestion, NivelAcceso.Edicion);
            var haySeleccion = dgvEpics.CurrentRow?.Tag is Epic;

            btnNuevo.Enabled = puedeEditar;
            btnAgregar.Enabled = puedeEditar && _modoAlta;
            btnModificar.Enabled = puedeEditar && haySeleccion;
            btnEliminar.Enabled = puedeEditar && haySeleccion;
        }

        /// <summary>Convierte el #RRGGBB de la entidad a Color; si no es valido usa el gris del tema.</summary>
        private static Color ConvertirColor(string? colorHex)
        {
            if (string.IsNullOrWhiteSpace(colorHex))
            {
                return Color.FromArgb(200, 200, 200);
            }

            try
            {
                return ColorTranslator.FromHtml(colorHex.Trim());
            }
            catch (Exception)
            {
                return Color.FromArgb(200, 200, 200);
            }
        }
    }
}
