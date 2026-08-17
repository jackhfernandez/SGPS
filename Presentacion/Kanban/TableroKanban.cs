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
    /// Tablero del sprint: cuatro columnas de estado con las historias del
    /// Sprint Backlog. El equipo arrastra las tarjetas entre columnas y abre el
    /// desglose técnico con doble clic (FASE 3.2 del flujo de vida).
    /// </summary>
    public partial class TableroKanban : Form
    {
        /// <summary>Columnas del tablero, en el orden del flujo Kanban.</summary>
        private static readonly string[] EstadosColumna =
        {
            UserStoryEstadoConstantes.ToDo,
            UserStoryEstadoConstantes.InProgress,
            UserStoryEstadoConstantes.InTesting,
            UserStoryEstadoConstantes.Done
        };

        private readonly SprintLN _sprintLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private readonly TareaLN _tareaLN = new();
        private readonly List<ColumnaKanban> _columnas = new();

        private bool _cargando;
        private int? _historiaSeleccionadaId;

        public TableroKanban()
        {
            InitializeComponent();
            ConstruirColumnas();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.TableroKanban);
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

        // ------------------------------------------------------------ Columnas

        /// <summary>
        /// Monta las cuatro columnas dentro del TableLayoutPanel. Se generan por
        /// código porque son la misma pieza repetida y porque el diseñador no
        /// puede cablear el arrastre entre ellas.
        /// </summary>
        private void ConstruirColumnas()
        {
            tlpColumnas.ColumnStyles.Clear();
            tlpColumnas.RowStyles.Clear();
            tlpColumnas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            for (var i = 0; i < EstadosColumna.Length; i++)
            {
                tlpColumnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / EstadosColumna.Length));

                var columna = new ColumnaKanban(EstadosColumna[i]);

                columna.Lista.DragEnter += (remitente, argumentos) => Columna_DragEnter(columna, argumentos);
                columna.Lista.DragLeave += (remitente, argumentos) => columna.Resaltar(false);
                columna.Lista.DragDrop += (remitente, argumentos) => Columna_DragDrop(columna, argumentos);
                columna.Lista.Resize += (remitente, argumentos) => columna.AjustarAnchoTarjetas();

                columna.Raiz.DragEnter += (remitente, argumentos) => Columna_DragEnter(columna, argumentos);
                columna.Raiz.DragLeave += (remitente, argumentos) => columna.Resaltar(false);
                columna.Raiz.DragDrop += (remitente, argumentos) => Columna_DragDrop(columna, argumentos);

                _columnas.Add(columna);
                tlpColumnas.Controls.Add(columna.Raiz, i, 0);
            }
        }

        // --------------------------------------------------------------- Carga

        private void CargarProyectos()
        {
            try
            {
                _cargando = true;

                // Un proyecto archivado (EsActivo = 0) sale de los tableros
                // activos y solo queda accesible desde el histórico.
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de usar el tablero.",
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

        /// <summary>
        /// Sprints del proyecto. Se posiciona en el activo, que es el que el
        /// equipo trabaja a diario.
        /// </summary>
        private void CargarSprints()
        {
            if (ProyectoSeleccionadoId() is not int proyectoId)
            {
                _cargando = true;
                cboSprint.DataSource = null;
                _cargando = false;

                CargarTablero();
                return;
            }

            try
            {
                var sprints = _sprintLN.ListarSprintsPorProyecto(proyectoId)
                    .Where(s => s.Estado != SprintEstadoConstantes.Planificado)
                    .ToList();

                _cargando = true;

                cboSprint.DisplayMember = nameof(Modelo.Sprint.NombreSprint);
                cboSprint.ValueMember = nameof(Modelo.Sprint.SprintId);
                cboSprint.DataSource = sprints;

                var activo = sprints.FirstOrDefault(s => s.Estado == SprintEstadoConstantes.Activo);
                cboSprint.SelectedItem = activo;

                if (cboSprint.SelectedItem is null && sprints.Count > 0)
                {
                    cboSprint.SelectedIndex = 0;
                }

                _cargando = false;

                CargarTablero();
            }
            catch (Exception ex)
            {
                _cargando = false;

                MessageBox.Show(
                    $"No se pudieron cargar los sprints: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cboSprint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            CargarTablero();
        }

        private void chkSoloMias_CheckedChanged(object sender, EventArgs e) => CargarTablero();

        private void btnActualizar_Click(object sender, EventArgs e) => CargarTablero();

        /// <summary>Reparte las historias del sprint entre las cuatro columnas.</summary>
        private void CargarTablero()
        {
            if (SprintSeleccionado() is not Modelo.Sprint sprint)
            {
                VaciarColumnas();
                lblResumen.Text = "Este proyecto todavía no tiene ningún sprint iniciado.";
                return;
            }

            try
            {
                var historias = _sprintLN.ListarHistoriasDelSprint(sprint.SprintId);

                if (chkSoloMias.Checked && UsuarioActualId() is int usuarioId)
                {
                    historias = historias.Where(h => h.UsuarioAsignadoId == usuarioId).ToList();
                }

                var puedeEditar = PermisoLN.TieneAcceso(Modulo.TableroKanban, NivelAcceso.Edicion) &&
                                  sprint.Estado == SprintEstadoConstantes.Activo;

                VaciarColumnas();

                foreach (var columna in _columnas)
                {
                    var delEstado = historias.Where(h => h.Estado == columna.Estado).ToList();

                    foreach (var historia in delEstado)
                    {
                        columna.Agregar(CrearTarjeta(historia, puedeEditar));
                    }

                    columna.ActualizarEncabezado(delEstado.Count, delEstado.Sum(h => h.StoryPoints));
                    columna.AjustarAnchoTarjetas();
                }

                ActualizarResumen(sprint, historias, puedeEditar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cargar el tablero: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private UcTarjetaKanban CrearTarjeta(UserStory historia, bool puedeEditar)
        {
            var tarjeta = new UcTarjetaKanban(historia)
            {
                PermiteArrastre = puedeEditar,
                Seleccionada = historia.UserStoryId == _historiaSeleccionadaId
            };

            tarjeta.TarjetaElegida += (remitente, argumentos) => SeleccionarTarjeta(tarjeta);
            tarjeta.TarjetaAbierta += (remitente, argumentos) => AbrirDesgloseTecnico(historia);

            return tarjeta;
        }

        private void VaciarColumnas()
        {
            foreach (var columna in _columnas)
            {
                columna.Vaciar();
                columna.ActualizarEncabezado(0, 0);
            }
        }

        private void ActualizarResumen(Modelo.Sprint sprint, List<UserStory> historias, bool puedeEditar)
        {
            var total = historias.Sum(h => h.StoryPoints);
            var completados = historias
                .Where(h => h.Estado == UserStoryEstadoConstantes.Done)
                .Sum(h => h.StoryPoints);

            var avance = total > 0 ? completados * 100 / total : 0;

            var pista = puedeEditar
                ? "Arrastra una tarjeta para cambiar su estado · doble clic para el desglose técnico."
                : sprint.Estado == SprintEstadoConstantes.Activo
                    ? "Solo lectura: tu rol no permite mover tarjetas."
                    : $"Solo lectura: el sprint está en estado '{sprint.Estado}'.";

            lblResumen.Text =
                $"{sprint.NombreSprint}  ·  {historias.Count} historias  ·  {total} SP  ·  " +
                $"Done {completados} SP ({avance}%)  —  {pista}";
        }

        private void SeleccionarTarjeta(UcTarjetaKanban elegida)
        {
            _historiaSeleccionadaId = elegida.Historia?.UserStoryId;

            foreach (var columna in _columnas)
            {
                columna.MarcarSeleccion(_historiaSeleccionadaId);
            }
        }

        // ----------------------------------------------------- Arrastrar y soltar

        private static void Columna_DragEnter(ColumnaKanban columna, DragEventArgs e)
        {
            var tarjeta = TarjetaArrastrada(e);

            var admitida = tarjeta?.Historia is not null &&
                           tarjeta.PermiteArrastre &&
                           tarjeta.Historia.Estado != columna.Estado;

            e.Effect = admitida ? DragDropEffects.Move : DragDropEffects.None;
            columna.Resaltar(admitida);
        }

        private void Columna_DragDrop(ColumnaKanban columna, DragEventArgs e)
        {
            columna.Resaltar(false);

            if (TarjetaArrastrada(e)?.Historia is UserStory historia)
            {
                MoverHistoria(historia, columna.Estado);
            }
        }

        private static UcTarjetaKanban? TarjetaArrastrada(DragEventArgs e) =>
            e.Data?.GetData(typeof(UcTarjetaKanban)) as UcTarjetaKanban;

        /// <summary>
        /// Aplica el cambio de estado en la capa de lógica, que valida la
        /// transición del flujo Kanban y las reglas de paso a 'Done'.
        /// </summary>
        private void MoverHistoria(UserStory historia, string nuevoEstado)
        {
            if (historia.Estado == nuevoEstado)
            {
                return;
            }

            if (!PermisoLN.TieneAcceso(Modulo.TableroKanban, NivelAcceso.Edicion))
            {
                MessageBox.Show(
                    "Tu rol solo tiene acceso de lectura sobre el tablero.",
                    "Acceso denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (UsuarioActualId() is not int usuarioId)
            {
                MessageBox.Show(
                    "No hay una sesión activa: vuelve a iniciar sesión para mover tarjetas.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _tareaLN.CambiarEstadoKanbanUserStory(historia.UserStoryId, nuevoEstado, usuarioId);
                _historiaSeleccionadaId = historia.UserStoryId;
            }
            catch (InvalidOperationException ex)
            {
                // Transición inválida o Definition of Done incumplida: el
                // mensaje de la capa Lógica ya explica el motivo al usuario.
                MessageBox.Show(ex.Message, "Movimiento no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo mover la historia: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            CargarTablero();
        }

        /// <summary>
        /// Abre el desglose técnico de la historia como diálogo, tal y como
        /// indica la guía del shell para las pantallas realmente modales.
        /// </summary>
        private void AbrirDesgloseTecnico(UserStory historia)
        {
            if (!PermisoLN.PuedeVer(Modulo.TareaEdicion))
            {
                MessageBox.Show(
                    "Tu rol no tiene acceso al desglose técnico de las historias.",
                    "Acceso denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (var dialogo = new TareaEdicion(historia))
            {
                dialogo.ShowDialog(this);
            }

            // Las tareas técnicas condicionan el paso a 'Done', así que el
            // tablero se recarga al volver.
            CargarTablero();
        }

        // ------------------------------------------------------------- Consultas

        private int? ProyectoSeleccionadoId() =>
            cboProyecto.SelectedItem is Proyecto proyecto ? proyecto.ProyectoId : null;

        private Modelo.Sprint? SprintSeleccionado() => cboSprint.SelectedItem as Modelo.Sprint;

        private static int? UsuarioActualId() => SesionContextoLN.UsuarioActual?.UsuarioId;

        /// <summary>
        /// Una columna del tablero: tarjeta contenedora, encabezado con el
        /// recuento y la lista desplazable de tarjetas.
        /// </summary>
        private sealed class ColumnaKanban
        {
            private readonly Label _encabezado;

            public ColumnaKanban(string estado)
            {
                Estado = estado;

                Raiz = new PanelTarjeta
                {
                    Dock = DockStyle.Fill,
                    Radio = 12,
                    ColorRelleno = Tema.Blanco,
                    ColorBorde = Tema.Borde,
                    ColorFondoDetras = Tema.Crema,
                    Margin = new Padding(6, 0, 6, 0),
                    Padding = new Padding(10, 8, 10, 10),
                    AllowDrop = true
                };

                _encabezado = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 34,
                    Font = Tema.Etiqueta,
                    ForeColor = Tema.TextoTenue,
                    BackColor = Tema.Blanco,
                    Padding = new Padding(6, 10, 0, 0),
                    Text = estado.ToUpperInvariant()
                };

                Lista = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoScroll = true,
                    BackColor = Tema.Blanco,
                    Padding = new Padding(4, 0, 4, 4),
                    AllowDrop = true
                };

                // El Fill primero y el Top despues: el orden de Controls.Add es
                // el que decide el reparto del Dock.
                Raiz.Controls.Add(Lista);
                Raiz.Controls.Add(_encabezado);
            }

            public string Estado { get; }

            public PanelTarjeta Raiz { get; }

            public FlowLayoutPanel Lista { get; }

            public void Agregar(UcTarjetaKanban tarjeta) => Lista.Controls.Add(tarjeta);

            public void Vaciar()
            {
                foreach (Control control in Lista.Controls)
                {
                    control.Dispose();
                }

                Lista.Controls.Clear();
            }

            public void ActualizarEncabezado(int historias, int storyPoints) =>
                _encabezado.Text = $"{Estado.ToUpperInvariant()}   ·   {historias}   ·   {storyPoints} SP";

            /// <summary>Borde teal mientras una tarjeta admitida sobrevuela la columna.</summary>
            public void Resaltar(bool activo)
            {
                Raiz.ColorBorde = activo ? Tema.Teal : Tema.Borde;
                Raiz.GrosorBorde = activo ? 2 : 1;
            }

            public void MarcarSeleccion(int? userStoryId)
            {
                foreach (var tarjeta in Lista.Controls.OfType<UcTarjetaKanban>())
                {
                    tarjeta.Seleccionada = tarjeta.Historia?.UserStoryId == userStoryId;
                }
            }

            /// <summary>
            /// Las tarjetas ocupan el ancho útil de la columna. Se recalcula al
            /// redimensionar porque la barra de desplazamiento aparece y
            /// desaparece según cuántas tarjetas haya.
            /// </summary>
            public void AjustarAnchoTarjetas()
            {
                var ancho = Lista.ClientSize.Width - Lista.Padding.Horizontal;

                if (ancho <= 0)
                {
                    return;
                }

                foreach (var tarjeta in Lista.Controls.OfType<UcTarjetaKanban>())
                {
                    tarjeta.Width = ancho;
                }
            }
        }
    }
}
