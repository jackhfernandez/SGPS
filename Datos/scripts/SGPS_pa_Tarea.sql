/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume TareaAD.cs
 *    (TASK-AD-06) sobre dbo.Tareas, siguiendo el formato de
 *    SGPS_pa_Proyecto.sql."
 * 3. Cambios del equipo: Los INSERT no fijan el estado inicial ni las horas en
 *    cero: se dejan a los DEFAULT de la tabla para no duplicar en el script
 *    una regla que ya vive en el esquema. Los procedimientos de escritura
 *    terminan con SELECT @@ROWCOUNT porque SET NOCOUNT ON impide que
 *    ExecuteNonQuery reciba el conteo de filas.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Insertar Tarea (desglose tecnico de una Historia)
-- El estado inicial 'Pendiente' lo aplica el DEFAULT de la tabla.
-- ============================================================================
IF OBJECT_ID('sp_Tarea_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tarea_Insertar;
GO

CREATE PROCEDURE sp_Tarea_Insertar
    @userStoryId       INT,
    @tituloTarea       VARCHAR(200),
    @horasEstimadas    DECIMAL(5,2),
    @horasTrabajadas   DECIMAL(5,2),
    @estado            VARCHAR(20),
    @usuarioAsignadoId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Tareas
        (UserStoryId, TituloTarea, HorasEstimadas, HorasTrabajadas, Estado, UsuarioAsignadoId)
    VALUES
        (@userStoryId, @tituloTarea, @horasEstimadas, @horasTrabajadas, @estado, @usuarioAsignadoId);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS TareaId;
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Listar Tareas de una User Story
-- Aprovecha el indice IX_Tareas_UserStory.
-- ============================================================================
IF OBJECT_ID('sp_Tarea_ListarPorUserStory', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tarea_ListarPorUserStory;
GO

CREATE PROCEDURE sp_Tarea_ListarPorUserStory
    @userStoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        TareaId,
        UserStoryId,
        TituloTarea,
        HorasEstimadas,
        HorasTrabajadas,
        Estado,
        UsuarioAsignadoId,
        FechaCreacion
    FROM dbo.Tareas
    WHERE UserStoryId = @userStoryId
    ORDER BY FechaCreacion ASC;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Actualizar horas estimadas e imputadas
-- ============================================================================
IF OBJECT_ID('sp_Tarea_ActualizarHoras', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tarea_ActualizarHoras;
GO

CREATE PROCEDURE sp_Tarea_ActualizarHoras
    @tareaId         INT,
    @horasEstimadas  DECIMAL(5,2),
    @horasTrabajadas DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Tareas
    SET HorasEstimadas  = @horasEstimadas,
        HorasTrabajadas = @horasTrabajadas
    WHERE TareaId = @tareaId;

    -- SET NOCOUNT ON impide que ExecuteNonQuery reciba el conteo de filas,
    -- por eso se devuelve @@ROWCOUNT de forma explicita.
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Actualizar estado de la Tarea
-- Lo consume el Drag & Drop del Tablero Kanban (CA-12.1).
-- ============================================================================
IF OBJECT_ID('sp_Tarea_ActualizarEstado', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tarea_ActualizarEstado;
GO

CREATE PROCEDURE sp_Tarea_ActualizarEstado
    @tareaId      INT,
    @nuevoEstado  VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Tareas
    SET Estado = @nuevoEstado
    WHERE TareaId = @tareaId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Asignar responsable de la Tarea
-- Lo consume la auto-asignacion al mover la tarjeta a "En Proceso" (CA-12.2).
-- ============================================================================
IF OBJECT_ID('sp_Tarea_AsignarUsuario', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tarea_AsignarUsuario;
GO

CREATE PROCEDURE sp_Tarea_AsignarUsuario
    @tareaId   INT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Tareas
    SET UsuarioAsignadoId = @usuarioId
    WHERE TareaId = @tareaId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO
