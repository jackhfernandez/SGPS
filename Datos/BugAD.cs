using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class BugAD
    {
        // INSERT
        public int RegistrarBug(Bug bug)
        {
            int nuevoBugId = 0;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_Registrar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@codigoBug", bug.CodigoBug);
                        cmd.Parameters.AddWithValue("@userStoryId", (object?)bug.UserStoryId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@proyectoId", bug.ProyectoId);
                        cmd.Parameters.AddWithValue("@titulo", bug.Titulo);
                        cmd.Parameters.AddWithValue("@pasosReproducir", bug.PasosReproducir);
                        cmd.Parameters.AddWithValue("@severidad", bug.Severidad);
                        cmd.Parameters.AddWithValue("@estado", bug.Estado);
                        cmd.Parameters.AddWithValue("@usuarioReportaId", bug.UsuarioReportaId);
                        cmd.Parameters.AddWithValue("@usuarioAsignadoId", (object?)bug.UsuarioAsignadoId ?? DBNull.Value);

                        object? result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            nuevoBugId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Registrar Bug): " + ex.Message);
            }

            return nuevoBugId;
        }

        /// <summary>
        /// Vincula el Bug a una User Story. Con <paramref name="userStoryId"/> nulo
        /// lo desvincula sin eliminarlo del backlog general.
        /// </summary>
        public bool VincularAUserStory(int bugId, int? userStoryId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_VincularUserStory", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@bugId", bugId);
                        cmd.Parameters.AddWithValue("@userStoryId", (object?)userStoryId ?? DBNull.Value);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Vincular Bug a User Story): " + ex.Message);
            }
        }

        /// <summary>
        /// Lista los Bugs del proyecto. Los filtros de estado y severidad son
        /// opcionales: en null no se aplican.
        /// </summary>
        public List<Bug> ListarBugsPorProyecto(int proyectoId, string? estado = null, string? severidad = null)
        {
            List<Bug> lista = new List<Bug>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_ListarPorProyecto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);
                        cmd.Parameters.AddWithValue("@estado", (object?)estado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@severidad", (object?)severidad ?? DBNull.Value);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(MapearBug(dr));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Bugs por Proyecto): " + ex.Message);
            }

            return lista;
        }

        // SELECT POR USER STORY (pestana "Bugs Asociados")
        public List<Bug> ListarBugsPorUserStory(int userStoryId)
        {
            List<Bug> lista = new List<Bug>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_ListarPorUserStory", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userStoryId", userStoryId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(MapearBug(dr));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Bugs por User Story): " + ex.Message);
            }

            return lista;
        }

        /// <summary>
        /// Indica si la User Story tiene Bugs de severidad Bloqueante o Alta
        /// todavia sin resolver, lo que impide moverla a "Done".
        /// </summary>
        public bool TieneBugsBloqueantesAbiertos(int userStoryId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_ContarBloqueantesAbiertos", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userStoryId", userStoryId);

                        object? result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Verificar Bugs Bloqueantes): " + ex.Message);
            }
        }

        /// <summary>
        /// Avanza el ciclo de vida del Bug (Nuevo, En Proceso, Resuelto, Cerrado).
        /// </summary>
        public bool ActualizarEstado(int bugId, string nuevoEstado)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_ActualizarEstado", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@bugId", bugId);
                        cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Actualizar Estado de Bug): " + ex.Message);
            }
        }

        /// <summary>
        /// Cambia la severidad de un Bug (Bloqueante, Alta, Media, Baja).
        /// </summary>
        public bool ActualizarSeveridad(int bugId, string nuevaSeveridad)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Bug_ActualizarSeveridad", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@bugId", bugId);
                        cmd.Parameters.AddWithValue("@nuevaSeveridad", nuevaSeveridad);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Actualizar Severidad de Bug): " + ex.Message);
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

        private static Bug MapearBug(SqlDataReader dr)
        {
            return new Bug
            {
                BugId = Convert.ToInt32(dr["BugId"]),
                CodigoBug = dr["CodigoBug"].ToString() ?? string.Empty,
                UserStoryId = dr["UserStoryId"] != DBNull.Value ? Convert.ToInt32(dr["UserStoryId"]) : (int?)null,
                ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                Titulo = dr["Titulo"].ToString() ?? string.Empty,
                PasosReproducir = dr["PasosReproducir"].ToString() ?? string.Empty,
                Severidad = dr["Severidad"].ToString() ?? string.Empty,
                Estado = dr["Estado"].ToString() ?? string.Empty,
                UsuarioReportaId = Convert.ToInt32(dr["UsuarioReportaId"]),
                UsuarioAsignadoId = dr["UsuarioAsignadoId"] != DBNull.Value ? Convert.ToInt32(dr["UsuarioAsignadoId"]) : (int?)null,
                FechaReporte = Convert.ToDateTime(dr["FechaReporte"])
            };
        }

        public List<Bug> ListarPorUserStory(int userStoryId)
        {
            var lista = new List<Bug>();
            string query = @"
                SELECT BugId, CodigoBug, UserStoryId, ProyectoId, Titulo, 
                       PasosReproducir, Severidad, Estado, UsuarioReportaId, 
                       UsuarioAsignadoId, FechaReporte
                FROM dbo.Bugs
                WHERE UserStoryId = @UserStoryId
                ORDER BY BugId ASC;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@UserStoryId", SqlDbType.Int).Value = userStoryId;

                    // Conexion.ObtenerConexion() ya devuelve la conexion abierta:
                    // volver a abrirla lanzaria InvalidOperationException.
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Bug
                            {
                                BugId = Convert.ToInt32(dr["BugId"]),
                                CodigoBug = dr["CodigoBug"].ToString(),
                                UserStoryId = dr["UserStoryId"] != DBNull.Value ? Convert.ToInt32(dr["UserStoryId"]) : (int?)null,
                                ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                                Titulo = dr["Titulo"].ToString(),
                                PasosReproducir = dr["PasosReproducir"].ToString(),
                                Severidad = dr["Severidad"].ToString(),
                                Estado = dr["Estado"].ToString(),
                                UsuarioReportaId = Convert.ToInt32(dr["UsuarioReportaId"]),
                                UsuarioAsignadoId = dr["UsuarioAsignadoId"] != DBNull.Value ? Convert.ToInt32(dr["UsuarioAsignadoId"]) : (int?)null,
                                FechaReporte = Convert.ToDateTime(dr["FechaReporte"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Obtiene todas las incidencias/bugs vinculados a una User Story específica.
        /// </summary>
        /// <param name="userStoryId">Identificador de la Historia de Usuario</param>
        /// <returns>Lista de entidades Bug asociadas</returns>
        public List<Bug> ObtenerBugsPorUserStory(int userStoryId)
        {
            List<Bug> lista = new List<Bug>();
            string query = @"
                SELECT BugId, CodigoBug, UserStoryId, ProyectoId, Titulo, 
                       PasosReproducir, Severidad, Estado, UsuarioReportaId, 
                       UsuarioAsignadoId, FechaReporte
                FROM dbo.Bugs
                WHERE UserStoryId = @UserStoryId
                ORDER BY BugId ASC;";

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
                            lista.Add(MapearBug(dr));
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Inserta un nuevo registro en dbo.Bugs y retorna el BugId generado por IDENTITY.
        /// </summary>
        /// <param name="bug">Entidad Bug a persistir</param>
        /// <returns>ID autogenerado del Bug insertado</returns>
        public int Insertar(Bug bug)
        {
            int nuevoId = 0;
            string query = @"
                INSERT INTO dbo.Bugs 
                    (CodigoBug, UserStoryId, ProyectoId, Titulo, PasosReproducir, 
                     Severidad, Estado, UsuarioReportaId, UsuarioAsignadoId, FechaReporte)
                VALUES 
                    (@CodigoBug, @UserStoryId, @ProyectoId, @Titulo, @PasosReproducir, 
                     @Severidad, @Estado, @UsuarioReportaId, @UsuarioAsignadoId, @FechaReporte);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CodigoBug", SqlDbType.VarChar, 20).Value = bug.CodigoBug;
                    cmd.Parameters.Add("@UserStoryId", SqlDbType.Int).Value = (object)bug.UserStoryId ?? DBNull.Value;
                    cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = bug.ProyectoId;
                    cmd.Parameters.Add("@Titulo", SqlDbType.VarChar, 200).Value = bug.Titulo;
                    cmd.Parameters.Add("@PasosReproducir", SqlDbType.VarChar, -1).Value = bug.PasosReproducir;
                    cmd.Parameters.Add("@Severidad", SqlDbType.VarChar, 20).Value = bug.Severidad ?? "Media";
                    cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value = bug.Estado ?? "Nuevo";
                    cmd.Parameters.Add("@UsuarioReportaId", SqlDbType.Int).Value = bug.UsuarioReportaId;
                    cmd.Parameters.Add("@UsuarioAsignadoId", SqlDbType.Int).Value = (object)bug.UsuarioAsignadoId ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaReporte", SqlDbType.DateTime).Value = bug.FechaReporte == default(DateTime) ? DateTime.Now : bug.FechaReporte;

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        nuevoId = Convert.ToInt32(result);
                    }
                }
            }

            return nuevoId;
        }

        /// <summary>
        /// Obtiene un Bug a partir de su clave primaria BugId.
        /// </summary>
        /// <param name="bugId">Identificador único del Bug</param>
        /// <returns>Entidad Bug o null si no se encuentra</returns>
        public Bug ObtenerPorId(int bugId)
        {
            Bug bug = null;
            string query = @"
                SELECT BugId, CodigoBug, UserStoryId, ProyectoId, Titulo, 
                       PasosReproducir, Severidad, Estado, UsuarioReportaId, 
                       UsuarioAsignadoId, FechaReporte
                FROM dbo.Bugs
                WHERE BugId = @BugId;";

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@BugId", SqlDbType.Int).Value = bugId;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            bug = MapearBug(dr);
                        }
                    }
                }
            }

            return bug;
        }
    }
}

