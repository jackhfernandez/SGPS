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
        btnRefrescar = new Button();
        btnEstado = new Button();
        btnNuevo = new Button();
        splitContainer1 = new SplitContainer();
        dgvUsuarios = new DataGridView();
        groupRoles = new GroupBox();
        flowLayoutRoles = new FlowLayoutPanel();
        btnGuardarRoles = new Button();
        panelToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
        groupRoles.SuspendLayout();
        SuspendLayout();
        // 
        // panelToolbar
        // 
        panelToolbar.BackColor = Color.FromArgb(245, 241, 232);
        panelToolbar.Controls.Add(btnRefrescar);
        panelToolbar.Controls.Add(btnEstado);
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
        btnNuevo.Text = "Nuevo usuario";
        btnNuevo.UseVisualStyleBackColor = false;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnEstado
        // 
        btnEstado.BackColor = Color.FromArgb(73, 143, 132);
        btnEstado.FlatAppearance.BorderSize = 0;
        btnEstado.FlatStyle = FlatStyle.Flat;
        btnEstado.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnEstado.ForeColor = Color.White;
        btnEstado.Location = new Point(153, 9);
        btnEstado.Name = "btnEstado";
        btnEstado.Size = new Size(155, 34);
        btnEstado.TabIndex = 1;
        btnEstado.Text = "Activar / Desactivar";
        btnEstado.UseVisualStyleBackColor = false;
        btnEstado.Click += btnEstado_Click;
        // 
        // btnRefrescar
        // 
        btnRefrescar.BackColor = Color.FromArgb(200, 200, 200);
        btnRefrescar.FlatAppearance.BorderSize = 0;
        btnRefrescar.FlatStyle = FlatStyle.Flat;
        btnRefrescar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnRefrescar.ForeColor = Color.FromArgb(44, 51, 47);
        btnRefrescar.Location = new Point(320, 9);
        btnRefrescar.Name = "btnRefrescar";
        btnRefrescar.Size = new Size(110, 34);
        btnRefrescar.TabIndex = 2;
        btnRefrescar.Text = "Refrescar";
        btnRefrescar.UseVisualStyleBackColor = false;
        btnRefrescar.Click += btnRefrescar_Click;
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
        groupRoles.Controls.Add(btnGuardarRoles);
        groupRoles.Dock = DockStyle.Fill;
        groupRoles.Font = new Font("Segoe UI Semibold", 10F);
        groupRoles.ForeColor = Color.FromArgb(44, 51, 47);
        groupRoles.Location = new Point(0, 0);
        groupRoles.Name = "groupRoles";
        groupRoles.Padding = new Padding(10, 6, 10, 10);
        groupRoles.Size = new Size(295, 598);
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
        flowLayoutRoles.Size = new Size(275, 523);
        flowLayoutRoles.TabIndex = 0;
        flowLayoutRoles.WrapContents = false;
        // 
        // btnGuardarRoles
        // 
        btnGuardarRoles.BackColor = Color.FromArgb(12, 110, 99);
        btnGuardarRoles.Dock = DockStyle.Bottom;
        btnGuardarRoles.FlatAppearance.BorderSize = 0;
        btnGuardarRoles.FlatStyle = FlatStyle.Flat;
        btnGuardarRoles.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnGuardarRoles.ForeColor = Color.White;
        btnGuardarRoles.Location = new Point(10, 548);
        btnGuardarRoles.Name = "btnGuardarRoles";
        btnGuardarRoles.Size = new Size(275, 44);
        btnGuardarRoles.TabIndex = 1;
        btnGuardarRoles.Text = "Guardar roles";
        btnGuardarRoles.UseVisualStyleBackColor = false;
        btnGuardarRoles.Click += btnGuardarRoles_Click;
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
        ResumeLayout(false);
    }

    #endregion

    private Panel panelToolbar;
    private Button btnRefrescar;
    private Button btnEstado;
    private Button btnNuevo;
    private SplitContainer splitContainer1;
    private DataGridView dgvUsuarios;
    private GroupBox groupRoles;
    private FlowLayoutPanel flowLayoutRoles;
    private Button btnGuardarRoles;
}