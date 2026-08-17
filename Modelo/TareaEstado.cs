using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    /// <summary>
    /// Estados del desglose tecnico (dbo.Tareas.Estado). Son independientes de
    /// los estados Kanban de la historia (<see cref="UserStoryEstadoConstantes"/>).
    /// </summary>
    public static class TareaEstadoConstantes
    {
        public const string Pendiente = "Pendiente";
        public const string EnProceso = "En Proceso";
        public const string Completado = "Completado";

        /// <summary>Estados en orden de avance, tal y como los valida TareaLN.</summary>
        public static readonly string[] Orden = { Pendiente, EnProceso, Completado };
    }
}
