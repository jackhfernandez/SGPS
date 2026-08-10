using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Bug
    {
        public int BugId { get; set; }
        public string CodigoBug { get; set; } // Ej: BUG-SGPS-01
        public int? UserStoryId { get; set; }
        public int ProyectoId { get; set; }
        public string Titulo { get; set; }
        public string PasosReproducir { get; set; }

        /// <summary>
        /// Severidad del defecto. Valores válidos: "Bloqueante", "Alta", "Media", "Baja"
        /// DoR/DoD: Atributos de severidad explícitos.
        /// </summary>
        public string Severidad { get; set; } // Default: 'Media'

        /// <summary>
        /// Estado del ciclo de vida del Bug: "Nuevo", "En Proceso", "Resuelto", "Cerrado".
        /// </summary>
        public string Estado { get; set; } // Default: 'Nuevo'

        public int UsuarioReportaId { get; set; }
        public int? UsuarioAsignadoId { get; set; }
        public DateTime FechaReporte { get; set; }

        public Bug()
        {
            Severidad = "Media";
            Estado = "Nuevo"; 
            FechaReporte = DateTime.Now;
        }
    }
}
