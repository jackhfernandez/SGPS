/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera los procedimientos almacenados que consumen
 *    ComentarioAD.cs y NotificacionAD.cs (TASK-AD-08) sobre dbo.Comentarios y
 *    dbo.Notificaciones, siguiendo el formato de SGPS_pa_Proyecto.sql."
 * 3. Cambios del equipo: Los dos grupos de procedimientos van en un mismo
 *    script porque la issue TASK-AD-08 entrega ambas entidades juntas como
 *    modulo de mensajeria. sp_Notificacion_MarcarLeida filtra tambien por
 *    Leido = 0, de modo que marcar dos veces la misma notificacion no cuenta
 *    como cambio. Los procedimientos de escritura terminan con SELECT
 *    @@ROWCOUNT porque SET NOCOUNT ON impide que ExecuteNonQuery reciba el
 *    conteo de filas.
 */

USE SGPS_DB;
GO

-- ============================================================================
-- COMENTARIOS (dbo.Comentarios)
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1. PROCEDIMIENTO ALMACENADO: Insertar Comentario
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Comentario_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Comentario_Insertar;
GO

CREATE PROCEDURE sp_Comentario_Insertar
    @userStoryId     INT,
    @usuarioId       INT,
    @textoComentario VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Comentarios
        (UserStoryId, UsuarioId, TextoComentario)
    VALUES
        (@userStoryId, @usuarioId, @textoComentario);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ComentarioId;
END;
GO

-- ----------------------------------------------------------------------------
-- 2. PROCEDIMIENTO ALMACENADO: Listar Comentarios de una User Story
-- El JOIN con dbo.Usuarios devuelve el nombre del autor que muestra el hilo
-- de comentarios junto al timestamp (CA-16.3).
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Comentario_ListarPorUserStory', 'P') IS NOT NULL
    DROP PROCEDURE sp_Comentario_ListarPorUserStory;
GO

CREATE PROCEDURE sp_Comentario_ListarPorUserStory
    @userStoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.ComentarioId,
        c.UserStoryId,
        c.UsuarioId,
        c.TextoComentario,
        c.FechaComentario,
        u.Nombres + ' ' + u.Apellidos AS AutorNombre
    FROM dbo.Comentarios AS c
    INNER JOIN dbo.Usuarios AS u ON u.UsuarioId = c.UsuarioId
    WHERE c.UserStoryId = @userStoryId
    ORDER BY c.FechaComentario ASC;
END;
GO

-- ============================================================================
-- NOTIFICACIONES (dbo.Notificaciones)
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 3. PROCEDIMIENTO ALMACENADO: Insertar Notificacion
-- Lo invocan la asignacion de tareas y las menciones @usuario de los
-- comentarios (CA-13.4, CA-16.4).
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Notificacion_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Notificacion_Insertar;
GO

CREATE PROCEDURE sp_Notificacion_Insertar
    @usuarioId   INT,
    @userStoryId INT,
    @titulo      VARCHAR(100),
    @mensaje     VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Notificaciones
        (UsuarioId, UserStoryId, Titulo, Mensaje)
    VALUES
        (@usuarioId, @userStoryId, @titulo, @mensaje);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NotificacionId;
END;
GO

-- ----------------------------------------------------------------------------
-- 4. PROCEDIMIENTO ALMACENADO: Notificaciones pendientes de un usuario
-- Aprovecha el indice IX_Notificaciones_UsuarioLeido.
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Notificacion_ListarPendientes', 'P') IS NOT NULL
    DROP PROCEDURE sp_Notificacion_ListarPendientes;
GO

CREATE PROCEDURE sp_Notificacion_ListarPendientes
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        NotificacionId,
        UsuarioId,
        UserStoryId,
        Titulo,
        Mensaje,
        Leido,
        FechaNotificacion
    FROM dbo.Notificaciones
    WHERE UsuarioId = @usuarioId
      AND Leido = 0
    ORDER BY FechaNotificacion DESC;
END;
GO

-- ----------------------------------------------------------------------------
-- 5. PROCEDIMIENTO ALMACENADO: Marcar una notificacion como leida
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Notificacion_MarcarLeida', 'P') IS NOT NULL
    DROP PROCEDURE sp_Notificacion_MarcarLeida;
GO

CREATE PROCEDURE sp_Notificacion_MarcarLeida
    @notificacionId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Notificaciones
    SET Leido = 1
    WHERE NotificacionId = @notificacionId
      AND Leido = 0;

    -- SET NOCOUNT ON impide que ExecuteNonQuery reciba el conteo de filas,
    -- por eso se devuelve @@ROWCOUNT de forma explicita.
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ----------------------------------------------------------------------------
-- 6. PROCEDIMIENTO ALMACENADO: Marcar todas como leidas (CA-17.4)
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Notificacion_MarcarTodasLeidas', 'P') IS NOT NULL
    DROP PROCEDURE sp_Notificacion_MarcarTodasLeidas;
GO

CREATE PROCEDURE sp_Notificacion_MarcarTodasLeidas
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Notificaciones
    SET Leido = 1
    WHERE UsuarioId = @usuarioId
      AND Leido = 0;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ----------------------------------------------------------------------------
-- 7. PROCEDIMIENTO ALMACENADO: Contador del badge de la campana (CA-17.1)
-- ----------------------------------------------------------------------------
IF OBJECT_ID('sp_Notificacion_ContarNoLeidas', 'P') IS NOT NULL
    DROP PROCEDURE sp_Notificacion_ContarNoLeidas;
GO

CREATE PROCEDURE sp_Notificacion_ContarNoLeidas
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS NoLeidas
    FROM dbo.Notificaciones
    WHERE UsuarioId = @usuarioId
      AND Leido = 0;
END;
GO
