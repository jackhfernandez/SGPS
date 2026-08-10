using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class UserStory
    {
        public int UserStoryId { get; set; }
        public string CodigoTicket { get; set; } = string.Empty; // Ej: SGPS-101
        public int ProyectoId { get; set; }
        public int? EpicId { get; set; }
        public int? SprintId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string ComoUsuario { get; set; } = string.Empty;
        public string QuieroFuncionalidad { get; set; } = string.Empty;
        public string ParaBeneficio { get; set; } = string.Empty;
        public string? CriteriosAceptacionTexto { get; set; }
        public string ValorNegocio { get; set; } = ValorNegocioConstantes.Medio;
        public int StoryPoints { get; set; } = 0;
        public string Estado { get; set; } = UserStoryEstadoConstantes.ToDo;
        public int OrdenPrioridad { get; set; } = 0;
        public int? UsuarioAsignadoId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaUltimaModificacion { get; set; } = DateTime.Now;

        public string? EpicNombre { get; set; }
        public string? UsuarioAsignadoNombre { get; set; }
        public string? SprintNombre { get; set; }

        public UserStory() { }
       
        public string FormatoAgilTexto => $"Como {ComoUsuario}, quiero {QuieroFuncionalidad} para {ParaBeneficio}.";
    }
}
