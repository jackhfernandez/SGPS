namespace Presentacion.Backlog
{
    partial class EpicGestion
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
            pnlColor = new Panel();
            txtColor = new TextBox();
            lblColor = new Label();
            txtDescripcion = new TextBox();
            lblDescripcion = new Label();
            txtTitulo = new TextBox();
            lblTitulo = new Label();
            dgvEpics = new DataGridView();
            pnlSelector.SuspendLayout();
            panelEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEpics).BeginInit();
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
            pnlSelector.Size = new Size(860, 56);
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
            // panelEditor
            //
            panelEditor.BackColor = Color.FromArgb(245, 241, 232);
            panelEditor.Controls.Add(btnCancelar);
            panelEditor.Controls.Add(btnLimpiar);
            panelEditor.Controls.Add(btnEliminar);
            panelEditor.Controls.Add(btnModificar);
            panelEditor.Controls.Add(btnAgregar);
            panelEditor.Controls.Add(btnNuevo);
            panelEditor.Controls.Add(pnlColor);
            panelEditor.Controls.Add(txtColor);
            panelEditor.Controls.Add(lblColor);
            panelEditor.Controls.Add(txtDescripcion);
            panelEditor.Controls.Add(lblDescripcion);
            panelEditor.Controls.Add(txtTitulo);
            panelEditor.Controls.Add(lblTitulo);
            panelEditor.Dock = DockStyle.Bottom;
            panelEditor.Location = new Point(0, 410);
            panelEditor.Name = "panelEditor";
            panelEditor.Size = new Size(860, 150);
            panelEditor.TabIndex = 2;
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTitulo.ForeColor = Color.FromArgb(44, 51, 47);
            lblTitulo.Location = new Point(24, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(80, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Título:";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtTitulo
            //
            txtTitulo.BorderStyle = BorderStyle.FixedSingle;
            txtTitulo.Font = new Font("Segoe UI", 10F);
            txtTitulo.Location = new Point(110, 12);
            txtTitulo.MaxLength = 200;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(436, 25);
            txtTitulo.TabIndex = 1;
            //
            // lblColor
            //
            lblColor.Font = new Font("Segoe UI Semibold", 9.5F);
            lblColor.ForeColor = Color.FromArgb(44, 51, 47);
            lblColor.Location = new Point(24, 54);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(80, 21);
            lblColor.TabIndex = 2;
            lblColor.Text = "Color:";
            lblColor.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtColor
            //
            txtColor.BorderStyle = BorderStyle.FixedSingle;
            txtColor.CharacterCasing = CharacterCasing.Upper;
            txtColor.Font = new Font("Segoe UI", 10F);
            txtColor.Location = new Point(110, 50);
            txtColor.MaxLength = 7;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(100, 25);
            txtColor.TabIndex = 3;
            txtColor.TextChanged += txtColor_TextChanged;
            //
            // pnlColor
            //
            pnlColor.BackColor = Color.FromArgb(49, 130, 206);
            pnlColor.BorderStyle = BorderStyle.FixedSingle;
            pnlColor.Location = new Point(218, 50);
            pnlColor.Name = "pnlColor";
            pnlColor.Size = new Size(28, 25);
            pnlColor.TabIndex = 4;
            //
            // lblDescripcion
            //
            lblDescripcion.Font = new Font("Segoe UI Semibold", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(44, 51, 47);
            lblDescripcion.Location = new Point(24, 92);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(84, 21);
            lblDescripcion.TabIndex = 5;
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtDescripcion
            //
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Font = new Font("Segoe UI", 10F);
            txtDescripcion.Location = new Point(110, 88);
            txtDescripcion.MaxLength = 4000;
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(436, 52);
            txtDescripcion.TabIndex = 6;
            //
            // btnNuevo
            //
            btnNuevo.BackColor = Color.FromArgb(12, 110, 99);
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(560, 14);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(88, 36);
            btnNuevo.TabIndex = 7;
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
            btnAgregar.Location = new Point(656, 14);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 36);
            btnAgregar.TabIndex = 8;
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
            btnModificar.Location = new Point(752, 14);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 36);
            btnModificar.TabIndex = 9;
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
            btnEliminar.Location = new Point(560, 74);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(88, 36);
            btnEliminar.TabIndex = 10;
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
            btnLimpiar.Location = new Point(656, 74);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(88, 36);
            btnLimpiar.TabIndex = 11;
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
            btnCancelar.Location = new Point(752, 74);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 36);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // dgvEpics
            //
            dgvEpics.AllowUserToAddRows = false;
            dgvEpics.AllowUserToDeleteRows = false;
            dgvEpics.AutoGenerateColumns = false;
            dgvEpics.BackgroundColor = Color.White;
            dgvEpics.BorderStyle = BorderStyle.None;
            dgvEpics.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEpics.Dock = DockStyle.Fill;
            dgvEpics.EnableHeadersVisualStyles = false;
            dgvEpics.Location = new Point(0, 56);
            dgvEpics.MultiSelect = false;
            dgvEpics.Name = "dgvEpics";
            dgvEpics.ReadOnly = true;
            dgvEpics.RowHeadersVisible = false;
            dgvEpics.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEpics.Size = new Size(860, 354);
            dgvEpics.TabIndex = 1;
            dgvEpics.SelectionChanged += dgvEpics_SelectionChanged;
            //
            // EpicGestion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(860, 560);
            Controls.Add(dgvEpics);
            Controls.Add(panelEditor);
            Controls.Add(pnlSelector);
            Name = "EpicGestion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de epics";
            pnlSelector.ResumeLayout(false);
            panelEditor.ResumeLayout(false);
            panelEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEpics).EndInit();
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
        private Panel pnlColor;
        private TextBox txtColor;
        private Label lblColor;
        private TextBox txtDescripcion;
        private Label lblDescripcion;
        private TextBox txtTitulo;
        private Label lblTitulo;
        private DataGridView dgvEpics;
    }
}
