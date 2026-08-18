namespace Presentacion.QA
{
    partial class BugReporte
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
            dgvHistorias = new DataGridView();
            lblListaTitulo = new Label();
            lblResumen = new Label();
            pnlAcciones = new Panel();
            btnCancelar = new Button();
            btnLimpiar = new Button();
            btnReportar = new Button();
            tblCampos = new TableLayoutPanel();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblTitulo = new Label();
            txtTitulo = new TextBox();
            lblSeveridad = new Label();
            cboSeveridad = new ComboBox();
            lblPasos = new Label();
            txtPasos = new TextBox();
            lblHistoriaTitulo = new Label();
            lblHistoria = new Label();
            pnlSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitVertical).BeginInit();
            splitVertical.Panel1.SuspendLayout();
            splitVertical.Panel2.SuspendLayout();
            splitVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorias).BeginInit();
            pnlAcciones.SuspendLayout();
            tblCampos.SuspendLayout();
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
            // splitVertical
            // 
            splitVertical.BackColor = Color.FromArgb(218, 212, 196);
            splitVertical.Dock = DockStyle.Fill;
            splitVertical.FixedPanel = FixedPanel.Panel1;
            splitVertical.Location = new Point(0, 56);
            splitVertical.Name = "splitVertical";
            // 
            // splitVertical.Panel1
            // 
            splitVertical.Panel1.Controls.Add(dgvHistorias);
            splitVertical.Panel1.Controls.Add(lblListaTitulo);
            splitVertical.Panel1.Controls.Add(lblResumen);
            splitVertical.Panel1MinSize = 300;
            // 
            // splitVertical.Panel2
            // 
            splitVertical.Panel2.Controls.Add(pnlAcciones);
            splitVertical.Panel2.Controls.Add(tblCampos);
            splitVertical.Panel2MinSize = 380;
            splitVertical.Size = new Size(900, 534);
            splitVertical.SplitterDistance = 300;
            splitVertical.SplitterWidth = 6;
            splitVertical.TabIndex = 1;
            // 
            // dgvHistorias
            // 
            dgvHistorias.BackgroundColor = Color.White;
            dgvHistorias.BorderStyle = BorderStyle.None;
            dgvHistorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorias.Dock = DockStyle.Fill;
            dgvHistorias.Location = new Point(0, 30);
            dgvHistorias.MultiSelect = false;
            dgvHistorias.Name = "dgvHistorias";
            dgvHistorias.ReadOnly = true;
            dgvHistorias.RowHeadersVisible = false;
            dgvHistorias.Size = new Size(300, 454);
            dgvHistorias.TabIndex = 1;
            dgvHistorias.SelectionChanged += dgvHistorias_SelectionChanged;
            // 
            // lblListaTitulo
            // 
            lblListaTitulo.BackColor = Color.FromArgb(245, 241, 232);
            lblListaTitulo.Dock = DockStyle.Top;
            lblListaTitulo.Font = new Font("Consolas", 8.5F);
            lblListaTitulo.ForeColor = Color.FromArgb(138, 133, 122);
            lblListaTitulo.Location = new Point(0, 0);
            lblListaTitulo.Name = "lblListaTitulo";
            lblListaTitulo.Padding = new Padding(12, 8, 0, 0);
            lblListaTitulo.Size = new Size(300, 30);
            lblListaTitulo.TabIndex = 0;
            lblListaTitulo.Text = "HISTORIAS DEL PROYECTO";
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
            lblResumen.TabIndex = 2;
            lblResumen.Text = "Selecciona un proyecto para cargar sus historias.";
            // 
            // pnlAcciones
            // 
            pnlAcciones.BackColor = Color.FromArgb(245, 241, 232);
            pnlAcciones.Controls.Add(btnCancelar);
            pnlAcciones.Controls.Add(btnLimpiar);
            pnlAcciones.Controls.Add(btnReportar);
            pnlAcciones.Dock = DockStyle.Bottom;
            pnlAcciones.Location = new Point(0, 450);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(594, 84);
            pnlAcciones.TabIndex = 0;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(256, 24);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(96, 40);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(152, 24);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(96, 40);
            btnLimpiar.TabIndex = 1;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnReportar
            // 
            btnReportar.Location = new Point(24, 24);
            btnReportar.Name = "btnReportar";
            btnReportar.Size = new Size(120, 40);
            btnReportar.TabIndex = 0;
            btnReportar.Text = "Reportar bug";
            btnReportar.UseVisualStyleBackColor = true;
            btnReportar.Click += btnReportar_Click;
            // 
            // tblCampos
            // 
            tblCampos.BackColor = Color.FromArgb(245, 241, 232);
            tblCampos.ColumnCount = 2;
            tblCampos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblCampos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblCampos.Controls.Add(lblCodigo, 0, 0);
            tblCampos.Controls.Add(txtCodigo, 1, 0);
            tblCampos.Controls.Add(lblTitulo, 0, 1);
            tblCampos.Controls.Add(txtTitulo, 1, 1);
            tblCampos.Controls.Add(lblSeveridad, 0, 2);
            tblCampos.Controls.Add(cboSeveridad, 1, 2);
            tblCampos.Controls.Add(lblPasos, 0, 3);
            tblCampos.Controls.Add(txtPasos, 1, 3);
            tblCampos.Controls.Add(lblHistoriaTitulo, 0, 4);
            tblCampos.Controls.Add(lblHistoria, 1, 4);
            tblCampos.Dock = DockStyle.Fill;
            tblCampos.Location = new Point(0, 0);
            tblCampos.Name = "tblCampos";
            tblCampos.RowCount = 5;
            tblCampos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblCampos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblCampos.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblCampos.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tblCampos.Size = new Size(594, 534);
            tblCampos.TabIndex = 1;
            // 
            // lblCodigo
            // 
            lblCodigo.Anchor = AnchorStyles.Left;
            lblCodigo.AutoSize = true;
            lblCodigo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblCodigo.ForeColor = Color.FromArgb(44, 51, 47);
            lblCodigo.Location = new Point(3, 15);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(54, 17);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código:";
            // 
            // txtCodigo
            // 
            txtCodigo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCodigo.BackColor = Color.White;
            txtCodigo.Font = new Font("Consolas", 10F, FontStyle.Bold);
            txtCodigo.Location = new Point(143, 3);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.ReadOnly = true;
            txtCodigo.Size = new Size(448, 23);
            txtCodigo.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Anchor = AnchorStyles.Left;
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTitulo.ForeColor = Color.FromArgb(44, 51, 47);
            lblTitulo.Location = new Point(3, 63);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(45, 17);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Título:";
            // 
            // txtTitulo
            // 
            txtTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTitulo.BorderStyle = BorderStyle.FixedSingle;
            txtTitulo.Font = new Font("Segoe UI", 10F);
            txtTitulo.Location = new Point(143, 51);
            txtTitulo.MaxLength = 200;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(448, 25);
            txtTitulo.TabIndex = 3;
            // 
            // lblSeveridad
            // 
            lblSeveridad.Anchor = AnchorStyles.Left;
            lblSeveridad.AutoSize = true;
            lblSeveridad.Font = new Font("Segoe UI Semibold", 9.5F);
            lblSeveridad.ForeColor = Color.FromArgb(44, 51, 47);
            lblSeveridad.Location = new Point(3, 111);
            lblSeveridad.Name = "lblSeveridad";
            lblSeveridad.Size = new Size(70, 17);
            lblSeveridad.TabIndex = 4;
            lblSeveridad.Text = "Severidad:";
            // 
            // cboSeveridad
            // 
            cboSeveridad.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboSeveridad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSeveridad.Font = new Font("Segoe UI", 10F);
            cboSeveridad.FormattingEnabled = true;
            cboSeveridad.Items.AddRange(new object[] { "Bloqueante", "Alta", "Media", "Baja" });
            cboSeveridad.Location = new Point(143, 99);
            cboSeveridad.Name = "cboSeveridad";
            cboSeveridad.Size = new Size(448, 25);
            cboSeveridad.TabIndex = 5;
            // 
            // lblPasos
            // 
            lblPasos.AutoSize = true;
            lblPasos.Font = new Font("Segoe UI Semibold", 9.5F);
            lblPasos.ForeColor = Color.FromArgb(44, 51, 47);
            lblPasos.Location = new Point(3, 144);
            lblPasos.Name = "lblPasos";
            lblPasos.Size = new Size(99, 17);
            lblPasos.TabIndex = 6;
            lblPasos.Text = "Pasos a seguir:";
            // 
            // txtPasos
            // 
            txtPasos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPasos.BorderStyle = BorderStyle.FixedSingle;
            txtPasos.Font = new Font("Segoe UI", 10F);
            txtPasos.Location = new Point(143, 147);
            txtPasos.Multiline = true;
            txtPasos.Name = "txtPasos";
            txtPasos.ScrollBars = ScrollBars.Vertical;
            txtPasos.Size = new Size(448, 340);
            txtPasos.TabIndex = 7;
            // 
            // lblHistoriaTitulo
            // 
            lblHistoriaTitulo.Anchor = AnchorStyles.Left;
            lblHistoriaTitulo.AutoSize = true;
            lblHistoriaTitulo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblHistoriaTitulo.ForeColor = Color.FromArgb(44, 51, 47);
            lblHistoriaTitulo.Location = new Point(3, 503);
            lblHistoriaTitulo.Name = "lblHistoriaTitulo";
            lblHistoriaTitulo.Size = new Size(58, 17);
            lblHistoriaTitulo.TabIndex = 8;
            lblHistoriaTitulo.Text = "Historia:";
            // 
            // lblHistoria
            // 
            lblHistoria.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblHistoria.AutoEllipsis = true;
            lblHistoria.Font = new Font("Segoe UI", 9F);
            lblHistoria.ForeColor = Color.FromArgb(138, 133, 122);
            lblHistoria.Location = new Point(143, 497);
            lblHistoria.Name = "lblHistoria";
            lblHistoria.Size = new Size(448, 30);
            lblHistoria.TabIndex = 9;
            lblHistoria.Text = "Sin historia vinculada";
            lblHistoria.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // BugReporte
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(900, 590);
            Controls.Add(splitVertical);
            Controls.Add(pnlSelector);
            Name = "BugReporte";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reportar bug";
            pnlSelector.ResumeLayout(false);
            splitVertical.Panel1.ResumeLayout(false);
            splitVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitVertical).EndInit();
            splitVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorias).EndInit();
            pnlAcciones.ResumeLayout(false);
            tblCampos.ResumeLayout(false);
            tblCampos.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private ComboBox cboProyecto;
        private Label lblProyecto;
        private SplitContainer splitVertical;
        private Label lblListaTitulo;
        private DataGridView dgvHistorias;
        private Label lblResumen;
        private Panel pnlAcciones;
        private Button btnReportar;
        private Button btnLimpiar;
        private Button btnCancelar;
        private TableLayoutPanel tblCampos;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblTitulo;
        private TextBox txtTitulo;
        private Label lblSeveridad;
        private ComboBox cboSeveridad;
        private Label lblPasos;
        private TextBox txtPasos;
        private Label lblHistoriaTitulo;
        private Label lblHistoria;
    }
}
