namespace Presentacion.Sprint
{
    partial class SprintPlanificacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlSelector = new Panel();
            cboProyecto = new ComboBox();
            lblProyecto = new Label();
            pnlEditor = new Panel();
            btnCancelar = new Button();
            btnIniciar = new Button();
            nudCapacidad = new NumericUpDown();
            lblCapacidad = new Label();
            btnLimpiar = new Button();
            btnAgregar = new Button();
            btnNuevo = new Button();
            dtpFin = new DateTimePicker();
            lblFin = new Label();
            dtpInicio = new DateTimePicker();
            lblInicio = new Label();
            txtObjetivo = new TextBox();
            lblObjetivo = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            pnlBacklog = new Panel();
            tblBacklog = new TableLayoutPanel();
            lblDisponibles = new Label();
            dgvDisponibles = new DataGridView();
            pnlBotones = new Panel();
            btnAsignarTodos = new Button();
            btnQuitar = new Button();
            btnQuitarTodos = new Button();
            btnAsignar = new Button();
            lblSprintBacklog = new Label();
            dgvSprintBacklog = new DataGridView();
            splitVertical = new SplitContainer();
            dgvSprints = new DataGridView();
            lblResumen = new Label();
            pnlSelector.SuspendLayout();
            pnlEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCapacidad).BeginInit();
            pnlBacklog.SuspendLayout();
            tblBacklog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).BeginInit();
            pnlBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSprintBacklog).BeginInit();
            splitVertical.SuspendLayout();
            splitVertical.Panel1.SuspendLayout();
            splitVertical.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSprints).BeginInit();
            SuspendLayout();
            //
            // pnlSelector
            //
            pnlSelector.BackColor = Color.FromArgb(245, 241, 232);
            pnlSelector.Controls.Add(cboProyecto);
            pnlSelector.Controls.Add(lblProyecto);
            pnlSelector.Dock = DockStyle.Top;
            pnlSelector.Location = new Point(0, 0);
            pnlSelector.Name = "pnlSelector";
            pnlSelector.Size = new Size(900, 56);
            pnlSelector.TabIndex = 0;
            //
            // lblProyecto
            //
            lblProyecto.Font = new Font("Segoe UI Semibold", 9.5F);
            lblProyecto.ForeColor = Color.FromArgb(44, 51, 47);
            lblProyecto.Location = new Point(24, 18);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(80, 21);
            lblProyecto.TabIndex = 0;
            lblProyecto.Text = "Proyecto:";
            lblProyecto.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboProyecto
            //
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyecto.Font = new Font("Segoe UI", 10F);
            cboProyecto.FormattingEnabled = true;
            cboProyecto.Location = new Point(110, 14);
            cboProyecto.Name = "cboProyecto";
            cboProyecto.Size = new Size(320, 25);
            cboProyecto.TabIndex = 1;
            cboProyecto.SelectedIndexChanged += cboProyecto_SelectedIndexChanged;
            //
            // pnlEditor
            //
            pnlEditor.BackColor = Color.FromArgb(245, 241, 232);
            pnlEditor.Controls.Add(btnCancelar);
            pnlEditor.Controls.Add(btnIniciar);
            pnlEditor.Controls.Add(nudCapacidad);
            pnlEditor.Controls.Add(lblCapacidad);
            pnlEditor.Controls.Add(btnLimpiar);
            pnlEditor.Controls.Add(btnAgregar);
            pnlEditor.Controls.Add(btnNuevo);
            pnlEditor.Controls.Add(dtpFin);
            pnlEditor.Controls.Add(lblFin);
            pnlEditor.Controls.Add(dtpInicio);
            pnlEditor.Controls.Add(lblInicio);
            pnlEditor.Controls.Add(txtObjetivo);
            pnlEditor.Controls.Add(lblObjetivo);
            pnlEditor.Controls.Add(txtNombre);
            pnlEditor.Controls.Add(lblNombre);
            pnlEditor.Dock = DockStyle.Bottom;
            pnlEditor.Location = new Point(0, 440);
            pnlEditor.Name = "pnlEditor";
            pnlEditor.Size = new Size(900, 150);
            pnlEditor.TabIndex = 2;
            //
            // lblNombre
            //
            lblNombre.Font = new Font("Segoe UI Semibold", 9.5F);
            lblNombre.ForeColor = Color.FromArgb(44, 51, 47);
            lblNombre.Location = new Point(24, 16);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(90, 21);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtNombre
            //
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.Location = new Point(120, 12);
            txtNombre.MaxLength = 100;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(240, 25);
            txtNombre.TabIndex = 1;
            //
            // lblInicio
            //
            lblInicio.Font = new Font("Segoe UI Semibold", 9.5F);
            lblInicio.ForeColor = Color.FromArgb(44, 51, 47);
            lblInicio.Location = new Point(390, 16);
            lblInicio.Name = "lblInicio";
            lblInicio.Size = new Size(80, 21);
            lblInicio.TabIndex = 2;
            lblInicio.Text = "Inicio:";
            lblInicio.TextAlign = ContentAlignment.MiddleLeft;
            //
            // dtpInicio
            //
            dtpInicio.CalendarMonthBackground = Color.White;
            dtpInicio.CustomFormat = "dd/MM/yyyy";
            dtpInicio.Format = DateTimePickerFormat.Custom;
            dtpInicio.Location = new Point(470, 12);
            dtpInicio.Name = "dtpInicio";
            dtpInicio.Size = new Size(110, 23);
            dtpInicio.TabIndex = 3;
            //
            // lblFin
            //
            lblFin.Font = new Font("Segoe UI Semibold", 9.5F);
            lblFin.ForeColor = Color.FromArgb(44, 51, 47);
            lblFin.Location = new Point(610, 16);
            lblFin.Name = "lblFin";
            lblFin.Size = new Size(80, 21);
            lblFin.TabIndex = 4;
            lblFin.Text = "Fin:";
            lblFin.TextAlign = ContentAlignment.MiddleLeft;
            //
            // dtpFin
            //
            dtpFin.CalendarMonthBackground = Color.White;
            dtpFin.CustomFormat = "dd/MM/yyyy";
            dtpFin.Format = DateTimePickerFormat.Custom;
            dtpFin.Location = new Point(690, 12);
            dtpFin.Name = "dtpFin";
            dtpFin.Size = new Size(110, 23);
            dtpFin.TabIndex = 5;
            //
            // lblObjetivo
            //
            lblObjetivo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblObjetivo.ForeColor = Color.FromArgb(44, 51, 47);
            lblObjetivo.Location = new Point(24, 54);
            lblObjetivo.Name = "lblObjetivo";
            lblObjetivo.Size = new Size(90, 21);
            lblObjetivo.TabIndex = 6;
            lblObjetivo.Text = "Sprint Goal:";
            lblObjetivo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtObjetivo
            //
            txtObjetivo.BorderStyle = BorderStyle.FixedSingle;
            txtObjetivo.Font = new Font("Segoe UI", 10F);
            txtObjetivo.Location = new Point(120, 50);
            txtObjetivo.MaxLength = 4000;
            txtObjetivo.Name = "txtObjetivo";
            txtObjetivo.Size = new Size(680, 25);
            txtObjetivo.TabIndex = 7;
            //
            // btnNuevo
            //
            btnNuevo.BackColor = Color.FromArgb(12, 110, 99);
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(24, 92);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(92, 40);
            btnNuevo.TabIndex = 8;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnAgregar
            //
            btnAgregar.BackColor = Color.FromArgb(12, 110, 99);
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(122, 92);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(92, 40);
            btnAgregar.TabIndex = 9;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAgregar_Click;
            //
            // btnLimpiar
            //
            btnLimpiar.BackColor = Color.FromArgb(200, 200, 200);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLimpiar.ForeColor = Color.FromArgb(44, 51, 47);
            btnLimpiar.Location = new Point(220, 92);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(92, 40);
            btnLimpiar.TabIndex = 10;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            //
            // lblCapacidad
            //
            lblCapacidad.Font = new Font("Segoe UI Semibold", 9.5F);
            lblCapacidad.ForeColor = Color.FromArgb(44, 51, 47);
            lblCapacidad.Location = new Point(390, 96);
            lblCapacidad.Name = "lblCapacidad";
            lblCapacidad.Size = new Size(90, 21);
            lblCapacidad.TabIndex = 11;
            lblCapacidad.Text = "Capacidad SP:";
            lblCapacidad.TextAlign = ContentAlignment.MiddleRight;
            //
            // nudCapacidad
            //
            nudCapacidad.Font = new Font("Segoe UI", 10F);
            nudCapacidad.Location = new Point(486, 92);
            nudCapacidad.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudCapacidad.Name = "nudCapacidad";
            nudCapacidad.Size = new Size(70, 25);
            nudCapacidad.TabIndex = 12;
            nudCapacidad.TextAlign = HorizontalAlignment.Center;
            //
            // btnIniciar
            //
            btnIniciar.BackColor = Color.FromArgb(12, 110, 99);
            btnIniciar.FlatAppearance.BorderSize = 0;
            btnIniciar.FlatStyle = FlatStyle.Flat;
            btnIniciar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnIniciar.ForeColor = Color.White;
            btnIniciar.Location = new Point(566, 88);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(116, 40);
            btnIniciar.TabIndex = 13;
            btnIniciar.Text = "Iniciar sprint";
            btnIniciar.UseVisualStyleBackColor = false;
            btnIniciar.Click += btnIniciar_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(200, 200, 200);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.FromArgb(44, 51, 47);
            btnCancelar.Location = new Point(688, 88);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(92, 40);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // pnlBacklog
            //
            pnlBacklog.BackColor = Color.FromArgb(245, 241, 232);
            pnlBacklog.Controls.Add(tblBacklog);
            pnlBacklog.Dock = DockStyle.Fill;
            pnlBacklog.Location = new Point(0, 56);
            pnlBacklog.Name = "pnlBacklog";
            pnlBacklog.Size = new Size(900, 384);
            pnlBacklog.TabIndex = 3;
            //
            // tblBacklog
            //
            tblBacklog.BackColor = Color.FromArgb(245, 241, 232);
            tblBacklog.ColumnCount = 3;
            tblBacklog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tblBacklog.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 116F));
            tblBacklog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tblBacklog.Controls.Add(dgvDisponibles, 0, 0);
            tblBacklog.Controls.Add(lblDisponibles, 0, 0);
            tblBacklog.Controls.Add(pnlBotones, 1, 0);
            tblBacklog.Controls.Add(dgvSprintBacklog, 2, 0);
            tblBacklog.Controls.Add(lblSprintBacklog, 2, 0);
            tblBacklog.Dock = DockStyle.Fill;
            tblBacklog.Location = new Point(0, 0);
            tblBacklog.Name = "tblBacklog";
            tblBacklog.RowCount = 1;
            tblBacklog.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblBacklog.Size = new Size(900, 384);
            tblBacklog.TabIndex = 0;
            //
            // lblDisponibles
            //
            lblDisponibles.BackColor = Color.FromArgb(245, 241, 232);
            lblDisponibles.Dock = DockStyle.Top;
            lblDisponibles.Font = new Font("Consolas", 8.5F);
            lblDisponibles.ForeColor = Color.FromArgb(138, 133, 122);
            lblDisponibles.Location = new Point(0, 0);
            lblDisponibles.Name = "lblDisponibles";
            lblDisponibles.Padding = new Padding(12, 8, 0, 0);
            lblDisponibles.Size = new Size(329, 30);
            lblDisponibles.TabIndex = 0;
            lblDisponibles.Text = "BACKLOG DISPONIBLE";
            //
            // dgvDisponibles
            //
            dgvDisponibles.AutoGenerateColumns = false;
            dgvDisponibles.BackgroundColor = Color.White;
            dgvDisponibles.BorderStyle = BorderStyle.None;
            dgvDisponibles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDisponibles.Dock = DockStyle.Fill;
            dgvDisponibles.Location = new Point(0, 30);
            dgvDisponibles.MultiSelect = false;
            dgvDisponibles.Name = "dgvDisponibles";
            dgvDisponibles.ReadOnly = true;
            dgvDisponibles.RowHeadersVisible = false;
            dgvDisponibles.Size = new Size(329, 354);
            dgvDisponibles.TabIndex = 1;
            //
            // pnlBotones
            //
            pnlBotones.BackColor = Color.FromArgb(245, 241, 232);
            pnlBotones.Controls.Add(btnAsignarTodos);
            pnlBotones.Controls.Add(btnQuitar);
            pnlBotones.Controls.Add(btnQuitarTodos);
            pnlBotones.Controls.Add(btnAsignar);
            pnlBotones.Dock = DockStyle.Fill;
            pnlBotones.Location = new Point(329, 0);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Size = new Size(116, 384);
            pnlBotones.TabIndex = 2;
            //
            // btnAsignar
            //
            btnAsignar.BackColor = Color.FromArgb(12, 110, 99);
            btnAsignar.FlatAppearance.BorderSize = 0;
            btnAsignar.FlatStyle = FlatStyle.Flat;
            btnAsignar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAsignar.ForeColor = Color.White;
            btnAsignar.Location = new Point(14, 90);
            btnAsignar.Name = "btnAsignar";
            btnAsignar.Size = new Size(88, 40);
            btnAsignar.TabIndex = 0;
            btnAsignar.Text = "Asignar →";
            btnAsignar.UseVisualStyleBackColor = false;
            btnAsignar.Click += btnAsignar_Click;
            //
            // btnAsignarTodos
            //
            btnAsignarTodos.BackColor = Color.FromArgb(12, 110, 99);
            btnAsignarTodos.FlatAppearance.BorderSize = 0;
            btnAsignarTodos.FlatStyle = FlatStyle.Flat;
            btnAsignarTodos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAsignarTodos.ForeColor = Color.White;
            btnAsignarTodos.Location = new Point(14, 136);
            btnAsignarTodos.Name = "btnAsignarTodos";
            btnAsignarTodos.Size = new Size(88, 40);
            btnAsignarTodos.TabIndex = 1;
            btnAsignarTodos.Text = "Asignar todos";
            btnAsignarTodos.UseVisualStyleBackColor = false;
            btnAsignarTodos.Click += btnAsignarTodos_Click;
            //
            // btnQuitar
            //
            btnQuitar.BackColor = Color.FromArgb(200, 200, 200);
            btnQuitar.FlatAppearance.BorderSize = 0;
            btnQuitar.FlatStyle = FlatStyle.Flat;
            btnQuitar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnQuitar.ForeColor = Color.FromArgb(44, 51, 47);
            btnQuitar.Location = new Point(14, 208);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(88, 40);
            btnQuitar.TabIndex = 2;
            btnQuitar.Text = "← Quitar";
            btnQuitar.UseVisualStyleBackColor = false;
            btnQuitar.Click += btnQuitar_Click;
            //
            // btnQuitarTodos
            //
            btnQuitarTodos.BackColor = Color.FromArgb(200, 200, 200);
            btnQuitarTodos.FlatAppearance.BorderSize = 0;
            btnQuitarTodos.FlatStyle = FlatStyle.Flat;
            btnQuitarTodos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnQuitarTodos.ForeColor = Color.FromArgb(44, 51, 47);
            btnQuitarTodos.Location = new Point(14, 254);
            btnQuitarTodos.Name = "btnQuitarTodos";
            btnQuitarTodos.Size = new Size(88, 40);
            btnQuitarTodos.TabIndex = 3;
            btnQuitarTodos.Text = "Quitar todos";
            btnQuitarTodos.UseVisualStyleBackColor = false;
            btnQuitarTodos.Click += btnQuitarTodos_Click;
            //
            // lblSprintBacklog
            //
            lblSprintBacklog.BackColor = Color.FromArgb(245, 241, 232);
            lblSprintBacklog.Dock = DockStyle.Top;
            lblSprintBacklog.Font = new Font("Consolas", 8.5F);
            lblSprintBacklog.ForeColor = Color.FromArgb(138, 133, 122);
            lblSprintBacklog.Location = new Point(445, 0);
            lblSprintBacklog.Name = "lblSprintBacklog";
            lblSprintBacklog.Padding = new Padding(12, 8, 0, 0);
            lblSprintBacklog.Size = new Size(455, 30);
            lblSprintBacklog.TabIndex = 3;
            lblSprintBacklog.Text = "SPRINT BACKLOG";
            //
            // dgvSprintBacklog
            //
            dgvSprintBacklog.AutoGenerateColumns = false;
            dgvSprintBacklog.BackgroundColor = Color.White;
            dgvSprintBacklog.BorderStyle = BorderStyle.None;
            dgvSprintBacklog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSprintBacklog.Dock = DockStyle.Fill;
            dgvSprintBacklog.Location = new Point(445, 30);
            dgvSprintBacklog.MultiSelect = false;
            dgvSprintBacklog.Name = "dgvSprintBacklog";
            dgvSprintBacklog.ReadOnly = true;
            dgvSprintBacklog.RowHeadersVisible = false;
            dgvSprintBacklog.Size = new Size(455, 354);
            dgvSprintBacklog.TabIndex = 4;
            //
            // splitVertical
            //
            splitVertical.BackColor = Color.FromArgb(218, 212, 196);
            splitVertical.Dock = DockStyle.Fill;
            splitVertical.FixedPanel = FixedPanel.Panel1;
            splitVertical.Location = new Point(0, 56);
            splitVertical.Name = "splitVertical";
            splitVertical.Orientation = Orientation.Vertical;
            splitVertical.Panel1.Controls.Add(dgvSprints);
            splitVertical.Panel1.Controls.Add(lblResumen);
            splitVertical.Panel2.Controls.Add(pnlBacklog);
            splitVertical.Size = new Size(900, 384);
            splitVertical.Panel1MinSize = 220;
            splitVertical.Panel2MinSize = 340;
            splitVertical.SplitterWidth = 6;
            splitVertical.TabIndex = 1;
            //
            // lblResumen
            //
            lblResumen.BackColor = Color.FromArgb(245, 241, 232);
            lblResumen.Dock = DockStyle.Bottom;
            lblResumen.Font = new Font("Segoe UI", 9F);
            lblResumen.ForeColor = Color.FromArgb(44, 51, 47);
            lblResumen.Location = new Point(0, 334);
            lblResumen.Name = "lblResumen";
            lblResumen.Padding = new Padding(12, 10, 12, 0);
            lblResumen.Size = new Size(260, 50);
            lblResumen.TabIndex = 0;
            lblResumen.Text = "Selecciona un sprint para ver su backlog.";
            //
            // dgvSprints
            //
            dgvSprints.AutoGenerateColumns = false;
            dgvSprints.BackgroundColor = Color.White;
            dgvSprints.BorderStyle = BorderStyle.None;
            dgvSprints.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSprints.Dock = DockStyle.Fill;
            dgvSprints.Location = new Point(0, 0);
            dgvSprints.MultiSelect = false;
            dgvSprints.Name = "dgvSprints";
            dgvSprints.ReadOnly = true;
            dgvSprints.RowHeadersVisible = false;
            dgvSprints.Size = new Size(260, 334);
            dgvSprints.TabIndex = 1;
            dgvSprints.SelectionChanged += dgvSprints_SelectionChanged;
            //
            // SprintPlanificacion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(900, 590);
            Controls.Add(splitVertical);
            Controls.Add(pnlEditor);
            Controls.Add(pnlSelector);
            Name = "SprintPlanificacion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Planificación de sprints";
            pnlSelector.ResumeLayout(false);
            pnlEditor.ResumeLayout(false);
            pnlEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCapacidad).EndInit();
            pnlBacklog.ResumeLayout(false);
            tblBacklog.ResumeLayout(false);
            tblBacklog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).EndInit();
            pnlBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSprintBacklog).EndInit();
            splitVertical.Panel1.ResumeLayout(false);
            splitVertical.Panel2.ResumeLayout(false);
            splitVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSprints).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private ComboBox cboProyecto;
        private Label lblProyecto;
        private Panel pnlEditor;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblInicio;
        private DateTimePicker dtpInicio;
        private Label lblFin;
        private DateTimePicker dtpFin;
        private Label lblObjetivo;
        private TextBox txtObjetivo;
        private Label lblCapacidad;
        private NumericUpDown nudCapacidad;
        private Button btnNuevo;
        private Button btnAgregar;
        private Button btnLimpiar;
        private Button btnIniciar;
        private Button btnCancelar;
        private SplitContainer splitVertical;
        private DataGridView dgvSprints;
        private Label lblResumen;
        private Panel pnlBacklog;
        private TableLayoutPanel tblBacklog;
        private Label lblDisponibles;
        private DataGridView dgvDisponibles;
        private Panel pnlBotones;
        private Button btnAsignar;
        private Button btnAsignarTodos;
        private Button btnQuitar;
        private Button btnQuitarTodos;
        private Label lblSprintBacklog;
        private DataGridView dgvSprintBacklog;
    }
}