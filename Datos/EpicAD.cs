/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo EpicAD.cs basandote en la entidad Epic y en
 *    la tabla dbo.Epics del script SGPS_crea.sql. Implementa listar por
 *    proyecto, obtener por id, insertar, actualizar, eliminar y el conteo de
 *    historias asociadas, respetando la estructura actual del proyecto."
 * 3. Cambios del equipo: Se usa SQL parametrizado en linea (como ProyectoAD
 *    y UserStoryAD) en lugar de procedimientos almacenados, porque no existe
 *    un SGPS_pa_Epic.sql y asi el modulo funciona sobre la base ya desplegada
 *    sin ejecutar scripts nuevos.
 */

using Microsoft.Data.SqlClient;
using Modelo;
using System.Data;

namespace Datos;

public class EpicAD
{
    private const string CamposEpic = @"
        e.EpicId, e.ProyectoId, e.Titulo, e.Descripcion, e.ColorHex, e.FechaCreacion,
        p.NombreProyecto";

    public List<Epic> ListarPorProyecto(int proyectoId)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand($@"
            SELECT {CamposEpic}
            FROM dbo.Epics e
            INNER JOIN dbo.Proyectos p ON p.ProyectoId = e.ProyectoId
            WHERE e.ProyectoId = @proyectoId
            ORDER BY e.Titulo;", conexion);

        comando.Parameters.Add("@proyectoId", SqlDbType.Int).Value = proyectoId;

        using var lector = comando.ExecuteReader();

        var epics = new List<Epic>();

        while (lector.Read())
        {
            epics.Add(MapearEpic(lector));
        }

        return epics;
    }

    public Epic? ObtenerPorId(int epicId)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand($@"
            SELECT {CamposEpic}
            FROM dbo.Epics e
            INNER JOIN dbo.Proyectos p ON p.ProyectoId = e.ProyectoId
            WHERE e.EpicId = @epicId;", conexion);

        comando.Parameters.Add("@epicId", SqlDbType.Int).Value = epicId;

        using var lector = comando.ExecuteReader();

        return lector.Read() ? MapearEpic(lector) : null;
    }

    public int InsertarEpic(Epic epic)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(@"
            INSERT INTO dbo.Epics (ProyectoId, Titulo, Descripcion, ColorHex, FechaCreacion)
            VALUES (@proyectoId, @titulo, @descripcion, @colorHex, @fechaCreacion);
            SELECT CAST(SCOPE_IDENTITY() AS INT);", conexion);

        comando.Parameters.Add("@proyectoId", SqlDbType.Int).Value = epic.ProyectoId;
        comando.Parameters.Add("@titulo", SqlDbType.VarChar, 200).Value = epic.Titulo;
        comando.Parameters.Add("@descripcion", SqlDbType.VarChar).Value =
            (object?)epic.Descripcion ?? DBNull.Value;
        comando.Parameters.Add("@colorHex", SqlDbType.VarChar, 7).Value = epic.ColorHex;
        comando.Parameters.Add("@fechaCreacion", SqlDbType.DateTime).Value =
            epic.FechaCreacion == default ? DateTime.Now : epic.FechaCreacion;

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public void ActualizarEpic(Epic epic)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(@"
            UPDATE dbo.Epics
            SET Titulo = @titulo,
                Descripcion = @descripcion,
                ColorHex = @colorHex
            WHERE EpicId = @epicId;", conexion);

        comando.Parameters.Add("@epicId", SqlDbType.Int).Value = epic.EpicId;
        comando.Parameters.Add("@titulo", SqlDbType.VarChar, 200).Value = epic.Titulo;
        comando.Parameters.Add("@descripcion", SqlDbType.VarChar).Value =
            (object?)epic.Descripcion ?? DBNull.Value;
        comando.Parameters.Add("@colorHex", SqlDbType.VarChar, 7).Value = epic.ColorHex;

        comando.ExecuteNonQuery();
    }

    public void EliminarEpic(int epicId)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("DELETE FROM dbo.Epics WHERE EpicId = @epicId;", conexion);

        comando.Parameters.Add("@epicId", SqlDbType.Int).Value = epicId;

        comando.ExecuteNonQuery();
    }

    public bool ExisteTitulo(int proyectoId, string titulo, int? excluirEpicId = null)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(@"
            SELECT COUNT(*)
            FROM dbo.Epics
            WHERE ProyectoId = @proyectoId
              AND Titulo = @titulo
              AND (@excluirEpicId IS NULL OR EpicId <> @excluirEpicId);", conexion);

        comando.Parameters.Add("@proyectoId", SqlDbType.Int).Value = proyectoId;
        comando.Parameters.Add("@titulo", SqlDbType.VarChar, 200).Value = titulo.Trim();
        comando.Parameters.Add("@excluirEpicId", SqlDbType.Int).Value =
            (object?)excluirEpicId ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }

    /// <summary>
    /// Historias que siguen apuntando al epic. La FK de dbo.UserStories no
    /// tiene ON DELETE CASCADE, por eso hay que comprobarlo antes de borrar.
    /// </summary>
    public int ContarHistorias(int epicId)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(
            "SELECT COUNT(*) FROM dbo.UserStories WHERE EpicId = @epicId;", conexion);

        comando.Parameters.Add("@epicId", SqlDbType.Int).Value = epicId;

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    private static Epic MapearEpic(SqlDataReader lector)
    {
        var descripcionOrdinal = lector.GetOrdinal("Descripcion");
        var colorOrdinal = lector.GetOrdinal("ColorHex");

        return new Epic
        {
            EpicId = lector.GetInt32(lector.GetOrdinal("EpicId")),
            ProyectoId = lector.GetInt32(lector.GetOrdinal("ProyectoId")),
            Titulo = lector.GetString(lector.GetOrdinal("Titulo")),
            Descripcion = lector.IsDBNull(descripcionOrdinal)
                ? null
                : lector.GetString(descripcionOrdinal),
            ColorHex = lector.IsDBNull(colorOrdinal)
                ? "#3182CE"
                : lector.GetString(colorOrdinal),
            FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion")),
            ProyectoNombre = lector.GetString(lector.GetOrdinal("NombreProyecto"))
        };
    }
}
