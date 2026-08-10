using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Epic
    {
        public int EpicId { get; set; }
        public int ProyectoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string ColorHex { get; set; } = "#3182CE";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string? ProyectoNombre { get; set; }

        public Epic() { }

        public Epic(int proyectoId, string titulo, string? descripcion, string colorHex = "#3182CE")
        {
            ProyectoId = proyectoId;
            Titulo = titulo;
            Descripcion = descripcion;
            ColorHex = colorHex;
            FechaCreacion = DateTime.Now;
        }
    }
}
