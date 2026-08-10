/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo NotificacionAD.cs para la issue TASK-AD-08 con
 *    los metodos ObtenerNotificacionesPendientes y MarcarComoLeida sobre la
 *    tabla dbo.Notificaciones. Usa procedimientos almacenados y respeta el
 *    patron de ProyectoAD.cs."
 * 3. Cambios del equipo:
 *    Se agregaron tambien ContarNoLeidas y MarcarTodasComoLeidas .
 *    MarcarComoLeida solo cuenta las filas que estaban sin leer, por lo que
 *    devuelve false si la notificacion ya se habia marcado.
 */

using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class NotificacionAD
    {
        // INSERT (alta de miembro, asignacion de tarea o mencion @usuario)
        public int InsertarNotificacion(Notificacion notificacion)
        {
            int nuevaNotificacionId = 0;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Notificacion_Insertar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@usuarioId", notificacion.UsuarioId);
                        cmd.Parameters.AddWithValue("@userStoryId", (object?)notificacion.UserStoryId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@titulo", notificacion.Titulo);
                        cmd.Parameters.AddWithValue("@mensaje", notificacion.Mensaje);

                        object? result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            nuevaNotificacionId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Insertar Notificacion): " + ex.Message);
            }

            return nuevaNotificacionId;
        }

        /// <summary>
        /// Notificaciones sin leer del usuario, de la mas reciente a la mas antigua.
        /// </summary>
        public List<Notificacion> ObtenerNotificacionesPendientes(int usuarioId)
        {
            List<Notificacion> lista = new List<Notificacion>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Notificacion_ListarPendientes", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Notificacion
                                {
                                    NotificacionId = Convert.ToInt32(dr["NotificacionId"]),
                                    UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                    UserStoryId = dr["UserStoryId"] != DBNull.Value ? Convert.ToInt32(dr["UserStoryId"]) : (int?)null,
                                    Titulo = dr["Titulo"].ToString() ?? string.Empty,
                                    Mensaje = dr["Mensaje"].ToString() ?? string.Empty,
                                    Leido = Convert.ToBoolean(dr["Leido"]),
                                    FechaNotificacion = Convert.ToDateTime(dr["FechaNotificacion"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Obtener Notificaciones Pendientes): " + ex.Message);
            }

            return lista;
        }

        /// <summary>
        /// Marca una notificacion como leida.
        /// </summary>
        /// <returns>False si la notificacion no existe o ya estaba leida.</returns>
        public bool MarcarComoLeida(int notificacionId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Notificacion_MarcarLeida", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@notificacionId", notificacionId);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Marcar Notificacion como Leida): " + ex.Message);
            }
        }

        /// <summary>
        /// Marca como leidas todas las notificaciones pendientes del usuario.
        /// </summary>
        /// <returns>Cantidad de notificaciones marcadas.</returns>
        public int MarcarTodasComoLeidas(int usuarioId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Notificacion_MarcarTodasLeidas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        return LeerFilasAfectadas(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Marcar Todas las Notificaciones como Leidas): " + ex.Message);
            }
        }

        /// <summary>
        /// Numero de notificaciones sin leer para el badge de la campana.
        /// </summary>
        public int ContarNoLeidas(int usuarioId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Notificacion_ContarNoLeidas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        object? result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Contar Notificaciones No Leidas): " + ex.Message);
            }
        }

        /// <summary>
        /// Los procedimientos usan SET NOCOUNT ON, por lo que ExecuteNonQuery
        /// devolveria -1: el conteo de filas llega como resultado del SELECT
        /// @@ROWCOUNT final del procedimiento.
        /// </summary>
        private static int LeerFilasAfectadas(SqlCommand cmd)
        {
            object? result = cmd.ExecuteScalar();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }
    }
}
