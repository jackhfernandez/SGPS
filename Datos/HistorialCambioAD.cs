using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class HistorialCambioAD
    { 
        public HistorialCambioAD()
        {
           
        }

        /// <summary>
        /// Inserta un nuevo registro inmutable en dbo.HistorialCambios.
        /// </summary>
        public int Insertar(HistorialCambio historial)
        {
            if (historial == null)
                throw new ArgumentNullException(nameof(historial), "El objeto historial no puede ser nulo.");

            string query = @"
                INSERT INTO dbo.HistorialCambios 
                    (Entidad, EntidadId, CampoModificado, ValorAnterior, ValorNuevo, UsuarioId, FechaModificacion)
                VALUES 
                    (@Entidad, @EntidadId, @CampoModificado, @ValorAnterior, @ValorNuevo, @UsuarioId, @FechaModificacion);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@Entidad", SqlDbType.VarChar, 50).Value = historial.Entidad;
                    cmd.Parameters.Add("@EntidadId", SqlDbType.Int).Value = historial.EntidadId;
                    cmd.Parameters.Add("@CampoModificado", SqlDbType.VarChar, 50).Value = historial.CampoModificado;
                    cmd.Parameters.Add("@ValorAnterior", SqlDbType.VarChar).Value = (object)historial.ValorAnterior ?? DBNull.Value;
                    cmd.Parameters.Add("@ValorNuevo", SqlDbType.VarChar).Value = (object)historial.ValorNuevo ?? DBNull.Value;
                    cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = historial.UsuarioId;
                    cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = historial.FechaModificacion == default(DateTime)
                        ? DateTime.Now
                        : historial.FechaModificacion;

                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Obtiene el historial cronológico de cambios de una entidad específica (ej. UserStory, Bug, Sprint).
        /// Incluye el nombre completo del usuario que ejecutó el cambio.
        /// </summary>
        public List<HistorialCambio> ListarPorEntidad(string entidad, int entidadId)
        {
            var lista = new List<HistorialCambio>();

            string query = @"
                SELECT 
                    h.HistorialId,
                    h.Entidad,
                    h.EntidadId,
                    h.CampoModificado,
                    h.ValorAnterior,
                    h.ValorNuevo,
                    h.UsuarioId,
                    h.FechaModificacion,
                    CONCAT(u.Nombres, ' ', u.Apellidos) AS NombreUsuario
                FROM dbo.HistorialCambios h
                INNER JOIN dbo.Usuarios u ON h.UsuarioId = u.UsuarioId
                WHERE h.Entidad = @Entidad AND h.EntidadId = @EntidadId
                ORDER BY h.FechaModificacion DESC;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Entidad", SqlDbType.VarChar, 50).Value = entidad;
                    cmd.Parameters.Add("@EntidadId", SqlDbType.Int).Value = entidadId;

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new HistorialCambio
                            {
                                HistorialId = Convert.ToInt32(dr["HistorialId"]),
                                Entidad = dr["Entidad"].ToString(),
                                EntidadId = Convert.ToInt32(dr["EntidadId"]),
                                CampoModificado = dr["CampoModificado"].ToString(),
                                ValorAnterior = dr["ValorAnterior"] != DBNull.Value ? dr["ValorAnterior"].ToString() : null,
                                ValorNuevo = dr["ValorNuevo"] != DBNull.Value ? dr["ValorNuevo"].ToString() : null,
                                UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
                                NombreUsuario = dr["NombreUsuario"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene todos los cambios realizados por un usuario en particular.
        /// </summary>
        public List<HistorialCambio> ListarPorUsuario(int usuarioId)
        {
            var lista = new List<HistorialCambio>();

            string query = @"
                SELECT 
                    h.HistorialId,
                    h.Entidad,
                    h.EntidadId,
                    h.CampoModificado,
                    h.ValorAnterior,
                    h.ValorNuevo,
                    h.UsuarioId,
                    h.FechaModificacion,
                    CONCAT(u.Nombres, ' ', u.Apellidos) AS NombreUsuario
                FROM dbo.HistorialCambios h
                INNER JOIN dbo.Usuarios u ON h.UsuarioId = u.UsuarioId
                WHERE h.UsuarioId = @UsuarioId
                ORDER BY h.FechaModificacion DESC;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new HistorialCambio
                            {
                                HistorialId = Convert.ToInt32(dr["HistorialId"]),
                                Entidad = dr["Entidad"].ToString(),
                                EntidadId = Convert.ToInt32(dr["EntidadId"]),
                                CampoModificado = dr["CampoModificado"].ToString(),
                                ValorAnterior = dr["ValorAnterior"] != DBNull.Value ? dr["ValorAnterior"].ToString() : null,
                                ValorNuevo = dr["ValorNuevo"] != DBNull.Value ? dr["ValorNuevo"].ToString() : null,
                                UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
                                NombreUsuario = dr["NombreUsuario"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public List<HistorialCambio> ListarPorEntidad(string entidad)
        {
            List<HistorialCambio> lista = new List<HistorialCambio>();
            string query = @"
                SELECT 
                    HistorialId, 
                    Entidad, 
                    EntidadId, 
                    CampoModificado, 
                    ValorAnterior, 
                    ValorNuevo, 
                    UsuarioId, 
                    FechaModificacion 
                FROM dbo.HistorialCambios 
                WHERE Entidad = @Entidad
                ORDER BY FechaModificacion ASC;";

            using (SqlConnection cnx = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Entidad", SqlDbType.VarChar, 50).Value = entidad;

                    cnx.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            HistorialCambio item = new HistorialCambio
                            {
                                HistorialId = Convert.ToInt32(dr["HistorialId"]),
                                Entidad = dr["Entidad"].ToString(),
                                EntidadId = Convert.ToInt32(dr["EntidadId"]),
                                CampoModificado = dr["CampoModificado"].ToString(),
                                ValorAnterior = dr["ValorAnterior"] != DBNull.Value ? dr["ValorAnterior"].ToString() : null,
                                ValorNuevo = dr["ValorNuevo"] != DBNull.Value ? dr["ValorNuevo"].ToString() : null,
                                UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"])
                            };

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }
    }
}
