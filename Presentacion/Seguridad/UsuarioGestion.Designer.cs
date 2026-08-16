namespace Presentacion.Seguridad;

partial class UsuarioGestion
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
        panelToolbar = new Panel();
        btnCancelar = new Button();
        btnLimpiar = new Button();
        btnDesactivar = new Button();
        btnModificar = new Button();
        btnAgregar = new Button();
        btnNuevo = new Button();
        splitContainer1 = new SplitContainer();
        dgvUsuarios = new DataGridView();
        groupRoles = new GroupBox();
        flowLayoutRoles = new FlowLayoutPanel();
        groupDatos = new GroupBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblApellidos = new Label();
        txtApellidos = new TextBox();
        lblNombres = new Label();
        txtNombres = new TextBox();
        panelToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
        groupRoles.SuspendLayout();
        groupDatos.SuspendLayout();
        SuspendLayout();
        // 
        // panelToolbar
        // 
        panelToolbar.BackColor = Color.FromArgb(245, 241, 232);
        panelToolbar.Controls.Add(btnCancelar);
        panelToolbar.Controls.Add(btnLimpiar);
        panelToolbar.Controls.Add(btnDesactivar);
        panelToolbar.Controls.Add(btnModificar);
        panelToolbar.Controls.Add(btnAgregar);
        panelToolbar.Controls.Add(btnNuevo);
        panelToolbar.Dock = DockStyle.Top;
        panelToolbar.Location = new Point(0, 0);
        panelToolbar.Name = "panelToolbar";
        panelToolbar.Size = new Size(1100, 52);
        panelToolbar.TabIndex = 0;
        // 
        // btnNuevo
        // 
        btnNuevo.BackColor = Color.FromArgb(12, 110, 99);
        btnNuevo.FlatAppearance.BorderSize = 0;
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnNuevo.ForeColor = Color.White;
        btnNuevo.Location = new Point(16, 9);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(125, 34);
        btnNuevo.TabIndex = 0;
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
        btnAgregar.Location = new Point(153, 9);
        btnAgregar.Name = "btnAgregar";
        btnAgregar.Size = new Size(125, 34);
        btnAgregar.TabIndex = 1;
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
        btnModificar.Location = new Point(290, 9);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(125, 34);
        btnModificar.TabIndex = 2;
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
        btnDesactivar.Location = new Point(427, 9);
        btnDesactivar.Name = "btnDesactivar";
        btnDesactivar.Size = new Size(125, 34);
        btnDesactivar.TabIndex = 3;
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
        btnLimpiar.Location = new Point(564, 9);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(125, 34);
        btnLimpiar.TabIndex = 4;
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
        btnCancelar.Location = new Point(701, 9);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(125, 34);
        btnCancelar.TabIndex = 5;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 52);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Panel1MinSize = 400;
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(dgvUsuarios);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.BackColor = Color.FromArgb(245, 241, 232);
        splitContainer1.Panel2.Controls.Add(groupRoles);
        splitContainer1.Panel2.Controls.Add(groupDatos);
        splitContainer1.Panel2MinSize = 260;
        splitContainer1.Size = new Size(1100, 598);
        splitContainer1.SplitterDistance = 800;
        splitContainer1.SplitterWidth = 5;
        splitContainer1.TabIndex = 1;
        // 
        // dgvUsuarios
        // 
        dgvUsuarios.AllowUserToAddRows = false;
        dgvUsuarios.AllowUserToDeleteRows = false;
        dgvUsuarios.AutoGenerateColumns = false;
        dgvUsuarios.BackgroundColor = Color.White;
        dgvUsuarios.BorderStyle = BorderStyle.None;
        dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvUsuarios.Dock = DockStyle.Fill;
        dgvUsuarios.EnableHeadersVisualStyles = false;
        dgvUsuarios.Location = new Point(0, 0);
        dgvUsuarios.MultiSelect = false;
        dgvUsuarios.Name = "dgvUsuarios";
        dgvUsuarios.ReadOnly = true;
        dgvUsuarios.RowHeadersVisible = false;
        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsuarios.Size = new Size(800, 598);
        dgvUsuarios.TabIndex = 0;
        dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
        // 
        // groupRoles
        // 
        groupRoles.BackColor = Color.FromArgb(245, 241, 232);
        groupRoles.Controls.Add(flowLayoutRoles);
        groupRoles.Dock = DockStyle.Fill;
        groupRoles.Font = new Font("Segoe UI Semibold", 10F);
        groupRoles.ForeColor = Color.FromArgb(44, 51, 47);
        groupRoles.Location = new Point(0, 182);
        groupRoles.Name = "groupRoles";
        groupRoles.Padding = new Padding(10, 6, 10, 10);
        groupRoles.Size = new Size(295, 416);
        groupRoles.TabIndex = 0;
        groupRoles.TabStop = false;
        groupRoles.Text = "  Roles del usuario  ";
        // 
        // flowLayoutRoles
        // 
        flowLayoutRoles.AutoScroll = true;
        flowLayoutRoles.BackColor = Color.FromArgb(245, 241, 232);
        flowLayoutRoles.Dock = DockStyle.Fill;
        flowLayoutRoles.FlowDirection = FlowDirection.TopDown;
        flowLayoutRoles.Location = new Point(10, 25);
        flowLayoutRoles.Name = "flowLayoutRoles";
        flowLayoutRoles.Padding = new Padding(4);
        flowLayoutRoles.Size = new Size(275, 375);
        flowLayoutRoles.TabIndex = 0;
        flowLayoutRoles.WrapContents = false;
        // 
        // groupDatos
        // 
        groupDatos.BackColor = Color.FromArgb(245, 241, 232);
        groupDatos.Controls.Add(lblPassword);
        groupDatos.Controls.Add(txtPassword);
        groupDatos.Controls.Add(lblEmail);
        groupDatos.Controls.Add(txtEmail);
        groupDatos.Controls.Add(lblApellidos);
        groupDatos.Controls.Add(txtApellidos);
        groupDatos.Controls.Add(lblNombres);
        groupDatos.Controls.Add(txtNombres);
        groupDatos.Dock = DockStyle.Top;
        groupDatos.Font = new Font("Segoe UI Semibold", 10F);
        groupDatos.ForeColor = Color.FromArgb(44, 51, 47);
        groupDatos.Location = new Point(0, 0);
        groupDatos.Name = "groupDatos";
        groupDatos.Padding = new Padding(10, 6, 10, 10);
        groupDatos.Size = new Size(295, 182);
        groupDatos.TabIndex = 1;
        groupDatos.TabStop = false;
        groupDatos.Text = "  Datos del usuario  ";
        // 
        // lblNombres
        // 
        lblNombres.Font = new Font("Segoe UI Semibold", 9.5F);
        lblNombres.ForeColor = Color.FromArgb(44, 51, 47);
        lblNombres.Location = new Point(8, 30);
        lblNombres.Name = "lblNombres";
        lblNombres.Size = new Size(84, 21);
        lblNombres.TabIndex = 0;
        lblNombres.Text = "Nombres:";
        lblNombres.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombres
        // 
        txtNombres.BorderStyle = BorderStyle.FixedSingle;
        txtNombres.Font = new Font("Segoe UI", 10F);
        txtNombres.Location = new Point(98, 26);
        txtNombres.MaxLength = 100;
        txtNombres.Name = "txtNombres";
        txtNombres.Size = new Size(169, 25);
        txtNombres.TabIndex = 1;
        // 
        // lblApellidos
        // 
        lblApellidos.Font = new Font("Segoe UI Semibold", 9.5F);
        lblApellidos.ForeColor = Color.FromArgb(44, 51, 47);
        lblApellidos.Location = new Point(8, 68);
        lblApellidos.Name = "lblApellidos";
        lblApellidos.Size = new Size(84, 21);
        lblApellidos.TabIndex = 2;
        lblApellidos.Text = "Apellidos:";
        lblApellidos.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtApellidos
        // 
        txtApellidos.BorderStyle = BorderStyle.FixedSingle;
        txtApellidos.Font = new Font("Segoe UI", 10F);
        txtApellidos.Location = new Point(98, 64);
        txtApellidos.MaxLength = 100;
        txtApellidos.Name = "txtApellidos";
        txtApellidos.Size = new Size(169, 25);
        txtApellidos.TabIndex = 3;
        // 
        // lblEmail
        // 
        lblEmail.Font = new Font("Segoe UI Semibold", 9.5F);
        lblEmail.ForeColor = Color.FromArgb(44, 51, 47);
        lblEmail.Location = new Point(8, 106);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(84, 21);
        lblEmail.TabIndex = 4;
        lblEmail.Text = "Correo:";
        lblEmail.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtEmail
        // 
        txtEmail.BorderStyle = BorderStyle.FixedSingle;
        txtEmail.Font = new Font("Segoe UI", 10F);
        txtEmail.Location = new Point(98, 102);
        txtEmail.MaxLength = 150;
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(169, 25);
        txtEmail.TabIndex = 5;
        // 
        // lblPassword
        // 
        lblPassword.Font = new Font("Segoe UI Semibold", 9.5F);
        lblPassword.ForeColor = Color.FromArgb(44, 51, 47);
        lblPassword.Location = new Point(8, 144);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(84, 21);
        lblPassword.TabIndex = 6;
        lblPassword.Text = "Contraseña:";
        lblPassword.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPassword
        // 
        txtPassword.BorderStyle = BorderStyle.FixedSingle;
        txtPassword.Font = new Font("Segoe UI", 10F);
        txtPassword.Location = new Point(98, 140);
        txtPassword.MaxLength = 64;
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '●';
        txtPassword.Size = new Size(169, 25);
        txtPassword.TabIndex = 7;
        // 
        // UsuarioGestion
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 241, 232);
        ClientSize = new Size(1100, 650);
        Controls.Add(splitContainer1);
        Controls.Add(panelToolbar);
        Name = "UsuarioGestion";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de usuarios";
        panelToolbar.ResumeLayout(false);
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
        groupRoles.ResumeLayout(false);
        groupDatos.ResumeLayout(false);
        groupDatos.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel panelToolbar;
    private Button btnCancelar;
    private Button btnLimpiar;
    private Button btnDesactivar;
    private Button btnModificar;
    private Button btnAgregar;
    private Button btnNuevo;
    private SplitContainer splitContainer1;
    private DataGridView dgvUsuarios;
    private GroupBox groupRoles;
    private FlowLayoutPanel flowLayoutRoles;
    private GroupBox groupDatos;
    private Label lblPassword;
    private TextBox txtPassword;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblApellidos;
    private TextBox txtApellidos;
    private Label lblNombres;
    private TextBox txtNombres;
}
