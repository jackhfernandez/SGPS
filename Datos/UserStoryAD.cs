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

        public void Insertar(UserStory historia)
        {
            const string query = @"
                INSERT INTO dbo.UserStories (
                    CodigoTicket, ProyectoId, EpicId, SprintId, Titulo,
                    ComoUsuario, QuieroFuncionalidad, ParaBeneficio,
                    CriteriosAceptacionTexto, ValorNegocio, StoryPoints,
                    Estado, OrdenPrioridad, UsuarioAsignadoId,
                    FechaCreacion, FechaUltimaModificacion
                )
                VALUES (
                    @CodigoTicket, @ProyectoId, @EpicId, @SprintId, @Titulo,
                    @ComoUsuario, @QuieroFuncionalidad, @ParaBeneficio,
                    @CriteriosAceptacionTexto, @ValorNegocio, @StoryPoints,
                    @Estado, @OrdenPrioridad, @UsuarioAsignadoId,
                    @FechaCreacion, @FechaUltimaModificacion
                );
                SELECT SCOPE_IDENTITY();";

            using (var conexion = Conexion.ObtenerConexion())
            {
                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@CodigoTicket", SqlDbType.VarChar, 20).Value = historia.CodigoTicket;
                    comando.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = historia.ProyectoId;
                    comando.Parameters.Add("@EpicId", SqlDbType.Int).Value = (object)historia.EpicId ?? DBNull.Value;
                    comando.Parameters.Add("@SprintId", SqlDbType.Int).Value = (object)historia.SprintId ?? DBNull.Value;
                    comando.Parameters.Add("@Titulo", SqlDbType.VarChar, 200).Value = historia.Titulo;
                    comando.Parameters.Add("@ComoUsuario", SqlDbType.VarChar, 100).Value = historia.ComoUsuario;
                    comando.Parameters.Add("@QuieroFuncionalidad", SqlDbType.VarChar, 255).Value = historia.QuieroFuncionalidad;
                    comando.Parameters.Add("@ParaBeneficio", SqlDbType.VarChar, 255).Value = historia.ParaBeneficio;
                    comando.Parameters.Add("@CriteriosAceptacionTexto", SqlDbType.VarChar).Value = (object)historia.CriteriosAceptacionTexto ?? DBNull.Value;
                    comando.Parameters.Add("@ValorNegocio", SqlDbType.VarChar, 10).Value = historia.ValorNegocio;
                    comando.Parameters.Add("@StoryPoints", SqlDbType.Int).Value = historia.StoryPoints;
                    comando.Parameters.Add("@Estado", SqlDbType.VarChar, 30).Value = historia.Estado ?? "To Do";
                    comando.Parameters.Add("@OrdenPrioridad", SqlDbType.Int).Value = historia.OrdenPrioridad;
                    comando.Parameters.Add("@UsuarioAsignadoId", SqlDbType.Int).Value = (object)historia.UsuarioAsignadoId ?? DBNull.Value;
                    comando.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = historia.FechaCreacion == default ? DateTime.Now : historia.FechaCreacion;
                    comando.Parameters.Add("@FechaUltimaModificacion", SqlDbType.DateTime).Value = DateTime.Now;

                    conexion.Open();
                    historia.UserStoryId = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
        }

        public void Actualizar(UserStory historia)
        {
            const string query = @"
                UPDATE dbo.UserStories
                SET EpicId = @EpicId,
                    SprintId = @SprintId,
                    Titulo = @Titulo,
                    ComoUsuario = @ComoUsuario,
                    QuieroFuncionalidad = @QuieroFuncionalidad,
                    ParaBeneficio = @ParaBeneficio,
                    CriteriosAceptacionTexto = @CriteriosAceptacionTexto,
                    ValorNegocio = @ValorNegocio,
                    StoryPoints = @StoryPoints,
                    Estado = @Estado,
                    OrdenPrioridad = @OrdenPrioridad,
                    UsuarioAsignadoId = @UsuarioAsignadoId,
                    FechaUltimaModificacion = @FechaUltimaModificacion
                WHERE UserStoryId = @UserStoryId;";

            using (var conexion = Conexion.ObtenerConexion())
            {
                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@UserStoryId", SqlDbType.Int).Value = historia.UserStoryId;
                    comando.Parameters.Add("@EpicId", SqlDbType.Int).Value = (object)historia.EpicId ?? DBNull.Value;
                    comando.Parameters.Add("@SprintId", SqlDbType.Int).Value = (object)historia.SprintId ?? DBNull.Value;
                    comando.Parameters.Add("@Titulo", SqlDbType.VarChar, 200).Value = historia.Titulo;
                    comando.Parameters.Add("@ComoUsuario", SqlDbType.VarChar, 100).Value = historia.ComoUsuario;
                    comando.Parameters.Add("@QuieroFuncionalidad", SqlDbType.VarChar, 255).Value = historia.QuieroFuncionalidad;
                    comando.Parameters.Add("@ParaBeneficio", SqlDbType.VarChar, 255).Value = historia.ParaBeneficio;
                    comando.Parameters.Add("@CriteriosAceptacionTexto", SqlDbType.VarChar).Value = (object)historia.CriteriosAceptacionTexto ?? DBNull.Value;
                    comando.Parameters.Add("@ValorNegocio", SqlDbType.VarChar, 10).Value = historia.ValorNegocio;
                    comando.Parameters.Add("@StoryPoints", SqlDbType.Int).Value = historia.StoryPoints;
                    comando.Parameters.Add("@Estado", SqlDbType.VarChar, 30).Value = historia.Estado;
                    comando.Parameters.Add("@OrdenPrioridad", SqlDbType.Int).Value = historia.OrdenPrioridad;
                    comando.Parameters.Add("@UsuarioAsignadoId", SqlDbType.Int).Value = (object)historia.UsuarioAsignadoId ?? DBNull.Value;
                    comando.Parameters.Add("@FechaUltimaModificacion", SqlDbType.DateTime).Value = DateTime.Now;

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<UserStory> ListarPorProyectoOrdenado(int proyectoId)
        {
            var lista = new List<UserStory>();
            // Aprovecha el índice IX_UserStories_ProyectoPrioridad sobre (ProyectoId, OrdenPrioridad)
            const string query = @"
                SELECT UserStoryId, CodigoTicket, ProyectoId, EpicId, SprintId,
                       Titulo, ComoUsuario, QuieroFuncionalidad, ParaBeneficio,
                       CriteriosAceptacionTexto, ValorNegocio, StoryPoints, Estado,
                       OrdenPrioridad, UsuarioAsignadoId, FechaCreacion, FechaUltimaModificacion
                FROM dbo.UserStories
                WHERE ProyectoId = @ProyectoId
                ORDER BY OrdenPrioridad ASC;";

            using (var conexion = Conexion.ObtenerConexion())
            {
                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = proyectoId;

                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var us = new UserStory
                            {
                                UserStoryId = reader.GetInt32(reader.GetOrdinal("UserStoryId")),
                                CodigoTicket = reader.GetString(reader.GetOrdinal("CodigoTicket")),
                                ProyectoId = reader.GetInt32(reader.GetOrdinal("ProyectoId")),
                                EpicId = reader.IsDBNull(reader.GetOrdinal("EpicId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("EpicId")),
                                SprintId = reader.IsDBNull(reader.GetOrdinal("SprintId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SprintId")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                ComoUsuario = reader.GetString(reader.GetOrdinal("ComoUsuario")),
                                QuieroFuncionalidad = reader.GetString(reader.GetOrdinal("QuieroFuncionalidad")),
                                ParaBeneficio = reader.GetString(reader.GetOrdinal("ParaBeneficio")),
                                CriteriosAceptacionTexto = reader.IsDBNull(reader.GetOrdinal("CriteriosAceptacionTexto")) ? null : reader.GetString(reader.GetOrdinal("CriteriosAceptacionTexto")),
                                ValorNegocio = reader.GetString(reader.GetOrdinal("ValorNegocio")),
                                StoryPoints = reader.GetInt32(reader.GetOrdinal("StoryPoints")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                OrdenPrioridad = reader.GetInt32(reader.GetOrdinal("OrdenPrioridad")),
                                UsuarioAsignadoId = reader.IsDBNull(reader.GetOrdinal("UsuarioAsignadoId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UsuarioAsignadoId")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FechaUltimaModificacion = reader.GetDateTime(reader.GetOrdinal("FechaUltimaModificacion"))
                            };

                            lista.Add(us);
                        }
                    }
                }
            }

            return lista;
        }

        public List<UserStory> ListarPorSprint(int sprintId)
        {
            var lista = new List<UserStory>();

            const string query = @"
                SELECT 
                    UserStoryId,
                    CodigoTicket,
                    ProyectoId,
                    EpicId,
                    SprintId,
                    Titulo,
                    ComoUsuario,
                    QuieroFuncionalidad,
                    ParaBeneficio,
                    CriteriosAceptacionTexto,
                    ValorNegocio,
                    StoryPoints,
                    Estado,
                    OrdenPrioridad,
                    UsuarioAsignadoId,
                    FechaCreacion,
                    FechaUltimaModificacion
                FROM dbo.UserStories
                WHERE SprintId = @SprintId
                ORDER BY OrdenPrioridad ASC;";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@SprintId", SqlDbType.Int).Value = sprintId;

                    try
                    {
                        if (conexion.State == ConnectionState.Closed)
                        {
                            conexion.Open();
                        }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var historia = new UserStory
                                {
                                    UserStoryId = dr.GetInt32(dr.GetOrdinal("UserStoryId")),
                                    CodigoTicket = dr.GetString(dr.GetOrdinal("CodigoTicket")),
                                    ProyectoId = dr.GetInt32(dr.GetOrdinal("ProyectoId")),
                                    EpicId = dr.IsDBNull(dr.GetOrdinal("EpicId")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("EpicId")),
                                    SprintId = dr.IsDBNull(dr.GetOrdinal("SprintId")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("SprintId")),
                                    Titulo = dr.GetString(dr.GetOrdinal("Titulo")),
                                    ComoUsuario = dr.GetString(dr.GetOrdinal("ComoUsuario")),
                                    QuieroFuncionalidad = dr.GetString(dr.GetOrdinal("QuieroFuncionalidad")),
                                    ParaBeneficio = dr.GetString(dr.GetOrdinal("ParaBeneficio")),
                                    CriteriosAceptacionTexto = dr.IsDBNull(dr.GetOrdinal("CriteriosAceptacionTexto")) ? null : dr.GetString(dr.GetOrdinal("CriteriosAceptacionTexto")),
                                    ValorNegocio = dr.GetString(dr.GetOrdinal("ValorNegocio")),
                                    StoryPoints = dr.GetInt32(dr.GetOrdinal("StoryPoints")),
                                    Estado = dr.GetString(dr.GetOrdinal("Estado")),
                                    OrdenPrioridad = dr.GetInt32(dr.GetOrdinal("OrdenPrioridad")),
                                    UsuarioAsignadoId = dr.IsDBNull(dr.GetOrdinal("UsuarioAsignadoId")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("UsuarioAsignadoId")),
                                    FechaCreacion = dr.GetDateTime(dr.GetOrdinal("FechaCreacion")),
                                    FechaUltimaModificacion = dr.GetDateTime(dr.GetOrdinal("FechaUltimaModificacion"))
                                };

                                lista.Add(historia);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new ApplicationException($"Error al consultar las Historias de Usuario para el Sprint ID {sprintId} en SGPS_DB: {ex.Message}", ex);
                    }
                }
            }

            return lista;
        }
    }
}

