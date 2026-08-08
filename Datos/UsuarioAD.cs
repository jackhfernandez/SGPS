using Microsoft.Data.SqlClient;
using SGPS.Entidades;
using System.Data;

namespace Datos;

public class UsuarioAD
{
    public Usuario? ObtenerPorEmail(string email)
    {
        using var conexion = Conexion.getConexion();
        using var comando = new SqlCommand("sp_AutenticarUsuario", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };

        comando.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = email;
        conexion.Open();

        using var lector = comando.ExecuteReader();
        if (!lector.Read())
        {
            return null;
        }

        return new Usuario
        {
            UsuarioId = lector.GetInt32(lector.GetOrdinal("UsuarioId")),
            Nombres = lector.GetString(lector.GetOrdinal("Nombres")),
            Apellidos = lector.GetString(lector.GetOrdinal("Apellidos")),
            Email = lector.GetString(lector.GetOrdinal("Email")),
            PasswordHash = lector.GetString(lector.GetOrdinal("PasswordHash")),
            PasswordSalt = lector.GetString(lector.GetOrdinal("PasswordSalt")),
            EsActivo = lector.GetBoolean(lector.GetOrdinal("EsActivo")),
            FechaRegistro = lector.GetDateTime(lector.GetOrdinal("FechaRegistro")),
            UltimoAcceso = lector.IsDBNull(lector.GetOrdinal("UltimoAcceso"))
                ? null
                : lector.GetDateTime(lector.GetOrdinal("UltimoAcceso"))
        };
    }

    public List<Rol> ObtenerRolesPorUsuario(int usuarioId)
    {
        const string consulta = """
            SELECT r.RolId, r.NombreRol, r.Descripcion
            FROM dbo.Roles AS r
            INNER JOIN dbo.UsuarioRoles AS ur ON ur.RolId = r.RolId
            WHERE ur.UsuarioId = @UsuarioId;
            """;

        using var conexion = Conexion.getConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;
        conexion.Open();

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
                    : lector.GetString(descripcionOrdinal)
            });
        }

        return roles;
    }
}
