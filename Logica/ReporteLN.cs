using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica
{
    public class ReporteLN
    {
        private readonly SprintAD _sprintAD;
        private readonly UserStoryAD _userStoryAD;
        private readonly HistorialCambioAD _historialCambioAD;

        public ReporteLN()
        {
            _sprintAD = new SprintAD();
            _userStoryAD = new UserStoryAD();
            _historialCambioAD = new HistorialCambioAD();
        }

        /// <summary>
        /// TASK-LN-08: Genera coordenadas para la línea guía ideal decreciente 
        /// y la línea real de Story Points restantes por día (US-08.1 / CA-15.1, CA-15.2).
        /// </summary>
        public BurndownReporte GenerarBurndownSprint(int sprintId)
        {
            // 1. Obtener información base del Sprint (dbo.Sprints)
            Sprint sprint = _sprintAD.ObtenerPorId(sprintId);
            if (sprint == null)
            {
                throw new ArgumentException($"El Sprint con ID {sprintId} no existe.");
            }

            // 2. Obtener Historias de Usuario asignadas al Sprint (dbo.UserStories)
            List<UserStory> historias = _userStoryAD.ListarPorSprint(sprintId);
            int totalPuntos = historias.Sum(u => u.StoryPoints);

            BurndownReporte reporte = new BurndownReporte
            {
                SprintId = sprint.SprintId,
                NombreSprint = sprint.NombreSprint,
                TotalStoryPoints = totalPuntos,
                FechaInicio = sprint.FechaInicio.Date,
                FechaFin = sprint.FechaFin.Date
            };

            // 3. Obtener transiciones a estado 'Done' desde el historial (dbo.HistorialCambios)
            List<HistorialCambio> cambiosDone = _historialCambioAD.ListarPorEntidad("UserStory")
                .Where(h => h.CampoModificado == "Estado" && h.ValorNuevo == "Done")
                .ToList();

            // 4. Calcular intervalo total de días (Eje X)
            int duracionDias = (sprint.FechaFin.Date - sprint.FechaInicio.Date).Days;
            if (duracionDias <= 0) duracionDias = 1;

            double decrementoIdealDiario = (double)totalPuntos / duracionDias;
            DateTime fechaCorteActual = DateTime.Today;

            // 5. Generar coordenadas día a día
            for (int dia = 0; dia <= duracionDias; dia++)
            {
                DateTime fechaDia = sprint.FechaInicio.Date.AddDays(dia);

                // Línea Guía Ideal (recta decreciente hasta 0)
                double ideal = Math.Max(0, totalPuntos - (decrementoIdealDiario * dia));

                // Línea Real de Trabajo Restante
                double? realRestante = null;
                if (fechaDia <= fechaCorteActual)
                {
                    // Obtener IDs de historias completadas hasta la fecha actual
                    var historiasCompletadasIds = cambiosDone
                        .Where(c => c.FechaModificacion.Date <= fechaDia)
                        .Select(c => c.EntidadId)
                        .Distinct();

                    int puntosCompletados = historias
                        .Where(h => historiasCompletadasIds.Contains(h.UserStoryId))
                        .Sum(h => h.StoryPoints);

                    realRestante = Math.Max(0, totalPuntos - puntosCompletados);
                }

                reporte.Puntos.Add(new PuntoBurndown
                {
                    DiaNumero = dia,
                    Fecha = fechaDia,
                    PuntosIdeales = Math.Round(ideal, 2),
                    PuntosRealesRestantes = realRestante.HasValue ? Math.Round(realRestante.Value, 2) : (double?)null
                });
            }

            return reporte;
        }
    }
}
