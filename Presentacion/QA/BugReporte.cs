using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Logica;
using Modelo;
using Presentacion.Ui;

namespace Presentacion.QA
{
    public partial class BugReporte : Form
    {
        private readonly BugLN _bugLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private readonly UserStoryLN _userStoryLN = new();
        private bool _cargando;

        public BugReporte()
        {
            InitializeComponent();

            Tema.AplicarEstiloGrid(dgvHistorias);
            Tema.EstiloBotonPrimario(btnReportar);
            Tema.EstiloBotonSecundario(btnLimpiar);
            Tema.EstiloBotonSecundario(btnCancelar);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.BugReporte);
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
                        (int)(splitVertical.Width * 0.34),
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de reportar bugs.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

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

            CargarHistorias();
        }

        private void CargarHistorias()
        {
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                dgvHistorias.Rows.Clear();
                lblResumen.Text = "Selecciona un proyecto para cargar sus historias.";
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                var historias = _userStoryLN.ObtenerProductBacklogPriorizado(proyecto.ProyectoId);
                var idSeleccionado = (dgvHistorias.CurrentRow?.Tag as UserStory)?.UserStoryId;

                dgvHistorias.DataSource = null;
                dgvHistorias.Columns.Clear();

                dgvHistorias.Columns.Add("CodigoTicket", "Código");
                dgvHistorias.Columns.Add("Titulo", "Título");
                dgvHistorias.Columns.Add("Estado", "Estado");

                dgvHistorias.Columns["CodigoTicket"]!.Width = 95;
                dgvHistorias.Columns["Titulo"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvHistorias.Columns["Estado"]!.Width = 90;

                foreach (var historia in historias)
                {
                    var indice = dgvHistorias.Rows.Add(
                        historia.CodigoTicket,
                        historia.Titulo,
                        historia.Estado);

                    dgvHistorias.Rows[indice].Tag = historia;
                }

                lblResumen.Text =
                    $"{historias.Count} historia(s) del proyecto. Selecciona una para vincularla al bug.";

                if (idSeleccionado.HasValue)
                {
                    SeleccionarHistoria(idSeleccionado.Value);
                }

                SincronizarHistoria();
                ActualizarEstadoBotones();
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

        private void dgvHistorias_SelectionChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            SincronizarHistoria();
            ActualizarEstadoBotones();
        }

        /// <summary>Muestra en el detalle la historia seleccionada como vínculo del bug.</summary>
        private void SincronizarHistoria()
        {
            if (dgvHistorias.CurrentRow?.Tag is UserStory historia)
            {
                lblHistoria.Text = $"{historia.CodigoTicket} · {historia.Titulo}";
            }
            else
            {
                lblHistoria.Text = "Sin historia vinculada";
            }
        }

        private void btnReportar_Click(object sender, EventArgs e)
        {
            if (ProyectoSeleccionado() is not Proyecto proyecto)
            {
                MessageBox.Show(
                    "Selecciona un proyecto antes de reportar un bug.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                var historia = dgvHistorias.CurrentRow?.Tag as UserStory;

                var bug = new Bug
                {
                    CodigoBug = _bugLN.GenerarCodigoBug(proyecto.ProyectoId, proyecto.ClaveProyecto),
                    ProyectoId = proyecto.ProyectoId,
                    UserStoryId = historia?.UserStoryId,
                    Titulo = txtTitulo.Text.Trim(),
                    PasosReproducir = txtPasos.Text.Trim(),
                    Severidad = cboSeveridad.SelectedItem?.ToString() ?? "Media"
                };

                if (!_bugLN.RegistrarBug(bug, SesionContextoLN.UsuarioActual.UsuarioId, out var mensajeValidacion))
                {
                    MessageBox.Show(mensajeValidacion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtCodigo.Text = bug.CodigoBug;

                MessageBox.Show(
                    $"Bug '{bug.CodigoBug}' reportado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo reportar el bug: {ex.Message}",
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

        private void LimpiarEditor()
        {
            txtTitulo.Text = string.Empty;
            txtPasos.Text = string.Empty;
            cboSeveridad.SelectedIndex = 2;
            txtCodigo.Text = string.Empty;
            dgvHistorias.ClearSelection();
            SincronizarHistoria();
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

        private Proyecto? ProyectoSeleccionado() => cboProyecto.SelectedItem as Proyecto;

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.BugReporte, NivelAcceso.Edicion);

            btnReportar.Enabled = puedeEditar && ProyectoSeleccionado() != null;
            btnLimpiar.Enabled = puedeEditar;
        }
    }
}
