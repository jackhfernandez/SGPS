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
    public partial class BugGestion : Form
    {
        private readonly BugLN _bugLN = new();
        private readonly ProyectoLN _proyectoLN = new();
        private readonly UserStoryLN _userStoryLN = new();
        private bool _cargando;

        public BugGestion()
        {
            InitializeComponent();

            Tema.AplicarEstiloGrid(dgvBugs);
            Tema.EstiloBotonPrimario(btnAvanzar);
            Tema.EstiloBotonSecundario(btnRetroceder);
            Tema.EstiloBotonSecundario(btnCancelar);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PermisoLN.ValidarLectura(Modulo.BugGestion);
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
                        (int)(splitVertical.Width * 0.38),
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
                        "No hay proyectos activos registrados. Crea un proyecto antes de gestionar bugs.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                CargarBugs();
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

            CargarBugs();
        }

        private void CargarBugs()
        {
            if (ProyectoSeleccionadoId() is not int proyectoId)
            {
                dgvBugs.Rows.Clear();
                LimpiarDetalle();
                ActualizarEstadoBotones();
                return;
            }

            try
            {
                _cargando = true;

                var bugs = _bugLN.ListarBugsPorProyecto(proyectoId);
                var idSeleccionado = (dgvBugs.CurrentRow?.Tag as Bug)?.BugId;

                dgvBugs.DataSource = null;
                dgvBugs.Columns.Clear();

                dgvBugs.Columns.Add("CodigoBug", "Código");
                dgvBugs.Columns.Add("Titulo", "Título");
                dgvBugs.Columns.Add("Severidad", "Severidad");
                dgvBugs.Columns.Add("Estado", "Estado");
                dgvBugs.Columns.Add("Fecha", "Fecha");

                dgvBugs.Columns["CodigoBug"]!.Width = 95;
                dgvBugs.Columns["Titulo"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvBugs.Columns["Severidad"]!.Width = 90;
                dgvBugs.Columns["Estado"]!.Width = 85;
                dgvBugs.Columns["Fecha"]!.Width = 80;

                foreach (DataGridViewColumn columna in dgvBugs.Columns)
                {
                    columna.SortMode = DataGridViewColumnSortMode.NotSortable;
                    columna.Resizable = DataGridViewTriState.False;
                    columna.ReadOnly = columna.Name != "Severidad";
                }

                ConfigurarColumnaSeveridad();

                foreach (var bug in bugs)
                {
                    var indice = dgvBugs.Rows.Add(
                        bug.CodigoBug,
                        bug.Titulo,
                        bug.Severidad,
                        bug.Estado,
                        bug.FechaReporte.ToShortDateString());

                    dgvBugs.Rows[indice].Tag = bug;
                }

                lblResumen.Text = $"{bugs.Count} bug(s) en el proyecto.";

                if (idSeleccionado.HasValue)
                {
                    SeleccionarBug(idSeleccionado.Value);
                }
                else if (bugs.Count > 0)
                {
                    SeleccionarBug(bugs[0].BugId);
                }

                MostrarDetalle();
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los bugs: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _cargando = false;
            }
        }

        private void dgvBugs_SelectionChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            MostrarDetalle();
            ActualizarEstadoBotones();
        }

        /// <summary>
        /// Convierte la columna de severidad en un combobox con las opciones
        /// predefinidas, respetando el permiso de edición del usuario.
        /// </summary>
        private void ConfigurarColumnaSeveridad()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.BugGestion, NivelAcceso.Edicion);
            var columnaSeveridad = dgvBugs.Columns["Severidad"];

            columnaSeveridad!.ReadOnly = !puedeEditar;

            if (columnaSeveridad is DataGridViewComboBoxColumn combo)
            {
                return;
            }

            var indice = dgvBugs.Columns.IndexOf(columnaSeveridad);
            var comboColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Severidad",
                Name = "Severidad",
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                ReadOnly = !puedeEditar,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Resizable = DataGridViewTriState.False
            };

            comboColumn.Items.AddRange("Bloqueante", "Alta", "Media", "Baja");

            var originales = new List<object>();
            foreach (DataGridViewRow fila in dgvBugs.Rows)
            {
                originales.Add(fila.Cells[indice].Value);
            }

            dgvBugs.Columns.RemoveAt(indice);
            dgvBugs.Columns.Insert(indice, comboColumn);

            for (int i = 0; i < dgvBugs.Rows.Count; i++)
            {
                dgvBugs.Rows[i].Cells[indice].Value = originales[i];
            }
        }

        /// <summary>
        /// Persiste el cambio de severidad cuando el usuario elige una opción
        /// en el combobox de la rejilla.
        /// </summary>
        private void dgvBugs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_cargando || e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvBugs.Columns[e.ColumnIndex].Name != "Severidad")
            {
                return;
            }

            if (dgvBugs.Rows[e.RowIndex].Tag is not Bug bug)
            {
                return;
            }

            var nuevoValor = dgvBugs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            if (string.IsNullOrWhiteSpace(nuevoValor) || string.Equals(nuevoValor, bug.Severidad, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (!PermisoLN.TieneAcceso(Modulo.BugGestion, NivelAcceso.Edicion))
            {
                CargarBugs();
                return;
            }

            try
            {
                if (_bugLN.ActualizarSeveridadBug(bug.BugId, nuevoValor, SesionContextoLN.UsuarioActual.UsuarioId, out var mensajeError))
                {
                    bug.Severidad = nuevoValor;
                    lblResumenDetalle.Text =
                        $"Reportado: {bug.FechaReporte.ToShortDateString()}  ·  {bug.Severidad}  ·  {bug.Estado}";
                }
                else
                {
                    MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CargarBugs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo actualizar la severidad del bug: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                CargarBugs();
            }
        }

        private void MostrarDetalle()
        {
            if (BugSeleccionado() is not Bug bug)
            {
                LimpiarDetalle();
                ActualizarEstadoBotones();
                return;
            }

            txtCodigo.Text = bug.CodigoBug;
            txtTitulo.Text = bug.Titulo;
            txtSeveridad.Text = bug.Severidad;
            txtEstado.Text = bug.Estado;
            txtHistoria.Text = ObtenerHistoriaLabel(bug.UserStoryId);
            txtPasos.Text = bug.PasosReproducir;

            lblTituloDetalle.Text = $"DETALLE DEL BUG — {bug.CodigoBug}";
            lblResumenDetalle.Text =
                $"Reportado: {bug.FechaReporte.ToShortDateString()}  ·  {bug.Severidad}  ·  {bug.Estado}";
        }

        private void LimpiarDetalle()
        {
            txtCodigo.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            txtSeveridad.Text = string.Empty;
            txtEstado.Text = string.Empty;
            txtHistoria.Text = string.Empty;
            txtPasos.Text = string.Empty;
            lblTituloDetalle.Text = "DETALLE DEL BUG —";
            lblResumenDetalle.Text = "Sin bug seleccionado.";
        }

        private string ObtenerHistoriaLabel(int? userStoryId)
        {
            if (!userStoryId.HasValue)
            {
                return "(sin historia)";
            }

            try
            {
                var historia = _userStoryLN.ObtenerPorId(userStoryId.Value);
                return historia != null ? $"{historia.CodigoTicket} · {historia.Titulo}" : "(sin historia)";
            }
            catch (Exception)
            {
                return "(sin historia)";
            }
        }

        private void btnAvanzar_Click(object sender, EventArgs e)
        {
            if (BugSeleccionado() is not Bug bug)
            {
                MessageBox.Show("Selecciona un bug para avanzar su estado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var siguiente = BugLN.SiguienteEstadoBug(bug.Estado);
            if (siguiente is null)
            {
                MessageBox.Show($"El bug ya está en '{bug.Estado}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CambiarEstado(bug.BugId, siguiente);
        }

        private void btnRetroceder_Click(object sender, EventArgs e)
        {
            if (BugSeleccionado() is not Bug bug)
            {
                MessageBox.Show("Selecciona un bug para retroceder su estado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var anterior = BugLN.AnteriorEstadoBug(bug.Estado);
            if (anterior is null)
            {
                MessageBox.Show($"El bug ya está en '{bug.Estado}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CambiarEstado(bug.BugId, anterior);
        }

        private void CambiarEstado(int bugId, string nuevoEstado)
        {
            try
            {
                if (!_bugLN.ActualizarEstadoBug(bugId, nuevoEstado, SesionContextoLN.UsuarioActual.UsuarioId, out var mensajeError))
                {
                    MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CargarBugs();
                SeleccionarBug(bugId);
                MostrarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cambiar el estado del bug: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        private void ActualizarEstadoBotones()
        {
            var puedeEditar = PermisoLN.TieneAcceso(Modulo.BugGestion, NivelAcceso.Edicion);
            var bug = BugSeleccionado();

            btnAvanzar.Enabled = puedeEditar && bug != null && BugLN.SiguienteEstadoBug(bug.Estado) != null;
            btnRetroceder.Enabled = puedeEditar && bug != null && BugLN.AnteriorEstadoBug(bug.Estado) != null;
        }

        private void SeleccionarBug(int bugId)
        {
            foreach (DataGridViewRow fila in dgvBugs.Rows)
            {
                if (fila.Tag is Bug bug && bug.BugId == bugId)
                {
                    dgvBugs.CurrentCell = fila.Cells[0];
                    return;
                }
            }
        }

        private int? ProyectoSeleccionadoId() =>
            cboProyecto.SelectedItem is Proyecto proyecto ? proyecto.ProyectoId : null;

        private Bug? BugSeleccionado() => dgvBugs.CurrentRow?.Tag as Bug;
    }
}