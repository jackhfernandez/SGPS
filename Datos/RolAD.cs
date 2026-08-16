using Microsoft.Data.SqlClient;
using Modelo;
using System.Data;

namespace Datos;

public class RolAD
{
    public List<Rol> ListarRoles()
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_Listar", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var lector = comando.ExecuteReader();

        var roles = new List<Rol>();

        while (lector.Read())
        {
            roles.Add(MapearRol(lector));
        }

        return roles;
    }

    public Dictionary<int, int> ObtenerConteoUsuariosPorRol()
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_ConteoUsuarios", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };

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
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_Insertar", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@nombreRol", SqlDbType.VarChar, 50).Value = rol.NombreRol;
        comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 255).Value =
            (object?)rol.Descripcion ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public void ActualizarRol(Rol rol)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_Modificar", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@rolId", SqlDbType.Int).Value = rol.RolId;
        comando.Parameters.Add("@nombreRol", SqlDbType.VarChar, 50).Value = rol.NombreRol;
        comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 255).Value =
            (object?)rol.Descripcion ?? DBNull.Value;

        comando.ExecuteNonQuery();
    }

    public void CambiarEstado(int rolId, bool esActivo)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_CambiarEstado", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@rolId", SqlDbType.Int).Value = rolId;
        comando.Parameters.Add("@esActivo", SqlDbType.Bit).Value = esActivo;

        comando.ExecuteNonQuery();
    }

    public bool ExisteNombre(string nombre, int? excluirRolId = null)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_ExisteNombre", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@nombreRol", SqlDbType.VarChar, 50).Value = nombre.Trim();
        comando.Parameters.Add("@excluirRolId", SqlDbType.Int).Value =
            (object?)excluirRolId ?? DBNull.Value;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }

    public bool TieneUsuariosAsignados(int rolId)
    {
        using var conexion = Conexion.ObtenerConexion();
        using var comando = new SqlCommand("sp_Rol_TieneUsuarios", conexion)
        {
            CommandType = CommandType.StoredProcedure
        };
        comando.Parameters.Add("@rolId", SqlDbType.Int).Value = rolId;

        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
    }

    private static Rol MapearRol(SqlDataReader lector)
    {
        var descripcionOrdinal = lector.GetOrdinal("Descripcion");

        return new Rol
        {
            RolId = lector.GetInt32(lector.GetOrdinal("RolId")),
            NombreRol = lector.GetString(lector.GetOrdinal("NombreRol")),
            Descripcion = lector.IsDBNull(descripcionOrdinal)
                ? string.Empty
                : lector.GetString(descripcionOrdinal),
            EsActivo = lector.GetBoolean(lector.GetOrdinal("EsActivo")),
            FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion"))
        };
    }
}
