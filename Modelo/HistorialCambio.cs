using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class HistorialCambio
    {
        public int HistorialId { get; set; }
        public string Entidad { get; set; }
        public int EntidadId { get; set; }
        public string CampoModificado { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaModificacion { get; set; }

        public HistorialCambio()
        {
            FechaModificacion = DateTime.Now;
        }

        public HistorialCambio(string entidad, int entidadId, string campoModificado, string valorAnterior, string valorNuevo, int usuarioId)
            : this()
        {
            Entidad = entidad;
            EntidadId = entidadId;
            CampoModificado = campoModificado;
            ValorAnterior = valorAnterior;
            ValorNuevo = valorNuevo;
            UsuarioId = usuarioId;
        }
    }
}
