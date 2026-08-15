using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class BurndownReporte
    {
        public int SprintId { get; set; }
        public string NombreSprint { get; set; }
        public int TotalStoryPoints { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<PuntoBurndown> Puntos { get; set; } = new List<PuntoBurndown>();
    }
}
