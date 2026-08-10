using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Tarea
    {
        public int TareaId { get; set; }

        public int UserStoryId { get; set; }

        public string TituloTarea { get; set; }

        public decimal HorasEstimadas { get; set; }
       
        public decimal HorasTrabajadas { get; set; }
        
        public string Estado { get; set; }
        
        public int? UsuarioAsignadoId { get; set; }
        
        public DateTime FechaCreacion { get; set; }

        public Tarea()
        {
            HorasEstimadas = 0.00m;
            HorasTrabajadas = 0.00m;
            Estado = "Pendiente";
            FechaCreacion = DateTime.Now;
        }
    }
}
