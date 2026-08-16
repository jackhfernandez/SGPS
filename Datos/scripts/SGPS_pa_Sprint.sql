/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume SprintAD.cs
 *    (TASK-AD-05) sobre dbo.Sprints y dbo.UserStories, siguiendo el formato de
 *    SGPS_pa_Proyecto.sql. Incluye el calculo de puntos pendientes por dia
 *    para el Burndown Chart."
 * 3. Cambios del equipo: El burndown se resuelve con un CTE recursivo que
 *    genera el calendario del Sprint y compara FechaUltimaModificacion de cada
 *    historia contra cada dia, en vez de leer dbo.HistorialCambios, que
 *    todavia no la alimenta ningun modulo. Las reglas de negocio de
 *    sp_Sprint_Iniciar viven en el WHERE (estado Planificado, al menos una
 *    historia y ningun otro Sprint activo) para que la comprobacion y la
 *    escritura sean una sola operacion atomica. Los procedimientos de
 *    escritura terminan con SELECT @@ROWCOUNT porque SET NOCOUNT ON impide
 *    que ExecuteNonQuery reciba el conteo de filas.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Crear Sprint
-- El estado inicial lo fija el DEFAULT de la tabla ('Planificado').
-- ============================================================================
IF OBJECT_ID('sp_Sprint_Crear', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_Crear;
GO

CREATE PROCEDURE sp_Sprint_Crear
    @proyectoId   INT,
    @nombreSprint VARCHAR(100),
    @sprintGoal   VARCHAR(MAX),
    @fechaInicio  DATE,
    @fechaFin     DATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Sprints
        (ProyectoId, NombreSprint, SprintGoal, FechaInicio, FechaFin)
    VALUES
        (@proyectoId, @nombreSprint, @sprintGoal, @fechaInicio, @fechaFin);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS SprintId;
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Asignar una Historia al Sprint Backlog
-- Solo toca SprintId, por lo que la historia conserva sus Story Points
-- y su OrdenPrioridad. El INNER JOIN impide asignar una historia a un
-- Sprint de otro proyecto (no afecta filas y el AD devuelve false).
-- ============================================================================
IF OBJECT_ID('sp_Sprint_AsignarHistoria', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_AsignarHistoria;
GO

CREATE PROCEDURE sp_Sprint_AsignarHistoria
    @userStoryId INT,
    @sprintId    INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE us
    SET us.SprintId                = @sprintId,
        us.FechaUltimaModificacion = GETDATE()
    FROM dbo.UserStories AS us
    INNER JOIN dbo.Sprints AS s
        ON s.SprintId = @sprintId
       AND s.ProyectoId = us.ProyectoId
    WHERE us.UserStoryId = @userStoryId;

    -- SET NOCOUNT ON impide que ExecuteNonQuery reciba el conteo de filas,
    -- por eso se devuelve @@ROWCOUNT de forma explicita.
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Iniciar Sprint (congela la linea base)
-- Las guardas del WHERE implementan CA-10.1 y CA-10.2: el Sprint debe estar
-- 'Planificado', tener al menos una historia asignada y no puede haber otro
-- Sprint 'Activo' en el mismo proyecto.
-- ============================================================================
IF OBJECT_ID('sp_Sprint_Iniciar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_Iniciar;
GO

CREATE PROCEDURE sp_Sprint_Iniciar
    @sprintId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
    SET s.Estado = 'Activo'
    FROM dbo.Sprints AS s
    WHERE s.SprintId = @sprintId
      AND s.Estado = 'Planificado'
      AND EXISTS (
            SELECT 1
            FROM dbo.UserStories AS us
            WHERE us.SprintId = s.SprintId)
      AND NOT EXISTS (
            SELECT 1
            FROM dbo.Sprints AS otro
            WHERE otro.ProyectoId = s.ProyectoId
              AND otro.Estado = 'Activo'
              AND otro.SprintId <> s.SprintId);

    -- 0 filas = alguna de las guardas anteriores no se cumplio.
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Obtener el Sprint activo de un proyecto
-- Lo consumen la validacion de solapamiento de fechas (CA-8.3) y la
-- habilitacion del boton "Iniciar Sprint" (CA-10.1).
-- ============================================================================
IF OBJECT_ID('sp_Sprint_ObtenerActivo', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_ObtenerActivo;
GO

CREATE PROCEDURE sp_Sprint_ObtenerActivo
    @proyectoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        s.SprintId,
        s.ProyectoId,
        s.NombreSprint,
        s.SprintGoal,
        s.FechaInicio,
        s.FechaFin,
        s.Estado,
        s.FechaCreacion,
        p.NombreProyecto
    FROM dbo.Sprints AS s
    INNER JOIN dbo.Proyectos AS p ON p.ProyectoId = s.ProyectoId
    WHERE s.ProyectoId = @proyectoId
      AND s.Estado = 'Activo'
    ORDER BY s.FechaInicio DESC;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Listar Sprints de un proyecto
-- ============================================================================
IF OBJECT_ID('sp_Sprint_ListarPorProyecto', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_ListarPorProyecto;
GO

CREATE PROCEDURE sp_Sprint_ListarPorProyecto
    @proyectoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.SprintId,
        s.ProyectoId,
        s.NombreSprint,
        s.SprintGoal,
        s.FechaInicio,
        s.FechaFin,
        s.Estado,
        s.FechaCreacion,
        p.NombreProyecto
    FROM dbo.Sprints AS s
    INNER JOIN dbo.Proyectos AS p ON p.ProyectoId = s.ProyectoId
    WHERE s.ProyectoId = @proyectoId
    ORDER BY s.FechaInicio DESC;
END;
GO

-- ============================================================================
-- 6. PROCEDIMIENTO ALMACENADO: Serie de puntos pendientes del Burndown Chart
-- Recorre dia a dia el rango del Sprint y suma los Story Points de las
-- historias que TODAVIA no estaban en 'Done' al cierre de ese dia.
-- La linea guia ideal (CA-15.2) la calcula la capa de negocio con el primer
-- y el ultimo punto de esta serie.
-- ============================================================================
IF OBJECT_ID('sp_Sprint_BurndownPuntosPendientes', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_BurndownPuntosPendientes;
GO

CREATE PROCEDURE sp_Sprint_BurndownPuntosPendientes
    @sprintId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fechaInicio DATE;
    DECLARE @fechaFin    DATE;

    SELECT @fechaInicio = FechaInicio,
           @fechaFin    = FechaFin
    FROM dbo.Sprints
    WHERE SprintId = @sprintId;

    IF @fechaInicio IS NULL
        RETURN;

    ;WITH Calendario AS
    (
        SELECT @fechaInicio AS Dia
        UNION ALL
        SELECT DATEADD(DAY, 1, Dia)
        FROM Calendario
        WHERE Dia < @fechaFin
    )
    SELECT
        c.Dia AS Fecha,
        ISNULL(SUM(
            CASE
                WHEN us.Estado = 'Done'
                 AND CAST(us.FechaUltimaModificacion AS DATE) <= c.Dia THEN 0
                ELSE us.StoryPoints
            END), 0) AS PuntosPendientes
    FROM Calendario AS c
    LEFT JOIN dbo.UserStories AS us
        ON us.SprintId = @sprintId
    GROUP BY c.Dia
    ORDER BY c.Dia
    OPTION (MAXRECURSION 366);
END;
GO

-- ============================================================================
-- 7. PROCEDIMIENTO ALMACENADO: Listar las historias del Sprint Backlog
-- Devuelve las UserStories asignadas a un Sprint con el titulo del Epic y el
-- nombre del usuario asignado, para que la planificacion muestre el desglose
-- sin consultas adicionales.
-- ============================================================================
IF OBJECT_ID('sp_Sprint_ListarHistorias', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_ListarHistorias;
GO

CREATE PROCEDURE sp_Sprint_ListarHistorias
    @sprintId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        us.UserStoryId,
        us.CodigoTicket,
        us.ProyectoId,
        us.EpicId,
        us.SprintId,
        us.Titulo,
        us.ComoUsuario,
        us.QuieroFuncionalidad,
        us.ParaBeneficio,
        us.CriteriosAceptacionTexto,
        us.ValorNegocio,
        us.StoryPoints,
        us.Estado,
        us.OrdenPrioridad,
        us.UsuarioAsignadoId,
        us.FechaCreacion,
        us.FechaUltimaModificacion,
        e.Titulo AS EpicNombre,
        CONCAT(u.Nombres, ' ', u.Apellidos) AS UsuarioAsignadoNombre
    FROM dbo.UserStories AS us
    LEFT JOIN dbo.Epics AS e ON e.EpicId = us.EpicId
    LEFT JOIN dbo.Usuarios AS u ON u.UsuarioId = us.UsuarioAsignadoId
    WHERE us.SprintId = @sprintId
    ORDER BY us.OrdenPrioridad ASC, us.CodigoTicket ASC;
END;
GO

-- ============================================================================
-- 8. PROCEDIMIENTO ALMACENADO: Listar el backlog disponible para planificar
-- Historias del proyecto que todavia no estan asignadas a ningun Sprint
-- (SprintId IS NULL) y que aun no estan 'Done'. Es la fuente del panel
-- izquierdo de la planificacion de Sprint.
-- ============================================================================
IF OBJECT_ID('sp_Sprint_ListarBacklogDisponible', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_ListarBacklogDisponible;
GO

CREATE PROCEDURE sp_Sprint_ListarBacklogDisponible
    @proyectoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        us.UserStoryId,
        us.CodigoTicket,
        us.ProyectoId,
        us.EpicId,
        us.SprintId,
        us.Titulo,
        us.ComoUsuario,
        us.QuieroFuncionalidad,
        us.ParaBeneficio,
        us.CriteriosAceptacionTexto,
        us.ValorNegocio,
        us.StoryPoints,
        us.Estado,
        us.OrdenPrioridad,
        us.UsuarioAsignadoId,
        us.FechaCreacion,
        us.FechaUltimaModificacion,
        e.Titulo AS EpicNombre,
        CONCAT(u.Nombres, ' ', u.Apellidos) AS UsuarioAsignadoNombre
    FROM dbo.UserStories AS us
    LEFT JOIN dbo.Epics AS e ON e.EpicId = us.EpicId
    LEFT JOIN dbo.Usuarios AS u ON u.UsuarioId = us.UsuarioAsignadoId
    WHERE us.ProyectoId = @proyectoId
      AND us.SprintId IS NULL
      AND us.Estado <> 'Done'
    ORDER BY us.OrdenPrioridad ASC, us.CodigoTicket ASC;
END;
GO

-- ============================================================================
-- 9. PROCEDIMIENTO ALMACENADO: Quitar una historia del Sprint Backlog
-- Devuelve la historia al backlog (SprintId = NULL). El INNER JOIN restringe
-- la operacion a Sprints en estado 'Planificado': una vez iniciado o cerrado
-- el Sprint su composicion queda congelada y la operacion no afecta filas
-- (el AD devuelve false).
-- ============================================================================
IF OBJECT_ID('sp_Sprint_QuitarHistoria', 'P') IS NOT NULL
    DROP PROCEDURE sp_Sprint_QuitarHistoria;
GO

CREATE PROCEDURE sp_Sprint_QuitarHistoria
    @userStoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE us
    SET us.SprintId                = NULL,
        us.FechaUltimaModificacion = GETDATE()
    FROM dbo.UserStories AS us
    INNER JOIN dbo.Sprints AS s
        ON s.SprintId = us.SprintId
    WHERE us.UserStoryId = @userStoryId
      AND s.Estado = 'Planificado';

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO
