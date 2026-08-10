using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class ProyectoMiembros
    {
        public int ProyectoId { get; set; }
        public int UsuarioId { get; set; }
        public string RolEnProyecto { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
