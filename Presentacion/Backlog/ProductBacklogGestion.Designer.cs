namespace Presentacion.Backlog
{
    partial class ProductBacklogGestion
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
            panelEditor = new Panel();
            lblAyuda = new Label();
            btnCancelar = new Button();
            btnGuardarOrden = new Button();
            btnBajar = new Button();
            btnSubir = new Button();
            btnEstimar = new Button();
            cboPuntos = new ComboBox();
            lblPuntos = new Label();
            cboValor = new ComboBox();
            lblValor = new Label();
            dgvBacklog = new DataGridView();
            pnlSelector.SuspendLayout();
            panelEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBacklog).BeginInit();
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
            pnlSelector.Size = new Size(1000, 56);
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
            cboProyecto.Location = new Point(108, 14);
            cboProyecto.Name = "cboProyecto";
            cboProyecto.Size = new Size(320, 25);
            cboProyecto.TabIndex = 1;
            cboProyecto.SelectedIndexChanged += cboProyecto_SelectedIndexChanged;
            //
            // panelEditor
            //
            panelEditor.BackColor = Color.FromArgb(245, 241, 232);
            panelEditor.Controls.Add(lblAyuda);
            panelEditor.Controls.Add(btnCancelar);
            panelEditor.Controls.Add(btnGuardarOrden);
            panelEditor.Controls.Add(btnBajar);
            panelEditor.Controls.Add(btnSubir);
            panelEditor.Controls.Add(btnEstimar);
            panelEditor.Controls.Add(cboPuntos);
            panelEditor.Controls.Add(lblPuntos);
            panelEditor.Controls.Add(cboValor);
            panelEditor.Controls.Add(lblValor);
            panelEditor.Dock = DockStyle.Bottom;
            panelEditor.Location = new Point(0, 510);
            panelEditor.Name = "panelEditor";
            panelEditor.Size = new Size(1000, 110);
            panelEditor.TabIndex = 2;
            //
            // lblValor
            //
            lblValor.Font = new Font("Segoe UI Semibold", 9.5F);
            lblValor.ForeColor = Color.FromArgb(44, 51, 47);
            lblValor.Location = new Point(24, 20);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(45, 21);
            lblValor.TabIndex = 0;
            lblValor.Text = "Valor:";
            lblValor.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboValor
            //
            cboValor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboValor.Font = new Font("Segoe UI", 10F);
            cboValor.FormattingEnabled = true;
            cboValor.Items.AddRange(new object[] { "Alto", "Medio", "Bajo" });
            cboValor.Location = new Point(74, 16);
            cboValor.Name = "cboValor";
            cboValor.Size = new Size(100, 25);
            cboValor.TabIndex = 1;
            //
            // lblPuntos
            //
            lblPuntos.Font = new Font("Segoe UI Semibold", 9.5F);
            lblPuntos.ForeColor = Color.FromArgb(44, 51, 47);
            lblPuntos.Location = new Point(190, 20);
            lblPuntos.Name = "lblPuntos";
            lblPuntos.Size = new Size(56, 21);
            lblPuntos.TabIndex = 2;
            lblPuntos.Text = "Puntos:";
            lblPuntos.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboPuntos
            //
            cboPuntos.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPuntos.Font = new Font("Segoe UI", 10F);
            cboPuntos.FormattingEnabled = true;
            cboPuntos.Items.AddRange(new object[] { 1, 2, 3, 5, 8, 13, 21 });
            cboPuntos.Location = new Point(252, 16);
            cboPuntos.Name = "cboPuntos";
            cboPuntos.Size = new Size(66, 25);
            cboPuntos.TabIndex = 3;
            //
            // btnEstimar
            //
            btnEstimar.BackColor = Color.FromArgb(232, 151, 50);
            btnEstimar.FlatAppearance.BorderSize = 0;
            btnEstimar.FlatStyle = FlatStyle.Flat;
            btnEstimar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEstimar.ForeColor = Color.White;
            btnEstimar.Location = new Point(336, 14);
            btnEstimar.Name = "btnEstimar";
            btnEstimar.Size = new Size(100, 36);
            btnEstimar.TabIndex = 4;
            btnEstimar.Text = "Estimar";
            btnEstimar.UseVisualStyleBackColor = false;
            btnEstimar.Click += btnEstimar_Click;
            //
            // btnSubir
            //
            btnSubir.BackColor = Color.FromArgb(200, 200, 200);
            btnSubir.FlatAppearance.BorderSize = 0;
            btnSubir.FlatStyle = FlatStyle.Flat;
            btnSubir.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSubir.ForeColor = Color.FromArgb(44, 51, 47);
            btnSubir.Location = new Point(500, 14);
            btnSubir.Name = "btnSubir";
            btnSubir.Size = new Size(88, 36);
            btnSubir.TabIndex = 5;
            btnSubir.Text = "Subir";
            btnSubir.UseVisualStyleBackColor = false;
            btnSubir.Click += btnSubir_Click;
            //
            // btnBajar
            //
            btnBajar.BackColor = Color.FromArgb(200, 200, 200);
            btnBajar.FlatAppearance.BorderSize = 0;
            btnBajar.FlatStyle = FlatStyle.Flat;
            btnBajar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnBajar.ForeColor = Color.FromArgb(44, 51, 47);
            btnBajar.Location = new Point(596, 14);
            btnBajar.Name = "btnBajar";
            btnBajar.Size = new Size(88, 36);
            btnBajar.TabIndex = 6;
            btnBajar.Text = "Bajar";
            btnBajar.UseVisualStyleBackColor = false;
            btnBajar.Click += btnBajar_Click;
            //
            // btnGuardarOrden
            //
            btnGuardarOrden.BackColor = Color.FromArgb(12, 110, 99);
            btnGuardarOrden.FlatAppearance.BorderSize = 0;
            btnGuardarOrden.FlatStyle = FlatStyle.Flat;
            btnGuardarOrden.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnGuardarOrden.ForeColor = Color.White;
            btnGuardarOrden.Location = new Point(692, 14);
            btnGuardarOrden.Name = "btnGuardarOrden";
            btnGuardarOrden.Size = new Size(120, 36);
            btnGuardarOrden.TabIndex = 7;
            btnGuardarOrden.Text = "Guardar orden";
            btnGuardarOrden.UseVisualStyleBackColor = false;
            btnGuardarOrden.Click += btnGuardarOrden_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(200, 200, 200);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.FromArgb(44, 51, 47);
            btnCancelar.Location = new Point(820, 14);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 36);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // lblAyuda
            //
            lblAyuda.Font = new Font("Segoe UI", 9F);
            lblAyuda.ForeColor = Color.FromArgb(138, 133, 122);
            lblAyuda.Location = new Point(24, 62);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(884, 34);
            lblAyuda.TabIndex = 9;
            lblAyuda.Text = "Prioriza con Subir / Bajar y pulsa 'Guardar orden' para persistirlo. 'Estimar' aplica el valor de negocio y los story points a la historia seleccionada.";
            //
            // dgvBacklog
            //
            dgvBacklog.AllowUserToAddRows = false;
            dgvBacklog.AllowUserToDeleteRows = false;
            dgvBacklog.AutoGenerateColumns = false;
            dgvBacklog.BackgroundColor = Color.White;
            dgvBacklog.BorderStyle = BorderStyle.None;
            dgvBacklog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBacklog.Dock = DockStyle.Fill;
            dgvBacklog.EnableHeadersVisualStyles = false;
            dgvBacklog.Location = new Point(0, 56);
            dgvBacklog.MultiSelect = false;
            dgvBacklog.Name = "dgvBacklog";
            dgvBacklog.ReadOnly = true;
            dgvBacklog.RowHeadersVisible = false;
            dgvBacklog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBacklog.Size = new Size(1000, 454);
            dgvBacklog.TabIndex = 1;
            dgvBacklog.SelectionChanged += dgvBacklog_SelectionChanged;
            //
            // ProductBacklogGestion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(1000, 620);
            Controls.Add(dgvBacklog);
            Controls.Add(panelEditor);
            Controls.Add(pnlSelector);
            Name = "ProductBacklogGestion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Product Backlog";
            pnlSelector.ResumeLayout(false);
            panelEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBacklog).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private ComboBox cboProyecto;
        private Label lblProyecto;
        private Panel panelEditor;
        private Label lblAyuda;
        private Button btnCancelar;
        private Button btnGuardarOrden;
        private Button btnBajar;
        private Button btnSubir;
        private Button btnEstimar;
        private ComboBox cboPuntos;
        private Label lblPuntos;
        private ComboBox cboValor;
        private Label lblValor;
        private DataGridView dgvBacklog;
    }
}
