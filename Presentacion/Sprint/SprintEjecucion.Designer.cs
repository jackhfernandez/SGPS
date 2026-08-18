namespace Presentacion.Sprint
{
    partial class SprintEjecucion
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
            splitVertical = new SplitContainer();
            dgvSprints = new DataGridView();
            lblResumen = new Label();
            pnlDetalle = new Panel();
            lblResumenSprint = new Label();
            lblTitulo = new Label();
            pnlAcciones = new Panel();
            btnCerrar = new Button();
            btnRetroceder = new Button();
            btnAvanzar = new Button();
            dgvHistorias = new DataGridView();
            pnlSelector.SuspendLayout();
            splitVertical.BeginInit();
            splitVertical.Panel1.SuspendLayout();
            splitVertical.Panel2.SuspendLayout();
            splitVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSprints).BeginInit();
            pnlDetalle.SuspendLayout();
            pnlAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorias).BeginInit();
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
            splitVertical.Panel2.Controls.Add(pnlDetalle);
            splitVertical.Size = new Size(900, 384);
            splitVertical.Panel1MinSize = 220;
            splitVertical.Panel2MinSize = 340;
            splitVertical.SplitterWidth = 6;
            splitVertical.TabIndex = 1;
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
            lblResumen.Text = "Selecciona un sprint para ver su avance.";
            //
            // pnlDetalle
            //
            pnlDetalle.BackColor = Color.FromArgb(245, 241, 232);
            pnlDetalle.Controls.Add(dgvHistorias);
            pnlDetalle.Controls.Add(pnlAcciones);
            pnlDetalle.Controls.Add(lblTitulo);
            pnlDetalle.Controls.Add(lblResumenSprint);
            pnlDetalle.Dock = DockStyle.Fill;
            pnlDetalle.Location = new Point(0, 0);
            pnlDetalle.Name = "pnlDetalle";
            pnlDetalle.Size = new Size(634, 384);
            pnlDetalle.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.BackColor = Color.FromArgb(245, 241, 232);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Consolas", 9F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 51, 47);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(12, 10, 0, 0);
            lblTitulo.Size = new Size(634, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "SPRINT —";
            //
            // pnlAcciones
            //
            pnlAcciones.BackColor = Color.FromArgb(245, 241, 232);
            pnlAcciones.Controls.Add(btnCerrar);
            pnlAcciones.Controls.Add(btnRetroceder);
            pnlAcciones.Controls.Add(btnAvanzar);
            pnlAcciones.Dock = DockStyle.Top;
            pnlAcciones.Location = new Point(0, 32);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(634, 56);
            pnlAcciones.TabIndex = 1;
            //
            // btnAvanzar
            //
            btnAvanzar.BackColor = Color.FromArgb(12, 110, 99);
            btnAvanzar.FlatAppearance.BorderSize = 0;
            btnAvanzar.FlatStyle = FlatStyle.Flat;
            btnAvanzar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAvanzar.ForeColor = Color.White;
            btnAvanzar.Location = new Point(24, 10);
            btnAvanzar.Name = "btnAvanzar";
            btnAvanzar.Size = new Size(120, 36);
            btnAvanzar.TabIndex = 0;
            btnAvanzar.Text = "Avanzar →";
            btnAvanzar.UseVisualStyleBackColor = false;
            btnAvanzar.Click += btnAvanzar_Click;
            //
            // btnRetroceder
            //
            btnRetroceder.BackColor = Color.FromArgb(12, 110, 99);
            btnRetroceder.FlatAppearance.BorderSize = 0;
            btnRetroceder.FlatStyle = FlatStyle.Flat;
            btnRetroceder.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnRetroceder.ForeColor = Color.White;
            btnRetroceder.Location = new Point(150, 10);
            btnRetroceder.Name = "btnRetroceder";
            btnRetroceder.Size = new Size(120, 36);
            btnRetroceder.TabIndex = 1;
            btnRetroceder.Text = "← Retroceder";
            btnRetroceder.UseVisualStyleBackColor = false;
            btnRetroceder.Click += btnRetroceder_Click;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(200, 200, 200);
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.FromArgb(44, 51, 47);
            btnCerrar.Location = new Point(276, 10);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 36);
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "Cerrar sprint";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // dgvHistorias
            //
            dgvHistorias.AutoGenerateColumns = false;
            dgvHistorias.BackgroundColor = Color.White;
            dgvHistorias.BorderStyle = BorderStyle.None;
            dgvHistorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorias.Dock = DockStyle.Fill;
            dgvHistorias.Location = new Point(0, 88);
            dgvHistorias.MultiSelect = false;
            dgvHistorias.Name = "dgvHistorias";
            dgvHistorias.ReadOnly = true;
            dgvHistorias.RowHeadersVisible = false;
            dgvHistorias.Size = new Size(634, 262);
            dgvHistorias.TabIndex = 2;
            dgvHistorias.SelectionChanged += dgvHistorias_SelectionChanged;
            //
            // lblResumenSprint
            //
            lblResumenSprint.BackColor = Color.FromArgb(245, 241, 232);
            lblResumenSprint.Dock = DockStyle.Bottom;
            lblResumenSprint.Font = new Font("Segoe UI", 9F);
            lblResumenSprint.ForeColor = Color.FromArgb(44, 51, 47);
            lblResumenSprint.Location = new Point(0, 350);
            lblResumenSprint.Name = "lblResumenSprint";
            lblResumenSprint.Padding = new Padding(12, 10, 12, 0);
            lblResumenSprint.Size = new Size(634, 34);
            lblResumenSprint.TabIndex = 3;
            lblResumenSprint.Text = "Sin sprint seleccionado.";
            //
            // SprintEjecucion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(900, 590);
            Controls.Add(splitVertical);
            Controls.Add(pnlSelector);
            Name = "SprintEjecucion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ejecución de sprint";
            pnlSelector.ResumeLayout(false);
            splitVertical.Panel1.ResumeLayout(false);
            splitVertical.Panel2.ResumeLayout(false);
            splitVertical.EndInit();
            splitVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSprints).EndInit();
            pnlDetalle.ResumeLayout(false);
            pnlAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorias).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private ComboBox cboProyecto;
        private Label lblProyecto;
        private SplitContainer splitVertical;
        private DataGridView dgvSprints;
        private Label lblResumen;
        private Panel pnlDetalle;
        private Label lblTitulo;
        private Panel pnlAcciones;
        private Button btnAvanzar;
        private Button btnRetroceder;
        private Button btnCerrar;
        private DataGridView dgvHistorias;
        private Label lblResumenSprint;
    }
}