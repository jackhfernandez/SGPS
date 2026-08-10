using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public enum UserStoryEstado
    {
        ToDo,
        InProgress,
        InTesting,
        Done
    }

    public static class UserStoryEstadoConstantes
    {
        public const string ToDo = "To Do";
        public const string InProgress = "In Progress";
        public const string InTesting = "In Testing";
        public const string Done = "Done";
    }

    public static class ValorNegocioConstantes
    {
        public const string Alto = "Alto";
        public const string Medio = "Medio";
        public const string Bajo = "Bajo";
    }
}
