/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume BugAD.cs
 *    (TASK-AD-07) sobre dbo.Bugs, siguiendo el formato de
 *    SGPS_pa_Proyecto.sql."
 * 3. Cambios del equipo: Los filtros de sp_Bug_ListarPorProyecto se declaran
 *    con DEFAULT NULL y se aplican con (@parametro IS NULL OR columna =
 *    @parametro), para resolver con un solo procedimiento el listado completo
 *    y los filtrados por estado o severidad. sp_Bug_ContarBloqueantesAbiertos
 *    considera bloqueantes tanto la severidad Bloqueante como la Alta, y da
 *    por cerrados los estados Resuelto y Cerrado, segun el CA-14.3.
 *    Los procedimientos de escritura terminan con SELECT @@ROWCOUNT porque
 *    SET NOCOUNT ON impide que ExecuteNonQuery reciba el conteo de filas.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Registrar Bug
-- El CodigoBug (BUG-<clave>-NN) lo compone la capa de negocio; aqui solo se
-- inserta y la restriccion UQ_Bugs_Codigo protege la unicidad.
-- El estado inicial 'Nuevo' lo aplica el DEFAULT de la tabla.
-- ============================================================================
IF OBJECT_ID('sp_Bug_Registrar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Bug_Registrar;
GO

CREATE PROCEDURE sp_Bug_Registrar
    @codigoBug         VARCHAR(20),
    @userStoryId       INT,
    @proyectoId        INT,
    @titulo            VARCHAR(200),
    @pasosReproducir   VARCHAR(MAX),
    @severidad         VARCHAR(20),
    @estado            VARCHAR(20),
    @usuarioReportaId  INT,
    @usuarioAsignadoId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Bugs
        (CodigoBug, UserStoryId, ProyectoId, Titulo, PasosReproducir,
         Severidad, Estado, UsuarioReportaId, UsuarioAsignadoId)
    VALUES
        (@codigoBug, @userStoryId, @proyectoId, @titulo, @pasosReproducir,
         @severidad, @estado, @usuarioReportaId, @usuarioAsignadoId);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS BugId;
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Vincular / desvincular un Bug de una User Story
-- Con @userStoryId NULL el Bug se desvincula pero sigue existiendo de forma
-- independiente en el backlog general (CA-14.4).
-- ============================================================================
IF OBJECT_ID('sp_Bug_VincularUserStory', 'P') IS NOT NULL
    DROP PROCEDURE sp_Bug_VincularUserStory;
GO

CREATE PROCEDURE sp_Bug_VincularUserStory
    @bugId       INT,
    @userStoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Bugs
    SET UserStoryId = @userStoryId
    WHERE BugId = @bugId;

    -- SET NOCOUNT ON impide que ExecuteNonQuery reciba el conteo de filas,
    -- por eso se devuelve @@ROWCOUNT de forma explicita.
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Listar Bugs de un proyecto
-- Los filtros son opcionales: con NULL no se aplican.
-- Aprovecha el indice IX_Bugs_ProyectoEstado.
-- ============================================================================
IF OBJECT_ID('sp_Bug_ListarPorProyecto', 'P') IS NOT NULL
    DROP PROCEDURE sp_Bug_ListarPorProyecto;
GO

CREATE PROCEDURE sp_Bug_ListarPorProyecto
    @proyectoId INT,
    @estado     VARCHAR(20) = NULL,
    @severidad  VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        BugId,
        CodigoBug,
        UserStoryId,
        ProyectoId,
        Titulo,
        PasosReproducir,
        Severidad,
        Estado,
        UsuarioReportaId,
        UsuarioAsignadoId,
        FechaReporte
    FROM dbo.Bugs
    WHERE ProyectoId = @proyectoId
      AND (@estado    IS NULL OR Estado    = @estado)
      AND (@severidad IS NULL OR Severidad = @severidad)
    ORDER BY FechaReporte DESC;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Listar Bugs vinculados a una User Story
-- Alimenta la pestana "Bugs Asociados" de la ficha de la historia (CA-14.2).
-- ============================================================================
IF OBJECT_ID('sp_Bug_ListarPorUserStory', 'P') IS NOT NULL
    DROP PROCEDURE sp_Bug_ListarPorUserStory;
GO

CREATE PROCEDURE sp_Bug_ListarPorUserStory
    @userStoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        BugId,
        CodigoBug,
        UserStoryId,
        ProyectoId,
        Titulo,
        PasosReproducir,
        Severidad,
        Estado,
        UsuarioReportaId,
        UsuarioAsignadoId,
        FechaReporte
    FROM dbo.Bugs
    WHERE UserStoryId = @userStoryId
    ORDER BY FechaReporte DESC;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Bugs bloqueantes o altos sin resolver
-- Impide que la historia pase a "Done" mientras devuelva 1 (CA-14.3).
-- ============================================================================
IF OBJECT_ID('sp_Bug_ContarBloqueantesAbiertos', 'P') IS NOT NULL
    DROP PROCEDURE sp_Bug_ContarBloqueantesAbiertos;
GO

CREATE PROCEDURE sp_Bug_ContarBloqueantesAbiertos
    @userStoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS BugsBloqueantes
    FROM dbo.Bugs
    WHERE UserStoryId = @userStoryId
      AND Severidad IN ('Bloqueante', 'Alta')
      AND Estado NOT IN ('Resuelto', 'Cerrado');
END;
GO

-- ============================================================================
-- 6. PROCEDIMIENTO ALMACENADO: Actualizar estado del Bug
-- Ciclo de re-prueba de QA: Nuevo -> En Proceso -> Resuelto -> Cerrado.
-- ============================================================================
IF OBJECT_ID('sp_Bug_ActualizarEstado', 'P') IS NOT NULL
    DROP PROCEDURE sp_Bug_ActualizarEstado;
GO

CREATE PROCEDURE sp_Bug_ActualizarEstado
    @bugId       INT,
    @nuevoEstado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Bugs
    SET Estado = @nuevoEstado
    WHERE BugId = @bugId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO
