using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class ProyectoAD
    {
        // INSERT INTO (Ejecuta inserción de proyecto y asignación atómica de PO)
        public int Agregar(Proyecto proyecto, int creadorUsuarioId)
        {
            int nuevoProyectoId = 0;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Proyecto_Insertar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@claveProyecto", proyecto.ClaveProyecto);
                        cmd.Parameters.AddWithValue("@nombreProyecto", proyecto.NombreProyecto);
                        cmd.Parameters.AddWithValue("@descripcion", (object)proyecto.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@metodologia", proyecto.Metodologia);
                        cmd.Parameters.AddWithValue("@fechaInicio", proyecto.FechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFinEstimada", (object)proyecto.FechaFinEstimada ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@creadorUsuarioId", creadorUsuarioId);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            nuevoProyectoId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Agregar Proyecto): " + ex.Message);
            }

            return nuevoProyectoId;
        }

        // UPDATE
        public void Modificar(Proyecto proyecto)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Proyecto_Modificar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@proyectoId", proyecto.ProyectoId);
                        cmd.Parameters.AddWithValue("@claveProyecto", proyecto.ClaveProyecto);
                        cmd.Parameters.AddWithValue("@nombreProyecto", proyecto.NombreProyecto);
                        cmd.Parameters.AddWithValue("@descripcion", (object)proyecto.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@metodologia", proyecto.Metodologia);
                        cmd.Parameters.AddWithValue("@fechaInicio", proyecto.FechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFinEstimada", (object)proyecto.FechaFinEstimada ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@esActivo", proyecto.EsActivo);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Modificar Proyecto): " + ex.Message);
            }
        }

        // SELECT POR USUARIO
        public List<Proyecto> ListarPorUsuario(int usuarioId)
        {
            List<Proyecto> lista = new List<Proyecto>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Proyecto_ListarPorUsuario", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Proyecto
                                {
                                    ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                                    ClaveProyecto = dr["ClaveProyecto"].ToString(),
                                    NombreProyecto = dr["NombreProyecto"].ToString(),
                                    Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : null,
                                    Metodologia = dr["Metodologia"].ToString(),
                                    FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                                    FechaFinEstimada = dr["FechaFinEstimada"] != DBNull.Value ? Convert.ToDateTime(dr["FechaFinEstimada"]) : (DateTime?)null,
                                    FechaFinReal = dr["FechaFinReal"] != DBNull.Value ? Convert.ToDateTime(dr["FechaFinReal"]) : (DateTime?)null,
                                    EsActivo = Convert.ToBoolean(dr["EsActivo"]),
                                    FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Proyectos por Usuario): " + ex.Message);
            }

            return lista;
        }

        // ASIGNAR MIEMBRO
        public void AsignarMiembro(int proyectoId, int usuarioId, string rolEnProyecto)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Proyecto_AsignarMiembro", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        cmd.Parameters.AddWithValue("@rolEnProyecto", rolEnProyecto);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Asignar Miembro a Proyecto): " + ex.Message);
            }
        }

    }
}
