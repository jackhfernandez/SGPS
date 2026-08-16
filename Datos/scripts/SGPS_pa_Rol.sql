/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume RolAD.cs
 *    sobre dbo.Roles, siguiendo el formato de SGPS_pa_Tarea.sql."
 * 3. Cambios del equipo: La tabla dbo.Roles incorpora EsActivo (ver
 *    SGPS_crea.sql) para soportar la baja logica del modulo RolGestion; por
 *    eso los procedimientos son CRUD + cambio de estado en lugar de borrado
 *    fisico. Los INSERT no fijan EsActivo ni FechaCreacion: se dejan a los
 *    DEFAULT de la tabla. Los procedimientos de escritura terminan con
 *    SELECT @@ROWCOUNT porque SET NOCOUNT ON impide que ExecuteNonQuery
 *    reciba el conteo de filas.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Listar Roles
-- Devuelve activos e inactivos para que la gestion pueda re-activarlos.
-- ============================================================================
IF OBJECT_ID('sp_Rol_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_Listar;
GO

CREATE PROCEDURE sp_Rol_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RolId,
        NombreRol,
        Descripcion,
        EsActivo,
        FechaCreacion
    FROM dbo.Roles
    ORDER BY NombreRol;
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Conteo de usuarios por rol
-- Alimenta la columna "Usuarios" de la grilla de roles (RolGestion).
-- ============================================================================
IF OBJECT_ID('sp_Rol_ConteoUsuarios', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_ConteoUsuarios;
GO

CREATE PROCEDURE sp_Rol_ConteoUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ur.RolId, COUNT(1) AS Conteo
    FROM dbo.UsuarioRoles AS ur
    GROUP BY ur.RolId;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Insertar Rol
-- EsActivo y FechaCreacion los aplican los DEFAULT de la tabla.
-- ============================================================================
IF OBJECT_ID('sp_Rol_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_Insertar;
GO

CREATE PROCEDURE sp_Rol_Insertar
    @nombreRol    VARCHAR(50),
    @descripcion  VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Roles (NombreRol, Descripcion)
    VALUES (@nombreRol, @descripcion);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS RolId;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Modificar Rol
-- ============================================================================
IF OBJECT_ID('sp_Rol_Modificar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_Modificar;
GO

CREATE PROCEDURE sp_Rol_Modificar
    @rolId        INT,
    @nombreRol    VARCHAR(50),
    @descripcion  VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Roles
    SET NombreRol   = @nombreRol,
        Descripcion = @descripcion
    WHERE RolId = @rolId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Activar / Desactivar Rol (baja logica)
-- ============================================================================
IF OBJECT_ID('sp_Rol_CambiarEstado', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_CambiarEstado;
GO

CREATE PROCEDURE sp_Rol_CambiarEstado
    @rolId    INT,
    @esActivo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Roles
    SET EsActivo = @esActivo
    WHERE RolId = @rolId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 6. PROCEDIMIENTO ALMACENADO: Verificar unicidad del nombre del rol
-- Con @excluirRolId NULL valida altas; con valor valida ediciones.
-- ============================================================================
IF OBJECT_ID('sp_Rol_ExisteNombre', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_ExisteNombre;
GO

CREATE PROCEDURE sp_Rol_ExisteNombre
    @nombreRol    VARCHAR(50),
    @excluirRolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1) AS Existe
    FROM dbo.Roles
    WHERE NombreRol = LTRIM(RTRIM(@nombreRol))
      AND (@excluirRolId IS NULL OR RolId <> @excluirRolId);
END;
GO

-- ============================================================================
-- 7. PROCEDIMIENTO ALMACENADO: Verificar si un rol tiene usuarios asignados
-- ============================================================================
IF OBJECT_ID('sp_Rol_TieneUsuarios', 'P') IS NOT NULL
    DROP PROCEDURE sp_Rol_TieneUsuarios;
GO

CREATE PROCEDURE sp_Rol_TieneUsuarios
    @rolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1) AS Conteo
    FROM dbo.UsuarioRoles
    WHERE RolId = @rolId;
END;
GO
