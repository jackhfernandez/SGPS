using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Modelo;

namespace Presentacion.Backlog
{
    public partial class ProductBacklogGestion : Form
    {
        private readonly UserStoryLN _userStoryLN = new();
        private readonly EpicLN _epicLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private Dictionary<int, string> _epicsPorId = new();
        private bool _cargando;
        private bool _ordenSinGuardar;

        public ProductBacklogGestion()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.ProductBacklogGestion);
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de priorizar el backlog.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                CargarBacklog();
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

        private void CargarBacklog()
        {
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                dgvBacklog.Rows.Clear();
                _ordenSinGuardar = false;
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                _epicsPorId = _epicLN.ListarPorProyecto(proyecto.ProyectoId)
                    .ToDictionary(epic => epic.EpicId, epic => epic.Titulo);

                var historias = _userStoryLN.ObtenerProductBacklogPriorizado(proyecto.ProyectoId);
                var idSeleccionado = (dgvBacklog.CurrentRow?.Tag as UserStory)?.UserStoryId;

                dgvBacklog.DataSource = null;
                dgvBacklog.Columns.Clear();

                dgvBacklog.EnableHeadersVisualStyles = false;
                dgvBacklog.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 110, 99);
                dgvBacklog.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvBacklog.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dgvBacklog.ColumnHeadersHeight = 30;

                dgvBacklog.Columns.Add("Orden", "#");
                dgvBacklog.Columns.Add("CodigoTicket", "Código");
                dgvBacklog.Columns.Add("Titulo", "Título");
                dgvBacklog.Columns.Add("Epic", "Epic");
                dgvBacklog.Columns.Add("ValorNegocio", "Valor");
                dgvBacklog.Columns.Add("StoryPoints", "Puntos");
                dgvBacklog.Columns.Add("Estado", "Estado");

                dgvBacklog.Columns["Orden"]!.Width = 45;
                dgvBacklog.Columns["CodigoTicket"]!.Width = 100;
                dgvBacklog.Columns["Titulo"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvBacklog.Columns["Epic"]!.Width = 160;
                dgvBacklog.Columns["ValorNegocio"]!.Width = 70;
                dgvBacklog.Columns["StoryPoints"]!.Width = 60;
                dgvBacklog.Columns["Estado"]!.Width = 100;

                foreach (var historia in historias)
                {
                    AgregarFila(historia);
                }

                Renumerar();
                _ordenSinGuardar = false;

                if (idSeleccionado.HasValue)
                {
                    SeleccionarHistoria(idSeleccionado.Value);
                }

                // SelectionChanged se dispara al agregar la primera fila, antes
                // de que esa fila tenga su Tag, asi que el editor se sincroniza
                // aqui explicitamente.
                SincronizarEditor();
                ActualizarEstadoBotones();
                ActualizarResumen(historias);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cargar el backlog: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AgregarFila(UserStory historia)
        {
            var nombreEpic = historia.EpicId.HasValue && _epicsPorId.TryGetValue(historia.EpicId.Value, out var titulo)
                ? titulo
                : string.Empty;

            var indice = dgvBacklog.Rows.Add(
                0,
                historia.CodigoTicket,
                historia.Titulo,
                nombreEpic,
                historia.ValorNegocio,
                historia.StoryPoints,
                historia.Estado);

            dgvBacklog.Rows[indice].Tag = historia;
        }

        /// <summary>Recalcula la columna # segun la posicion visual de cada fila.</summary>
        private void Renumerar()
        {
            for (var indice = 0; indice < dgvBacklog.Rows.Count; indice++)
            {
                dgvBacklog.Rows[indice].Cells["Orden"].Value = indice + 1;
            }
        }

        private void ActualizarResumen(List<UserStory> historias)
        {
            var puntos = historias.Sum(h => h.StoryPoints);

            lblAyuda.Text =
                $"{historias.Count} historia(s) · {puntos} story points en total.  " +
                "Prioriza con Subir / Bajar y pulsa 'Guardar orden' para persistirlo. " +
                "'Estimar' aplica el valor de negocio y los puntos a la historia seleccionada.";
        }

        private void cboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            if (!ConfirmarDescartarOrden())
            {
                return;
            }

            CargarBacklog();
        }

        private void dgvBacklog_SelectionChanged(object sender, EventArgs e)
        {
            SincronizarEditor();
            ActualizarEstadoBotones();
        }

        /// <summary>Vuelca el valor de negocio y los puntos de la fila actual en los combos.</summary>
        private void SincronizarEditor()
        {
            if (dgvBacklog.CurrentRow?.Tag is UserStory historia)
            {
                cboValor.Text = historia.ValorNegocio;
                cboPuntos.SelectedItem = historia.StoryPoints;
            }
            else
            {
                cboValor.SelectedIndex = -1;
                cboPuntos.SelectedIndex = -1;
            }
        }

        private void btnSubir_Click(object sender, EventArgs e) => Mover(-1);

        private void btnBajar_Click(object sender, EventArgs e) => Mover(1);

        /// <summary>
        /// Mueve la fila seleccionada. Solo altera el orden visual: la
        /// persistencia ocurre en 'Guardar orden'.
        /// </summary>
        private void Mover(int desplazamiento)
        {
            if (dgvBacklog.CurrentRow is null)
            {
                return;
            }

            var origen = dgvBacklog.CurrentRow.Index;
            var destino = origen + desplazamiento;

            if (destino < 0 || destino >= dgvBacklog.Rows.Count)
            {
                return;
            }

            var fila = dgvBacklog.Rows[origen];
            dgvBacklog.Rows.RemoveAt(origen);
            dgvBacklog.Rows.Insert(destino, fila);

            Renumerar();

            dgvBacklog.CurrentCell = dgvBacklog.Rows[destino].Cells["CodigoTicket"];

            // RemoveAt dispara SelectionChanged sobre una fila intermedia, y si
            // tras Insert la fila actual ya es la de destino no vuelve a
            // dispararse: hay que resincronizar a mano.
            SincronizarEditor();
            _ordenSinGuardar = true;
            ActualizarEstadoBotones();
        }

        private void btnGuardarOrden_Click(object sender, EventArgs e)
        {
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                return;
            }

            var identificadores = new List<int>();

            foreach (DataGridViewRow fila in dgvBacklog.Rows)
            {
                if (fila.Tag is UserStory historia)
                {
                    identificadores.Add(historia.UserStoryId);
                }
            }

            if (identificadores.Count == 0)
            {
                MessageBox.Show(
                    "No hay historias que ordenar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                _userStoryLN.ActualizarOrdenWSJF(proyecto.ProyectoId, identificadores);

                _ordenSinGuardar = false;

                // Se recarga desde la BD: ActualizarOrdenWSJF no es
                // transaccional, asi que conviene ver el estado real.
                CargarBacklog();

                MessageBox.Show("Orden del backlog guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo guardar el orden: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                CargarBacklog();
            }
        }

        private void btnEstimar_Click(object sender, EventArgs e)
        {
            if (dgvBacklog.CurrentRow?.Tag is not UserStory historia)
            {
                MessageBox.Show(
                    "Selecciona una historia para estimar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(cboValor.Text) || cboPuntos.SelectedItem is not int puntos)
            {
                MessageBox.Show(
                    "Selecciona el valor de negocio y los story points.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Se parte de la historia completa: Modificar revalida todos
                // los campos obligatorios, no solo los dos que cambian aqui.
                historia.ValorNegocio = cboValor.Text;
                historia.StoryPoints = puntos;

                _userStoryLN.Modificar(historia);

                CargarBacklog();
                SeleccionarHistoria(historia.UserStoryId);

                MessageBox.Show("Estimación actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo actualizar la estimación: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                CargarBacklog();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (!ConfirmarDescartarOrden())
            {
                return;
            }

            Close();
        }

        /// <summary>Avisa si hay un reordenamiento pendiente de guardar.</summary>
        private bool ConfirmarDescartarOrden()
        {
            if (!_ordenSinGuardar)
            {
                return true;
            }

            var respuesta = MessageBox.Show(
                "Has reordenado el backlog y no lo has guardado. ¿Descartar los cambios?",
                "Orden sin guardar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                _ordenSinGuardar = false;
                return true;
            }

            return false;
        }

        private void SeleccionarHistoria(int userStoryId)
        {
            foreach (DataGridViewRow fila in dgvBacklog.Rows)
            {
                if (fila.Tag is UserStory historia && historia.UserStoryId == userStoryId)
                {
                    dgvBacklog.CurrentCell = fila.Cells["CodigoTicket"];
                    return;
                }
            }
        }

        private Proyecto? ProyectoSeleccionado() => cboProyecto.SelectedItem as Proyecto;

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.ProductBacklogGestion, NivelAcceso.Edicion);
            var haySeleccion = dgvBacklog.CurrentRow?.Tag is UserStory;
            var hayFilas = dgvBacklog.Rows.Count > 0;

            btnSubir.Enabled = puedeEditar && haySeleccion && dgvBacklog.CurrentRow!.Index > 0;
            btnBajar.Enabled = puedeEditar && haySeleccion && dgvBacklog.CurrentRow!.Index < dgvBacklog.Rows.Count - 1;
            btnGuardarOrden.Enabled = puedeEditar && hayFilas && _ordenSinGuardar;
            btnEstimar.Enabled = puedeEditar && haySeleccion;
        }
    }
}
