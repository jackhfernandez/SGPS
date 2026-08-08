-- ============================================================================
-- SISTEMA DE GESTIÓN DE PROYECTOS DE SOFTWARE (SGPS)
-- Script de Creación de Estructura de Base de Datos SQL Server
-- Cantidad de Tablas: 13
-- ============================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SGPS_DB')
BEGIN
    CREATE DATABASE SGPS_DB;
END
GO

USE SGPS_DB;
GO

-- ----------------------------------------------------------------------------
-- TABLA 1: Roles
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Roles (
    RolId INT IDENTITY(1,1) CONSTRAINT PK_Roles PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL CONSTRAINT UQ_Roles_Nombre UNIQUE,
    Descripcion VARCHAR(255) NULL,
    FechaCreacion DATETIME CONSTRAINT DF_Roles_Fecha DEFAULT GETDATE()
);

-- ----------------------------------------------------------------------------
-- TABLA 2: Usuarios
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Usuarios (
    UsuarioId INT IDENTITY(1,1) CONSTRAINT PK_Usuarios PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL CONSTRAINT UQ_Usuarios_Email UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    PasswordSalt VARCHAR(256) NOT NULL,
    EsActivo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT 1,
    FechaRegistro DATETIME CONSTRAINT DF_Usuarios_Fecha DEFAULT GETDATE(),
    UltimoAcceso DATETIME NULL
);

-- ----------------------------------------------------------------------------
-- TABLA 3: UsuarioRoles (Relación Muchos a Muchos)
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.UsuarioRoles (
    UsuarioId INT NOT NULL,
    RolId INT NOT NULL,
    FechaAsignacion DATETIME CONSTRAINT DF_UsuarioRoles_Fecha DEFAULT GETDATE(),
    CONSTRAINT PK_UsuarioRoles PRIMARY KEY (UsuarioId, RolId),
    CONSTRAINT FK_UsuarioRoles_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(UsuarioId) ON DELETE CASCADE,
    CONSTRAINT FK_UsuarioRoles_Roles FOREIGN KEY (RolId) REFERENCES dbo.Roles(RolId) ON DELETE CASCADE
);

-- ----------------------------------------------------------------------------
-- TABLA 4: Proyectos
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Proyectos (
    ProyectoId INT IDENTITY(1,1) CONSTRAINT PK_Proyectos PRIMARY KEY,
    ClaveProyecto VARCHAR(10) NOT NULL CONSTRAINT UQ_Proyectos_Clave UNIQUE, -- Ej: SGPS, PRJ
    NombreProyecto VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    Metodologia VARCHAR(30) NOT NULL CONSTRAINT DF_Proyectos_Metodologia DEFAULT 'Scrum',
    FechaInicio DATE NOT NULL,
    FechaFinEstimada DATE NULL,
    FechaFinReal DATE NULL,
    EsActivo BIT NOT NULL CONSTRAINT DF_Proyectos_Activo DEFAULT 1,
    FechaCreacion DATETIME CONSTRAINT DF_Proyectos_Fecha DEFAULT GETDATE()
);

-- ----------------------------------------------------------------------------
-- TABLA 5: ProyectoMiembros
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.ProyectoMiembros (
    ProyectoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    RolEnProyecto VARCHAR(50) NOT NULL, -- PO, SM, Developer, QA, Cliente
    FechaAsignacion DATETIME CONSTRAINT DF_ProyectoMiembros_Fecha DEFAULT GETDATE(),
    CONSTRAINT PK_ProyectoMiembros PRIMARY KEY (ProyectoId, UsuarioId),
    CONSTRAINT FK_ProyectoMiembros_Proyectos FOREIGN KEY (ProyectoId) REFERENCES dbo.Proyectos(ProyectoId) ON DELETE CASCADE,
    CONSTRAINT FK_ProyectoMiembros_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(UsuarioId)
);

-- ----------------------------------------------------------------------------
-- TABLA 6: Epics
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Epics (
    EpicId INT IDENTITY(1,1) CONSTRAINT PK_Epics PRIMARY KEY,
    ProyectoId INT NOT NULL,
    Titulo VARCHAR(200) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    ColorHex VARCHAR(7) CONSTRAINT DF_Epics_Color DEFAULT '#3182CE',
    FechaCreacion DATETIME CONSTRAINT DF_Epics_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Epics_Proyectos FOREIGN KEY (ProyectoId) REFERENCES dbo.Proyectos(ProyectoId) ON DELETE CASCADE
);

-- ----------------------------------------------------------------------------
-- TABLA 7: Sprints
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Sprints (
    SprintId INT IDENTITY(1,1) CONSTRAINT PK_Sprints PRIMARY KEY,
    ProyectoId INT NOT NULL,
    NombreSprint VARCHAR(100) NOT NULL,
    SprintGoal VARCHAR(MAX) NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Estado VARCHAR(20) NOT NULL CONSTRAINT DF_Sprints_Estado DEFAULT 'Planificado', -- Planificado, Activo, Cerrado
    FechaCreacion DATETIME CONSTRAINT DF_Sprints_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Sprints_Proyectos FOREIGN KEY (ProyectoId) REFERENCES dbo.Proyectos(ProyectoId)
);

-- ----------------------------------------------------------------------------
-- TABLA 8: UserStories
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.UserStories (
    UserStoryId INT IDENTITY(1,1) CONSTRAINT PK_UserStories PRIMARY KEY,
    CodigoTicket VARCHAR(20) NOT NULL CONSTRAINT UQ_UserStories_Codigo UNIQUE, -- Ej: SGPS-101
    ProyectoId INT NOT NULL,
    EpicId INT NULL,
    SprintId INT NULL,
    Titulo VARCHAR(200) NOT NULL,
    ComoUsuario VARCHAR(100) NOT NULL,
    QuieroFuncionalidad VARCHAR(255) NOT NULL,
    ParaBeneficio VARCHAR(255) NOT NULL,
    CriteriosAceptacionTexto VARCHAR(MAX) NULL,
    ValorNegocio VARCHAR(10) NOT NULL CONSTRAINT DF_UserStories_Valor DEFAULT 'Medio', -- Alto, Medio, Bajo
    StoryPoints INT NOT NULL CONSTRAINT DF_UserStories_Points DEFAULT 0,
    Estado VARCHAR(30) NOT NULL CONSTRAINT DF_UserStories_Estado DEFAULT 'To Do', -- To Do, In Progress, In Testing, Done
    OrdenPrioridad INT NOT NULL CONSTRAINT DF_UserStories_Orden DEFAULT 0,
    UsuarioAsignadoId INT NULL,
    FechaCreacion DATETIME CONSTRAINT DF_UserStories_Fecha DEFAULT GETDATE(),
    FechaUltimaModificacion DATETIME CONSTRAINT DF_UserStories_Modif DEFAULT GETDATE(),
    CONSTRAINT FK_UserStories_Proyectos FOREIGN KEY (ProyectoId) REFERENCES dbo.Proyectos(ProyectoId),
    CONSTRAINT FK_UserStories_Epics FOREIGN KEY (EpicId) REFERENCES dbo.Epics(EpicId),
    CONSTRAINT FK_UserStories_Sprints FOREIGN KEY (SprintId) REFERENCES dbo.Sprints(SprintId),
    CONSTRAINT FK_UserStories_Usuarios FOREIGN KEY (UsuarioAsignadoId) REFERENCES dbo.Usuarios(UsuarioId)
);

-- ----------------------------------------------------------------------------
-- TABLA 9: Tareas (Desglose Técnico de cada User Story)
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Tareas (
    TareaId INT IDENTITY(1,1) CONSTRAINT PK_Tareas PRIMARY KEY,
    UserStoryId INT NOT NULL,
    TituloTarea VARCHAR(200) NOT NULL,
    HorasEstimadas DECIMAL(5,2) NOT NULL CONSTRAINT DF_Tareas_HorasEst DEFAULT 0.00,
    HorasTrabajadas DECIMAL(5,2) NOT NULL CONSTRAINT DF_Tareas_HorasTrab DEFAULT 0.00,
    Estado VARCHAR(20) NOT NULL CONSTRAINT DF_Tareas_Estado DEFAULT 'Pendiente', -- Pendiente, En Proceso, Completado
    UsuarioAsignadoId INT NULL,
    FechaCreacion DATETIME CONSTRAINT DF_Tareas_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Tareas_UserStories FOREIGN KEY (UserStoryId) REFERENCES dbo.UserStories(UserStoryId) ON DELETE CASCADE,
    CONSTRAINT FK_Tareas_Usuarios FOREIGN KEY (UsuarioAsignadoId) REFERENCES dbo.Usuarios(UsuarioId)
);

-- ----------------------------------------------------------------------------
-- TABLA 10: Bugs
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Bugs (
    BugId INT IDENTITY(1,1) CONSTRAINT PK_Bugs PRIMARY KEY,
    CodigoBug VARCHAR(20) NOT NULL CONSTRAINT UQ_Bugs_Codigo UNIQUE, -- Ej: BUG-SGPS-01
    UserStoryId INT NULL,
    ProyectoId INT NOT NULL,
    Titulo VARCHAR(200) NOT NULL,
    PasosReproducir VARCHAR(MAX) NOT NULL,
    Severidad VARCHAR(20) NOT NULL CONSTRAINT DF_Bugs_Severidad DEFAULT 'Media', -- Bloqueante, Alta, Media, Baja
    Estado VARCHAR(20) NOT NULL CONSTRAINT DF_Bugs_Estado DEFAULT 'Nuevo', -- Nuevo, En Proceso, Resuelto, Cerrado
    UsuarioReportaId INT NOT NULL,
    UsuarioAsignadoId INT NULL,
    FechaReporte DATETIME CONSTRAINT DF_Bugs_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Bugs_UserStories FOREIGN KEY (UserStoryId) REFERENCES dbo.UserStories(UserStoryId),
    CONSTRAINT FK_Bugs_Proyectos FOREIGN KEY (ProyectoId) REFERENCES dbo.Proyectos(ProyectoId),
    CONSTRAINT FK_Bugs_UsuarioReporta FOREIGN KEY (UsuarioReportaId) REFERENCES dbo.Usuarios(UsuarioId),
    CONSTRAINT FK_Bugs_UsuarioAsignado FOREIGN KEY (UsuarioAsignadoId) REFERENCES dbo.Usuarios(UsuarioId)
);

-- ----------------------------------------------------------------------------
-- TABLA 11: Comentarios
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Comentarios (
    ComentarioId INT IDENTITY(1,1) CONSTRAINT PK_Comentarios PRIMARY KEY,
    UserStoryId INT NOT NULL,
    UsuarioId INT NOT NULL,
    TextoComentario VARCHAR(MAX) NOT NULL,
    FechaComentario DATETIME CONSTRAINT DF_Comentarios_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Comentarios_UserStories FOREIGN KEY (UserStoryId) REFERENCES dbo.UserStories(UserStoryId) ON DELETE CASCADE,
    CONSTRAINT FK_Comentarios_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(UsuarioId)
);

-- ----------------------------------------------------------------------------
-- TABLA 12: Notificaciones
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Notificaciones (
    NotificacionId INT IDENTITY(1,1) CONSTRAINT PK_Notificaciones PRIMARY KEY,
    UsuarioId INT NOT NULL,
    UserStoryId INT NULL,
    Titulo VARCHAR(100) NOT NULL,
    Mensaje VARCHAR(500) NOT NULL,
    Leido BIT NOT NULL CONSTRAINT DF_Notificaciones_Leido DEFAULT 0,
    FechaNotificacion DATETIME CONSTRAINT DF_Notificaciones_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Notificaciones_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(UsuarioId) ON DELETE CASCADE,
    CONSTRAINT FK_Notificaciones_UserStories FOREIGN KEY (UserStoryId) REFERENCES dbo.UserStories(UserStoryId)
);

-- ----------------------------------------------------------------------------
-- TABLA 13: HistorialCambios (Audit Trail)
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.HistorialCambios (
    HistorialId INT IDENTITY(1,1) CONSTRAINT PK_HistorialCambios PRIMARY KEY,
    Entidad VARCHAR(50) NOT NULL, -- UserStory, Sprint, Bug, Proyecto
    EntidadId INT NOT NULL,
    CampoModificado VARCHAR(50) NOT NULL,
    ValorAnterior VARCHAR(MAX) NULL,
    ValorNuevo VARCHAR(MAX) NULL,
    UsuarioId INT NOT NULL,
    FechaModificacion DATETIME CONSTRAINT DF_Historial_Fecha DEFAULT GETDATE(),
    CONSTRAINT FK_Historial_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(UsuarioId)
);
GO

-- ----------------------------------------------------------------------------
-- ÍNDICES DE RENDIMIENTO PARA CONSULTAS RÁPIDAS
-- ----------------------------------------------------------------------------
CREATE INDEX IX_UserStories_ProyectoPrioridad ON dbo.UserStories(ProyectoId, OrdenPrioridad);
CREATE INDEX IX_UserStories_SprintEstado ON dbo.UserStories(SprintId, Estado);
CREATE INDEX IX_Bugs_ProyectoEstado ON dbo.Bugs(ProyectoId, Estado);
CREATE INDEX IX_Tareas_UserStory ON dbo.Tareas(UserStoryId);
CREATE INDEX IX_Notificaciones_UsuarioLeido ON dbo.Notificaciones(UsuarioId, Leido);
GO
