using Microsoft.Data.SqlClient;
using Modelo;
using System.Data;

namespace Datos;

public class UsuarioAD
{
    public Usuario? ObtenerPorEmail(string email)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("dbo.sp_AutenticarUsuario", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };

        comando.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = email;
        using var lector = comando.ExecuteReader();
        return lector.Read() ? MapearUsuario(lector) : null;
    }

    public List<Rol> ObtenerRolesPorUsuario(int usuarioId)
    {
        const string consulta = """
            SELECT r.RolId, r.NombreRol, r.Descripcion
            FROM dbo.Roles AS r
            INNER JOIN dbo.UsuarioRoles AS ur ON ur.RolId = r.RolId
            WHERE ur.UsuarioId = @UsuarioId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;

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

    public List<Usuario> ListarUsuarios()
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_Listar", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var lector = comando.ExecuteReader();

        var usuarios = new List<Usuario>();
        while (lector.Read())
        {
            usuarios.Add(MapearUsuario(lector));
        }

        return usuarios;
    }

    public Dictionary<int, List<Rol>> ObtenerRolesDeTodosLosUsuarios()
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_ListarRoles", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var lector = comando.ExecuteReader();

        var rolesPorUsuario = new Dictionary<int, List<Rol>>();

        while (lector.Read())
        {
            var usuarioId = lector.GetInt32(lector.GetOrdinal("UsuarioId"));

            if (!rolesPorUsuario.TryGetValue(usuarioId, out var roles))
            {
                roles = new List<Rol>();
                rolesPorUsuario[usuarioId] = roles;
            }

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

        return rolesPorUsuario;
    }

    public int CrearUsuario(Usuario usuario, string passwordHash, string passwordSalt)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_Insertar", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@nombres", SqlDbType.VarChar, 100).Value = usuario.Nombres;
        comando.Parameters.Add("@apellidos", SqlDbType.VarChar, 100).Value = usuario.Apellidos;
        comando.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = usuario.Email;
        comando.Parameters.Add("@passwordHash", SqlDbType.VarChar, 256).Value = passwordHash;
        comando.Parameters.Add("@passwordSalt", SqlDbType.VarChar, 256).Value = passwordSalt;
        comando.Parameters.Add("@esActivo", SqlDbType.Bit).Value = usuario.EsActivo;

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_Modificar", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@usuarioId", SqlDbType.Int).Value = usuario.UsuarioId;
        comando.Parameters.Add("@nombres", SqlDbType.VarChar, 100).Value = usuario.Nombres;
        comando.Parameters.Add("@apellidos", SqlDbType.VarChar, 100).Value = usuario.Apellidos;
        comando.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = usuario.Email;
        comando.Parameters.Add("@esActivo", SqlDbType.Bit).Value = usuario.EsActivo;

        comando.ExecuteNonQuery();
    }

    public void CambiarEstado(int usuarioId, bool esActivo)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_CambiarEstado", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@usuarioId", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@esActivo", SqlDbType.Bit).Value = esActivo;

        comando.ExecuteNonQuery();
    }

    public void ActualizarPassword(int usuarioId, string passwordHash, string passwordSalt)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_ActualizarPassword", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@usuarioId", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@passwordHash", SqlDbType.VarChar, 256).Value = passwordHash;
        comando.Parameters.Add("@passwordSalt", SqlDbType.VarChar, 256).Value = passwordSalt;

        comando.ExecuteNonQuery();
    }

    public bool ExisteEmail(string email, int? excluirUsuarioId = null)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_ExisteEmail", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = email.Trim();
        comando.Parameters.Add("@excluirUsuarioId", SqlDbType.Int).Value =
            (object?)excluirUsuarioId ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }

    public void AsignarRoles(int usuarioId, ICollection<int> rolIds)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Usuario_AsignarRoles", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@usuarioId", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@rolIds", SqlDbType.VarChar, -1).Value =
            string.Join(",", rolIds ?? Array.Empty<int>());

        comando.ExecuteNonQuery();
    }

    private static Usuario MapearUsuario(SqlDataReader lector)
    {
        var ultimoAccesoOrdinal = lector.GetOrdinal("UltimoAcceso");

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
            UltimoAcceso = lector.IsDBNull(ultimoAccesoOrdinal)
                ? null
                : lector.GetDateTime(ultimoAccesoOrdinal)
        };
    }
}
