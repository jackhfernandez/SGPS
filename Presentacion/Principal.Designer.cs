namespace Presentacion;

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
        menuStrip1 = new MenuStrip();
        archivoMenu = new ToolStripMenuItem();
        mnuCerrarSesion = new ToolStripMenuItem();
        proyectosMenu = new ToolStripMenuItem();
        mnuProyectoNuevo = new ToolStripMenuItem();
        backlogMenu = new ToolStripMenuItem();
        sprintsMenu = new ToolStripMenuItem();
        portalMenu = new ToolStripMenuItem();
        seguridadMenu = new ToolStripMenuItem();
        mnuUsuarios = new ToolStripMenuItem();
        mnuRoles = new ToolStripMenuItem();
        ventanaMenu = new ToolStripMenuItem();
        mnuVentanaMosaicoH = new ToolStripMenuItem();
        mnuVentanaMosaicoV = new ToolStripMenuItem();
        mnuVentanaCascada = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        mnuVentanaCerrarTodos = new ToolStripMenuItem();
        statusStrip1 = new StatusStrip();
        lblStatusUsuario = new ToolStripStatusLabel();
        lblStatusRol = new ToolStripStatusLabel();
        menuStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[]
        {
            archivoMenu,
            proyectosMenu,
            backlogMenu,
            sprintsMenu,
            portalMenu,
            seguridadMenu,
            ventanaMenu
        });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.MdiWindowListItem = ventanaMenu;
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(1100, 24);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // archivoMenu
        // 
        archivoMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuCerrarSesion });
        archivoMenu.Name = "archivoMenu";
        archivoMenu.Size = new Size(60, 20);
        archivoMenu.Text = "&Archivo";
        // 
        // mnuCerrarSesion
        // 
        mnuCerrarSesion.Name = "mnuCerrarSesion";
        mnuCerrarSesion.Size = new Size(180, 22);
        mnuCerrarSesion.Text = "&Cerrar sesión";
        mnuCerrarSesion.Click += mnuCerrarSesion_Click;
        // 
        // proyectosMenu
        // 
        proyectosMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuProyectoNuevo });
        proyectosMenu.Name = "proyectosMenu";
        proyectosMenu.Size = new Size(75, 20);
        proyectosMenu.Text = "&Proyectos";
        // 
        // mnuProyectoNuevo
        // 
        mnuProyectoNuevo.Name = "mnuProyectoNuevo";
        mnuProyectoNuevo.Size = new Size(180, 22);
        mnuProyectoNuevo.Text = "&Nuevo proyecto...";
        mnuProyectoNuevo.Click += mnuProyectoNuevo_Click;
        // 
        // backlogMenu
        // 
        backlogMenu.Name = "backlogMenu";
        backlogMenu.Size = new Size(66, 20);
        backlogMenu.Text = "&Backlog";
        backlogMenu.Click += backlogMenu_Click;
        // 
        // sprintsMenu
        // 
        sprintsMenu.Name = "sprintsMenu";
        sprintsMenu.Size = new Size(65, 20);
        sprintsMenu.Text = "S&prints";
        sprintsMenu.Click += sprintsMenu_Click;
        // 
        // portalMenu
        // 
        portalMenu.Name = "portalMenu";
        portalMenu.Size = new Size(117, 20);
        portalMenu.Text = "&Portal de cliente";
        portalMenu.Click += portalMenu_Click;
        // 
        // seguridadMenu
        // 
        seguridadMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuUsuarios, mnuRoles });
        seguridadMenu.Name = "seguridadMenu";
        seguridadMenu.Size = new Size(77, 20);
        seguridadMenu.Text = "&Seguridad";
        // 
        // mnuUsuarios
        // 
        mnuUsuarios.Name = "mnuUsuarios";
        mnuUsuarios.Size = new Size(180, 22);
        mnuUsuarios.Text = "&Usuarios";
        mnuUsuarios.Click += mnuUsuarios_Click;
        // 
        // mnuRoles
        // 
        mnuRoles.Name = "mnuRoles";
        mnuRoles.Size = new Size(180, 22);
        mnuRoles.Text = "&Roles";
        mnuRoles.Click += mnuRoles_Click;
        // 
        // ventanaMenu
        // 
        ventanaMenu.DropDownItems.AddRange(new ToolStripItem[]
        {
            mnuVentanaMosaicoH,
            mnuVentanaMosaicoV,
            mnuVentanaCascada,
            toolStripSeparator1,
            mnuVentanaCerrarTodos
        });
        ventanaMenu.Name = "ventanaMenu";
        ventanaMenu.Size = new Size(68, 20);
        ventanaMenu.Text = "&Ventana";
        // 
        // mnuVentanaMosaicoH
        // 
        mnuVentanaMosaicoH.Name = "mnuVentanaMosaicoH";
        mnuVentanaMosaicoH.Size = new Size(180, 22);
        mnuVentanaMosaicoH.Text = "Mosaico &horizontal";
        mnuVentanaMosaicoH.Click += mnuVentanaMosaicoH_Click;
        // 
        // mnuVentanaMosaicoV
        // 
        mnuVentanaMosaicoV.Name = "mnuVentanaMosaicoV";
        mnuVentanaMosaicoV.Size = new Size(180, 22);
        mnuVentanaMosaicoV.Text = "Mosaico &vertical";
        mnuVentanaMosaicoV.Click += mnuVentanaMosaicoV_Click;
        // 
        // mnuVentanaCascada
        // 
        mnuVentanaCascada.Name = "mnuVentanaCascada";
        mnuVentanaCascada.Size = new Size(180, 22);
        mnuVentanaCascada.Text = "Ca&scada";
        mnuVentanaCascada.Click += mnuVentanaCascada_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(177, 6);
        // 
        // mnuVentanaCerrarTodos
        // 
        mnuVentanaCerrarTodos.Name = "mnuVentanaCerrarTodos";
        mnuVentanaCerrarTodos.Size = new Size(180, 22);
        mnuVentanaCerrarTodos.Text = "Cerrar &todos";
        mnuVentanaCerrarTodos.Click += mnuVentanaCerrarTodos_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatusUsuario, lblStatusRol });
        statusStrip1.Location = new Point(0, 628);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(1100, 22);
        statusStrip1.TabIndex = 1;
        statusStrip1.Text = "statusStrip1";
        // 
        // lblStatusUsuario
        // 
        lblStatusUsuario.Name = "lblStatusUsuario";
        lblStatusUsuario.Spring = true;
        lblStatusUsuario.Text = "Usuario: -";
        lblStatusUsuario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblStatusRol
        // 
        lblStatusRol.Name = "lblStatusRol";
        lblStatusRol.Text = "Rol: -";
        // 
        // Principal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(statusStrip1);
        Controls.Add(menuStrip1);
        IsMdiContainer = true;
        MainMenuStrip = menuStrip1;
        Name = "Principal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SGPS - Sistema de Gestión de Proyectos de Software";
        WindowState = FormWindowState.Maximized;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
    private ToolStripMenuItem archivoMenu;
    private ToolStripMenuItem mnuCerrarSesion;
    private ToolStripMenuItem proyectosMenu;
    private ToolStripMenuItem mnuProyectoNuevo;
    private ToolStripMenuItem backlogMenu;
    private ToolStripMenuItem sprintsMenu;
    private ToolStripMenuItem portalMenu;
    private ToolStripMenuItem seguridadMenu;
    private ToolStripMenuItem mnuUsuarios;
    private ToolStripMenuItem mnuRoles;
    private ToolStripMenuItem ventanaMenu;
    private ToolStripMenuItem mnuVentanaMosaicoH;
    private ToolStripMenuItem mnuVentanaMosaicoV;
    private ToolStripMenuItem mnuVentanaCascada;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem mnuVentanaCerrarTodos;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel lblStatusUsuario;
    private ToolStripStatusLabel lblStatusRol;
}