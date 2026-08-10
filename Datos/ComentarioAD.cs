/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo ComentarioAD.cs para la issue TASK-AD-08 con
 *    los metodos InsertarComentario y ObtenerComentariosPorStory sobre la
 *    tabla dbo.Comentarios. Usa procedimientos almacenados y respeta el patron
 *    de ProyectoAD.cs."
 * 3. Cambios del equipo:
 *    El listado hace INNER JOIN con dbo.Usuarios para
 *    traer el nombre completo del autor en la propiedad AutorNombre, porque el
 *    hilo de comentarios debe mostrar autor y marca de tiempo y si no fuera asi
 *    la capa de presentacion tendria que consultar los usuarios uno
 *    por uno.
 */

using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class ComentarioAD
    {
        // INSERT
        public int InsertarComentario(Comentario comentario)
        {
            int nuevoComentarioId = 0;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Comentario_Insertar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userStoryId", comentario.UserStoryId);
                        cmd.Parameters.AddWithValue("@usuarioId", comentario.UsuarioId);
                        cmd.Parameters.AddWithValue("@textoComentario", comentario.TextoComentario);

                        object? result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            nuevoComentarioId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Insertar Comentario): " + ex.Message);
            }

            return nuevoComentarioId;
        }

        /// <summary>
        /// Hilo de comentarios de una User Story, del mas antiguo al mas reciente
        /// e incluyendo el nombre completo del autor.
        /// </summary>
        public List<Comentario> ObtenerComentariosPorStory(int userStoryId)
        {
            List<Comentario> lista = new List<Comentario>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Comentario_ListarPorUserStory", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userStoryId", userStoryId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Comentario
                                {
                                    ComentarioId = Convert.ToInt32(dr["ComentarioId"]),
                                    UserStoryId = Convert.ToInt32(dr["UserStoryId"]),
                                    UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                    TextoComentario = dr["TextoComentario"].ToString() ?? string.Empty,
                                    FechaComentario = Convert.ToDateTime(dr["FechaComentario"]),
                                    AutorNombre = dr["AutorNombre"] != DBNull.Value ? dr["AutorNombre"].ToString() : null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Obtener Comentarios por User Story): " + ex.Message);
            }

            return lista;
        }
    }
}
