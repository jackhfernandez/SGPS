-- ============================================================================
-- SISTEMA DE GESTIÓN DE PROYECTOS DE SOFTWARE (SGPS)
-- Script de Procedimientos Almacenados
-- Requisito previo: ejecutar SGPS_crea.sql
-- El script es re-ejecutable (CREATE OR ALTER).
-- ============================================================================

USE SGPS_DB;
GO

-- sp_AutenticarUsuario
-- Recupera los datos y las credenciales almacenadas del usuario
CREATE OR ALTER PROCEDURE dbo.sp_AutenticarUsuario
    @Email VARCHAR(150)
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
    WHERE u.Email = LTRIM(RTRIM(@Email));
END
GO
