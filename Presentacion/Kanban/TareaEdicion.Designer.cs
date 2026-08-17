namespace Presentacion.Kanban
{
    partial class TareaEdicion
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
            components = new System.ComponentModel.Container();
            pnlSelector = new Panel();
            lblProyecto = new Label();
            cboProyecto = new ComboBox();
            lblHistoria = new Label();
            cboHistoria = new ComboBox();
            lblCabecera = new Label();
            splitVertical = new SplitContainer();
            dgvTareas = new DataGridView();
            lblResumenHoras = new Label();
            pnlDetalle = new Panel();
            pnlCampos = new Panel();
            lblCampoTitulo = new Label();
            txtTitulo = new TextBox();
            lblCampoEstado = new Label();
            cboEstado = new ComboBox();
            lblCampoAsignado = new Label();
            cboAsignado = new ComboBox();
            lblCampoHorasEstimadas = new Label();
            numHorasEstimadas = new NumericUpDown();
            lblCampoHorasTrabajadas = new Label();
            numHorasTrabajadas = new NumericUpDown();
            lblAyuda = new Label();
            lblTituloDetalle = new Label();
            pnlAcciones = new Panel();
            btnNueva = new Button();
            btnGuardar = new Button();
            btnAvanzar = new Button();
            btnCerrar = new Button();
            pnlSelector.SuspendLayout();
            splitVertical.BeginInit();
            splitVertical.Panel1.SuspendLayout();
            splitVertical.Panel2.SuspendLayout();
            splitVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTareas).BeginInit();
            pnlDetalle.SuspendLayout();
            pnlCampos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numHorasEstimadas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHorasTrabajadas).BeginInit();
            pnlAcciones.SuspendLayout();
            SuspendLayout();
            //
            // pnlSelector
            //
            pnlSelector.BackColor = Color.FromArgb(245, 241, 232);
            pnlSelector.Controls.Add(cboProyecto);
            pnlSelector.Controls.Add(lblProyecto);
            pnlSelector.Controls.Add(cboHistoria);
            pnlSelector.Controls.Add(lblHistoria);
            pnlSelector.Dock = DockStyle.Top;
            pnlSelector.Location = new Point(0, 0);
            pnlSelector.Name = "pnlSelector";
            pnlSelector.Size = new Size(940, 56);
            pnlSelector.TabIndex = 0;
            //
            // lblProyecto
            //
            lblProyecto.Font = new Font("Segoe UI Semibold", 9.5F);
            lblProyecto.ForeColor = Color.FromArgb(44, 51, 47);
            lblProyecto.Location = new Point(24, 18);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(70, 21);
            lblProyecto.TabIndex = 0;
            lblProyecto.Text = "Proyecto:";
            lblProyecto.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboProyecto
            //
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyecto.Font = new Font("Segoe UI", 10F);
            cboProyecto.FormattingEnabled = true;
            cboProyecto.Location = new Point(98, 14);
            cboProyecto.Name = "cboProyecto";
            cboProyecto.Size = new Size(240, 25);
            cboProyecto.TabIndex = 1;
            cboProyecto.SelectedIndexChanged += cboProyecto_SelectedIndexChanged;
            //
            // lblHistoria
            //
            lblHistoria.Font = new Font("Segoe UI Semibold", 9.5F);
            lblHistoria.ForeColor = Color.FromArgb(44, 51, 47);
            lblHistoria.Location = new Point(356, 18);
            lblHistoria.Name = "lblHistoria";
            lblHistoria.Size = new Size(60, 21);
            lblHistoria.TabIndex = 2;
            lblHistoria.Text = "Historia:";
            lblHistoria.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboHistoria
            //
            cboHistoria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboHistoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHistoria.Font = new Font("Segoe UI", 10F);
            cboHistoria.FormattingEnabled = true;
            cboHistoria.Location = new Point(420, 14);
            cboHistoria.Name = "cboHistoria";
            cboHistoria.Size = new Size(496, 25);
            cboHistoria.TabIndex = 3;
            cboHistoria.SelectedIndexChanged += cboHistoria_SelectedIndexChanged;
            //
            // lblCabecera
            //
            lblCabecera.BackColor = Color.FromArgb(245, 241, 232);
            lblCabecera.Dock = DockStyle.Top;
            lblCabecera.Font = new Font("Consolas", 9F, FontStyle.Bold);
            lblCabecera.ForeColor = Color.FromArgb(44, 51, 47);
            lblCabecera.Location = new Point(0, 56);
            lblCabecera.Name = "lblCabecera";
            lblCabecera.Padding = new Padding(24, 8, 12, 0);
            lblCabecera.Size = new Size(940, 40);
            lblCabecera.TabIndex = 1;
            lblCabecera.Text = "HISTORIA —";
            //
            // splitVertical
            //
            splitVertical.BackColor = Color.FromArgb(218, 212, 196);
            splitVertical.Dock = DockStyle.Fill;
            splitVertical.FixedPanel = FixedPanel.Panel2;
            splitVertical.Location = new Point(0, 96);
            splitVertical.Name = "splitVertical";
            splitVertical.Orientation = Orientation.Vertical;
            splitVertical.Panel1.Controls.Add(dgvTareas);
            splitVertical.Panel1.Controls.Add(lblResumenHoras);
            splitVertical.Panel2.Controls.Add(pnlDetalle);
            splitVertical.Size = new Size(940, 504);
            splitVertical.Panel1MinSize = 320;
            splitVertical.Panel2MinSize = 360;
            splitVertical.SplitterWidth = 6;
            splitVertical.TabIndex = 2;
            //
            // dgvTareas
            //
            dgvTareas.AutoGenerateColumns = false;
            dgvTareas.BackgroundColor = Color.White;
            dgvTareas.BorderStyle = BorderStyle.None;
            dgvTareas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTareas.Dock = DockStyle.Fill;
            dgvTareas.Location = new Point(0, 0);
            dgvTareas.MultiSelect = false;
            dgvTareas.Name = "dgvTareas";
            dgvTareas.ReadOnly = true;
            dgvTareas.RowHeadersVisible = false;
            dgvTareas.Size = new Size(514, 452);
            dgvTareas.TabIndex = 0;
            dgvTareas.SelectionChanged += dgvTareas_SelectionChanged;
            dgvTareas.CellDoubleClick += dgvTareas_CellDoubleClick;
            //
            // lblResumenHoras
            //
            lblResumenHoras.BackColor = Color.FromArgb(245, 241, 232);
            lblResumenHoras.Dock = DockStyle.Bottom;
            lblResumenHoras.Font = new Font("Segoe UI", 9F);
            lblResumenHoras.ForeColor = Color.FromArgb(44, 51, 47);
            lblResumenHoras.Location = new Point(0, 452);
            lblResumenHoras.Name = "lblResumenHoras";
            lblResumenHoras.Padding = new Padding(12, 10, 12, 0);
            lblResumenHoras.Size = new Size(514, 52);
            lblResumenHoras.TabIndex = 1;
            lblResumenHoras.Text = "Sin historia seleccionada.";
            //
            // pnlDetalle
            //
            pnlDetalle.BackColor = Color.FromArgb(245, 241, 232);
            pnlDetalle.Controls.Add(pnlCampos);
            pnlDetalle.Controls.Add(lblTituloDetalle);
            pnlDetalle.Controls.Add(pnlAcciones);
            pnlDetalle.Dock = DockStyle.Fill;
            pnlDetalle.Location = new Point(0, 0);
            pnlDetalle.Name = "pnlDetalle";
            pnlDetalle.Size = new Size(420, 504);
            pnlDetalle.TabIndex = 0;
            //
            // lblTituloDetalle
            //
            lblTituloDetalle.BackColor = Color.FromArgb(245, 241, 232);
            lblTituloDetalle.Dock = DockStyle.Top;
            lblTituloDetalle.Font = new Font("Consolas", 9F, FontStyle.Bold);
            lblTituloDetalle.ForeColor = Color.FromArgb(44, 51, 47);
            lblTituloDetalle.Location = new Point(0, 0);
            lblTituloDetalle.Name = "lblTituloDetalle";
            lblTituloDetalle.Padding = new Padding(16, 10, 0, 0);
            lblTituloDetalle.Size = new Size(420, 32);
            lblTituloDetalle.TabIndex = 0;
            lblTituloDetalle.Text = "NUEVA TAREA TÉCNICA";
            //
            // pnlCampos
            //
            pnlCampos.BackColor = Color.FromArgb(245, 241, 232);
            pnlCampos.Controls.Add(lblCampoTitulo);
            pnlCampos.Controls.Add(txtTitulo);
            pnlCampos.Controls.Add(lblCampoEstado);
            pnlCampos.Controls.Add(cboEstado);
            pnlCampos.Controls.Add(lblCampoAsignado);
            pnlCampos.Controls.Add(cboAsignado);
            pnlCampos.Controls.Add(lblCampoHorasEstimadas);
            pnlCampos.Controls.Add(numHorasEstimadas);
            pnlCampos.Controls.Add(lblCampoHorasTrabajadas);
            pnlCampos.Controls.Add(numHorasTrabajadas);
            pnlCampos.Controls.Add(lblAyuda);
            pnlCampos.Dock = DockStyle.Fill;
            pnlCampos.Location = new Point(0, 32);
            pnlCampos.Name = "pnlCampos";
            pnlCampos.Size = new Size(420, 416);
            pnlCampos.TabIndex = 1;
            //
            // lblCampoTitulo
            //
            lblCampoTitulo.Font = new Font("Consolas", 8.5F);
            lblCampoTitulo.ForeColor = Color.FromArgb(138, 133, 122);
            lblCampoTitulo.Location = new Point(16, 12);
            lblCampoTitulo.Name = "lblCampoTitulo";
            lblCampoTitulo.Size = new Size(200, 18);
            lblCampoTitulo.TabIndex = 0;
            lblCampoTitulo.Text = "TÍTULO DE LA TAREA";
            //
            // txtTitulo
            //
            txtTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTitulo.BorderStyle = BorderStyle.FixedSingle;
            txtTitulo.Font = new Font("Segoe UI", 10F);
            txtTitulo.Location = new Point(16, 32);
            txtTitulo.MaxLength = 200;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(388, 25);
            txtTitulo.TabIndex = 1;
            //
            // lblCampoEstado
            //
            lblCampoEstado.Font = new Font("Consolas", 8.5F);
            lblCampoEstado.ForeColor = Color.FromArgb(138, 133, 122);
            lblCampoEstado.Location = new Point(16, 72);
            lblCampoEstado.Name = "lblCampoEstado";
            lblCampoEstado.Size = new Size(120, 18);
            lblCampoEstado.TabIndex = 2;
            lblCampoEstado.Text = "ESTADO";
            //
            // cboEstado
            //
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Font = new Font("Segoe UI", 10F);
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(16, 92);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(170, 25);
            cboEstado.TabIndex = 3;
            //
            // lblCampoAsignado
            //
            lblCampoAsignado.Font = new Font("Consolas", 8.5F);
            lblCampoAsignado.ForeColor = Color.FromArgb(138, 133, 122);
            lblCampoAsignado.Location = new Point(202, 72);
            lblCampoAsignado.Name = "lblCampoAsignado";
            lblCampoAsignado.Size = new Size(150, 18);
            lblCampoAsignado.TabIndex = 4;
            lblCampoAsignado.Text = "RESPONSABLE";
            //
            // cboAsignado
            //
            cboAsignado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboAsignado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAsignado.Font = new Font("Segoe UI", 10F);
            cboAsignado.FormattingEnabled = true;
            cboAsignado.Location = new Point(202, 92);
            cboAsignado.Name = "cboAsignado";
            cboAsignado.Size = new Size(202, 25);
            cboAsignado.TabIndex = 5;
            //
            // lblCampoHorasEstimadas
            //
            lblCampoHorasEstimadas.Font = new Font("Consolas", 8.5F);
            lblCampoHorasEstimadas.ForeColor = Color.FromArgb(138, 133, 122);
            lblCampoHorasEstimadas.Location = new Point(16, 132);
            lblCampoHorasEstimadas.Name = "lblCampoHorasEstimadas";
            lblCampoHorasEstimadas.Size = new Size(150, 18);
            lblCampoHorasEstimadas.TabIndex = 6;
            lblCampoHorasEstimadas.Text = "HORAS ESTIMADAS";
            //
            // numHorasEstimadas
            //
            numHorasEstimadas.BorderStyle = BorderStyle.FixedSingle;
            numHorasEstimadas.DecimalPlaces = 2;
            numHorasEstimadas.Font = new Font("Segoe UI", 10F);
            numHorasEstimadas.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numHorasEstimadas.Location = new Point(16, 152);
            numHorasEstimadas.Maximum = new decimal(new int[] { 99999, 0, 0, 131072 });
            numHorasEstimadas.Name = "numHorasEstimadas";
            numHorasEstimadas.Size = new Size(170, 25);
            numHorasEstimadas.TabIndex = 7;
            //
            // lblCampoHorasTrabajadas
            //
            lblCampoHorasTrabajadas.Font = new Font("Consolas", 8.5F);
            lblCampoHorasTrabajadas.ForeColor = Color.FromArgb(138, 133, 122);
            lblCampoHorasTrabajadas.Location = new Point(202, 132);
            lblCampoHorasTrabajadas.Name = "lblCampoHorasTrabajadas";
            lblCampoHorasTrabajadas.Size = new Size(160, 18);
            lblCampoHorasTrabajadas.TabIndex = 8;
            lblCampoHorasTrabajadas.Text = "HORAS TRABAJADAS";
            //
            // numHorasTrabajadas
            //
            numHorasTrabajadas.BorderStyle = BorderStyle.FixedSingle;
            numHorasTrabajadas.DecimalPlaces = 2;
            numHorasTrabajadas.Font = new Font("Segoe UI", 10F);
            numHorasTrabajadas.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numHorasTrabajadas.Location = new Point(202, 152);
            numHorasTrabajadas.Maximum = new decimal(new int[] { 99999, 0, 0, 131072 });
            numHorasTrabajadas.Name = "numHorasTrabajadas";
            numHorasTrabajadas.Size = new Size(170, 25);
            numHorasTrabajadas.TabIndex = 9;
            //
            // lblAyuda
            //
            lblAyuda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAyuda.Font = new Font("Segoe UI", 9F);
            lblAyuda.ForeColor = Color.FromArgb(138, 133, 122);
            lblAyuda.Location = new Point(16, 196);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(388, 76);
            lblAyuda.TabIndex = 10;
            lblAyuda.Text = "La historia no puede pasar a 'Done' mientras alguna de sus tareas técnicas siga en 'Pendiente' o 'En Proceso'.";
            //
            // pnlAcciones
            //
            pnlAcciones.BackColor = Color.FromArgb(245, 241, 232);
            pnlAcciones.Controls.Add(btnGuardar);
            pnlAcciones.Controls.Add(btnNueva);
            pnlAcciones.Controls.Add(btnAvanzar);
            pnlAcciones.Controls.Add(btnCerrar);
            pnlAcciones.Dock = DockStyle.Bottom;
            pnlAcciones.Location = new Point(0, 448);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(420, 56);
            pnlAcciones.TabIndex = 2;
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.FromArgb(12, 110, 99);
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(16, 10);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(96, 36);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnNueva
            //
            btnNueva.BackColor = Color.FromArgb(245, 241, 232);
            btnNueva.FlatAppearance.BorderColor = Color.FromArgb(218, 212, 196);
            btnNueva.FlatStyle = FlatStyle.Flat;
            btnNueva.Font = new Font("Segoe UI Semibold", 9.5F);
            btnNueva.ForeColor = Color.FromArgb(44, 51, 47);
            btnNueva.Location = new Point(118, 10);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(88, 36);
            btnNueva.TabIndex = 1;
            btnNueva.Text = "Nueva";
            btnNueva.UseVisualStyleBackColor = false;
            btnNueva.Click += btnNueva_Click;
            //
            // btnAvanzar
            //
            btnAvanzar.BackColor = Color.FromArgb(245, 241, 232);
            btnAvanzar.FlatAppearance.BorderColor = Color.FromArgb(218, 212, 196);
            btnAvanzar.FlatStyle = FlatStyle.Flat;
            btnAvanzar.Font = new Font("Segoe UI Semibold", 9.5F);
            btnAvanzar.ForeColor = Color.FromArgb(44, 51, 47);
            btnAvanzar.Location = new Point(212, 10);
            btnAvanzar.Name = "btnAvanzar";
            btnAvanzar.Size = new Size(104, 36);
            btnAvanzar.TabIndex = 2;
            btnAvanzar.Text = "Avanzar →";
            btnAvanzar.UseVisualStyleBackColor = false;
            btnAvanzar.Click += btnAvanzar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.BackColor = Color.FromArgb(245, 241, 232);
            btnCerrar.FlatAppearance.BorderColor = Color.FromArgb(218, 212, 196);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI Semibold", 9.5F);
            btnCerrar.ForeColor = Color.FromArgb(44, 51, 47);
            btnCerrar.Location = new Point(332, 10);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(72, 36);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // TareaEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(940, 600);
            Controls.Add(splitVertical);
            Controls.Add(pnlSelector);
            Controls.Add(lblCabecera);
            MinimumSize = new Size(820, 480);
            Name = "TareaEdicion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Desglose técnico de la historia";
            pnlSelector.ResumeLayout(false);
            splitVertical.Panel1.ResumeLayout(false);
            splitVertical.Panel2.ResumeLayout(false);
            splitVertical.EndInit();
            splitVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTareas).EndInit();
            pnlDetalle.ResumeLayout(false);
            pnlCampos.ResumeLayout(false);
            pnlCampos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numHorasEstimadas).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHorasTrabajadas).EndInit();
            pnlAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private Label lblProyecto;
        private ComboBox cboProyecto;
        private Label lblHistoria;
        private ComboBox cboHistoria;
        private Label lblCabecera;
        private SplitContainer splitVertical;
        private DataGridView dgvTareas;
        private Label lblResumenHoras;
        private Panel pnlDetalle;
        private Label lblTituloDetalle;
        private Panel pnlCampos;
        private Label lblCampoTitulo;
        private TextBox txtTitulo;
        private Label lblCampoEstado;
        private ComboBox cboEstado;
        private Label lblCampoAsignado;
        private ComboBox cboAsignado;
        private Label lblCampoHorasEstimadas;
        private NumericUpDown numHorasEstimadas;
        private Label lblCampoHorasTrabajadas;
        private NumericUpDown numHorasTrabajadas;
        private Label lblAyuda;
        private Panel pnlAcciones;
        private Button btnGuardar;
        private Button btnNueva;
        private Button btnAvanzar;
        private Button btnCerrar;
    }
}
