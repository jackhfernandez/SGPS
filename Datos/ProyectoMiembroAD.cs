using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class ProyectoMiembroAD
    {
        // ASIGNAR MIEMBRO (inserta o actualiza rol si ya es miembro)
        public void Asignar(int proyectoId, int usuarioId, string rolEnProyecto)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProyectoMiembro_Asignar", cn))
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

        // QUITAR MIEMBRO DEL PROYECTO
        public void Quitar(int proyectoId, int usuarioId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProyectoMiembro_Quitar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Quitar Miembro del Proyecto): " + ex.Message);
            }
        }

        // CAMBIAR ROL DE UN MIEMBRO
        public void CambiarRol(int proyectoId, int usuarioId, string rolEnProyecto)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProyectoMiembro_CambiarRol", cn))
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
                throw new Exception("Error en la capa datos (Cambiar Rol de Miembro): " + ex.Message);
            }
        }

        // LISTAR MIEMBROS DE UN PROYECTO (con datos del usuario)
        public List<ProyectoMiembro> ListarPorProyecto(int proyectoId)
        {
            List<ProyectoMiembro> lista = new List<ProyectoMiembro>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProyectoMiembro_ListarPorProyecto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new ProyectoMiembro
                                {
                                    ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                                    UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                    RolEnProyecto = dr["RolEnProyecto"].ToString(),
                                    FechaAsignacion = Convert.ToDateTime(dr["FechaAsignacion"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    EsActivo = Convert.ToBoolean(dr["EsActivo"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Miembros por Proyecto): " + ex.Message);
            }

            return lista;
        }

        // LISTAR USUARIOS DISPONIBLES (activos y sin asignacion al proyecto)
        public List<Usuario> ListarDisponibles(int proyectoId)
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProyectoMiembro_ListarDisponibles", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Usuario
                                {
                                    UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    EsActivo = Convert.ToBoolean(dr["EsActivo"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Usuarios Disponibles): " + ex.Message);
            }

            return lista;
        }
    }
}
