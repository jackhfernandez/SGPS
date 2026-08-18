namespace Presentacion.QA
{
    partial class BugGestion
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
            dgvBugs = new DataGridView();
            lblResumen = new Label();
            pnlDetalle = new Panel();
            lblResumenDetalle = new Label();
            lblTituloDetalle = new Label();
            pnlAcciones = new Panel();
            btnCancelar = new Button();
            btnRetroceder = new Button();
            btnAvanzar = new Button();
            tblDetalle = new TableLayoutPanel();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblTitulo = new Label();
            txtTitulo = new TextBox();
            lblSeveridad = new Label();
            txtSeveridad = new TextBox();
            lblEstado = new Label();
            txtEstado = new TextBox();
            lblHistoria = new Label();
            txtHistoria = new TextBox();
            lblPasos = new Label();
            txtPasos = new TextBox();
            pnlSelector.SuspendLayout();
            splitVertical.SuspendLayout();
            splitVertical.Panel1.SuspendLayout();
            splitVertical.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBugs).BeginInit();
            pnlDetalle.SuspendLayout();
            pnlAcciones.SuspendLayout();
            tblDetalle.SuspendLayout();
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
            splitVertical.Panel1.Controls.Add(dgvBugs);
            splitVertical.Panel1.Controls.Add(lblResumen);
            splitVertical.Panel2.Controls.Add(pnlDetalle);
            splitVertical.Size = new Size(900, 534);
            splitVertical.Panel1MinSize = 300;
            splitVertical.Panel2MinSize = 380;
            splitVertical.SplitterWidth = 6;
            splitVertical.TabIndex = 1;
            //
            // dgvBugs
            //
            dgvBugs.AutoGenerateColumns = false;
            dgvBugs.BackgroundColor = Color.White;
            dgvBugs.BorderStyle = BorderStyle.None;
            dgvBugs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBugs.Dock = DockStyle.Fill;
            dgvBugs.Location = new Point(0, 0);
            dgvBugs.MultiSelect = false;
            dgvBugs.Name = "dgvBugs";
            dgvBugs.ReadOnly = false;
            dgvBugs.RowHeadersVisible = false;
            dgvBugs.Size = new Size(300, 484);
            dgvBugs.TabIndex = 1;
            dgvBugs.CellValueChanged += dgvBugs_CellValueChanged;
            dgvBugs.SelectionChanged += dgvBugs_SelectionChanged;
            //
            // lblResumen
            //
            lblResumen.BackColor = Color.FromArgb(245, 241, 232);
            lblResumen.Dock = DockStyle.Bottom;
            lblResumen.Font = new Font("Segoe UI", 9F);
            lblResumen.ForeColor = Color.FromArgb(44, 51, 47);
            lblResumen.Location = new Point(0, 484);
            lblResumen.Name = "lblResumen";
            lblResumen.Padding = new Padding(12, 10, 12, 0);
            lblResumen.Size = new Size(300, 50);
            lblResumen.TabIndex = 0;
            lblResumen.Text = "Selecciona un proyecto para cargar sus bugs.";
            //
            // pnlDetalle
            //
            pnlDetalle.BackColor = Color.FromArgb(245, 241, 232);
            pnlDetalle.Controls.Add(tblDetalle);
            pnlDetalle.Controls.Add(lblTituloDetalle);
            pnlDetalle.Controls.Add(pnlAcciones);
            pnlDetalle.Controls.Add(lblResumenDetalle);
            pnlDetalle.Dock = DockStyle.Fill;
            pnlDetalle.Location = new Point(0, 0);
            pnlDetalle.Name = "pnlDetalle";
            pnlDetalle.Size = new Size(594, 534);
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
            lblTituloDetalle.Padding = new Padding(12, 10, 0, 0);
            lblTituloDetalle.Size = new Size(594, 32);
            lblTituloDetalle.TabIndex = 0;
            lblTituloDetalle.Text = "DETALLE DEL BUG —";
            //
            // pnlAcciones
            //
            pnlAcciones.BackColor = Color.FromArgb(245, 241, 232);
            pnlAcciones.Controls.Add(btnCancelar);
            pnlAcciones.Controls.Add(btnRetroceder);
            pnlAcciones.Controls.Add(btnAvanzar);
            pnlAcciones.Dock = DockStyle.Top;
            pnlAcciones.Location = new Point(0, 32);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(594, 56);
            pnlAcciones.TabIndex = 1;
            //
            // btnAvanzar
            //
            btnAvanzar.Location = new Point(24, 10);
            btnAvanzar.Name = "btnAvanzar";
            btnAvanzar.Size = new Size(120, 36);
            btnAvanzar.TabIndex = 0;
            btnAvanzar.Text = "Avanzar →";
            btnAvanzar.UseVisualStyleBackColor = true;
            btnAvanzar.Click += btnAvanzar_Click;
            //
            // btnRetroceder
            //
            btnRetroceder.Location = new Point(150, 10);
            btnRetroceder.Name = "btnRetroceder";
            btnRetroceder.Size = new Size(120, 36);
            btnRetroceder.TabIndex = 1;
            btnRetroceder.Text = "← Retroceder";
            btnRetroceder.UseVisualStyleBackColor = true;
            btnRetroceder.Click += btnRetroceder_Click;
            //
            // btnCancelar
            //
            btnCancelar.Location = new Point(276, 10);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(96, 36);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // tblDetalle
            //
            tblDetalle.BackColor = Color.FromArgb(245, 241, 232);
            tblDetalle.ColumnCount = 2;
            tblDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDetalle.Controls.Add(lblCodigo, 0, 0);
            tblDetalle.Controls.Add(txtCodigo, 1, 0);
            tblDetalle.Controls.Add(lblTitulo, 0, 1);
            tblDetalle.Controls.Add(txtTitulo, 1, 1);
            tblDetalle.Controls.Add(lblSeveridad, 0, 2);
            tblDetalle.Controls.Add(txtSeveridad, 1, 2);
            tblDetalle.Controls.Add(lblEstado, 0, 3);
            tblDetalle.Controls.Add(txtEstado, 1, 3);
            tblDetalle.Controls.Add(lblHistoria, 0, 4);
            tblDetalle.Controls.Add(txtHistoria, 1, 4);
            tblDetalle.Controls.Add(lblPasos, 0, 5);
            tblDetalle.Controls.Add(txtPasos, 1, 5);
            tblDetalle.Dock = DockStyle.Fill;
            tblDetalle.Location = new Point(0, 88);
            tblDetalle.Name = "tblDetalle";
            tblDetalle.RowCount = 6;
            tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblDetalle.Size = new Size(594, 412);
            tblDetalle.TabIndex = 2;
            //
            // lblCodigo
            //
            lblCodigo.Anchor = AnchorStyles.Left;
            lblCodigo.AutoSize = true;
            lblCodigo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblCodigo.ForeColor = Color.FromArgb(44, 51, 47);
            lblCodigo.Location = new Point(24, 15);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(50, 17);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código:";
            //
            // txtCodigo
            //
            txtCodigo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCodigo.BackColor = Color.White;
            txtCodigo.Font = new Font("Consolas", 10F, FontStyle.Bold);
            txtCodigo.Location = new Point(164, 12);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.ReadOnly = true;
            txtCodigo.Size = new Size(406, 24);
            txtCodigo.TabIndex = 1;
            //
            // lblTitulo
            //
            lblTitulo.Anchor = AnchorStyles.Left;
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTitulo.ForeColor = Color.FromArgb(44, 51, 47);
            lblTitulo.Location = new Point(24, 63);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(44, 17);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Título:";
            //
            // txtTitulo
            //
            txtTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTitulo.BackColor = Color.White;
            txtTitulo.BorderStyle = BorderStyle.FixedSingle;
            txtTitulo.Font = new Font("Segoe UI", 10F);
            txtTitulo.Location = new Point(164, 60);
            txtTitulo.Name = "txtTitulo";
            txtTitulo.ReadOnly = true;
            txtTitulo.Size = new Size(406, 25);
            txtTitulo.TabIndex = 3;
            //
            // lblSeveridad
            //
            lblSeveridad.Anchor = AnchorStyles.Left;
            lblSeveridad.AutoSize = true;
            lblSeveridad.Font = new Font("Segoe UI Semibold", 9.5F);
            lblSeveridad.ForeColor = Color.FromArgb(44, 51, 47);
            lblSeveridad.Location = new Point(24, 111);
            lblSeveridad.Name = "lblSeveridad";
            lblSeveridad.Size = new Size(68, 17);
            lblSeveridad.TabIndex = 4;
            lblSeveridad.Text = "Severidad:";
            //
            // txtSeveridad
            //
            txtSeveridad.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSeveridad.BackColor = Color.White;
            txtSeveridad.BorderStyle = BorderStyle.FixedSingle;
            txtSeveridad.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtSeveridad.Location = new Point(164, 108);
            txtSeveridad.Name = "txtSeveridad";
            txtSeveridad.ReadOnly = true;
            txtSeveridad.Size = new Size(160, 25);
            txtSeveridad.TabIndex = 5;
            //
            // lblEstado
            //
            lblEstado.Anchor = AnchorStyles.Left;
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI Semibold", 9.5F);
            lblEstado.ForeColor = Color.FromArgb(44, 51, 47);
            lblEstado.Location = new Point(24, 159);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(47, 17);
            lblEstado.TabIndex = 6;
            lblEstado.Text = "Estado:";
            //
            // txtEstado
            //
            txtEstado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEstado.BackColor = Color.White;
            txtEstado.BorderStyle = BorderStyle.FixedSingle;
            txtEstado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtEstado.Location = new Point(164, 156);
            txtEstado.Name = "txtEstado";
            txtEstado.ReadOnly = true;
            txtEstado.Size = new Size(160, 25);
            txtEstado.TabIndex = 7;
            //
            // lblHistoria
            //
            lblHistoria.Anchor = AnchorStyles.Left;
            lblHistoria.AutoSize = true;
            lblHistoria.Font = new Font("Segoe UI Semibold", 9.5F);
            lblHistoria.ForeColor = Color.FromArgb(44, 51, 47);
            lblHistoria.Location = new Point(24, 207);
            lblHistoria.Name = "lblHistoria";
            lblHistoria.Size = new Size(59, 17);
            lblHistoria.TabIndex = 8;
            lblHistoria.Text = "Historia:";
            //
            // txtHistoria
            //
            txtHistoria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtHistoria.BackColor = Color.White;
            txtHistoria.BorderStyle = BorderStyle.FixedSingle;
            txtHistoria.Font = new Font("Segoe UI", 10F);
            txtHistoria.Location = new Point(164, 204);
            txtHistoria.Name = "txtHistoria";
            txtHistoria.ReadOnly = true;
            txtHistoria.Size = new Size(406, 25);
            txtHistoria.TabIndex = 9;
            //
            // lblPasos
            //
            lblPasos.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblPasos.AutoSize = true;
            lblPasos.Font = new Font("Segoe UI Semibold", 9.5F);
            lblPasos.ForeColor = Color.FromArgb(44, 51, 47);
            lblPasos.Location = new Point(24, 260);
            lblPasos.Name = "lblPasos";
            lblPasos.Size = new Size(105, 17);
            lblPasos.TabIndex = 10;
            lblPasos.Text = "Pasos a seguir:";
            //
            // txtPasos
            //
            txtPasos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPasos.BackColor = Color.White;
            txtPasos.BorderStyle = BorderStyle.FixedSingle;
            txtPasos.Font = new Font("Segoe UI", 10F);
            txtPasos.Location = new Point(164, 257);
            txtPasos.Multiline = true;
            txtPasos.Name = "txtPasos";
            txtPasos.ReadOnly = true;
            txtPasos.ScrollBars = ScrollBars.Vertical;
            txtPasos.Size = new Size(406, 130);
            txtPasos.TabIndex = 11;
            //
            // lblResumenDetalle
            //
            lblResumenDetalle.BackColor = Color.FromArgb(245, 241, 232);
            lblResumenDetalle.Dock = DockStyle.Bottom;
            lblResumenDetalle.Font = new Font("Segoe UI", 9F);
            lblResumenDetalle.ForeColor = Color.FromArgb(44, 51, 47);
            lblResumenDetalle.Location = new Point(0, 500);
            lblResumenDetalle.Name = "lblResumenDetalle";
            lblResumenDetalle.Padding = new Padding(12, 10, 12, 0);
            lblResumenDetalle.Size = new Size(594, 34);
            lblResumenDetalle.TabIndex = 3;
            lblResumenDetalle.Text = "Sin bug seleccionado.";
            //
            // BugGestion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(900, 590);
            Controls.Add(splitVertical);
            Controls.Add(pnlSelector);
            Name = "BugGestion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de bugs";
            pnlSelector.ResumeLayout(false);
            splitVertical.Panel1.ResumeLayout(false);
            splitVertical.Panel2.ResumeLayout(false);
            splitVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBugs).EndInit();
            pnlDetalle.ResumeLayout(false);
            pnlAcciones.ResumeLayout(false);
            tblDetalle.ResumeLayout(false);
            tblDetalle.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private ComboBox cboProyecto;
        private Label lblProyecto;
        private SplitContainer splitVertical;
        private DataGridView dgvBugs;
        private Label lblResumen;
        private Panel pnlDetalle;
        private Label lblTituloDetalle;
        private Panel pnlAcciones;
        private Button btnAvanzar;
        private Button btnRetroceder;
        private Button btnCancelar;
        private TableLayoutPanel tblDetalle;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblTitulo;
        private TextBox txtTitulo;
        private Label lblSeveridad;
        private TextBox txtSeveridad;
        private Label lblEstado;
        private TextBox txtEstado;
        private Label lblHistoria;
        private TextBox txtHistoria;
        private Label lblPasos;
        private TextBox txtPasos;
        private Label lblResumenDetalle;
    }
}