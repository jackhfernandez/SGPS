-- ============================================================================
-- SISTEMA DE GESTIÓN DE PROYECTOS DE SOFTWARE (SGPS)
-- Script de Procedimientos Almacenados
-- Requisito previo: ejecutar SGPS_crea.sql
-- El script es re-ejecutable (CREATE OR ALTER).
-- ============================================================================

USE SGPS_DB;
GO

-- ----------------------------------------------------------------------------
-- sp_AutenticarUsuario
-- Recupera los datos y las credenciales almacenadas del usuario cuyo correo
-- coincide con @Email. La verificación del hash (PBKDF2) se realiza en la capa
-- de lógica de negocio, por lo que este procedimiento nunca recibe ni devuelve
-- la contraseña en texto plano.
-- Devuelve 0 filas si el correo no está registrado y 1 fila en caso contrario
-- (dbo.Usuarios.Email tiene restricción UNIQUE).
-- Se devuelve EsActivo para que la capa superior distinga entre credenciales
-- inválidas y cuenta desactivada.
-- ----------------------------------------------------------------------------
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
