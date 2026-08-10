using Microsoft.Data.SqlClient;
using Modelo;
using System.Data;

namespace Datos;

public class RolAD
{
    public List<Rol> ListarRoles()
    {
        const string consulta = """
            SELECT r.RolId, r.NombreRol, r.Descripcion, r.FechaCreacion
            FROM dbo.Roles AS r
            ORDER BY r.NombreRol;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        using var lector = comando.ExecuteReader();

        var roles = new List<Rol>();

        while (lector.Read())
        {
            var descripcionOrdinal = lector.GetOrdinal("Descripcion");
            roles.Add(new Rol
            {
                RolId = lector.GetInt32(lector.GetOrdinal("RolId")),
                NombreRol = lector.GetString(lector.GetOrdinal("NombreRol")),
                Descripcion = lector.IsDBNull(descripcionOrdinal)
                    ? string.Empty
                    : lector.GetString(descripcionOrdinal),
                FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion"))
            });
        }

        return roles;
    }

    public Dictionary<int, int> ObtenerConteoUsuariosPorRol()
    {
        const string consulta = """
            SELECT ur.RolId, COUNT(1) AS Conteo
            FROM dbo.UsuarioRoles AS ur
            GROUP BY ur.RolId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        using var lector = comando.ExecuteReader();

        var conteos = new Dictionary<int, int>();
        while (lector.Read())
        {
            conteos[lector.GetInt32(lector.GetOrdinal("RolId"))] =
                lector.GetInt32(lector.GetOrdinal("Conteo"));
        }

        return conteos;
    }

    public int CrearRol(Rol rol)
    {
        const string consulta = """
            INSERT INTO dbo.Roles (NombreRol, Descripcion)
            OUTPUT INSERTED.RolId
            VALUES (@NombreRol, @Descripcion);
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@NombreRol", SqlDbType.VarChar, 50).Value = rol.NombreRol;
        comando.Parameters.Add("@Descripcion", SqlDbType.VarChar, 255).Value =
            (object?)rol.Descripcion ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public void ActualizarRol(Rol rol)
    {
        const string consulta = """
            UPDATE dbo.Roles
            SET NombreRol = @NombreRol,
                Descripcion = @Descripcion
            WHERE RolId = @RolId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@RolId", SqlDbType.Int).Value = rol.RolId;
        comando.Parameters.Add("@NombreRol", SqlDbType.VarChar, 50).Value = rol.NombreRol;
        comando.Parameters.Add("@Descripcion", SqlDbType.VarChar, 255).Value =
            (object?)rol.Descripcion ?? DBNull.Value;

        comando.ExecuteNonQuery();
    }

    public void EliminarRol(int rolId)
    {
        const string consulta = "DELETE FROM dbo.Roles WHERE RolId = @RolId;";

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@RolId", SqlDbType.Int).Value = rolId;

        comando.ExecuteNonQuery();
    }

    public bool ExisteNombre(string nombre, int? excluirRolId = null)
    {
        const string consulta = """
            SELECT COUNT(1)
            FROM dbo.Roles
            WHERE NombreRol = LTRIM(RTRIM(@NombreRol))
              AND (@ExcluirId IS NULL OR RolId <> @ExcluirId);
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@NombreRol", SqlDbType.VarChar, 50).Value = nombre.Trim();
        comando.Parameters.Add("@ExcluirId", SqlDbType.Int).Value = (object?)excluirRolId ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }

    public bool TieneUsuariosAsignados(int rolId)
    {
        const string consulta = """
            SELECT COUNT(1)
            FROM dbo.UsuarioRoles
            WHERE RolId = @RolId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@RolId", SqlDbType.Int).Value = rolId;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }
}