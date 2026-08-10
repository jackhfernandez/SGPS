/*
 * 1. Reporte de uso de IA
 *
 * 2. Prompt: "Genera el archivo SprintAD.cs para la issue TASK-AD-05 con los
 *    metodos CrearSprint, AsignarHistoriaASprint, IniciarSprint y
 *    ObtenerPuntosPendientesBurndown. Usa procedimientos almacenados y respeta
 *    el patron de ProyectoAD.cs: Conexion.ObtenerConexion() ya devuelve la
 *    conexion abierta y los errores se relanzan indicando la capa."
 *
 * 3. Cambios del equipo: Los metodos de escritura leen @@ROWCOUNT con
 *    ExecuteScalar (helper LeerFilasAfectadas) en lugar de ExecuteNonQuery,
 *    porque los procedimientos usan SET NOCOUNT ON y ExecuteNonQuery devolvia
 *    -1 haciendo que todos los bool salieran false.
 */

using Microsoft.Data.SqlClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Datos
{
    public class SprintAD
    {
        // INSERT (el estado inicial 'Planificado' lo aplica el DEFAULT de la tabla)
        public int CrearSprint(Sprint sprint)
        {
            int nuevoSprintId = 0;

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Sprint_Crear", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@proyectoId", sprint.ProyectoId);
                        cmd.Parameters.AddWithValue("@nombreSprint", sprint.NombreSprint);
                        cmd.Parameters.AddWithValue("@sprintGoal", (object?)sprint.SprintGoal ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaInicio", sprint.FechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", sprint.FechaFin);

                        object? result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            nuevoSprintId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Crear Sprint): " + ex.Message);
            }

            return nuevoSprintId;
        }

        // UPDATE (mueve una historia del Product Backlog al Sprint Backlog)
        public bool AsignarHistoriaASprint(int userStoryId, int sprintId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Sprint_AsignarHistoria", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userStoryId", userStoryId);
                        cmd.Parameters.AddWithValue("@sprintId", sprintId);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Asignar Historia a Sprint): " + ex.Message);
            }
        }

        /// <summary>
        /// Cambia el estado del Sprint a 'Activo' y congela la linea base.
        /// </summary>
        /// <returns>
        /// False si el Sprint no estaba 'Planificado', no tiene historias asignadas
        /// o ya hay otro Sprint activo en el proyecto.
        /// </returns>
        public bool IniciarSprint(int sprintId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Sprint_Iniciar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sprintId", sprintId);

                        return LeerFilasAfectadas(cmd) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Iniciar Sprint): " + ex.Message);
            }
        }

        /// <summary>
        /// Suma de Story Points pendientes al cierre de cada dia del Sprint,
        /// que es la linea de trabajo real del Burndown Chart.
        /// </summary>
        public List<PuntoBurndown> ObtenerPuntosPendientesBurndown(int sprintId)
        {
            List<PuntoBurndown> serie = new List<PuntoBurndown>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Sprint_BurndownPuntosPendientes", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sprintId", sprintId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                serie.Add(new PuntoBurndown
                                {
                                    Fecha = Convert.ToDateTime(dr["Fecha"]),
                                    PuntosPendientes = Convert.ToInt32(dr["PuntosPendientes"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Obtener Puntos Pendientes Burndown): " + ex.Message);
            }

            return serie;
        }

        // SELECT (Sprint activo del proyecto, o null si no hay ninguno)
        public Sprint? ObtenerSprintActivo(int proyectoId)
        {
            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Sprint_ObtenerActivo", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                return MapearSprint(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Obtener Sprint Activo): " + ex.Message);
            }

            return null;
        }

        // SELECT POR PROYECTO
        public List<Sprint> ListarPorProyecto(int proyectoId)
        {
            List<Sprint> lista = new List<Sprint>();

            try
            {
                using (var cn = Conexion.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Sprint_ListarPorProyecto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@proyectoId", proyectoId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(MapearSprint(dr));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa datos (Listar Sprints por Proyecto): " + ex.Message);
            }

            return lista;
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

        private static Sprint MapearSprint(SqlDataReader dr)
        {
            return new Sprint
            {
                SprintId = Convert.ToInt32(dr["SprintId"]),
                ProyectoId = Convert.ToInt32(dr["ProyectoId"]),
                NombreSprint = dr["NombreSprint"].ToString() ?? string.Empty,
                SprintGoal = dr["SprintGoal"] != DBNull.Value ? dr["SprintGoal"].ToString() : null,
                FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                Estado = dr["Estado"].ToString() ?? SprintEstadoConstantes.Planificado,
                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                ProyectoNombre = dr["NombreProyecto"] != DBNull.Value ? dr["NombreProyecto"].ToString() : null
            };
        }
    }
}
