using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class UserStoryAD
    {
        public List<UserStory> ObtenerBacklogPorProyecto(int proyectoId)
        {
            List<UserStory> listaBacklog = new List<UserStory>();

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerBacklogPriorizado", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProyectoId", proyectoId);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            UserStory item = new UserStory
                            {
                                UserStoryId = Convert.ToInt32(dr["UserStoryId"]),
                                CodigoTicket = dr["CodigoTicket"].ToString(),
                                ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                                EpicId = dr["EpicId"] != DBNull.Value ? Convert.ToInt32(dr["EpicId"]) : (int?)null,
                                SprintId = dr["SprintId"] != DBNull.Value ? Convert.ToInt32(dr["SprintId"]) : (int?)null,
                                Titulo = dr["Titulo"].ToString(),
                                ComoUsuario = dr["ComoUsuario"].ToString(),
                                QuieroFuncionalidad = dr["QuieroFuncionalidad"].ToString(),
                                ParaBeneficio = dr["ParaBeneficio"].ToString(),
                                CriteriosAceptacionTexto = dr["CriteriosAceptacionTexto"] != DBNull.Value ? dr["CriteriosAceptacionTexto"].ToString() : null,
                                ValorNegocio = dr["ValorNegocio"].ToString(),
                                StoryPoints = Convert.ToInt32(dr["StoryPoints"]),
                                Estado = dr["Estado"].ToString(),
                                OrdenPrioridad = Convert.ToInt32(dr["OrdenPrioridad"]),
                                UsuarioAsignadoId = dr["UsuarioAsignadoId"] != DBNull.Value ? Convert.ToInt32(dr["UsuarioAsignadoId"]) : (int?)null,
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaUltimaModificacion = Convert.ToDateTime(dr["FechaUltimaModificacion"])
                            };

                            listaBacklog.Add(item);
                        }
                    }
                }
            }

            return listaBacklog;
        }

        /// <summary>
        /// Actualiza la prioridad/orden jerárquico de una Historia de Usuario en dbo.UserStories.
        /// </summary>
        /// <param name="userStoryId">ID de la User Story.</param>
        /// <param name="nuevoOrden">Nuevo valor entero de ordenación.</param>
        /// <returns>True si la actualización fue exitosa.</returns>
        public bool ActualizarOrdenPrioridad(int userStoryId, int nuevoOrden)
        {
            string query = @"UPDATE dbo.UserStories 
                            SET OrdenPrioridad = @NuevoOrden, 
                                FechaUltimaModificacion = GETDATE() 
                            WHERE UserStoryId = @UserStoryId";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@NuevoOrden", nuevoOrden);
                    cmd.Parameters.AddWithValue("@UserStoryId", userStoryId);

                    cn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        /// <summary>
        /// Actualiza el estado de ciclo de vida de una Historia de Usuario en dbo.UserStories.
        /// </summary>
        /// <param name="userStoryId">ID de la User Story.</param>
        /// <param name="nuevoEstado">Nuevo estado (To Do, In Progress, In Testing, Done).</param>
        /// <returns>True si la actualización fue exitosa.</returns>
        public bool ActualizarEstado(int userStoryId, string nuevoEstado)
        {
            string query = @"UPDATE dbo.UserStories 
                            SET Estado = @NuevoEstado, 
                                FechaUltimaModificacion = GETDATE() 
                            WHERE UserStoryId = @UserStoryId";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@UserStoryId", userStoryId);

                    cn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}
