USE SGPS_DB;
GO

CREATE PROCEDURE dbo.sp_ObtenerBacklogPriorizado
    @ProyectoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserStoryId,
        CodigoTicket,
        ProyectoId,
        EpicId,
        SprintId,
        Titulo,
        ComoUsuario,
        QuieroFuncionalidad,
        ParaBeneficio,
        CriteriosAceptacionTexto,
        ValorNegocio,
        StoryPoints,
        Estado,
        OrdenPrioridad,
        UsuarioAsignadoId,
        FechaCreacion,
        FechaUltimaModificacion
    FROM dbo.UserStories
    WHERE ProyectoId = @ProyectoId
    ORDER BY OrdenPrioridad ASC, FechaCreacion ASC;
END
GO