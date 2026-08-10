USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Insertar Proyecto y Creador como PO (Atómico)
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
    @fechaFinEstimada  DATE,
    @creadorUsuarioId  INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Insertar el Proyecto
        INSERT INTO dbo.Proyectos 
            (ClaveProyecto, NombreProyecto, Descripcion, Metodologia, FechaInicio, FechaFinEstimada)
        VALUES 
            (@claveProyecto, @nombreProyecto, @descripcion, @metodologia, @fechaInicio, @fechaFinEstimada);

        DECLARE @nuevoProyectoId INT = SCOPE_IDENTITY();

        -- 2. Inserción atómica del creador como Product Owner (PO)
        INSERT INTO dbo.ProyectoMiembros 
            (ProyectoId, UsuarioId, RolEnProyecto)
        VALUES 
            (@nuevoProyectoId, @creadorUsuarioId, 'PO');

        COMMIT TRANSACTION;

        -- Retornar el ID autogenerado
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
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Listar Proyectos por Usuario
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
-- 4. PROCEDIMIENTO ALMACENADO: Asignar Miembro al Proyecto
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