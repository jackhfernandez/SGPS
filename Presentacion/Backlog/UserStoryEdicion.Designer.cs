namespace Presentacion.Backlog
{
    partial class UserStoryEdicion
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
            btnCancelar = new Button();
            btnLimpiar = new Button();
            btnEliminar = new Button();
            btnModificar = new Button();
            btnAgregar = new Button();
            btnNuevo = new Button();
            txtCriterios = new TextBox();
            lblCriterios = new Label();
            cboEstado = new ComboBox();
            lblEstado = new Label();
            cboPuntos = new ComboBox();
            lblPuntos = new Label();
            cboValor = new ComboBox();
            lblValor = new Label();
            cboEpic = new ComboBox();
            lblEpic = new Label();
            txtPara = new TextBox();
            lblPara = new Label();
            txtQuiero = new TextBox();
            lblQuiero = new Label();
            txtComo = new TextBox();
            lblComo = new Label();
            txtTitulo = new TextBox();
            lblTitulo = new Label();
            txtCodigo = new TextBox();
            lblCodigo = new Label();
            dgvHistorias = new DataGridView();
            pnlSelector.SuspendLayout();
            panelEditor.SuspendLayout();
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
            panelEditor.Controls.Add(btnCancelar);
            panelEditor.Controls.Add(btnLimpiar);
            panelEditor.Controls.Add(btnEliminar);
            panelEditor.Controls.Add(btnModificar);
            panelEditor.Controls.Add(btnAgregar);
            panelEditor.Controls.Add(btnNuevo);
            panelEditor.Controls.Add(txtCriterios);
            panelEditor.Controls.Add(lblCriterios);
            panelEditor.Controls.Add(cboEstado);
            panelEditor.Controls.Add(lblEstado);
            panelEditor.Controls.Add(cboPuntos);
            panelEditor.Controls.Add(lblPuntos);
            panelEditor.Controls.Add(cboValor);
            panelEditor.Controls.Add(lblValor);
            panelEditor.Controls.Add(cboEpic);
            panelEditor.Controls.Add(lblEpic);
            panelEditor.Controls.Add(txtPara);
            panelEditor.Controls.Add(lblPara);
            panelEditor.Controls.Add(txtQuiero);
            panelEditor.Controls.Add(lblQuiero);
            panelEditor.Controls.Add(txtComo);
            panelEditor.Controls.Add(lblComo);
            panelEditor.Controls.Add(txtTitulo);
            panelEditor.Controls.Add(lblTitulo);
            panelEditor.Controls.Add(txtCodigo);
            panelEditor.Controls.Add(lblCodigo);
            panelEditor.Dock = DockStyle.Bottom;
            panelEditor.Location = new Point(0, 440);
            panelEditor.Name = "panelEditor";
            panelEditor.Size = new Size(1000, 260);
            panelEditor.TabIndex = 2;
            //
            // lblCodigo
            //
            lblCodigo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblCodigo.ForeColor = Color.FromArgb(44, 51, 47);
            lblCodigo.Location = new Point(24, 16);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(80, 21);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código:";
            lblCodigo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtCodigo
            //
            txtCodigo.BackColor = Color.FromArgb(236, 231, 219);
            txtCodigo.BorderStyle = BorderStyle.FixedSingle;
            txtCodigo.Font = new Font("Segoe UI", 10F);
            txtCodigo.Location = new Point(108, 12);
            txtCodigo.MaxLength = 20;
            txtCodigo.Name = "txtCodigo";
            txtCodigo.ReadOnly = true;
            txtCodigo.Size = new Size(110, 25);
            txtCodigo.TabIndex = 1;
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTitulo.ForeColor = Color.FromArgb(44, 51, 47);
            lblTitulo.Location = new Point(232, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(50, 21);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Título:";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtTitulo
            //
            txtTitulo.BorderStyle = BorderStyle.FixedSingle;
            txtTitulo.Font = new Font("Segoe UI", 10F);
            txtTitulo.Location = new Point(288, 12);
            txtTitulo.MaxLength = 200;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(470, 25);
            txtTitulo.TabIndex = 3;
            //
            // lblComo
            //
            lblComo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblComo.ForeColor = Color.FromArgb(44, 51, 47);
            lblComo.Location = new Point(24, 54);
            lblComo.Name = "lblComo";
            lblComo.Size = new Size(80, 21);
            lblComo.TabIndex = 4;
            lblComo.Text = "Como:";
            lblComo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtComo
            //
            txtComo.BorderStyle = BorderStyle.FixedSingle;
            txtComo.Font = new Font("Segoe UI", 10F);
            txtComo.Location = new Point(108, 50);
            txtComo.MaxLength = 100;
            txtComo.Name = "txtComo";
            txtComo.PlaceholderText = "tipo de usuario";
            txtComo.Size = new Size(170, 25);
            txtComo.TabIndex = 5;
            //
            // lblQuiero
            //
            lblQuiero.Font = new Font("Segoe UI Semibold", 9.5F);
            lblQuiero.ForeColor = Color.FromArgb(44, 51, 47);
            lblQuiero.Location = new Point(288, 54);
            lblQuiero.Name = "lblQuiero";
            lblQuiero.Size = new Size(55, 21);
            lblQuiero.TabIndex = 6;
            lblQuiero.Text = "Quiero:";
            lblQuiero.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtQuiero
            //
            txtQuiero.BorderStyle = BorderStyle.FixedSingle;
            txtQuiero.Font = new Font("Segoe UI", 10F);
            txtQuiero.Location = new Point(348, 50);
            txtQuiero.MaxLength = 255;
            txtQuiero.Name = "txtQuiero";
            txtQuiero.PlaceholderText = "funcionalidad";
            txtQuiero.Size = new Size(410, 25);
            txtQuiero.TabIndex = 7;
            //
            // lblPara
            //
            lblPara.Font = new Font("Segoe UI Semibold", 9.5F);
            lblPara.ForeColor = Color.FromArgb(44, 51, 47);
            lblPara.Location = new Point(24, 92);
            lblPara.Name = "lblPara";
            lblPara.Size = new Size(80, 21);
            lblPara.TabIndex = 8;
            lblPara.Text = "Para:";
            lblPara.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtPara
            //
            txtPara.BorderStyle = BorderStyle.FixedSingle;
            txtPara.Font = new Font("Segoe UI", 10F);
            txtPara.Location = new Point(108, 88);
            txtPara.MaxLength = 255;
            txtPara.Name = "txtPara";
            txtPara.PlaceholderText = "beneficio";
            txtPara.Size = new Size(650, 25);
            txtPara.TabIndex = 9;
            //
            // lblEpic
            //
            lblEpic.Font = new Font("Segoe UI Semibold", 9.5F);
            lblEpic.ForeColor = Color.FromArgb(44, 51, 47);
            lblEpic.Location = new Point(24, 130);
            lblEpic.Name = "lblEpic";
            lblEpic.Size = new Size(80, 21);
            lblEpic.TabIndex = 10;
            lblEpic.Text = "Epic:";
            lblEpic.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboEpic
            //
            cboEpic.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEpic.Font = new Font("Segoe UI", 10F);
            cboEpic.FormattingEnabled = true;
            cboEpic.Location = new Point(108, 126);
            cboEpic.Name = "cboEpic";
            cboEpic.Size = new Size(170, 25);
            cboEpic.TabIndex = 11;
            //
            // lblValor
            //
            lblValor.Font = new Font("Segoe UI Semibold", 9.5F);
            lblValor.ForeColor = Color.FromArgb(44, 51, 47);
            lblValor.Location = new Point(288, 130);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(45, 21);
            lblValor.TabIndex = 12;
            lblValor.Text = "Valor:";
            lblValor.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboValor
            //
            cboValor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboValor.Font = new Font("Segoe UI", 10F);
            cboValor.FormattingEnabled = true;
            cboValor.Items.AddRange(new object[] { "Alto", "Medio", "Bajo" });
            cboValor.Location = new Point(338, 126);
            cboValor.Name = "cboValor";
            cboValor.Size = new Size(90, 25);
            cboValor.TabIndex = 13;
            //
            // lblPuntos
            //
            lblPuntos.Font = new Font("Segoe UI Semibold", 9.5F);
            lblPuntos.ForeColor = Color.FromArgb(44, 51, 47);
            lblPuntos.Location = new Point(438, 130);
            lblPuntos.Name = "lblPuntos";
            lblPuntos.Size = new Size(56, 21);
            lblPuntos.TabIndex = 14;
            lblPuntos.Text = "Puntos:";
            lblPuntos.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboPuntos
            //
            cboPuntos.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPuntos.Font = new Font("Segoe UI", 10F);
            cboPuntos.FormattingEnabled = true;
            cboPuntos.Items.AddRange(new object[] { 1, 2, 3, 5, 8, 13, 21 });
            cboPuntos.Location = new Point(498, 126);
            cboPuntos.Name = "cboPuntos";
            cboPuntos.Size = new Size(58, 25);
            cboPuntos.TabIndex = 15;
            //
            // lblEstado
            //
            lblEstado.Font = new Font("Segoe UI Semibold", 9.5F);
            lblEstado.ForeColor = Color.FromArgb(44, 51, 47);
            lblEstado.Location = new Point(562, 130);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(55, 21);
            lblEstado.TabIndex = 16;
            lblEstado.Text = "Estado:";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cboEstado
            //
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Font = new Font("Segoe UI", 10F);
            cboEstado.FormattingEnabled = true;
            cboEstado.Items.AddRange(new object[] { "To Do", "In Progress", "In Testing", "Done" });
            cboEstado.Location = new Point(620, 126);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(138, 25);
            cboEstado.TabIndex = 17;
            //
            // lblCriterios
            //
            lblCriterios.Font = new Font("Segoe UI Semibold", 9.5F);
            lblCriterios.ForeColor = Color.FromArgb(44, 51, 47);
            lblCriterios.Location = new Point(24, 168);
            lblCriterios.Name = "lblCriterios";
            lblCriterios.Size = new Size(80, 21);
            lblCriterios.TabIndex = 18;
            lblCriterios.Text = "Criterios:";
            lblCriterios.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtCriterios
            //
            txtCriterios.BorderStyle = BorderStyle.FixedSingle;
            txtCriterios.Font = new Font("Segoe UI", 10F);
            txtCriterios.Location = new Point(108, 164);
            txtCriterios.MaxLength = 4000;
            txtCriterios.Multiline = true;
            txtCriterios.Name = "txtCriterios";
            txtCriterios.PlaceholderText = "Criterios de aceptación";
            txtCriterios.ScrollBars = ScrollBars.Vertical;
            txtCriterios.Size = new Size(650, 80);
            txtCriterios.TabIndex = 19;
            //
            // btnNuevo
            //
            btnNuevo.BackColor = Color.FromArgb(12, 110, 99);
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(780, 14);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(88, 36);
            btnNuevo.TabIndex = 20;
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
            btnAgregar.Location = new Point(876, 14);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 36);
            btnAgregar.TabIndex = 21;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAgregar_Click;
            //
            // btnModificar
            //
            btnModificar.BackColor = Color.FromArgb(232, 151, 50);
            btnModificar.FlatAppearance.BorderSize = 0;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(780, 58);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 36);
            btnModificar.TabIndex = 22;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            //
            // btnEliminar
            //
            btnEliminar.BackColor = Color.FromArgb(178, 52, 43);
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(876, 58);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(88, 36);
            btnEliminar.TabIndex = 23;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            //
            // btnLimpiar
            //
            btnLimpiar.BackColor = Color.FromArgb(200, 200, 200);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLimpiar.ForeColor = Color.FromArgb(44, 51, 47);
            btnLimpiar.Location = new Point(780, 102);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(88, 36);
            btnLimpiar.TabIndex = 24;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(200, 200, 200);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.FromArgb(44, 51, 47);
            btnCancelar.Location = new Point(876, 102);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 36);
            btnCancelar.TabIndex = 25;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // dgvHistorias
            //
            dgvHistorias.AllowUserToAddRows = false;
            dgvHistorias.AllowUserToDeleteRows = false;
            dgvHistorias.AutoGenerateColumns = false;
            dgvHistorias.BackgroundColor = Color.White;
            dgvHistorias.BorderStyle = BorderStyle.None;
            dgvHistorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorias.Dock = DockStyle.Fill;
            dgvHistorias.EnableHeadersVisualStyles = false;
            dgvHistorias.Location = new Point(0, 56);
            dgvHistorias.MultiSelect = false;
            dgvHistorias.Name = "dgvHistorias";
            dgvHistorias.ReadOnly = true;
            dgvHistorias.RowHeadersVisible = false;
            dgvHistorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorias.Size = new Size(1000, 384);
            dgvHistorias.TabIndex = 1;
            dgvHistorias.SelectionChanged += dgvHistorias_SelectionChanged;
            //
            // UserStoryEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(1000, 700);
            Controls.Add(dgvHistorias);
            Controls.Add(panelEditor);
            Controls.Add(pnlSelector);
            Name = "UserStoryEdicion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Historias de usuario";
            pnlSelector.ResumeLayout(false);
            panelEditor.ResumeLayout(false);
            panelEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorias).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private ComboBox cboProyecto;
        private Label lblProyecto;
        private Panel panelEditor;
        private Button btnCancelar;
        private Button btnLimpiar;
        private Button btnEliminar;
        private Button btnModificar;
        private Button btnAgregar;
        private Button btnNuevo;
        private TextBox txtCriterios;
        private Label lblCriterios;
        private ComboBox cboEstado;
        private Label lblEstado;
        private ComboBox cboPuntos;
        private Label lblPuntos;
        private ComboBox cboValor;
        private Label lblValor;
        private ComboBox cboEpic;
        private Label lblEpic;
        private TextBox txtPara;
        private Label lblPara;
        private TextBox txtQuiero;
        private Label lblQuiero;
        private TextBox txtComo;
        private Label lblComo;
        private TextBox txtTitulo;
        private Label lblTitulo;
        private TextBox txtCodigo;
        private Label lblCodigo;
        private DataGridView dgvHistorias;
    }
}
