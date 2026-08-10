using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Proyecto
    {
        public int ProyectoId { get; set; }
        public string ClaveProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string Descripcion { get; set; }
        public string Metodologia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinEstimada { get; set; }
        public DateTime? FechaFinReal { get; set; }
        public bool EsActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
