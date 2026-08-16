namespace Presentacion;

using Presentacion.Ui;

partial class Principal
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
        components = new System.ComponentModel.Container();
        panelPrincipal = new Panel();
        panelContenido = new Panel();
        panelSuperior = new Panel();
        lblBreadcrumb = new Label();
        lblAviso = new Label();
        panelLateral = new Panel();
        tablaLateral = new TableLayoutPanel();
        flowNav = new FlowLayoutPanel();
        flowAdmin = new FlowLayoutPanel();
        panelUsuario = new Panel();
        btnMenuUsuario = new Button();
        menuUsuario = new ContextMenuStrip(components);
        mnuCerrarSesion = new ToolStripMenuItem();
        panelLogo = new Panel();
        panelPrincipal.SuspendLayout();
        panelSuperior.SuspendLayout();
        panelLateral.SuspendLayout();
        tablaLateral.SuspendLayout();
        panelUsuario.SuspendLayout();
        menuUsuario.SuspendLayout();
        SuspendLayout();
        //
        // panelPrincipal
        //
        panelPrincipal.AutoScroll = false;
        panelPrincipal.BackColor = Color.FromArgb(245, 241, 232);
        panelPrincipal.Controls.Add(panelContenido);
        panelPrincipal.Controls.Add(panelSuperior);
        panelPrincipal.Dock = DockStyle.Fill;
        panelPrincipal.Location = new Point(260, 0);
        panelPrincipal.Name = "panelPrincipal";
        panelPrincipal.Size = new Size(840, 650);
        panelPrincipal.TabIndex = 1;
        //
        // panelContenido
        //
        panelContenido.AutoScroll = false;
        panelContenido.BackColor = Color.FromArgb(245, 241, 232);
        panelContenido.Dock = DockStyle.Fill;
        panelContenido.Location = new Point(0, 64);
        panelContenido.Name = "panelContenido";
        panelContenido.Padding = new Padding(32, 24, 32, 24);
        panelContenido.Size = new Size(840, 586);
        panelContenido.TabIndex = 1;
        //
        // panelSuperior
        //
        panelSuperior.BackColor = Color.FromArgb(245, 241, 232);
        panelSuperior.Controls.Add(lblAviso);
        panelSuperior.Controls.Add(lblBreadcrumb);
        panelSuperior.Dock = DockStyle.Top;
        panelSuperior.Location = new Point(0, 0);
        panelSuperior.Name = "panelSuperior";
        panelSuperior.Size = new Size(840, 64);
        panelSuperior.TabIndex = 0;
        panelSuperior.Paint += panelSuperior_Paint;
        //
        // lblBreadcrumb
        //
        lblBreadcrumb.AutoSize = true;
        lblBreadcrumb.BackColor = Color.Transparent;
        lblBreadcrumb.Font = new Font("Consolas", 8.5F);
        lblBreadcrumb.ForeColor = Color.FromArgb(138, 133, 122);
        lblBreadcrumb.Location = new Point(32, 24);
        lblBreadcrumb.Name = "lblBreadcrumb";
        lblBreadcrumb.Size = new Size(50, 13);
        lblBreadcrumb.TabIndex = 0;
        lblBreadcrumb.Text = "SGPS";
        //
        // lblAviso
        //
        lblAviso.AutoSize = true;
        lblAviso.BackColor = Color.Transparent;
        lblAviso.Font = new Font("Consolas", 8.5F);
        lblAviso.ForeColor = Color.FromArgb(226, 114, 91);
        lblAviso.Location = new Point(340, 24);
        lblAviso.Name = "lblAviso";
        lblAviso.Size = new Size(0, 13);
        lblAviso.TabIndex = 1;
        lblAviso.Visible = false;
        //
        // panelLateral
        //
        panelLateral.AutoScroll = false;
        panelLateral.BackColor = Color.FromArgb(15, 59, 51);
        panelLateral.Controls.Add(tablaLateral);
        panelLateral.Dock = DockStyle.Left;
        panelLateral.Location = new Point(0, 0);
        panelLateral.Name = "panelLateral";
        panelLateral.Size = new Size(260, 650);
        panelLateral.TabIndex = 0;
        //
        // tablaLateral
        //
        // Se usa una tabla en lugar de apilar Dock para que el orden de las
        // cuatro zonas no dependa del z-order de la coleccion de controles.
        tablaLateral.AutoScroll = false;
        tablaLateral.BackColor = Color.FromArgb(15, 59, 51);
        tablaLateral.ColumnCount = 1;
        tablaLateral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tablaLateral.Controls.Add(panelLogo, 0, 0);
        tablaLateral.Controls.Add(flowNav, 0, 1);
        tablaLateral.Controls.Add(flowAdmin, 0, 2);
        tablaLateral.Controls.Add(panelUsuario, 0, 3);
        tablaLateral.Dock = DockStyle.Fill;
        tablaLateral.Location = new Point(0, 0);
        tablaLateral.Name = "tablaLateral";
        tablaLateral.RowCount = 4;
        tablaLateral.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
        tablaLateral.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tablaLateral.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tablaLateral.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        tablaLateral.Size = new Size(260, 650);
        tablaLateral.TabIndex = 0;
        //
        // flowNav
        //
        flowNav.AutoScroll = false;
        flowNav.BackColor = Color.FromArgb(15, 59, 51);
        flowNav.Dock = DockStyle.Fill;
        flowNav.FlowDirection = FlowDirection.TopDown;
        flowNav.Location = new Point(0, 96);
        flowNav.Margin = new Padding(0);
        flowNav.Name = "flowNav";
        flowNav.Padding = new Padding(0, 8, 0, 0);
        flowNav.Size = new Size(260, 412);
        flowNav.TabIndex = 1;
        flowNav.WrapContents = false;
        //
        // flowAdmin
        //
        flowAdmin.AutoScroll = false;
        flowAdmin.AutoSize = true;
        flowAdmin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowAdmin.BackColor = Color.FromArgb(15, 59, 51);
        flowAdmin.Dock = DockStyle.Fill;
        flowAdmin.FlowDirection = FlowDirection.TopDown;
        flowAdmin.Location = new Point(0, 508);
        flowAdmin.Margin = new Padding(0);
        flowAdmin.Name = "flowAdmin";
        flowAdmin.Padding = new Padding(0, 0, 0, 8);
        flowAdmin.Size = new Size(260, 54);
        flowAdmin.TabIndex = 2;
        flowAdmin.WrapContents = false;
        //
        // panelUsuario
        //
        panelUsuario.BackColor = Color.FromArgb(15, 59, 51);
        panelUsuario.Controls.Add(btnMenuUsuario);
        panelUsuario.Dock = DockStyle.Fill;
        panelUsuario.Location = new Point(0, 562);
        panelUsuario.Margin = new Padding(0);
        panelUsuario.Name = "panelUsuario";
        panelUsuario.Size = new Size(260, 88);
        panelUsuario.TabIndex = 3;
        panelUsuario.Paint += panelUsuario_Paint;
        //
        // btnMenuUsuario
        //
        btnMenuUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnMenuUsuario.BackColor = Color.FromArgb(15, 59, 51);
        btnMenuUsuario.FlatAppearance.BorderSize = 0;
        btnMenuUsuario.FlatAppearance.MouseDownBackColor = Color.FromArgb(23, 74, 65);
        btnMenuUsuario.FlatAppearance.MouseOverBackColor = Color.FromArgb(23, 74, 65);
        btnMenuUsuario.FlatStyle = FlatStyle.Flat;
        btnMenuUsuario.ForeColor = Color.FromArgb(73, 143, 132);
        btnMenuUsuario.Location = new Point(214, 30);
        btnMenuUsuario.Name = "btnMenuUsuario";
        btnMenuUsuario.Size = new Size(32, 28);
        btnMenuUsuario.TabIndex = 0;
        btnMenuUsuario.Text = "···";
        btnMenuUsuario.UseVisualStyleBackColor = false;
        btnMenuUsuario.Click += btnMenuUsuario_Click;
        //
        // menuUsuario
        //
        menuUsuario.Items.AddRange(new ToolStripItem[] { mnuCerrarSesion });
        menuUsuario.Name = "menuUsuario";
        menuUsuario.Size = new Size(153, 26);
        //
        // mnuCerrarSesion
        //
        mnuCerrarSesion.Name = "mnuCerrarSesion";
        mnuCerrarSesion.Size = new Size(152, 22);
        mnuCerrarSesion.Text = "&Cerrar sesión";
        mnuCerrarSesion.Click += mnuCerrarSesion_Click;
        //
        // panelLogo
        //
        panelLogo.BackColor = Color.FromArgb(15, 59, 51);
        panelLogo.Dock = DockStyle.Fill;
        panelLogo.Location = new Point(0, 0);
        panelLogo.Margin = new Padding(0);
        panelLogo.Name = "panelLogo";
        panelLogo.Size = new Size(260, 96);
        panelLogo.TabIndex = 0;
        panelLogo.Paint += panelLogo_Paint;
        //
        // Principal
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = false;
        BackColor = Color.FromArgb(245, 241, 232);
        ClientSize = new Size(1100, 650);
        Controls.Add(panelPrincipal);
        Controls.Add(panelLateral);
        MinimumSize = new Size(1100, 700);
        Name = "Principal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SGPS - Sistema de Gestión de Proyectos de Software";
        WindowState = FormWindowState.Maximized;
        panelPrincipal.ResumeLayout(false);
        panelSuperior.ResumeLayout(false);
        panelSuperior.PerformLayout();
        panelLateral.ResumeLayout(false);
        tablaLateral.ResumeLayout(false);
        tablaLateral.PerformLayout();
        panelUsuario.ResumeLayout(false);
        menuUsuario.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Panel panelPrincipal;
    private Panel panelContenido;
    private Panel panelSuperior;
    private Label lblBreadcrumb;
    private Label lblAviso;
    private Panel panelLateral;
    private TableLayoutPanel tablaLateral;
    private FlowLayoutPanel flowNav;
    private FlowLayoutPanel flowAdmin;
    private Panel panelUsuario;
    private Button btnMenuUsuario;
    private ContextMenuStrip menuUsuario;
    private ToolStripMenuItem mnuCerrarSesion;
    private Panel panelLogo;
}
