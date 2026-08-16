/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume UsuarioAD.cs
 *    sobre dbo.Usuarios y dbo.UsuarioRoles, siguiendo el formato de
 *    SGPS_pa_Tarea.sql."
 * 3. Cambios del equipo: sp_Usuario_AsignarRoles recibe los identificadores de
 *    rol separados por coma y los reemplaza de forma atomica dentro de una
 *    transaccion usando STRING_SPLIT. El INSERT de Usuario no fija EsActivo ni
 *    FechaRegistro: los aplican los DEFAULT de la tabla. Los procedimientos de
 *    escritura terminan con SELECT @@ROWCOUNT porque SET NOCOUNT ON impide que
 *    ExecuteNonQuery reciba el conteo de filas.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Listar Usuarios
-- ============================================================================
IF OBJECT_ID('sp_Usuario_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_Listar;
GO

CREATE PROCEDURE sp_Usuario_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.UsuarioId,
        u.Nombres,
        u.Apellidos,
        u.Email,
        u.PasswordHash,
        u.PasswordSalt,
        u.EsActivo,
        u.FechaRegistro,
        u.UltimoAcceso
    FROM dbo.Usuarios AS u
    ORDER BY u.Nombres, u.Apellidos;
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Listar roles de todos los usuarios
-- Alimenta el resumen de roles de la grilla (UsuarioGestion).
-- ============================================================================
IF OBJECT_ID('sp_Usuario_ListarRoles', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_ListarRoles;
GO

CREATE PROCEDURE sp_Usuario_ListarRoles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ur.UsuarioId, r.RolId, r.NombreRol, r.Descripcion
    FROM dbo.UsuarioRoles AS ur
    INNER JOIN dbo.Roles AS r ON r.RolId = ur.RolId
    ORDER BY ur.UsuarioId, r.NombreRol;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Insertar Usuario
-- EsActivo y FechaRegistro los aplican los DEFAULT de la tabla.
-- ============================================================================
IF OBJECT_ID('sp_Usuario_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_Insertar;
GO

CREATE PROCEDURE sp_Usuario_Insertar
    @nombres       VARCHAR(100),
    @apellidos     VARCHAR(100),
    @email         VARCHAR(150),
    @passwordHash  VARCHAR(256),
    @passwordSalt  VARCHAR(256),
    @esActivo      BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Usuarios
        (Nombres, Apellidos, Email, PasswordHash, PasswordSalt, EsActivo)
    VALUES
        (@nombres, @apellidos, @email, @passwordHash, @passwordSalt, @esActivo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS UsuarioId;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Modificar Usuario
-- La contrasena se cambia aparte con sp_Usuario_ActualizarPassword.
-- ============================================================================
IF OBJECT_ID('sp_Usuario_Modificar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_Modificar;
GO

CREATE PROCEDURE sp_Usuario_Modificar
    @usuarioId  INT,
    @nombres    VARCHAR(100),
    @apellidos  VARCHAR(100),
    @email      VARCHAR(150),
    @esActivo   BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuarios
    SET Nombres   = @nombres,
        Apellidos = @apellidos,
        Email     = @email,
        EsActivo  = @esActivo
    WHERE UsuarioId = @usuarioId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Activar / Desactivar Usuario
-- ============================================================================
IF OBJECT_ID('sp_Usuario_CambiarEstado', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_CambiarEstado;
GO

CREATE PROCEDURE sp_Usuario_CambiarEstado
    @usuarioId  INT,
    @esActivo   BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuarios
    SET EsActivo = @esActivo
    WHERE UsuarioId = @usuarioId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 6. PROCEDIMIENTO ALMACENADO: Verificar unicidad del correo
-- Con @excluirUsuarioId NULL valida altas; con valor valida ediciones.
-- ============================================================================
IF OBJECT_ID('sp_Usuario_ExisteEmail', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_ExisteEmail;
GO

CREATE PROCEDURE sp_Usuario_ExisteEmail
    @email             VARCHAR(150),
    @excluirUsuarioId  INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1) AS Existe
    FROM dbo.Usuarios
    WHERE Email = LTRIM(RTRIM(@email))
      AND (@excluirUsuarioId IS NULL OR UsuarioId <> @excluirUsuarioId);
END;
GO

-- ============================================================================
-- 7. PROCEDIMIENTO ALMACENADO: Restablecer contrasena de un usuario
-- El hash y el salt se generan en la capa de negocio (PBKDF2-SHA256).
-- ============================================================================
IF OBJECT_ID('sp_Usuario_ActualizarPassword', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_ActualizarPassword;
GO

CREATE PROCEDURE sp_Usuario_ActualizarPassword
    @usuarioId     INT,
    @passwordHash  VARCHAR(256),
    @passwordSalt  VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuarios
    SET PasswordHash = @passwordHash,
        PasswordSalt = @passwordSalt
    WHERE UsuarioId = @usuarioId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================================
-- 8. PROCEDIMIENTO ALMACENADO: Reemplazar roles de un usuario (atomico)
-- Recibe los RolId separados por coma; con cadena vacia solo se limpian.
-- ============================================================================
IF OBJECT_ID('sp_Usuario_AsignarRoles', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuario_AsignarRoles;
GO

CREATE PROCEDURE sp_Usuario_AsignarRoles
    @usuarioId  INT,
    @rolIds     VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM dbo.UsuarioRoles
        WHERE UsuarioId = @usuarioId;

        INSERT INTO dbo.UsuarioRoles (UsuarioId, RolId)
        SELECT @usuarioId, CAST(s.value AS INT)
        FROM STRING_SPLIT(@rolIds, ',') AS s
        WHERE LTRIM(RTRIM(s.value)) <> '';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
