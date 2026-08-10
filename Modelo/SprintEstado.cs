using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public enum SprintEstado
    {
        Planificado,
        Activo,
        Cerrado
    }

    public static class SprintEstadoConstantes
    {
        public const string Planificado = "Planificado";
        public const string Activo = "Activo";
        public const string Cerrado = "Cerrado";
    }
}
