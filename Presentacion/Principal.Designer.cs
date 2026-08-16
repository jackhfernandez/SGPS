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
        backlogMenu = new ToolStripMenuItem();
        backlogEpic = new ToolStripMenuItem();
        backlogProducto = new ToolStripMenuItem();
        backlogHistoriaUsuarios = new ToolStripMenuItem();
        clienteToolStripMenuItem = new ToolStripMenuItem();
        clientePortal = new ToolStripMenuItem();
        kanbanToolStripMenuItem = new ToolStripMenuItem();
        kanbanTablero = new ToolStripMenuItem();
        kanbanTarea = new ToolStripMenuItem();
        kanbanControlTarjetas = new ToolStripMenuItem();
        proyectosMenu = new ToolStripMenuItem();
        proyectoNuevo = new ToolStripMenuItem();
        proyectoMiembros = new ToolStripMenuItem();
        qaGestionBug = new ToolStripMenuItem();
        gestionBugToolStripMenuItem = new ToolStripMenuItem();
        qaReporteBug = new ToolStripMenuItem();
        reporteToolStripMenuItem = new ToolStripMenuItem();
        reporteVistaGraficos = new ToolStripMenuItem();
        reporteMetricas = new ToolStripMenuItem();
        seguridadMenu = new ToolStripMenuItem();
        seguridadRol = new ToolStripMenuItem();
        seguridadUsuarios = new ToolStripMenuItem();
        sprintsMenu = new ToolStripMenuItem();
        sprintEjecucion = new ToolStripMenuItem();
        sprintPlanificaciones = new ToolStripMenuItem();
        seguridadRoles = new ToolStripMenuItem();
        sprintPlanificacion = new ToolStripMenuItem();
        statusStrip1 = new StatusStrip();
        lblStatusUsuario = new ToolStripStatusLabel();
        lblStatusRol = new ToolStripStatusLabel();
        menuStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { archivoMenu, backlogMenu, clienteToolStripMenuItem, kanbanToolStripMenuItem, proyectosMenu, qaGestionBug, reporteToolStripMenuItem, seguridadMenu, sprintsMenu });
        menuStrip1.Location = new Point(0, 0);
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
        mnuCerrarSesion.Size = new Size(142, 22);
        mnuCerrarSesion.Text = "&Cerrar sesión";
        mnuCerrarSesion.Click += mnuCerrarSesion_Click;
        // 
        // backlogMenu
        // 
        backlogMenu.DropDownItems.AddRange(new ToolStripItem[] { backlogEpic, backlogProducto, backlogHistoriaUsuarios });
        backlogMenu.Name = "backlogMenu";
        backlogMenu.Size = new Size(61, 20);
        backlogMenu.Text = "&Backlog";
        // 
        // backlogEpic
        // 
        backlogEpic.Name = "backlogEpic";
        backlogEpic.Size = new Size(174, 22);
        backlogEpic.Text = "Epic";
        backlogEpic.Click += backlogEpic_Click;
        // 
        // backlogProducto
        // 
        backlogProducto.Name = "backlogProducto";
        backlogProducto.Size = new Size(174, 22);
        backlogProducto.Text = "Backlog Producto";
        backlogProducto.Click += backlogProducto_Click;
        // 
        // backlogHistoriaUsuarios
        // 
        backlogHistoriaUsuarios.Name = "backlogHistoriaUsuarios";
        backlogHistoriaUsuarios.Size = new Size(174, 22);
        backlogHistoriaUsuarios.Text = "Historia de Usuario";
        backlogHistoriaUsuarios.Click += backlogHistoriaUsuarios_Click;
        // 
        // clienteToolStripMenuItem
        // 
        clienteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clientePortal });
        clienteToolStripMenuItem.Name = "clienteToolStripMenuItem";
        clienteToolStripMenuItem.Size = new Size(56, 20);
        clienteToolStripMenuItem.Text = "Cliente";
        // 
        // clientePortal
        // 
        clientePortal.Name = "clientePortal";
        clientePortal.Size = new Size(105, 22);
        clientePortal.Text = "Portal";
        clientePortal.Click += clientePortal_Click;
        // 
        // kanbanToolStripMenuItem
        // 
        kanbanToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { kanbanTablero, kanbanTarea, kanbanControlTarjetas });
        kanbanToolStripMenuItem.Name = "kanbanToolStripMenuItem";
        kanbanToolStripMenuItem.Size = new Size(59, 20);
        kanbanToolStripMenuItem.Text = "Kanban";
        // 
        // kanbanTablero
        // 
        kanbanTablero.Name = "kanbanTablero";
        kanbanTablero.Size = new Size(157, 22);
        kanbanTablero.Text = "Tablero Kanban";
        kanbanTablero.Click += kanbanTablero_Click;
        // 
        // kanbanTarea
        // 
        kanbanTarea.Name = "kanbanTarea";
        kanbanTarea.Size = new Size(157, 22);
        kanbanTarea.Text = "Tarea Edicion";
        kanbanTarea.Click += kanbanTarea_Click;
        // 
        // kanbanControlTarjetas
        // 
        kanbanControlTarjetas.Name = "kanbanControlTarjetas";
        kanbanControlTarjetas.Size = new Size(157, 22);
        kanbanControlTarjetas.Text = "Control Tarjetas";
        kanbanControlTarjetas.Click += kanbanControlTarjetas_Click;
        // 
        // proyectosMenu
        // 
        proyectosMenu.DropDownItems.AddRange(new ToolStripItem[] { proyectoNuevo, proyectoMiembros });
        proyectosMenu.Name = "proyectosMenu";
        proyectosMenu.Size = new Size(71, 20);
        proyectosMenu.Text = "&Proyectos";
        // 
        // proyectoNuevo
        // 
        proyectoNuevo.Name = "proyectoNuevo";
        proyectoNuevo.Size = new Size(171, 22);
        proyectoNuevo.Text = "&Nuevo";
        proyectoNuevo.Click += proyectoNuevo_Click;
        // 
        // proyectoMiembros
        // 
        proyectoMiembros.Name = "proyectoMiembros";
        proyectoMiembros.Size = new Size(171, 22);
        proyectoMiembros.Text = "Asignar Miembros";
        proyectoMiembros.Click += proyectoMiembros_Click;
        // 
        // qaGestionBug
        // 
        qaGestionBug.DropDownItems.AddRange(new ToolStripItem[] { gestionBugToolStripMenuItem, qaReporteBug });
        qaGestionBug.Name = "qaGestionBug";
        qaGestionBug.Size = new Size(36, 20);
        qaGestionBug.Text = "QA";
        // 
        // gestionBugToolStripMenuItem
        // 
        gestionBugToolStripMenuItem.Name = "gestionBugToolStripMenuItem";
        gestionBugToolStripMenuItem.Size = new Size(139, 22);
        gestionBugToolStripMenuItem.Text = "Gestion Bug";
        gestionBugToolStripMenuItem.Click += gestionBugToolStripMenuItem_Click;
        // 
        // qaReporteBug
        // 
        qaReporteBug.Name = "qaReporteBug";
        qaReporteBug.Size = new Size(139, 22);
        qaReporteBug.Text = "Reporte Bug";
        qaReporteBug.Click += qaReporteBug_Click;
        // 
        // reporteToolStripMenuItem
        // 
        reporteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { reporteVistaGraficos, reporteMetricas });
        reporteToolStripMenuItem.Name = "reporteToolStripMenuItem";
        reporteToolStripMenuItem.Size = new Size(60, 20);
        reporteToolStripMenuItem.Text = "Reporte";
        // 
        // reporteVistaGraficos
        // 
        reporteVistaGraficos.Name = "reporteVistaGraficos";
        reporteVistaGraficos.Size = new Size(145, 22);
        reporteVistaGraficos.Text = "Vista Graficos";
        reporteVistaGraficos.Click += reporteVistaGraficos_Click;
        // 
        // reporteMetricas
        // 
        reporteMetricas.Name = "reporteMetricas";
        reporteMetricas.Size = new Size(145, 22);
        reporteMetricas.Text = "Metricas";
        reporteMetricas.Click += reporteMetricas_Click;
        // 
        // seguridadMenu
        // 
        seguridadMenu.DropDownItems.AddRange(new ToolStripItem[] { seguridadRol, seguridadUsuarios });
        seguridadMenu.Name = "seguridadMenu";
        seguridadMenu.Size = new Size(72, 20);
        seguridadMenu.Text = "&Seguridad";
        // 
        // seguridadRol
        // 
        seguridadRol.Name = "seguridadRol";
        seguridadRol.Size = new Size(119, 22);
        seguridadRol.Text = "Roles";
        seguridadRol.Click += seguridadRol_Click;
        // 
        // seguridadUsuarios
        // 
        seguridadUsuarios.Name = "seguridadUsuarios";
        seguridadUsuarios.Size = new Size(119, 22);
        seguridadUsuarios.Text = "Usuarios";
        seguridadUsuarios.Click += seguridadUsuarios_Click;
        // 
        // sprintsMenu
        // 
        sprintsMenu.DropDownItems.AddRange(new ToolStripItem[] { sprintEjecucion, sprintPlanificaciones });
        sprintsMenu.Name = "sprintsMenu";
        sprintsMenu.Size = new Size(50, 20);
        sprintsMenu.Text = "S&print";
        // 
        // sprintEjecucion
        // 
        sprintEjecucion.Name = "sprintEjecucion";
        sprintEjecucion.Size = new Size(142, 22);
        sprintEjecucion.Text = "Ejecucion";
        sprintEjecucion.Click += sprintEjecucion_Click;
        // 
        // sprintPlanificaciones
        // 
        sprintPlanificaciones.Name = "sprintPlanificaciones";
        sprintPlanificaciones.Size = new Size(142, 22);
        sprintPlanificaciones.Text = "Planificacion";
        sprintPlanificaciones.Click += sprintPlanificaciones_Click;
        // 
        // seguridadRoles
        // 
        seguridadRoles.Name = "seguridadRoles";
        seguridadRoles.Size = new Size(119, 22);
        seguridadRoles.Text = "&Roles";
        // 
        // sprintPlanificacion
        // 
        sprintPlanificacion.Name = "sprintPlanificacion";
        sprintPlanificacion.Size = new Size(180, 22);
        sprintPlanificacion.Text = "Planificacion";
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
        lblStatusUsuario.Size = new Size(1050, 17);
        lblStatusUsuario.Spring = true;
        lblStatusUsuario.Text = "Usuario: -";
        lblStatusUsuario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblStatusRol
        // 
        lblStatusRol.Name = "lblStatusRol";
        lblStatusRol.Size = new Size(35, 17);
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
    private ToolStripMenuItem backlogMenu;
    private ToolStripMenuItem sprintsMenu;
    private ToolStripMenuItem portalMenu;
    private ToolStripMenuItem seguridadMenu;
    private ToolStripMenuItem mnuUsuarios;
    private ToolStripMenuItem seguridadRoles;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel lblStatusUsuario;
    private ToolStripStatusLabel lblStatusRol;
    private ToolStripMenuItem backlogEpic;
    private ToolStripMenuItem backlogProducto;
    private ToolStripMenuItem backlogHistoriaUsuarios;
    private ToolStripMenuItem clienteToolStripMenuItem;
    private ToolStripMenuItem clientePortal;
    private ToolStripMenuItem kanbanToolStripMenuItem;
    private ToolStripMenuItem kanbanTablero;
    private ToolStripMenuItem kanbanTarea;
    private ToolStripMenuItem kanbanControlTarjetas;
    private ToolStripMenuItem proyectoMiembros;
    private ToolStripMenuItem qaGestionBug;
    private ToolStripMenuItem gestionBugToolStripMenuItem;
    private ToolStripMenuItem qaReporteBug;
    private ToolStripMenuItem reporteToolStripMenuItem;
    private ToolStripMenuItem reporteVistaGraficos;
    private ToolStripMenuItem reporteMetricas;
    private ToolStripMenuItem gestionUsuariosToolStripMenuItem;
    private ToolStripMenuItem ejecucToolStripMenuItem;
    private ToolStripMenuItem sprintPlanificacion;
    private ToolStripMenuItem proyectoNuevo;
    private ToolStripMenuItem seguridadRol;
    private ToolStripMenuItem seguridadUsuarios;
    private ToolStripMenuItem sprintEjecucion;
    private ToolStripMenuItem sprintPlanificaciones;
}