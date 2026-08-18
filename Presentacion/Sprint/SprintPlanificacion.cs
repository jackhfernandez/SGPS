using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;
using Presentacion.Ui;

namespace Presentacion.Sprint
{
    public partial class SprintPlanificacion : Form
    {
        private readonly SprintLN _sprintLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private bool _cargando;
        private bool _modoAlta;

        public SprintPlanificacion()
        {
            InitializeComponent();

            Tema.AplicarEstiloGrid(dgvSprints);
            Tema.AplicarEstiloGrid(dgvDisponibles);
            Tema.AplicarEstiloGrid(dgvSprintBacklog);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.SprintPlanificacion);
            }
            catch (PermisoDenegadoException ex)
            {
                MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            base.OnLoad(e);

            AjustarSplit();
            CargarProyectos();
        }

        /// <summary>
        /// Reparte la barra del split en el ancho real del panel incrustado. Se
        /// aplica en OnLoad porque en InitializeComponent el contenedor aun no
        /// tiene su tamano final y fijar SplitterDistance ahi lanza
        /// ArgumentOutOfRangeException.
        /// </summary>
        private void AjustarSplit()
        {
            try
            {
                var maximo = splitVertical.Width - splitVertical.Panel2MinSize - splitVertical.SplitterWidth;

                if (maximo > splitVertical.Panel1MinSize)
                {
                    splitVertical.SplitterDistance = Math.Clamp(
                        (int)(splitVertical.Width * 0.30),
                        splitVertical.Panel1MinSize,
                        maximo);
                }
            }
            catch (Exception)
            {
                // Si el contenedor aun no tiene ancho valido se deja el reparto por defecto.
            }
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de planificar sprints.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                CargarSprints();
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

        private void cboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarSprints();
        }

        private void CargarSprints()
        {
            if (ProyectoSeleccionadoId() is not int proyectoId)
            {
                dgvSprints.Rows.Clear();
                LimpiarBacklogs();
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                var sprints = _sprintLN.ListarSprintsPorProyecto(proyectoId);
                var idSeleccionado = (dgvSprints.CurrentRow?.Tag as Modelo.Sprint)?.SprintId;

                _cargando = true;

                dgvSprints.DataSource = null;
                dgvSprints.Columns.Clear();

                dgvSprints.Columns.Add("Nombre", "Nombre");
                dgvSprints.Columns.Add("Estado", "Estado");
                dgvSprints.Columns.Add("Inicio", "Inicio");
                dgvSprints.Columns.Add("Fin", "Fin");
                dgvSprints.Columns.Add("SP", "SP");

                dgvSprints.Columns["Nombre"]!.Width = 180;
                dgvSprints.Columns["Estado"]!.Width = 80;
                dgvSprints.Columns["Inicio"]!.Width = 65;
                dgvSprints.Columns["Fin"]!.Width = 65;
                dgvSprints.Columns["SP"]!.Width = 40;

                foreach (var sprint in sprints)
                {
                    var indice = dgvSprints.Rows.Add(
                        sprint.NombreSprint,
                        sprint.Estado,
                        sprint.FechaInicio.ToShortDateString(),
                        sprint.FechaFin.ToShortDateString(),
                        _sprintLN.CalcularStoryPointsComprometidos(sprint.SprintId));

                    dgvSprints.Rows[indice].Tag = sprint;
                }

                _cargando = false;

                if (idSeleccionado.HasValue)
                {
                    SeleccionarSprint(idSeleccionado.Value);
                }

                CargarBacklogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los sprints: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvSprints_SelectionChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarBacklogs();
        }

        /// <summary>Carga las historias disponibles y las del Sprint Backlog del sprint seleccionado.</summary>
        private void CargarBacklogs()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                LimpiarBacklogs();
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                var historiasSprint = _sprintLN.ListarHistoriasDelSprint(sprint.SprintId);
                var disponibles = _sprintLN.ListarBacklogDisponible(sprint.ProyectoId);

                var idSeleccionadoDisponible = (dgvDisponibles.CurrentRow?.Tag as UserStory)?.UserStoryId;
                var idSeleccionadoSprint = (dgvSprintBacklog.CurrentRow?.Tag as UserStory)?.UserStoryId;

                LlenarGridHistorias(dgvDisponibles, disponibles);
                LlenarGridHistorias(dgvSprintBacklog, historiasSprint);

                if (idSeleccionadoDisponible.HasValue)
                {
                    SeleccionarHistoria(dgvDisponibles, idSeleccionadoDisponible.Value);
                }

                if (idSeleccionadoSprint.HasValue)
                {
                    SeleccionarHistoria(dgvSprintBacklog, idSeleccionadoSprint.Value);
                }

                ActualizarResumen(sprint, historiasSprint, disponibles);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cargar el sprint backlog: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LlenarGridHistorias(DataGridView grid, List<UserStory> historias)
        {
            var idColumnas = new[]
            {
                "CodigoTicket", "Titulo", "Epic", "ValorNegocio", "StoryPoints", "Estado"
            };

            grid.DataSource = null;
            grid.Columns.Clear();

            foreach (var columna in idColumnas)
            {
                grid.Columns.Add(columna, columna switch
                {
                    "CodigoTicket" => "Código",
                    "Titulo" => "Título",
                    "Epic" => "Epic",
                    "ValorNegocio" => "Valor",
                    "StoryPoints" => "Puntos",
                    _ => "Estado"
                });
            }

            grid.Columns["CodigoTicket"]!.Width = 95;
            grid.Columns["Titulo"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns["Epic"]!.Width = 130;
            grid.Columns["ValorNegocio"]!.Width = 65;
            grid.Columns["StoryPoints"]!.Width = 60;
            grid.Columns["Estado"]!.Width = 95;

            foreach (var historia in historias)
            {
                var indice = grid.Rows.Add(
                    historia.CodigoTicket,
                    historia.Titulo,
                    historia.EpicNombre ?? string.Empty,
                    historia.ValorNegocio,
                    historia.StoryPoints,
                    historia.Estado);

                grid.Rows[indice].Tag = historia;
            }
        }

        private void ActualizarResumen(Modelo.Sprint sprint, List<UserStory> historiasSprint, List<UserStory> disponibles)
        {
            var puntosSprint = historiasSprint.Sum(h => h.StoryPoints);
            var puntosDisponibles = disponibles.Sum(h => h.StoryPoints);

            lblResumen.Text =
                $"Sprint: {sprint.NombreSprint}  ·  {sprint.Estado}  ·  " +
                $"{historiasSprint.Count} historia(s) / {puntosSprint} SP  |  " +
                $"Backlog disponible: {disponibles.Count} historia(s) / {puntosDisponibles} SP";
        }

        private void LimpiarBacklogs()
        {
            dgvDisponibles.Rows.Clear();
            dgvSprintBacklog.Rows.Clear();
            lblResumen.Text = "Selecciona un sprint para ver su backlog.";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (ProyectoSeleccionadoId() is null)
            {
                MessageBox.Show(
                    "Selecciona un proyecto antes de crear un sprint.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            LimpiarEditor();
            dtpInicio.Value = DateTime.Today;
            dtpFin.Value = DateTime.Today.AddDays(14);
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

            if (ProyectoSeleccionadoId() is not int proyectoId)
            {
                return;
            }

            try
            {
                var nuevo = new Modelo.Sprint
                {
                    ProyectoId = proyectoId,
                    NombreSprint = txtNombre.Text.Trim(),
                    SprintGoal = string.IsNullOrWhiteSpace(txtObjetivo.Text) ? null : txtObjetivo.Text.Trim(),
                    FechaInicio = dtpInicio.Value,
                    FechaFin = dtpFin.Value
                };

                if (!_sprintLN.CrearSprint(nuevo, out var mensajeError))
                {
                    MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _modoAlta = false;
                CargarSprints();
                SeleccionarSprint(nuevo.SprintId);

                MessageBox.Show("Sprint creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo crear el sprint: {ex.Message}",
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

        private void btnAsignar_Click(object sender, EventArgs e) => AsignarSeleccionadas();

        private void btnAsignarTodos_Click(object sender, EventArgs e) => AsignarTodas();

        private void btnQuitar_Click(object sender, EventArgs e) => QuitarSeleccionadas();

        private void btnQuitarTodos_Click(object sender, EventArgs e) => QuitarTodas();

        private void AsignarSeleccionadas()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                return;
            }

            if (dgvDisponibles.CurrentRow?.Tag is not UserStory historia)
            {
                MessageBox.Show("Selecciona una historia del backlog disponible.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AsignarALista(sprint, new List<UserStory> { historia });
        }

        private void AsignarTodas()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                return;
            }

            var historias = new List<UserStory>();

            foreach (DataGridViewRow fila in dgvDisponibles.Rows)
            {
                if (fila.Tag is UserStory historia)
                {
                    historias.Add(historia);
                }
            }

            if (historias.Count == 0)
            {
                MessageBox.Show("No hay historias disponibles para asignar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AsignarALista(sprint, historias);
        }

        private void AsignarALista(Modelo.Sprint sprint, List<UserStory> historias)
        {
            try
            {
                foreach (var historia in historias)
                {
                    _sprintLN.AsignarHistoriaASprint(historia.UserStoryId, sprint.SprintId, out _);
                }

                CargarBacklogs();
                ActualizarResumenSprintActual();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron asignar las historias: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void QuitarSeleccionadas()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                return;
            }

            if (dgvSprintBacklog.CurrentRow?.Tag is not UserStory historia)
            {
                MessageBox.Show("Selecciona una historia del sprint backlog.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            QuitarDeLista(sprint, new List<UserStory> { historia });
        }

        private void QuitarTodas()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                return;
            }

            var historias = new List<UserStory>();

            foreach (DataGridViewRow fila in dgvSprintBacklog.Rows)
            {
                if (fila.Tag is UserStory historia)
                {
                    historias.Add(historia);
                }
            }

            if (historias.Count == 0)
            {
                MessageBox.Show("El sprint backlog está vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            QuitarDeLista(sprint, historias);
        }

        private void QuitarDeLista(Modelo.Sprint sprint, List<UserStory> historias)
        {
            try
            {
                var huboError = false;

                foreach (var historia in historias)
                {
                    if (!_sprintLN.QuitarHistoriaDeSprint(historia.UserStoryId, out var mensajeError))
                    {
                        huboError = true;
                        MessageBox.Show(mensajeError, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }

                CargarBacklogs();
                ActualizarResumenSprintActual();

                if (!huboError)
                {
                    ActualizarEstadoBotones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron quitar las historias: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                MessageBox.Show("Selecciona un sprint para iniciar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (sprint.Estado != SprintEstadoConstantes.Planificado)
            {
                MessageBox.Show("Solo se puede iniciar un sprint en estado 'Planificado'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (!_sprintLN.IniciarSprint(sprint.SprintId, (int)nudCapacidad.Value, out var mensajeError))
                {
                    MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CargarSprints();
                SeleccionarSprint(sprint.SprintId);

                MessageBox.Show("Sprint iniciado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo iniciar el sprint: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        private void ActualizarResumenSprintActual()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                return;
            }

            var historiasSprint = _sprintLN.ListarHistoriasDelSprint(sprint.SprintId);
            var disponibles = _sprintLN.ListarBacklogDisponible(sprint.ProyectoId);
            ActualizarResumen(sprint, historiasSprint, disponibles);
        }

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.SprintPlanificacion, NivelAcceso.Edicion);
            var sprint = SprintSeleccionado();
            var haySprint = sprint != null;
            var esPlanificado = sprint?.Estado == SprintEstadoConstantes.Planificado;

            btnNuevo.Enabled = puedeEditar;
            btnAgregar.Enabled = puedeEditar && _modoAlta;
            btnAsignar.Enabled = puedeEditar && haySprint && esPlanificado && dgvDisponibles.Rows.Count > 0;
            btnAsignarTodos.Enabled = puedeEditar && haySprint && esPlanificado && dgvDisponibles.Rows.Count > 0;
            btnQuitar.Enabled = puedeEditar && haySprint && esPlanificado && dgvSprintBacklog.Rows.Count > 0;
            btnQuitarTodos.Enabled = puedeEditar && haySprint && esPlanificado && dgvSprintBacklog.Rows.Count > 0;
            btnIniciar.Enabled = puedeEditar && haySprint && esPlanificado;
        }

        private void LimpiarEditor()
        {
            txtNombre.Text = string.Empty;
            txtObjetivo.Text = string.Empty;
            _modoAlta = false;
            ActualizarEstadoBotones();
        }

        private void SeleccionarSprint(int sprintId)
        {
            foreach (DataGridViewRow fila in dgvSprints.Rows)
            {
                if (fila.Tag is Modelo.Sprint sprint && sprint.SprintId == sprintId)
                {
                    dgvSprints.CurrentCell = fila.Cells[0];
                    return;
                }
            }
        }

        private static void SeleccionarHistoria(DataGridView grid, int userStoryId)
        {
            foreach (DataGridViewRow fila in grid.Rows)
            {
                if (fila.Tag is UserStory historia && historia.UserStoryId == userStoryId)
                {
                    grid.CurrentCell = fila.Cells[0];
                    return;
                }
            }
        }

        private int? ProyectoSeleccionadoId() =>
            cboProyecto.SelectedItem is Proyecto proyecto ? proyecto.ProyectoId : null;

        private Modelo.Sprint? SprintSeleccionado() => dgvSprints.CurrentRow?.Tag as Modelo.Sprint;
    }
}