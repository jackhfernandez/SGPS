namespace Presentacion.Proyectos
{
    partial class ProyectoCreacion
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
            panelEditor = new Panel();
            btnCancelar = new Button();
            btnLimpiar = new Button();
            btnDesactivar = new Button();
            btnModificar = new Button();
            btnAgregar = new Button();
            btnNuevo = new Button();
            txtDescripcion = new TextBox();
            lblDescripcion = new Label();
            dtpFinEstimada = new DateTimePicker();
            lblFinEstimada = new Label();
            dtpInicio = new DateTimePicker();
            lblInicio = new Label();
            cboMetodologia = new ComboBox();
            lblMetodologia = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            txtClave = new TextBox();
            lblClave = new Label();
            dgvProyectos = new DataGridView();
            panelEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProyectos).BeginInit();
            SuspendLayout();
            // 
            // panelEditor
            // 
            panelEditor.BackColor = Color.FromArgb(245, 241, 232);
            panelEditor.Controls.Add(btnCancelar);
            panelEditor.Controls.Add(btnLimpiar);
            panelEditor.Controls.Add(btnDesactivar);
            panelEditor.Controls.Add(btnModificar);
            panelEditor.Controls.Add(btnAgregar);
            panelEditor.Controls.Add(btnNuevo);
            panelEditor.Controls.Add(txtDescripcion);
            panelEditor.Controls.Add(lblDescripcion);
            panelEditor.Controls.Add(dtpFinEstimada);
            panelEditor.Controls.Add(lblFinEstimada);
            panelEditor.Controls.Add(dtpInicio);
            panelEditor.Controls.Add(lblInicio);
            panelEditor.Controls.Add(cboMetodologia);
            panelEditor.Controls.Add(lblMetodologia);
            panelEditor.Controls.Add(txtNombre);
            panelEditor.Controls.Add(lblNombre);
            panelEditor.Controls.Add(txtClave);
            panelEditor.Controls.Add(lblClave);
            panelEditor.Dock = DockStyle.Bottom;
            panelEditor.Location = new Point(0, 410);
            panelEditor.Name = "panelEditor";
            panelEditor.Size = new Size(860, 150);
            panelEditor.TabIndex = 0;
            // 
            // lblClave
            // 
            lblClave.Font = new Font("Segoe UI Semibold", 9.5F);
            lblClave.ForeColor = Color.FromArgb(44, 51, 47);
            lblClave.Location = new Point(24, 16);
            lblClave.Name = "lblClave";
            lblClave.Size = new Size(80, 21);
            lblClave.TabIndex = 0;
            lblClave.Text = "Clave:";
            lblClave.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtClave
            // 
            txtClave.BorderStyle = BorderStyle.FixedSingle;
            txtClave.Font = new Font("Segoe UI", 10F);
            txtClave.Location = new Point(110, 12);
            txtClave.MaxLength = 10;
            txtClave.Name = "txtClave";
            txtClave.Size = new Size(120, 25);
            txtClave.TabIndex = 1;
            txtClave.CharacterCasing = CharacterCasing.Upper;
            // 
            // lblNombre
            // 
            lblNombre.Font = new Font("Segoe UI Semibold", 9.5F);
            lblNombre.ForeColor = Color.FromArgb(44, 51, 47);
            lblNombre.Location = new Point(250, 16);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(80, 21);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNombre
            // 
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.Location = new Point(336, 12);
            txtNombre.MaxLength = 150;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(210, 25);
            txtNombre.TabIndex = 3;
            // 
            // lblMetodologia
            // 
            lblMetodologia.Font = new Font("Segoe UI Semibold", 9.5F);
            lblMetodologia.ForeColor = Color.FromArgb(44, 51, 47);
            lblMetodologia.Location = new Point(24, 54);
            lblMetodologia.Name = "lblMetodologia";
            lblMetodologia.Size = new Size(80, 21);
            lblMetodologia.TabIndex = 4;
            lblMetodologia.Text = "Metodología:";
            lblMetodologia.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboMetodologia
            // 
            cboMetodologia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMetodologia.Font = new Font("Segoe UI", 10F);
            cboMetodologia.FormattingEnabled = true;
            cboMetodologia.Items.AddRange(new object[] { "Scrum", "Kanban", "Híbrido" });
            cboMetodologia.Location = new Point(110, 50);
            cboMetodologia.Name = "cboMetodologia";
            cboMetodologia.Size = new Size(120, 25);
            cboMetodologia.TabIndex = 5;
            // 
            // lblInicio
            // 
            lblInicio.Font = new Font("Segoe UI Semibold", 9.5F);
            lblInicio.ForeColor = Color.FromArgb(44, 51, 47);
            lblInicio.Location = new Point(250, 54);
            lblInicio.Name = "lblInicio";
            lblInicio.Size = new Size(80, 21);
            lblInicio.TabIndex = 6;
            lblInicio.Text = "Inicio:";
            lblInicio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpInicio
            // 
            dtpInicio.CustomFormat = "dd/MM/yyyy";
            dtpInicio.Font = new Font("Segoe UI", 10F);
            dtpInicio.Format = DateTimePickerFormat.Custom;
            dtpInicio.Location = new Point(336, 50);
            dtpInicio.Name = "dtpInicio";
            dtpInicio.Size = new Size(150, 25);
            dtpInicio.TabIndex = 7;
            // 
            // lblFinEstimada
            // 
            lblFinEstimada.Font = new Font("Segoe UI Semibold", 9.5F);
            lblFinEstimada.ForeColor = Color.FromArgb(44, 51, 47);
            lblFinEstimada.Location = new Point(24, 92);
            lblFinEstimada.Name = "lblFinEstimada";
            lblFinEstimada.Size = new Size(80, 21);
            lblFinEstimada.TabIndex = 8;
            lblFinEstimada.Text = "Fin estimado:";
            lblFinEstimada.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpFinEstimada
            // 
            dtpFinEstimada.Checked = false;
            dtpFinEstimada.CustomFormat = "dd/MM/yyyy";
            dtpFinEstimada.Font = new Font("Segoe UI", 10F);
            dtpFinEstimada.Format = DateTimePickerFormat.Custom;
            dtpFinEstimada.Location = new Point(110, 88);
            dtpFinEstimada.Name = "dtpFinEstimada";
            dtpFinEstimada.ShowCheckBox = true;
            dtpFinEstimada.Size = new Size(150, 25);
            dtpFinEstimada.TabIndex = 9;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Font = new Font("Segoe UI Semibold", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(44, 51, 47);
            lblDescripcion.Location = new Point(250, 92);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(80, 21);
            lblDescripcion.TabIndex = 10;
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescripcion
            // 
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Font = new Font("Segoe UI", 10F);
            txtDescripcion.Location = new Point(336, 88);
            txtDescripcion.MaxLength = 4000;
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(210, 52);
            txtDescripcion.TabIndex = 11;
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
            btnNuevo.TabIndex = 12;
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
            btnAgregar.TabIndex = 13;
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
            btnModificar.TabIndex = 14;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnDesactivar
            // 
            btnDesactivar.BackColor = Color.FromArgb(178, 52, 43);
            btnDesactivar.FlatAppearance.BorderSize = 0;
            btnDesactivar.FlatStyle = FlatStyle.Flat;
            btnDesactivar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDesactivar.ForeColor = Color.White;
            btnDesactivar.Location = new Point(560, 74);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(88, 36);
            btnDesactivar.TabIndex = 15;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = false;
            btnDesactivar.Click += btnDesactivar_Click;
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
            btnLimpiar.TabIndex = 16;
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
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // dgvProyectos
            // 
            dgvProyectos.AllowUserToAddRows = false;
            dgvProyectos.AllowUserToDeleteRows = false;
            dgvProyectos.AutoGenerateColumns = false;
            dgvProyectos.BackgroundColor = Color.White;
            dgvProyectos.BorderStyle = BorderStyle.None;
            dgvProyectos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProyectos.Dock = DockStyle.Fill;
            dgvProyectos.EnableHeadersVisualStyles = false;
            dgvProyectos.Location = new Point(0, 0);
            dgvProyectos.MultiSelect = false;
            dgvProyectos.Name = "dgvProyectos";
            dgvProyectos.ReadOnly = true;
            dgvProyectos.RowHeadersVisible = false;
            dgvProyectos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProyectos.Size = new Size(860, 410);
            dgvProyectos.TabIndex = 1;
            dgvProyectos.SelectionChanged += dgvProyectos_SelectionChanged;
            // 
            // ProyectoCreacion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(860, 560);
            Controls.Add(dgvProyectos);
            Controls.Add(panelEditor);
            Name = "ProyectoCreacion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de proyectos";
            panelEditor.ResumeLayout(false);
            panelEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProyectos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelEditor;
        private Button btnCancelar;
        private Button btnLimpiar;
        private Button btnDesactivar;
        private Button btnModificar;
        private Button btnAgregar;
        private Button btnNuevo;
        private TextBox txtDescripcion;
        private Label lblDescripcion;
        private DateTimePicker dtpFinEstimada;
        private Label lblFinEstimada;
        private DateTimePicker dtpInicio;
        private Label lblInicio;
        private ComboBox cboMetodologia;
        private Label lblMetodologia;
        private TextBox txtNombre;
        private Label lblNombre;
        private TextBox txtClave;
        private Label lblClave;
        private DataGridView dgvProyectos;
    }
}
