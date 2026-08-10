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
        const string consulta = """
            SELECT u.UsuarioId, u.Nombres, u.Apellidos, u.Email, u.PasswordHash, u.PasswordSalt,
                   u.EsActivo, u.FechaRegistro, u.UltimoAcceso
            FROM dbo.Usuarios AS u
            ORDER BY u.Nombres, u.Apellidos;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
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
        const string consulta = """
            SELECT ur.UsuarioId, r.RolId, r.NombreRol, r.Descripcion
            FROM dbo.UsuarioRoles AS ur
            INNER JOIN dbo.Roles AS r ON r.RolId = ur.RolId
            ORDER BY ur.UsuarioId, r.NombreRol;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
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
        const string consulta = """
            INSERT INTO dbo.Usuarios (Nombres, Apellidos, Email, PasswordHash, PasswordSalt, EsActivo)
            OUTPUT INSERTED.UsuarioId
            VALUES (@Nombres, @Apellidos, @Email, @PasswordHash, @PasswordSalt, @EsActivo);
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Nombres", SqlDbType.VarChar, 100).Value = usuario.Nombres;
        comando.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100).Value = usuario.Apellidos;
        comando.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = usuario.Email;
        comando.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 256).Value = passwordHash;
        comando.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 256).Value = passwordSalt;
        comando.Parameters.Add("@EsActivo", SqlDbType.Bit).Value = usuario.EsActivo;

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        const string consulta = """
            UPDATE dbo.Usuarios
            SET Nombres = @Nombres,
                Apellidos = @Apellidos,
                Email = @Email,
                EsActivo = @EsActivo
            WHERE UsuarioId = @UsuarioId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuario.UsuarioId;
        comando.Parameters.Add("@Nombres", SqlDbType.VarChar, 100).Value = usuario.Nombres;
        comando.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100).Value = usuario.Apellidos;
        comando.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = usuario.Email;
        comando.Parameters.Add("@EsActivo", SqlDbType.Bit).Value = usuario.EsActivo;

        comando.ExecuteNonQuery();
    }

    public void CambiarEstado(int usuarioId, bool esActivo)
    {
        const string consulta = """
            UPDATE dbo.Usuarios
            SET EsActivo = @EsActivo
            WHERE UsuarioId = @UsuarioId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@EsActivo", SqlDbType.Bit).Value = esActivo;

        comando.ExecuteNonQuery();
    }

    public void ActualizarPassword(int usuarioId, string passwordHash, string passwordSalt)
    {
        const string consulta = """
            UPDATE dbo.Usuarios
            SET PasswordHash = @PasswordHash,
                PasswordSalt = @PasswordSalt
            WHERE UsuarioId = @UsuarioId;
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 256).Value = passwordHash;
        comando.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 256).Value = passwordSalt;

        comando.ExecuteNonQuery();
    }

    public bool ExisteEmail(string email, int? excluirUsuarioId = null)
    {
        const string consulta = """
            SELECT COUNT(1)
            FROM dbo.Usuarios
            WHERE Email = LTRIM(RTRIM(@Email))
              AND (@ExcluirId IS NULL OR UsuarioId <> @ExcluirId);
            """;

        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = email.Trim();
        comando.Parameters.Add("@ExcluirId", SqlDbType.Int).Value = (object?)excluirUsuarioId ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }

    public void AsignarRoles(int usuarioId, ICollection<int> rolIds)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var transaccion = conexion.BeginTransaction();

        try
        {
            using (var eliminar = new SqlCommand(
                "DELETE FROM dbo.UsuarioRoles WHERE UsuarioId = @UsuarioId;",
                conexion, transaccion))
            {
                eliminar.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;
                eliminar.ExecuteNonQuery();
            }

            foreach (var rolId in rolIds)
            {
                using var insertar = new SqlCommand(
                    "INSERT INTO dbo.UsuarioRoles (UsuarioId, RolId) VALUES (@UsuarioId, @RolId);",
                    conexion, transaccion);
                insertar.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;
                insertar.Parameters.Add("@RolId", SqlDbType.Int).Value = rolId;
                insertar.ExecuteNonQuery();
            }

            transaccion.Commit();
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
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