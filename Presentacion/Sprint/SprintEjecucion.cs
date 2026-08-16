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
    public partial class SprintEjecucion : Form
    {
        private readonly SprintLN _sprintLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private bool _cargando;

        public SprintEjecucion()
        {
            InitializeComponent();

            Tema.AplicarEstiloGrid(dgvSprints);
            Tema.AplicarEstiloGrid(dgvHistorias);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.SprintEjecucion);
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de ejecutar sprints.",
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
                LimpiarDetalle();
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

                dgvSprints.Columns["Nombre"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSprints.Columns["Estado"]!.Width = 80;
                dgvSprints.Columns["Inicio"]!.Width = 65;
                dgvSprints.Columns["Fin"]!.Width = 65;

                foreach (var sprint in sprints)
                {
                    var indice = dgvSprints.Rows.Add(
                        sprint.NombreSprint,
                        sprint.Estado,
                        sprint.FechaInicio.ToShortDateString(),
                        sprint.FechaFin.ToShortDateString());

                    dgvSprints.Rows[indice].Tag = sprint;
                }

                _cargando = false;

                if (idSeleccionado.HasValue)
                {
                    SeleccionarSprint(idSeleccionado.Value);
                }
                else if (sprints.Count > 0)
                {
                    // Por defecto se posiciona el Sprint activo, si lo hay.
                    var activo = _sprintLN.ObtenerSprintActivo(proyectoId);

                    if (activo != null)
                    {
                        SeleccionarSprint(activo.SprintId);
                    }
                }

                CargarHistorias();
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

            CargarHistorias();
        }

        /// <summary>Carga el Sprint Backlog del sprint seleccionado.</summary>
        private void CargarHistorias()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                LimpiarDetalle();
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                var historias = _sprintLN.ListarHistoriasDelSprint(sprint.SprintId);
                var idSeleccionado = (dgvHistorias.CurrentRow?.Tag as UserStory)?.UserStoryId;

                LlenarGridHistorias(historias);

                if (idSeleccionado.HasValue)
                {
                    SeleccionarHistoria(idSeleccionado.Value);
                }

                ActualizarEncabezado(sprint);
                ActualizarResumen(sprint, historias);
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

        private void LlenarGridHistorias(List<UserStory> historias)
        {
            var idColumnas = new[]
            {
                "CodigoTicket", "Titulo", "StoryPoints", "Estado", "EpicNombre", "UsuarioAsignadoNombre"
            };

            dgvHistorias.DataSource = null;
            dgvHistorias.Columns.Clear();

            foreach (var columna in idColumnas)
            {
                dgvHistorias.Columns.Add(columna, columna switch
                {
                    "CodigoTicket" => "Código",
                    "Titulo" => "Título",
                    "StoryPoints" => "Puntos",
                    "Estado" => "Estado",
                    "EpicNombre" => "Epic",
                    _ => "Asignado"
                });
            }

            dgvHistorias.Columns["CodigoTicket"]!.Width = 95;
            dgvHistorias.Columns["Titulo"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHistorias.Columns["StoryPoints"]!.Width = 60;
            dgvHistorias.Columns["Estado"]!.Width = 95;
            dgvHistorias.Columns["EpicNombre"]!.Width = 120;
            dgvHistorias.Columns["UsuarioAsignadoNombre"]!.Width = 120;

            foreach (var historia in historias)
            {
                var indice = dgvHistorias.Rows.Add(
                    historia.CodigoTicket,
                    historia.Titulo,
                    historia.StoryPoints,
                    historia.Estado,
                    historia.EpicNombre ?? string.Empty,
                    historia.UsuarioAsignadoNombre ?? string.Empty);

                dgvHistorias.Rows[indice].Tag = historia;
            }
        }

        private void ActualizarEncabezado(Modelo.Sprint sprint)
        {
            lblTitulo.Text = $"SPRINT — {sprint.NombreSprint}  ·  {sprint.Estado}  ·  " +
                             $"{sprint.FechaInicio.ToShortDateString()} → {sprint.FechaFin.ToShortDateString()}";
        }

        private void ActualizarResumen(Modelo.Sprint sprint, List<UserStory> historias)
        {
            var total = historias.Sum(h => h.StoryPoints);
            var completados = historias.Where(h => h.Estado == UserStoryEstadoConstantes.Done).Sum(h => h.StoryPoints);
            var restantes = total - completados;

            lblResumenSprint.Text =
                $"Historias: {historias.Count}  ·  SP total: {total}  ·  Completados: {completados}  ·  " +
                $"Restantes: {restantes}";

            lblResumen.Text =
                $"Sprint: {sprint.NombreSprint}  ·  {sprint.Estado}  ·  " +
                $"{sprint.FechaInicio.ToShortDateString()} → {sprint.FechaFin.ToShortDateString()}";
        }

        private void LimpiarDetalle()
        {
            dgvHistorias.Rows.Clear();
            lblTitulo.Text = "SPRINT —";
            lblResumenSprint.Text = "Sin sprint seleccionado.";
            lblResumen.Text = "Selecciona un sprint para ver su avance.";
        }

        private void btnAvanzar_Click(object sender, EventArgs e)
        {
            if (HistoriaSeleccionada() is not UserStory historia)
            {
                MessageBox.Show("Selecciona una historia del sprint backlog.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var siguiente = SprintLN.SiguienteEstadoHistoria(historia.Estado);
            if (siguiente is null)
            {
                MessageBox.Show($"La historia ya está en '{historia.Estado}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CambiarEstado(historia.UserStoryId, siguiente);
        }

        private void btnRetroceder_Click(object sender, EventArgs e)
        {
            if (HistoriaSeleccionada() is not UserStory historia)
            {
                MessageBox.Show("Selecciona una historia del sprint backlog.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var anterior = SprintLN.AnteriorEstadoHistoria(historia.Estado);
            if (anterior is null)
            {
                MessageBox.Show($"La historia ya está en '{historia.Estado}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CambiarEstado(historia.UserStoryId, anterior);
        }

        private void CambiarEstado(int userStoryId, string nuevoEstado)
        {
            try
            {
                if (!_sprintLN.CambiarEstadoHistoria(userStoryId, nuevoEstado, out var mensajeError))
                {
                    MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CargarHistorias();
                SeleccionarHistoria(userStoryId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cambiar el estado de la historia: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                MessageBox.Show("Selecciona un sprint para cerrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Cerrar el sprint '{sprint.NombreSprint}'? Las historias no completadas volverán a la planificación.",
                "Cerrar sprint",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (!_sprintLN.CerrarSprint(sprint.SprintId, out var mensajeError))
                {
                    MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CargarSprints();
                SeleccionarSprint(sprint.SprintId);

                MessageBox.Show("Sprint cerrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cerrar el sprint: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvHistorias_SelectionChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            ActualizarEstadoBotones();
        }

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.SprintEjecucion, NivelAcceso.Edicion);
            var sprint = SprintSeleccionado();
            var historia = HistoriaSeleccionada();
            var esActivo = sprint?.Estado == SprintEstadoConstantes.Activo;

            btnAvanzar.Enabled = puedeEditar && esActivo && historia != null &&
                                 SprintLN.SiguienteEstadoHistoria(historia.Estado) != null;
            btnRetroceder.Enabled = puedeEditar && esActivo && historia != null &&
                                    SprintLN.AnteriorEstadoHistoria(historia.Estado) != null;
            btnCerrar.Enabled = puedeEditar && esActivo;
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

        private void SeleccionarHistoria(int userStoryId)
        {
            foreach (DataGridViewRow fila in dgvHistorias.Rows)
            {
                if (fila.Tag is UserStory historia && historia.UserStoryId == userStoryId)
                {
                    dgvHistorias.CurrentCell = fila.Cells[0];
                    return;
                }
            }
        }

        private int? ProyectoSeleccionadoId() =>
            cboProyecto.SelectedItem is Proyecto proyecto ? proyecto.ProyectoId : null;

        private Modelo.Sprint? SprintSeleccionado() => dgvSprints.CurrentRow?.Tag as Modelo.Sprint;

        private UserStory? HistoriaSeleccionada() => dgvHistorias.CurrentRow?.Tag as UserStory;
    }
}