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

        // Metodos Adicionales - Reglas de Negocio

        // VERIFICAR UNICIDAD DE CLAVE
        public bool ExisteClaveProyecto(string claveProyecto)
        {
            bool existe = false;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    string query = "SELECT COUNT(1) FROM dbo.Proyectos WHERE UPPER(ClaveProyecto) = UPPER(@claveProyecto)";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@claveProyecto", claveProyecto.Trim());

                        if (cn.State == ConnectionState.Closed)
                            cn.Open();

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (ExisteClaveProyecto): " + ex.Message);
            }

            return existe;
        }

        // OBTENER PROYECTO POR ID
        public Proyecto ObtenerPorId(int proyectoId)
        {
            Proyecto proyecto = null;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    string query = @"SELECT ProyectoId, ClaveProyecto, NombreProyecto, Descripcion, 
                                            Metodologia, FechaInicio, FechaFinEstimada, FechaFinReal, 
                                            EsActivo, FechaCreacion 
                                     FROM dbo.Proyectos 
                                     WHERE ProyectoId = @proyectoId";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);

                        if (cn.State == ConnectionState.Closed)
                            cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                proyecto = new Proyecto
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
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Obtener Proyecto por ID): " + ex.Message);
            }

            return proyecto;
        }

        // DESASIGNAR MIEMBRO DEL PROYECTO
        public void DesasignarMiembro(int proyectoId, int usuarioId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    string query = "DELETE FROM dbo.ProyectoMiembros WHERE ProyectoId = @proyectoId AND UsuarioId = @usuarioId";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                        if (cn.State == ConnectionState.Closed)
                            cn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Desasignar Miembro): " + ex.Message);
            }
        }

        // LISTAR MIEMBROS DE UN PROYECTO
        public List<ProyectoMiembro> ObtenerMiembrosPorProyecto(int proyectoId)
        {
            List<ProyectoMiembro> lista = new List<ProyectoMiembro>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    string query = @"SELECT ProyectoId, UsuarioId, RolEnProyecto, FechaAsignacion 
                                     FROM dbo.ProyectoMiembros 
                                     WHERE ProyectoId = @proyectoId";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);

                        if (cn.State == ConnectionState.Closed)
                            cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new ProyectoMiembro
                                {
                                    ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                                    UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                    RolEnProyecto = dr["RolEnProyecto"].ToString(),
                                    FechaAsignacion = Convert.ToDateTime(dr["FechaAsignacion"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Obtener Miembros Por Proyecto): " + ex.Message);
            }

            return lista;
        }

        // CAMBIAR ESTADO ACTIVO (ARCHIVAR / FINALIZAR PROYECTO)
        public void CambiarEstadoActivo(int proyectoId, bool esActivo)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    string query = @"UPDATE dbo.Proyectos 
                                     SET EsActivo = @esActivo, 
                                         FechaFinReal = CASE WHEN @esActivo = 0 THEN GETDATE() ELSE NULL END 
                                     WHERE ProyectoId = @proyectoId";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);
                        cmd.Parameters.AddWithValue("@esActivo", esActivo);

                        if (cn.State == ConnectionState.Closed)
                            cn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Cambiar Estado Activo Proyecto): " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta un nuevo proyecto y registra atómicamente a sus miembros iniciales dentro de una transacción.
        /// Requerido por ProyectoLN (TASK-LN-03 / US-02.1 / US-02.2).
        /// </summary>
        /// <param name="proyecto">Entidad con los datos maestros del proyecto.</param>
        /// <param name="miembrosIniciales">Lista de miembros asignados al proyecto.</param>
        /// <returns>El ProyectoId generado por la base de datos SQL Server.</returns>
        public int InsertarProyectoConMiembros(Proyecto proyecto, List<ProyectoMiembro> miembrosIniciales)
        {
            int nuevoProyectoId = 0;

            using (var cn = Conexion.ObtenerConexion())
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                // Iniciamos la transacción para asegurar atomicidad
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        // 1. Inserción del Proyecto mediante Stored Procedure sp_Proyecto_Insertar
                        using (SqlCommand cmdProyecto = new SqlCommand("sp_Proyecto_Insertar", cn, tx))
                        {
                            cmdProyecto.CommandType = CommandType.StoredProcedure;

                            cmdProyecto.Parameters.AddWithValue("@claveProyecto", proyecto.ClaveProyecto.Trim().ToUpper());
                            cmdProyecto.Parameters.AddWithValue("@nombreProyecto", proyecto.NombreProyecto.Trim());
                            cmdProyecto.Parameters.AddWithValue("@descripcion", (object)proyecto.Descripcion ?? DBNull.Value);
                            cmdProyecto.Parameters.AddWithValue("@metodologia", proyecto.Metodologia);
                            cmdProyecto.Parameters.AddWithValue("@fechaInicio", proyecto.FechaInicio);
                            cmdProyecto.Parameters.AddWithValue("@fechaFinEstimada", (object)proyecto.FechaFinEstimada ?? DBNull.Value);

                            object result = cmdProyecto.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                nuevoProyectoId = Convert.ToInt32(result);
                            }
                            else
                            {
                                throw new Exception("No se pudo obtener el ID del proyecto recién generado.");
                            }
                        }

                        // 2. Inserción de los miembros en la tabla dbo.ProyectoMiembros
                        if (miembrosIniciales != null && miembrosIniciales.Count > 0)
                        {
                            string queryMiembro = @"INSERT INTO dbo.ProyectoMiembros (ProyectoId, UsuarioId, RolEnProyecto, FechaAsignacion) 
                                                    VALUES (@proyectoId, @usuarioId, @rolEnProyecto, GETDATE());";

                            foreach (var miembro in miembrosIniciales)
                            {
                                using (SqlCommand cmdMiembro = new SqlCommand(queryMiembro, cn, tx))
                                {
                                    cmdMiembro.CommandType = CommandType.Text;
                                    cmdMiembro.Parameters.AddWithValue("@proyectoId", nuevoProyectoId);
                                    cmdMiembro.Parameters.AddWithValue("@usuarioId", miembro.UsuarioId);
                                    cmdMiembro.Parameters.AddWithValue("@rolEnProyecto", miembro.RolEnProyecto);

                                    cmdMiembro.ExecuteNonQuery();
                                }
                            }
                        }

                        // Confirma la transacción si todo fue exitoso
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Revierte cualquier cambio en caso de error
                        tx.Rollback();
                        throw new Exception("Error en la capa de datos (InsertarProyectoConMiembros): " + ex.Message, ex);
                    }
                }
            }

            return nuevoProyectoId;
        }
    }
}
