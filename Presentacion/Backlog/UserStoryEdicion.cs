using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Backlog
{
    public partial class UserStoryEdicion : Form
    {
        private readonly UserStoryLN _userStoryLN = new();
        private readonly EpicLN _epicLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private bool _modoAlta;
        private bool _cargando;

        public UserStoryEdicion()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.UserStoryEdicion);
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de redactar historias.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                CargarEpics();
                CargarHistorias();
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
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                cboEpic.DataSource = null;
                return;
            }

            try
            {
                // El primer elemento representa "sin epic" (EpicId nulo en la BD).
                var epics = new List<Epic> { new() { EpicId = 0, Titulo = "(sin epic)" } };
                epics.AddRange(_epicLN.ListarPorProyecto(proyecto.ProyectoId));

                cboEpic.DisplayMember = nameof(Epic.Titulo);
                cboEpic.ValueMember = nameof(Epic.EpicId);
                cboEpic.DataSource = epics;
                cboEpic.SelectedIndex = 0;
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

        private void CargarHistorias()
        {
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                dgvHistorias.Rows.Clear();
                LimpiarEditor();
                return;
            }

            try
            {
                var historias = _userStoryLN.ObtenerProductBacklogPriorizado(proyecto.ProyectoId);
                var idSeleccionado = (dgvHistorias.CurrentRow?.Tag as UserStory)?.UserStoryId;

                dgvHistorias.DataSource = null;
                dgvHistorias.Columns.Clear();

                dgvHistorias.EnableHeadersVisualStyles = false;
                dgvHistorias.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
                dgvHistorias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvHistorias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dgvHistorias.ColumnHeadersHeight = 30;

                dgvHistorias.Columns.Add("CodigoTicket", "Código");
                dgvHistorias.Columns.Add("Titulo", "Título");
                dgvHistorias.Columns.Add("Epic", "Epic");
                dgvHistorias.Columns.Add("ValorNegocio", "Valor");
                dgvHistorias.Columns.Add("StoryPoints", "Puntos");
                dgvHistorias.Columns.Add("Estado", "Estado");

                dgvHistorias.Columns["CodigoTicket"]!.Width = 100;
                dgvHistorias.Columns["Titulo"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvHistorias.Columns["Epic"]!.Width = 160;
                dgvHistorias.Columns["ValorNegocio"]!.Width = 70;
                dgvHistorias.Columns["StoryPoints"]!.Width = 60;
                dgvHistorias.Columns["Estado"]!.Width = 100;

                var epicsPorId = ObtenerEpicsPorId();

                foreach (var historia in historias)
                {
                    var nombreEpic = historia.EpicId.HasValue && epicsPorId.TryGetValue(historia.EpicId.Value, out var titulo)
                        ? titulo
                        : string.Empty;

                    var indice = dgvHistorias.Rows.Add(
                        historia.CodigoTicket,
                        historia.Titulo,
                        nombreEpic,
                        historia.ValorNegocio,
                        historia.StoryPoints,
                        historia.Estado);

                    dgvHistorias.Rows[indice].Tag = historia;
                }

                if (idSeleccionado.HasValue)
                {
                    SeleccionarHistoria(idSeleccionado.Value);
                }
                else
                {
                    LimpiarEditor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar las historias: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private Dictionary<int, string> ObtenerEpicsPorId()
        {
            var mapa = new Dictionary<int, string>();

            if (cboEpic.DataSource is List<Epic> epics)
            {
                foreach (var epic in epics.Where(e => e.EpicId > 0))
                {
                    mapa[epic.EpicId] = epic.Titulo;
                }
            }

            return mapa;
        }

        private void cboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarEpics();
            CargarHistorias();
        }

        private void dgvHistorias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHistorias.CurrentRow?.Tag is UserStory historia)
            {
                txtCodigo.Text = historia.CodigoTicket;
                txtTitulo.Text = historia.Titulo;
                txtComo.Text = historia.ComoUsuario;
                txtQuiero.Text = historia.QuieroFuncionalidad;
                txtPara.Text = historia.ParaBeneficio;
                txtCriterios.Text = historia.CriteriosAceptacionTexto ?? string.Empty;
                cboValor.Text = historia.ValorNegocio;
                cboEstado.Text = historia.Estado;
                cboPuntos.SelectedItem = historia.StoryPoints;
                cboEpic.SelectedValue = historia.EpicId ?? 0;

                _modoAlta = false;
                ActualizarEstadoBotones();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                MessageBox.Show(
                    "Selecciona un proyecto antes de crear una historia.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            LimpiarEditor();

            try
            {
                // El código se genera solo, con el formato CLAVE-N de la especificación.
                txtCodigo.Text = _userStoryLN.GenerarCodigoTicket(proyecto.ProyectoId, proyecto.ClaveProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo generar el código de ticket: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            cboValor.Text = ValorNegocioConstantes.Medio;
            cboEstado.Text = UserStoryEstadoConstantes.ToDo;
            cboPuntos.SelectedItem = 3;
            cboEpic.SelectedValue = 0;

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
                var nueva = ObtenerHistoriaDelEditor();
                _userStoryLN.Crear(nueva);

                _modoAlta = false;
                CargarHistorias();
                SeleccionarHistoria(nueva.UserStoryId);

                MessageBox.Show("Historia creada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo crear la historia: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvHistorias.CurrentRow?.Tag is not UserStory historia)
            {
                MessageBox.Show(
                    "Selecciona una historia para modificar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                var modificada = ObtenerHistoriaDelEditor();
                modificada.UserStoryId = historia.UserStoryId;
                modificada.CodigoTicket = historia.CodigoTicket;
                modificada.ProyectoId = historia.ProyectoId;
                modificada.SprintId = historia.SprintId;
                modificada.OrdenPrioridad = historia.OrdenPrioridad;
                modificada.UsuarioAsignadoId = historia.UsuarioAsignadoId;

                _userStoryLN.Modificar(modificada);

                CargarHistorias();
                SeleccionarHistoria(modificada.UserStoryId);

                MessageBox.Show("Historia actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo modificar la historia: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvHistorias.CurrentRow?.Tag is not UserStory historia)
            {
                MessageBox.Show(
                    "Selecciona una historia para eliminar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar la historia '{historia.CodigoTicket} - {historia.Titulo}'?\n\n" +
                "También se eliminarán sus tareas y comentarios.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _userStoryLN.Eliminar(historia.UserStoryId);

                CargarHistorias();
                LimpiarEditor();

                MessageBox.Show("Historia eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo eliminar la historia: {ex.Message}",
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

        private UserStory ObtenerHistoriaDelEditor()
        {
            var epicId = cboEpic.SelectedItem is Epic epic && epic.EpicId > 0 ? epic.EpicId : (int?)null;

            return new UserStory
            {
                CodigoTicket = txtCodigo.Text.Trim(),
                ProyectoId = ProyectoSeleccionado()?.ProyectoId ?? 0,
                EpicId = epicId,
                Titulo = txtTitulo.Text.Trim(),
                ComoUsuario = txtComo.Text.Trim(),
                QuieroFuncionalidad = txtQuiero.Text.Trim(),
                ParaBeneficio = txtPara.Text.Trim(),
                CriteriosAceptacionTexto = txtCriterios.Text.Trim(),
                ValorNegocio = cboValor.Text,
                StoryPoints = cboPuntos.SelectedItem is int puntos ? puntos : 0,
                Estado = string.IsNullOrWhiteSpace(cboEstado.Text) ? UserStoryEstadoConstantes.ToDo : cboEstado.Text
            };
        }

        private void LimpiarEditor()
        {
            txtCodigo.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            txtComo.Text = string.Empty;
            txtQuiero.Text = string.Empty;
            txtPara.Text = string.Empty;
            txtCriterios.Text = string.Empty;
            cboValor.SelectedIndex = -1;
            cboEstado.SelectedIndex = -1;
            cboPuntos.SelectedIndex = -1;

            if (cboEpic.Items.Count > 0)
            {
                cboEpic.SelectedIndex = 0;
            }

            _modoAlta = false;
            dgvHistorias.ClearSelection();
            ActualizarEstadoBotones();
        }

        private void SeleccionarHistoria(int userStoryId)
        {
            foreach (DataGridViewRow fila in dgvHistorias.Rows)
            {
                if (fila.Tag is UserStory historia && historia.UserStoryId == userStoryId)
                {
                    for (var indice = 0; indice < fila.Cells.Count; indice++)
                    {
                        if (fila.Cells[indice].Visible)
                        {
                            dgvHistorias.CurrentCell = fila.Cells[indice];
                            return;
                        }
                    }
                }
            }
        }

        private Proyecto? ProyectoSeleccionado() => cboProyecto.SelectedItem as Proyecto;

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.UserStoryEdicion, NivelAcceso.Edicion);
            var haySeleccion = dgvHistorias.CurrentRow?.Tag is UserStory;

            btnNuevo.Enabled = puedeEditar;
            btnAgregar.Enabled = puedeEditar && _modoAlta;
            btnModificar.Enabled = puedeEditar && haySeleccion;
            btnEliminar.Enabled = puedeEditar && haySeleccion;
        }
    }
}
