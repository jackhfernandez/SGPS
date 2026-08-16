using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class ProyectoMiembro
    {
        public int ProyectoId { get; set; }
        public int UsuarioId { get; set; }
        public string RolEnProyecto { get; set; } = string.Empty;
        public DateTime FechaAsignacion { get; set; }

        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EsActivo { get; set; }

        public string NombreCompleto => string.IsNullOrWhiteSpace(Nombres)
            ? $"Usuario {UsuarioId}"
            : $"{Nombres} {Apellidos}".Trim();
    }
}
