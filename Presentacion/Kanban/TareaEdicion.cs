using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Logica;
using Modelo;
using Presentacion.Ui;

namespace Presentacion.Kanban
{
    /// <summary>
    /// Desglose técnico de una historia: alta de tareas de dbo.Tareas e
    /// imputación de horas estimadas y trabajadas (FASE 3.2 del flujo de vida).
    /// Se abre como diálogo desde <see cref="TableroKanban"/> con la historia ya
    /// fijada, o como pantalla del shell con su propio selector.
    /// </summary>
    public partial class TareaEdicion : Form
    {
        private const int SinAsignar = 0;

        private readonly TareaLN _tareaLN = new();
        private readonly UserStoryLN _userStoryLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private readonly ProyectoMiembroLN _miembroLN = new();

        /// <summary>Historia recibida del tablero; null en modo pantalla.</summary>
        private readonly UserStory? _historiaFijada;

        private readonly Dictionary<int, string> _nombresPorUsuario = new();

        private UserStory? _historiaActual;
        private bool _cargando;
        private bool _modoAlta = true;

        public TareaEdicion() : this(null)
        {
        }

        public TareaEdicion(UserStory? historia)
        {
            InitializeComponent();

            _historiaFijada = historia;

            Tema.AplicarEstiloGrid(dgvTareas);
            Tema.EstiloBotonPrimario(btnGuardar);
            Tema.EstiloBotonSecundario(btnNueva);
            Tema.EstiloBotonSecundario(btnAvanzar);
            Tema.EstiloBotonSecundario(btnCerrar);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.TareaEdicion);
            }
            catch (PermisoDenegadoException ex)
            {
                MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            base.OnLoad(e);

            AjustarSplit();
            CargarEstados();

            if (_historiaFijada is not null)
            {
                // Abierto desde el tablero: la historia ya viene decidida.
                pnlSelector.Visible = false;
                CargarMiembros(_historiaFijada.ProyectoId);
                MostrarHistoria(_historiaFijada);
                return;
            }

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
                        (int)(splitVertical.Width * 0.55),
                        splitVertical.Panel1MinSize,
                        maximo);
                }
            }
            catch (Exception)
            {
                // Si el contenedor aun no tiene ancho valido se deja el reparto por defecto.
            }
        }

        // --------------------------------------------------------------- Carga

        private void CargarEstados()
        {
            _cargando = true;
            cboEstado.DataSource = TareaEstadoConstantes.Orden.ToList();
            cboEstado.SelectedItem = TareaEstadoConstantes.Pendiente;
            _cargando = false;
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de desglosar tareas.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                CargarMiembros(ProyectoSeleccionadoId());
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

        private void cboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarMiembros(ProyectoSeleccionadoId());
            CargarHistorias();
        }

        /// <summary>
        /// Equipo del proyecto: alimenta el combo de responsable y la columna
        /// "Responsable" de la rejilla, porque dbo.Tareas solo guarda el id.
        /// </summary>
        private void CargarMiembros(int? proyectoId)
        {
            _nombresPorUsuario.Clear();

            var opciones = new List<ProyectoMiembro>
            {
                new() { UsuarioId = SinAsignar, Nombres = "(sin asignar)" }
            };

            try
            {
                if (proyectoId is int id)
                {
                    var miembros = _miembroLN.ListarPorProyecto(id)
                        .OrderBy(m => m.NombreCompleto)
                        .ToList();

                    opciones.AddRange(miembros);

                    foreach (var miembro in miembros)
                    {
                        _nombresPorUsuario[miembro.UsuarioId] = miembro.NombreCompleto;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cargar el equipo del proyecto: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            _cargando = true;
            cboAsignado.DisplayMember = nameof(ProyectoMiembro.NombreCompleto);
            cboAsignado.ValueMember = nameof(ProyectoMiembro.UsuarioId);
            cboAsignado.DataSource = opciones;
            cboAsignado.SelectedIndex = 0;
            _cargando = false;
        }

        private void CargarHistorias()
        {
            if (ProyectoSeleccionadoId() is not int proyectoId)
            {
                _cargando = true;
                cboHistoria.DataSource = null;
                _cargando = false;

                MostrarHistoria(null);
                return;
            }

            try
            {
                var historias = _userStoryLN.ObtenerProductBacklogPriorizado(proyectoId)
                    .Select(h => new ItemHistoria(h))
                    .ToList();

                _cargando = true;
                cboHistoria.DataSource = historias;
                cboHistoria.SelectedIndex = historias.Count > 0 ? 0 : -1;
                _cargando = false;

                MostrarHistoria(HistoriaSeleccionada());
            }
            catch (Exception ex)
            {
                _cargando = false;

                MessageBox.Show(
                    $"No se pudieron cargar las historias: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cboHistoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            MostrarHistoria(HistoriaSeleccionada());
        }

        private void MostrarHistoria(UserStory? historia)
        {
            _historiaActual = historia;

            lblCabecera.Text = historia is null
                ? "HISTORIA —"
                : $"{historia.CodigoTicket}  ·  {historia.Titulo}  ·  {historia.Estado}  ·  {historia.StoryPoints} SP";

            CargarTareas();
        }

        private void CargarTareas()
        {
            if (_historiaActual is null)
            {
                _cargando = true;
                dgvTareas.Rows.Clear();
                _cargando = false;

                LimpiarEditor();
                lblResumenHoras.Text = "Sin historia seleccionada.";
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                var tareas = _tareaLN.ListarTareasPorUserStory(_historiaActual.UserStoryId);
                var idSeleccionado = TareaSeleccionada()?.TareaId;

                LlenarGrid(tareas);
                ActualizarResumen(tareas);

                if (idSeleccionado.HasValue && SeleccionarTarea(idSeleccionado.Value))
                {
                    CargarEditorDesdeSeleccion();
                }
                else
                {
                    LimpiarEditor();
                }

                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar las tareas técnicas: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LlenarGrid(List<Tarea> tareas)
        {
            _cargando = true;

            dgvTareas.DataSource = null;
            dgvTareas.Columns.Clear();

            dgvTareas.Columns.Add("TituloTarea", "Tarea");
            dgvTareas.Columns.Add("Estado", "Estado");
            dgvTareas.Columns.Add("HorasEstimadas", "Est.");
            dgvTareas.Columns.Add("HorasTrabajadas", "Trab.");
            dgvTareas.Columns.Add("Responsable", "Responsable");

            dgvTareas.Columns["TituloTarea"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTareas.Columns["Estado"]!.Width = 90;
            dgvTareas.Columns["HorasEstimadas"]!.Width = 58;
            dgvTareas.Columns["HorasTrabajadas"]!.Width = 62;
            dgvTareas.Columns["Responsable"]!.Width = 120;

            foreach (var tarea in tareas)
            {
                var indice = dgvTareas.Rows.Add(
                    tarea.TituloTarea,
                    tarea.Estado,
                    tarea.HorasEstimadas.ToString("0.##"),
                    tarea.HorasTrabajadas.ToString("0.##"),
                    NombreDeUsuario(tarea.UsuarioAsignadoId));

                dgvTareas.Rows[indice].Tag = tarea;
            }

            dgvTareas.ClearSelection();

            _cargando = false;
        }

        private string NombreDeUsuario(int? usuarioId)
        {
            if (usuarioId is not int id)
            {
                return string.Empty;
            }

            return _nombresPorUsuario.TryGetValue(id, out var nombre) ? nombre : $"Usuario {id}";
        }

        private void ActualizarResumen(List<Tarea> tareas)
        {
            var estimadas = tareas.Sum(t => t.HorasEstimadas);
            var trabajadas = tareas.Sum(t => t.HorasTrabajadas);
            var completadas = tareas.Count(t => t.Estado == TareaEstadoConstantes.Completado);
            var desviacion = trabajadas - estimadas;

            lblResumenHoras.Text =
                $"Tareas: {tareas.Count}  ·  Completadas: {completadas}/{tareas.Count}  ·  " +
                $"Estimadas: {estimadas:0.##} h  ·  Trabajadas: {trabajadas:0.##} h  ·  " +
                $"Desviación: {desviacion:+0.##;-0.##;0} h";
        }

        // ------------------------------------------------------------- Editor

        private void dgvTareas_SelectionChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarEditorDesdeSeleccion();
            ActualizarEstadoBotones();
        }

        private void dgvTareas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtTitulo.Focus();
            }
        }

        private void CargarEditorDesdeSeleccion()
        {
            if (TareaSeleccionada() is not Tarea tarea)
            {
                LimpiarEditor();
                return;
            }

            _cargando = true;

            _modoAlta = false;
            lblTituloDetalle.Text = $"TAREA #{tarea.TareaId}";
            txtTitulo.Text = tarea.TituloTarea;
            cboEstado.SelectedItem = tarea.Estado;
            numHorasEstimadas.Value = AcotarHoras(tarea.HorasEstimadas);
            numHorasTrabajadas.Value = AcotarHoras(tarea.HorasTrabajadas);
            cboAsignado.SelectedValue = tarea.UsuarioAsignadoId ?? SinAsignar;

            _cargando = false;
        }

        private void LimpiarEditor()
        {
            _cargando = true;

            _modoAlta = true;
            lblTituloDetalle.Text = "NUEVA TAREA TÉCNICA";
            txtTitulo.Clear();
            cboEstado.SelectedItem = TareaEstadoConstantes.Pendiente;
            numHorasEstimadas.Value = 0;
            numHorasTrabajadas.Value = 0;

            if (cboAsignado.Items.Count > 0)
            {
                cboAsignado.SelectedIndex = 0;
            }

            _cargando = false;
        }

        /// <summary>dbo.Tareas guarda DECIMAL(5,2): el control no admite mas.</summary>
        private decimal AcotarHoras(decimal horas) =>
            Math.Clamp(horas, numHorasEstimadas.Minimum, numHorasEstimadas.Maximum);

        // ------------------------------------------------------------ Acciones

        private void btnNueva_Click(object sender, EventArgs e)
        {
            _cargando = true;
            dgvTareas.ClearSelection();
            _cargando = false;

            LimpiarEditor();
            ActualizarEstadoBotones();
            txtTitulo.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_historiaActual is not UserStory historia)
            {
                MessageBox.Show("Selecciona una historia antes de registrar tareas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título de la tarea técnica es obligatorio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTitulo.Focus();
                return;
            }

            try
            {
                int tareaId;

                if (_modoAlta)
                {
                    tareaId = _tareaLN.RegistrarTarea(new Tarea
                    {
                        UserStoryId = historia.UserStoryId,
                        TituloTarea = txtTitulo.Text.Trim(),
                        HorasEstimadas = numHorasEstimadas.Value,
                        HorasTrabajadas = numHorasTrabajadas.Value,
                        Estado = EstadoElegido(),
                        UsuarioAsignadoId = ResponsableElegido()
                    });
                }
                else
                {
                    if (TareaSeleccionada() is not Tarea tarea)
                    {
                        return;
                    }

                    tarea.TituloTarea = txtTitulo.Text.Trim();
                    tarea.HorasEstimadas = numHorasEstimadas.Value;
                    tarea.HorasTrabajadas = numHorasTrabajadas.Value;
                    tarea.Estado = EstadoElegido();
                    tarea.UsuarioAsignadoId = ResponsableElegido();

                    _tareaLN.ActualizarTarea(tarea);
                    tareaId = tarea.TareaId;
                }

                CargarTareas();
                SeleccionarTarea(tareaId);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                MessageBox.Show(ex.Message, "Datos no válidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo guardar la tarea técnica: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAvanzar_Click(object sender, EventArgs e)
        {
            if (TareaSeleccionada() is not Tarea tarea)
            {
                MessageBox.Show("Selecciona una tarea de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (TareaLN.SiguienteEstadoTarea(tarea.Estado) is not string siguiente)
            {
                MessageBox.Show($"La tarea ya está en '{tarea.Estado}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _tareaLN.CambiarEstadoTareaTecnica(tarea.TareaId, siguiente);

                CargarTareas();
                SeleccionarTarea(tarea.TareaId);
            }
            catch (Exception ex) when (ex is InvalidOperationException or KeyNotFoundException)
            {
                MessageBox.Show(ex.Message, "Movimiento no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cambiar el estado de la tarea: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();

        /// <summary>
        /// Un unico punto para el Enabled de las acciones: permiso de edicion +
        /// historia vigente + seleccion en la rejilla.
        /// </summary>
        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.TareaEdicion, NivelAcceso.Edicion);
            var hayHistoria = _historiaActual is not null;
            var tarea = TareaSeleccionada();

            btnGuardar.Enabled = puedeEditar && hayHistoria;
            btnNueva.Enabled = puedeEditar && hayHistoria;
            btnAvanzar.Enabled = puedeEditar && tarea != null &&
                                 TareaLN.SiguienteEstadoTarea(tarea.Estado) != null;

            txtTitulo.ReadOnly = !puedeEditar;
            cboEstado.Enabled = puedeEditar;
            cboAsignado.Enabled = puedeEditar;
            numHorasEstimadas.Enabled = puedeEditar;
            numHorasTrabajadas.Enabled = puedeEditar;
        }

        // ------------------------------------------------------------ Consultas

        private string EstadoElegido() =>
            cboEstado.SelectedItem as string ?? TareaEstadoConstantes.Pendiente;

        private int? ResponsableElegido() =>
            cboAsignado.SelectedItem is ProyectoMiembro miembro && miembro.UsuarioId != SinAsignar
                ? miembro.UsuarioId
                : null;

        private int? ProyectoSeleccionadoId() =>
            cboProyecto.SelectedItem is Proyecto proyecto ? proyecto.ProyectoId : null;

        private UserStory? HistoriaSeleccionada() =>
            (cboHistoria.SelectedItem as ItemHistoria)?.Historia;

        private Tarea? TareaSeleccionada() => dgvTareas.CurrentRow?.Tag as Tarea;

        private bool SeleccionarTarea(int tareaId)
        {
            foreach (DataGridViewRow fila in dgvTareas.Rows)
            {
                if (fila.Tag is Tarea tarea && tarea.TareaId == tareaId)
                {
                    dgvTareas.CurrentCell = fila.Cells[0];
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Envoltorio de la historia para el combo: el ComboBox necesita un
        /// texto y UserStory no tiene una propiedad "codigo + titulo".
        /// </summary>
        private sealed class ItemHistoria
        {
            public ItemHistoria(UserStory historia) => Historia = historia;

            public UserStory Historia { get; }

            public override string ToString() => $"{Historia.CodigoTicket} — {Historia.Titulo}";
        }
    }
}
