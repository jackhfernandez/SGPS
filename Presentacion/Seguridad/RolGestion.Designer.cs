namespace Presentacion.Seguridad;

partial class RolGestion
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
        btnEliminar = new Button();
        btnGuardar = new Button();
        btnNuevo = new Button();
        txtDescripcion = new TextBox();
        lblDescripcion = new Label();
        txtNombre = new TextBox();
        lblNombre = new Label();
        dgvRoles = new DataGridView();
        panelEditor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
        SuspendLayout();
        // 
        // panelEditor
        // 
        panelEditor.BackColor = Color.FromArgb(245, 241, 232);
        panelEditor.Controls.Add(btnCancelar);
        panelEditor.Controls.Add(btnEliminar);
        panelEditor.Controls.Add(btnGuardar);
        panelEditor.Controls.Add(btnNuevo);
        panelEditor.Controls.Add(txtDescripcion);
        panelEditor.Controls.Add(lblDescripcion);
        panelEditor.Controls.Add(txtNombre);
        panelEditor.Controls.Add(lblNombre);
        panelEditor.Dock = DockStyle.Bottom;
        panelEditor.Location = new Point(0, 396);
        panelEditor.Name = "panelEditor";
        panelEditor.Size = new Size(752, 128);
        panelEditor.TabIndex = 0;
        // 
        // lblNombre
        // 
        lblNombre.Font = new Font("Segoe UI Semibold", 9.5F);
        lblNombre.ForeColor = Color.FromArgb(44, 51, 47);
        lblNombre.Location = new Point(24, 18);
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
        txtNombre.Location = new Point(120, 14);
        txtNombre.MaxLength = 50;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(250, 25);
        txtNombre.TabIndex = 1;
        // 
        // lblDescripcion
        // 
        lblDescripcion.Font = new Font("Segoe UI Semibold", 9.5F);
        lblDescripcion.ForeColor = Color.FromArgb(44, 51, 47);
        lblDescripcion.Location = new Point(24, 56);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(90, 21);
        lblDescripcion.TabIndex = 2;
        lblDescripcion.Text = "Descripción:";
        lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDescripcion
        // 
        txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
        txtDescripcion.Font = new Font("Segoe UI", 10F);
        txtDescripcion.Location = new Point(120, 52);
        txtDescripcion.MaxLength = 255;
        txtDescripcion.Multiline = true;
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.Size = new Size(250, 58);
        txtDescripcion.TabIndex = 3;
        // 
        // btnNuevo
        // 
        btnNuevo.BackColor = Color.FromArgb(12, 110, 99);
        btnNuevo.FlatAppearance.BorderSize = 0;
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnNuevo.ForeColor = Color.White;
        btnNuevo.Location = new Point(420, 18);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(110, 36);
        btnNuevo.TabIndex = 4;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = false;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(12, 110, 99);
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnGuardar.ForeColor = Color.White;
        btnGuardar.Location = new Point(545, 18);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(110, 36);
        btnGuardar.TabIndex = 5;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnEliminar
        // 
        btnEliminar.BackColor = Color.FromArgb(178, 52, 43);
        btnEliminar.FlatAppearance.BorderSize = 0;
        btnEliminar.FlatStyle = FlatStyle.Flat;
        btnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnEliminar.ForeColor = Color.White;
        btnEliminar.Location = new Point(420, 74);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(110, 36);
        btnEliminar.TabIndex = 6;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = false;
        btnEliminar.Click += btnEliminar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(200, 200, 200);
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCancelar.ForeColor = Color.FromArgb(44, 51, 47);
        btnCancelar.Location = new Point(545, 74);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(110, 36);
        btnCancelar.TabIndex = 7;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // dgvRoles
        // 
        dgvRoles.AllowUserToAddRows = false;
        dgvRoles.AllowUserToDeleteRows = false;
        dgvRoles.AutoGenerateColumns = false;
        dgvRoles.BackgroundColor = Color.White;
        dgvRoles.BorderStyle = BorderStyle.None;
        dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvRoles.Dock = DockStyle.Fill;
        dgvRoles.EnableHeadersVisualStyles = false;
        dgvRoles.Location = new Point(0, 0);
        dgvRoles.MultiSelect = false;
        dgvRoles.Name = "dgvRoles";
        dgvRoles.ReadOnly = true;
        dgvRoles.RowHeadersVisible = false;
        dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRoles.Size = new Size(752, 396);
        dgvRoles.TabIndex = 1;
        dgvRoles.SelectionChanged += dgvRoles_SelectionChanged;
        // 
        // RolGestion
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 241, 232);
        ClientSize = new Size(752, 524);
        Controls.Add(dgvRoles);
        Controls.Add(panelEditor);
        Name = "RolGestion";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de roles";
        panelEditor.ResumeLayout(false);
        panelEditor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel panelEditor;
    private Button btnCancelar;
    private Button btnEliminar;
    private Button btnGuardar;
    private Button btnNuevo;
    private TextBox txtDescripcion;
    private Label lblDescripcion;
    private TextBox txtNombre;
    private Label lblNombre;
    private DataGridView dgvRoles;
}