using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Sprint
    {
        public int SprintId { get; set; }
        public int ProyectoId { get; set; }
        public string NombreSprint { get; set; } = string.Empty;
        public string? SprintGoal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = SprintEstadoConstantes.Planificado;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string? ProyectoNombre { get; set; }

        public Sprint() { }

        public Sprint(int proyectoId, string nombreSprint, string? sprintGoal, DateTime fechaInicio, DateTime fechaFin)
        {
            ProyectoId = proyectoId;
            NombreSprint = nombreSprint;
            SprintGoal = sprintGoal;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = SprintEstadoConstantes.Planificado;
            FechaCreacion = DateTime.Now;
        }
    }
}
