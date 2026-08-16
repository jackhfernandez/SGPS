namespace Presentacion.Proyectos
{
    partial class ProyectoMiembros
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
            lblAyuda = new Label();
            cboProyectos = new ComboBox();
            lblProyecto = new Label();
            pnlEditor = new Panel();
            lblAyudaEditor = new Label();
            cboRol = new ComboBox();
            lblRol = new Label();
            btnCancelar = new Button();
            btnLimpiar = new Button();
            btnQuitar = new Button();
            btnModificar = new Button();
            btnAsignar = new Button();
            btnNuevo = new Button();
            pnlDisponibles = new Panel();
            dgvDisponibles = new DataGridView();
            lblTituloDisponibles = new Label();
            pnlSeparador = new Panel();
            pnlMiembros = new Panel();
            dgvMiembros = new DataGridView();
            lblTituloMiembros = new Label();
            pnlSelector.SuspendLayout();
            pnlEditor.SuspendLayout();
            pnlDisponibles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).BeginInit();
            pnlMiembros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMiembros).BeginInit();
            SuspendLayout();
            // 
            // pnlSelector
            // 
            pnlSelector.BackColor = Color.FromArgb(245, 241, 232);
            pnlSelector.Controls.Add(lblAyuda);
            pnlSelector.Controls.Add(cboProyectos);
            pnlSelector.Controls.Add(lblProyecto);
            pnlSelector.Dock = DockStyle.Top;
            pnlSelector.Location = new Point(0, 0);
            pnlSelector.Name = "pnlSelector";
            pnlSelector.Size = new Size(920, 68);
            pnlSelector.TabIndex = 0;
            // 
            // lblProyecto
            // 
            lblProyecto.Font = new Font("Segoe UI Semibold", 9.5F);
            lblProyecto.ForeColor = Color.FromArgb(44, 51, 47);
            lblProyecto.Location = new Point(24, 24);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(70, 21);
            lblProyecto.TabIndex = 0;
            lblProyecto.Text = "Proyecto:";
            lblProyecto.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboProyectos
            // 
            cboProyectos.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyectos.Font = new Font("Segoe UI", 10F);
            cboProyectos.FormattingEnabled = true;
            cboProyectos.Location = new Point(100, 20);
            cboProyectos.Name = "cboProyectos";
            cboProyectos.Size = new Size(430, 25);
            cboProyectos.TabIndex = 1;
            cboProyectos.SelectedIndexChanged += cboProyectos_SelectedIndexChanged;
            // 
            // lblAyuda
            // 
            lblAyuda.Font = new Font("Segoe UI", 8.5F);
            lblAyuda.ForeColor = Color.FromArgb(100, 100, 100);
            lblAyuda.Location = new Point(100, 46);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(700, 16);
            lblAyuda.TabIndex = 2;
            lblAyuda.Text = "Elige un proyecto para ver y gestionar su equipo.";
            // 
            // pnlEditor
            // 
            pnlEditor.BackColor = Color.FromArgb(245, 241, 232);
            pnlEditor.Controls.Add(lblAyudaEditor);
            pnlEditor.Controls.Add(cboRol);
            pnlEditor.Controls.Add(lblRol);
            pnlEditor.Controls.Add(btnCancelar);
            pnlEditor.Controls.Add(btnLimpiar);
            pnlEditor.Controls.Add(btnQuitar);
            pnlEditor.Controls.Add(btnModificar);
            pnlEditor.Controls.Add(btnAsignar);
            pnlEditor.Controls.Add(btnNuevo);
            pnlEditor.Dock = DockStyle.Bottom;
            pnlEditor.Location = new Point(0, 460);
            pnlEditor.Name = "pnlEditor";
            pnlEditor.Size = new Size(920, 140);
            pnlEditor.TabIndex = 1;
            // 
            // lblRol
            // 
            lblRol.Font = new Font("Segoe UI Semibold", 9.5F);
            lblRol.ForeColor = Color.FromArgb(44, 51, 47);
            lblRol.Location = new Point(24, 24);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(130, 21);
            lblRol.TabIndex = 0;
            lblRol.Text = "Rol para asignar:";
            lblRol.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboRol
            // 
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Font = new Font("Segoe UI", 10F);
            cboRol.FormattingEnabled = true;
            cboRol.Items.AddRange(new object[] { "PO", "SM", "Developer", "QA", "Cliente" });
            cboRol.Location = new Point(160, 20);
            cboRol.Name = "cboRol";
            cboRol.Size = new Size(190, 25);
            cboRol.TabIndex = 1;
            // 
            // lblAyudaEditor
            // 
            lblAyudaEditor.Font = new Font("Segoe UI", 8.5F);
            lblAyudaEditor.ForeColor = Color.FromArgb(100, 100, 100);
            lblAyudaEditor.Location = new Point(24, 58);
            lblAyudaEditor.Name = "lblAyudaEditor";
            lblAyudaEditor.Size = new Size(500, 60);
            lblAyudaEditor.TabIndex = 2;
            lblAyudaEditor.Text = "Selecciona un usuario del equipo disponible, elige un rol y presiona 'Asignar'.\r\nPara cambiar el rol o quitar a un miembro, selecciónalo en la lista izquierda.";
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.FromArgb(12, 110, 99);
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(560, 18);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(88, 40);
            btnNuevo.TabIndex = 3;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnAsignar
            // 
            btnAsignar.BackColor = Color.FromArgb(12, 110, 99);
            btnAsignar.FlatAppearance.BorderSize = 0;
            btnAsignar.FlatStyle = FlatStyle.Flat;
            btnAsignar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAsignar.ForeColor = Color.White;
            btnAsignar.Location = new Point(656, 18);
            btnAsignar.Name = "btnAsignar";
            btnAsignar.Size = new Size(88, 40);
            btnAsignar.TabIndex = 4;
            btnAsignar.Text = "Asignar";
            btnAsignar.UseVisualStyleBackColor = false;
            btnAsignar.Click += btnAsignar_Click;
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.FromArgb(232, 151, 50);
            btnModificar.FlatAppearance.BorderSize = 0;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(752, 18);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 40);
            btnModificar.TabIndex = 5;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnQuitar
            // 
            btnQuitar.BackColor = Color.FromArgb(178, 52, 43);
            btnQuitar.FlatAppearance.BorderSize = 0;
            btnQuitar.FlatStyle = FlatStyle.Flat;
            btnQuitar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnQuitar.ForeColor = Color.White;
            btnQuitar.Location = new Point(560, 76);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(88, 40);
            btnQuitar.TabIndex = 6;
            btnQuitar.Text = "Quitar";
            btnQuitar.UseVisualStyleBackColor = false;
            btnQuitar.Click += btnQuitar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(200, 200, 200);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLimpiar.ForeColor = Color.FromArgb(44, 51, 47);
            btnLimpiar.Location = new Point(656, 76);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(88, 40);
            btnLimpiar.TabIndex = 7;
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
            btnCancelar.Location = new Point(752, 76);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 40);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // pnlDisponibles
            // 
            pnlDisponibles.Controls.Add(dgvDisponibles);
            pnlDisponibles.Controls.Add(lblTituloDisponibles);
            pnlDisponibles.Dock = DockStyle.Fill;
            pnlDisponibles.Location = new Point(456, 68);
            pnlDisponibles.Name = "pnlDisponibles";
            pnlDisponibles.Size = new Size(464, 392);
            pnlDisponibles.TabIndex = 3;
            // 
            // lblTituloDisponibles
            // 
            lblTituloDisponibles.BackColor = Color.FromArgb(12, 110, 99);
            lblTituloDisponibles.Dock = DockStyle.Top;
            lblTituloDisponibles.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTituloDisponibles.ForeColor = Color.White;
            lblTituloDisponibles.Location = new Point(0, 0);
            lblTituloDisponibles.Name = "lblTituloDisponibles";
            lblTituloDisponibles.Padding = new Padding(12, 0, 0, 0);
            lblTituloDisponibles.Size = new Size(464, 36);
            lblTituloDisponibles.TabIndex = 1;
            lblTituloDisponibles.Text = "EQUIPO DISPONIBLE";
            lblTituloDisponibles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvDisponibles
            // 
            dgvDisponibles.AllowUserToAddRows = false;
            dgvDisponibles.AllowUserToDeleteRows = false;
            dgvDisponibles.AutoGenerateColumns = false;
            dgvDisponibles.BackgroundColor = Color.White;
            dgvDisponibles.BorderStyle = BorderStyle.None;
            dgvDisponibles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDisponibles.Dock = DockStyle.Fill;
            dgvDisponibles.Location = new Point(0, 36);
            dgvDisponibles.MultiSelect = false;
            dgvDisponibles.Name = "dgvDisponibles";
            dgvDisponibles.ReadOnly = true;
            dgvDisponibles.RowHeadersVisible = false;
            dgvDisponibles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDisponibles.Size = new Size(464, 356);
            dgvDisponibles.TabIndex = 0;
            dgvDisponibles.SelectionChanged += dgvDisponibles_SelectionChanged;
            // 
            // pnlSeparador
            // 
            pnlSeparador.BackColor = Color.FromArgb(12, 110, 99);
            pnlSeparador.Dock = DockStyle.Left;
            pnlSeparador.Location = new Point(452, 68);
            pnlSeparador.Name = "pnlSeparador";
            pnlSeparador.Size = new Size(4, 392);
            pnlSeparador.TabIndex = 4;
            // 
            // pnlMiembros
            // 
            pnlMiembros.Controls.Add(dgvMiembros);
            pnlMiembros.Controls.Add(lblTituloMiembros);
            pnlMiembros.Dock = DockStyle.Left;
            pnlMiembros.Location = new Point(0, 68);
            pnlMiembros.Name = "pnlMiembros";
            pnlMiembros.Size = new Size(452, 392);
            pnlMiembros.TabIndex = 5;
            // 
            // lblTituloMiembros
            // 
            lblTituloMiembros.BackColor = Color.FromArgb(12, 110, 99);
            lblTituloMiembros.Dock = DockStyle.Top;
            lblTituloMiembros.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTituloMiembros.ForeColor = Color.White;
            lblTituloMiembros.Location = new Point(0, 0);
            lblTituloMiembros.Name = "lblTituloMiembros";
            lblTituloMiembros.Padding = new Padding(12, 0, 0, 0);
            lblTituloMiembros.Size = new Size(452, 36);
            lblTituloMiembros.TabIndex = 1;
            lblTituloMiembros.Text = "MIEMBROS DEL PROYECTO";
            lblTituloMiembros.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvMiembros
            // 
            dgvMiembros.AllowUserToAddRows = false;
            dgvMiembros.AllowUserToDeleteRows = false;
            dgvMiembros.AutoGenerateColumns = false;
            dgvMiembros.BackgroundColor = Color.White;
            dgvMiembros.BorderStyle = BorderStyle.None;
            dgvMiembros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMiembros.Dock = DockStyle.Fill;
            dgvMiembros.Location = new Point(0, 36);
            dgvMiembros.MultiSelect = false;
            dgvMiembros.Name = "dgvMiembros";
            dgvMiembros.ReadOnly = true;
            dgvMiembros.RowHeadersVisible = false;
            dgvMiembros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMiembros.Size = new Size(452, 356);
            dgvMiembros.TabIndex = 0;
            dgvMiembros.SelectionChanged += dgvMiembros_SelectionChanged;
            // 
            // ProyectoMiembros
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 241, 232);
            ClientSize = new Size(920, 600);
            Controls.Add(pnlDisponibles);
            Controls.Add(pnlSeparador);
            Controls.Add(pnlMiembros);
            Controls.Add(pnlEditor);
            Controls.Add(pnlSelector);
            Name = "ProyectoMiembros";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Equipo del proyecto";
            pnlSelector.ResumeLayout(false);
            pnlEditor.ResumeLayout(false);
            pnlDisponibles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).EndInit();
            pnlMiembros.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMiembros).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelector;
        private Label lblAyuda;
        private ComboBox cboProyectos;
        private Label lblProyecto;
        private Panel pnlEditor;
        private Label lblAyudaEditor;
        private ComboBox cboRol;
        private Label lblRol;
        private Button btnCancelar;
        private Button btnLimpiar;
        private Button btnQuitar;
        private Button btnModificar;
        private Button btnAsignar;
        private Button btnNuevo;
        private Panel pnlDisponibles;
        private DataGridView dgvDisponibles;
        private Label lblTituloDisponibles;
        private Panel pnlSeparador;
        private Panel pnlMiembros;
        private DataGridView dgvMiembros;
        private Label lblTituloMiembros;
    }
}
