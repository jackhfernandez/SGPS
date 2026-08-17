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
 *
 *    Se quitaron los cn.Open() de los metodos con SQL en linea:
 *    Conexion.ObtenerConexion() ya devuelve la conexion abierta y el segundo
 *    Open() lanzaba "The connection was not closed" al abrir TareaEdicion.cs.
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

        public int Insertar(Tarea tarea)
        {
            string query = @"
                INSERT INTO dbo.Tareas 
                    (UserStoryId, TituloTarea, HorasEstimadas, HorasTrabajadas, Estado, UsuarioAsignadoId, FechaCreacion)
                VALUES 
                    (@UserStoryId, @TituloTarea, @HorasEstimadas, @HorasTrabajadas, @Estado, @UsuarioAsignadoId, @FechaCreacion);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@UserStoryId", SqlDbType.Int).Value = tarea.UserStoryId;
                    cmd.Parameters.Add("@TituloTarea", SqlDbType.VarChar, 200).Value = tarea.TituloTarea;
                    cmd.Parameters.Add("@HorasEstimadas", SqlDbType.Decimal).Value = tarea.HorasEstimadas;
                    cmd.Parameters.Add("@HorasTrabajadas", SqlDbType.Decimal).Value = tarea.HorasTrabajadas;
                    cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(tarea.Estado) ? "Pendiente" : tarea.Estado;
                    cmd.Parameters.Add("@UsuarioAsignadoId", SqlDbType.Int).Value = (object)tarea.UsuarioAsignadoId ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = tarea.FechaCreacion == default(DateTime) ? DateTime.Now : tarea.FechaCreacion;

                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public Tarea ObtenerPorId(int tareaId)
        {
            Tarea tarea = null;
            string query = @"
                SELECT TareaId, UserStoryId, TituloTarea, HorasEstimadas, HorasTrabajadas, Estado, UsuarioAsignadoId, FechaCreacion
                FROM dbo.Tareas
                WHERE TareaId = @TareaId;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@TareaId", SqlDbType.Int).Value = tareaId;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tarea = new Tarea
                            {
                                TareaId = Convert.ToInt32(dr["TareaId"]),
                                UserStoryId = Convert.ToInt32(dr["UserStoryId"]),
                                TituloTarea = dr["TituloTarea"].ToString(),
                                HorasEstimadas = Convert.ToDecimal(dr["HorasEstimadas"]),
                                HorasTrabajadas = Convert.ToDecimal(dr["HorasTrabajadas"]),
                                Estado = dr["Estado"].ToString(),
                                UsuarioAsignadoId = dr["UsuarioAsignadoId"] != DBNull.Value ? Convert.ToInt32(dr["UsuarioAsignadoId"]) : (int?)null,
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                            };
                        }
                    }
                }
            }
            return tarea;
        }

        public bool Actualizar(Tarea tarea)
        {
            string query = @"
                UPDATE dbo.Tareas
                SET TituloTarea = @TituloTarea,
                    HorasEstimadas = @HorasEstimadas,
                    HorasTrabajadas = @HorasTrabajadas,
                    Estado = @Estado,
                    UsuarioAsignadoId = @UsuarioAsignadoId
                WHERE TareaId = @TareaId;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@TareaId", SqlDbType.Int).Value = tarea.TareaId;
                    cmd.Parameters.Add("@TituloTarea", SqlDbType.VarChar, 200).Value = tarea.TituloTarea;
                    cmd.Parameters.Add("@HorasEstimadas", SqlDbType.Decimal).Value = tarea.HorasEstimadas;
                    cmd.Parameters.Add("@HorasTrabajadas", SqlDbType.Decimal).Value = tarea.HorasTrabajadas;
                    cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value = tarea.Estado;
                    cmd.Parameters.Add("@UsuarioAsignadoId", SqlDbType.Int).Value = (object)tarea.UsuarioAsignadoId ?? DBNull.Value;

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Tarea> ListarPorUserStory(int userStoryId)
        {
            var lista = new List<Tarea>();
            string query = @"
                SELECT TareaId, UserStoryId, TituloTarea, HorasEstimadas, HorasTrabajadas, Estado, UsuarioAsignadoId, FechaCreacion
                FROM dbo.Tareas
                WHERE UserStoryId = @UserStoryId
                ORDER BY TareaId ASC;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@UserStoryId", SqlDbType.Int).Value = userStoryId;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Tarea
                            {
                                TareaId = Convert.ToInt32(dr["TareaId"]),
                                UserStoryId = Convert.ToInt32(dr["UserStoryId"]),
                                TituloTarea = dr["TituloTarea"].ToString(),
                                HorasEstimadas = Convert.ToDecimal(dr["HorasEstimadas"]),
                                HorasTrabajadas = Convert.ToDecimal(dr["HorasTrabajadas"]),
                                Estado = dr["Estado"].ToString(),
                                UsuarioAsignadoId = dr["UsuarioAsignadoId"] != DBNull.Value ? Convert.ToInt32(dr["UsuarioAsignadoId"]) : (int?)null,
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
