/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consume
 *    ProyectoMiembroAD.cs sobre dbo.ProyectoMiembros y dbo.Usuarios,
 *    siguiendo el formato de SGPS_pa_Proyecto.sql."
 * 3. Cambios del equipo: sp_ProyectoMiembro_Asignar es idempotente: si el
 *    usuario ya es miembro, actualiza su rol en lugar de duplicar (la PK
 *    (ProyectoId, UsuarioId) no admite duplicados). Los listados unen
 *    dbo.Usuarios para mostrar nombre y correo junto al id. El listado de
 *    disponibles excluye a los miembros actuales y solo devuelve usuarios
 *    activos.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- 1. PROCEDIMIENTO ALMACENADO: Asignar Miembro (inserta o actualiza rol)
-- ============================================================================
IF OBJECT_ID('sp_ProyectoMiembro_Asignar', 'P') IS NOT NULL
    DROP PROCEDURE sp_ProyectoMiembro_Asignar;
GO

CREATE PROCEDURE sp_ProyectoMiembro_Asignar
    @proyectoId    INT,
    @usuarioId     INT,
    @rolEnProyecto VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.ProyectoMiembros
               WHERE ProyectoId = @proyectoId AND UsuarioId = @usuarioId)
    BEGIN
        UPDATE dbo.ProyectoMiembros
        SET RolEnProyecto = @rolEnProyecto
        WHERE ProyectoId = @proyectoId AND UsuarioId = @usuarioId;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.ProyectoMiembros (ProyectoId, UsuarioId, RolEnProyecto)
        VALUES (@proyectoId, @usuarioId, @rolEnProyecto);
    END
END;
GO

-- ============================================================================
-- 2. PROCEDIMIENTO ALMACENADO: Quitar Miembro del Proyecto
-- ============================================================================
IF OBJECT_ID('sp_ProyectoMiembro_Quitar', 'P') IS NOT NULL
    DROP PROCEDURE sp_ProyectoMiembro_Quitar;
GO

CREATE PROCEDURE sp_ProyectoMiembro_Quitar
    @proyectoId INT,
    @usuarioId  INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.ProyectoMiembros
    WHERE ProyectoId = @proyectoId AND UsuarioId = @usuarioId;
END;
GO

-- ============================================================================
-- 3. PROCEDIMIENTO ALMACENADO: Cambiar Rol de un Miembro
-- ============================================================================
IF OBJECT_ID('sp_ProyectoMiembro_CambiarRol', 'P') IS NOT NULL
    DROP PROCEDURE sp_ProyectoMiembro_CambiarRol;
GO

CREATE PROCEDURE sp_ProyectoMiembro_CambiarRol
    @proyectoId    INT,
    @usuarioId     INT,
    @rolEnProyecto VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ProyectoMiembros
    SET RolEnProyecto = @rolEnProyecto
    WHERE ProyectoId = @proyectoId AND UsuarioId = @usuarioId;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTO ALMACENADO: Listar Miembros de un Proyecto
-- ============================================================================
IF OBJECT_ID('sp_ProyectoMiembro_ListarPorProyecto', 'P') IS NOT NULL
    DROP PROCEDURE sp_ProyectoMiembro_ListarPorProyecto;
GO

CREATE PROCEDURE sp_ProyectoMiembro_ListarPorProyecto
    @proyectoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT pm.ProyectoId,
           pm.UsuarioId,
           u.Nombres,
           u.Apellidos,
           u.Email,
           u.EsActivo,
           pm.RolEnProyecto,
           pm.FechaAsignacion
    FROM dbo.ProyectoMiembros pm
    INNER JOIN dbo.Usuarios u ON u.UsuarioId = pm.UsuarioId
    WHERE pm.ProyectoId = @proyectoId
    ORDER BY u.Nombres, u.Apellidos;
END;
GO

-- ============================================================================
-- 5. PROCEDIMIENTO ALMACENADO: Listar Usuarios Disponibles para un Proyecto
-- ============================================================================
IF OBJECT_ID('sp_ProyectoMiembro_ListarDisponibles', 'P') IS NOT NULL
    DROP PROCEDURE sp_ProyectoMiembro_ListarDisponibles;
GO

CREATE PROCEDURE sp_ProyectoMiembro_ListarDisponibles
    @proyectoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.UsuarioId,
           u.Nombres,
           u.Apellidos,
           u.Email,
           u.EsActivo
    FROM dbo.Usuarios u
    LEFT JOIN dbo.ProyectoMiembros pm ON pm.UsuarioId = u.UsuarioId
                                      AND pm.ProyectoId = @proyectoId
    WHERE pm.UsuarioId IS NULL
      AND u.EsActivo = 1
    ORDER BY u.Nombres, u.Apellidos;
END;
GO
