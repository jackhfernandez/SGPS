/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume ProyectoAD.cs
 *    sobre dbo.Proyectos y dbo.ProyectoMiembros, siguiendo el formato de
 *    SGPS_pa_Tarea.sql."
 * 3. Cambios del equipo: sp_Proyecto_Insertar recibe @creadorUsuarioId con
 *    DEFAULT NULL: si viene, registra de forma atomica al creador como PO
 *    dentro de la misma transaccion; si no viene (flujo que inserta sus
 *    propios miembros), se omite. Se incorporan sp_Proyecto_Listar,
 *    sp_Proyecto_ExisteClave y sp_Proyecto_CambiarEstado para el CRUD del
 *    modulo ProyectoCreacion. sp_Proyecto_CambiarEstado aplica FechaFinReal
 *    al desactivar, igual que lo hacia la consulta inline anterior. Los
 *    INSERT no fijan EsActivo ni FechaCreacion: los aplican los DEFAULT.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Insertar Proyecto y Creador como PO (Atomico)
-- El estado inicial 'EsActivo=1' lo aplica el DEFAULT de la tabla.
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_Insertar;
GO

CREATE PROCEDURE sp_Proyecto_Insertar
    @claveProyecto     VARCHAR(10),
    @nombreProyecto    VARCHAR(150),
    @descripcion       VARCHAR(MAX),
    @metodologia       VARCHAR(30),
    @fechaInicio       DATE,
    @fechaFinEstimada  DATE = NULL,
    @creadorUsuarioId  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO dbo.Proyectos
            (ClaveProyecto, NombreProyecto, Descripcion, Metodologia, FechaInicio, FechaFinEstimada)
        VALUES
            (@claveProyecto, @nombreProyecto, @descripcion, @metodologia, @fechaInicio, @fechaFinEstimada);

        DECLARE @nuevoProyectoId INT = SCOPE_IDENTITY();

        IF @creadorUsuarioId IS NOT NULL
        BEGIN
            INSERT INTO dbo.ProyectoMiembros (ProyectoId, UsuarioId, RolEnProyecto)
            VALUES (@nuevoProyectoId, @creadorUsuarioId, 'PO');
        END

        COMMIT TRANSACTION;

        SELECT @nuevoProyectoId AS ProyectoId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Modificar Proyecto
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_Modificar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_Modificar;
GO

CREATE PROCEDURE sp_Proyecto_Modificar
    @proyectoId        INT,
    @claveProyecto     VARCHAR(10),
    @nombreProyecto    VARCHAR(150),
    @descripcion       VARCHAR(MAX),
    @metodologia       VARCHAR(30),
    @fechaInicio       DATE,
    @fechaFinEstimada  DATE,
    @esActivo          BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Proyectos
    SET
        ClaveProyecto    = @claveProyecto,
        NombreProyecto   = @nombreProyecto,
        Descripcion      = @descripcion,
        Metodologia      = @metodologia,
        FechaInicio      = @fechaInicio,
        FechaFinEstimada = @fechaFinEstimada,
        EsActivo         = @esActivo
    WHERE ProyectoId = @proyectoId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Listar todos los Proyectos
-- Alimenta la grilla del modulo ProyectoCreacion (activos e inactivos).
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_Listar;
GO

CREATE PROCEDURE sp_Proyecto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ProyectoId,
        ClaveProyecto,
        NombreProyecto,
        Descripcion,
        Metodologia,
        FechaInicio,
        FechaFinEstimada,
        FechaFinReal,
        EsActivo,
        FechaCreacion
    FROM dbo.Proyectos
    ORDER BY FechaCreacion DESC;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Listar Proyectos por Usuario (activos)
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_ListarPorUsuario', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_ListarPorUsuario;
GO

CREATE PROCEDURE sp_Proyecto_ListarPorUsuario
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ProyectoId,
        p.ClaveProyecto,
        p.NombreProyecto,
        p.Descripcion,
        p.Metodologia,
        p.FechaInicio,
        p.FechaFinEstimada,
        p.FechaFinReal,
        p.EsActivo,
        p.FechaCreacion
    FROM dbo.Proyectos p
    INNER JOIN dbo.ProyectoMiembros pm ON p.ProyectoId = pm.ProyectoId
    WHERE pm.UsuarioId = @usuarioId AND p.EsActivo = 1
    ORDER BY p.FechaCreacion DESC;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Activar / Desactivar Proyecto
-- Al desactivar se fija FechaFinReal; al reactivar vuelve a NULL.
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_CambiarEstado', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_CambiarEstado;
GO

CREATE PROCEDURE sp_Proyecto_CambiarEstado
    @proyectoId  INT,
    @esActivo    BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Proyectos
    SET EsActivo = @esActivo,
        FechaFinReal = CASE WHEN @esActivo = 0 THEN GETDATE() ELSE NULL END
    WHERE ProyectoId = @proyectoId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 6. PROCEDIMIENTO ALMACENADO: Verificar unicidad de la clave del proyecto
-- Con @excluirProyectoId NULL valida altas; con valor valida ediciones.
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_ExisteClave', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_ExisteClave;
GO

CREATE PROCEDURE sp_Proyecto_ExisteClave
    @claveProyecto      VARCHAR(10),
    @excluirProyectoId  INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1) AS Existe
    FROM dbo.Proyectos
    WHERE UPPER(LTRIM(RTRIM(ClaveProyecto))) = UPPER(LTRIM(RTRIM(@claveProyecto)))
      AND (@excluirProyectoId IS NULL OR ProyectoId <> @excluirProyectoId);
END;
GO

-- ============================================================================
-- 7. PROCEDIMIENTO ALMACENADO: Asignar Miembro al Proyecto
-- ============================================================================
IF OBJECT_ID('sp_Proyecto_AsignarMiembro', 'P') IS NOT NULL
    DROP PROCEDURE sp_Proyecto_AsignarMiembro;
GO

CREATE PROCEDURE sp_Proyecto_AsignarMiembro
    @proyectoId    INT,
    @usuarioId     INT,
    @rolEnProyecto VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ProyectoMiembros (ProyectoId, UsuarioId, RolEnProyecto)
    VALUES (@proyectoId, @usuarioId, @rolEnProyecto);
END;
GO
