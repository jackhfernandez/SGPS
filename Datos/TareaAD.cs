/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo TareaAD.cs para la issue TASK-AD-06 con los
 *    metodos InsertarTarea, ListarTareasPorUserStory y ActualizarHoras sobre
 *    la tabla dbo.Tareas. Usa procedimientos almacenados y respeta el patron
 *    de ProyectoAD.cs."
 * 3. Cambios del equipo:
 *
 *    ActualizarHoras recibe horas estimadas y trabajadas
 *    en lugar de solo las trabajadas, porque TareaEdicion.cs registra las dos
 *    al imputar.
 *
 *    Se agregaron ActualizarEstado y AsignarUsuario para el Drag &
 *    Drop del tablero Kanban y la auto-asignacion al pasar a "En Proceso"
 *
 *    Los metodos de escritura leen @@ROWCOUNT con
 *    ExecuteScalar porque los procedimientos usan SET NOCOUNT ON.
 */

using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class TareaAD
    {
        // INSERT
        public int InsertarTarea(Tarea tarea)
        {
            int nuevaTareaId = 0;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Tarea_Insertar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userStoryId", tarea.UserStoryId);
                        cmd.Parameters.AddWithValue("@tituloTarea", tarea.TituloTarea);
                        cmd.Parameters.AddWithValue("@horasEstimadas", tarea.HorasEstimadas);
                        cmd.Parameters.AddWithValue("@horasTrabajadas", tarea.HorasTrabajadas);
                        cmd.Parameters.AddWithValue("@estado", tarea.Estado);
                        cmd.Parameters.AddWithValue("@usuarioAsignadoId", (object?)tarea.UsuarioAsignadoId ?? DBNull.Value);

                        object? result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            nuevaTareaId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Insertar Tarea): " + ex.Message);
            }

            return nuevaTareaId;
        }

        // SELECT POR USER STORY
        public List<Tarea> ListarTareasPorUserStory(int userStoryId)
        {
            List<Tarea> lista = new List<Tarea>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Tarea_ListarPorUserStory", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userStoryId", userStoryId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Tarea
                                {
                                    TareaId = Convert.ToInt32(dr["TareaId"]),
                                    UserStoryId = Convert.ToInt32(dr["UserStoryId"]),
                                    TituloTarea = dr["TituloTarea"].ToString() ?? string.Empty,
                                    HorasEstimadas = Convert.ToDecimal(dr["HorasEstimadas"]),
                                    HorasTrabajadas = Convert.ToDecimal(dr["HorasTrabajadas"]),
                                    Estado = dr["Estado"].ToString() ?? string.Empty,
                                    UsuarioAsignadoId = dr["UsuarioAsignadoId"] != DBNull.Value ? Convert.ToInt32(dr["UsuarioAsignadoId"]) : (int?)null,
                                    FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Tareas por User Story): " + ex.Message);
            }

            return lista;
        }

        /// <summary>
        /// Registra la imputacion de horas de la tarea (estimadas y trabajadas).
        /// </summary>
        public bool ActualizarHoras(int tareaId, decimal horasEstimadas, decimal horasTrabajadas)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Tarea_ActualizarHoras", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@tareaId", tareaId);
                        cmd.Parameters.AddWithValue("@horasEstimadas", horasEstimadas);
                        cmd.Parameters.AddWithValue("@horasTrabajadas", horasTrabajadas);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Actualizar Horas de Tarea): " + ex.Message);
            }
        }

        /// <summary>
        /// Cambia el estado de la tarea (Pendiente, En Proceso, Completado).
        /// </summary>
        public bool ActualizarEstado(int tareaId, string nuevoEstado)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Tarea_ActualizarEstado", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@tareaId", tareaId);
                        cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Actualizar Estado de Tarea): " + ex.Message);
            }
        }

        /// <summary>
        /// Asigna la tarea a un usuario del equipo.
        /// </summary>
        public bool AsignarUsuario(int tareaId, int usuarioId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Tarea_AsignarUsuario", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@tareaId", tareaId);
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Asignar Usuario a Tarea): " + ex.Message);
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
