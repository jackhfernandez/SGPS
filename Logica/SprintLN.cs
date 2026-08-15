using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica
{
    public class SprintLN
    {
        private readonly SprintAD _sprintAD;
        private readonly UserStoryAD _userStoryAD;

        public SprintLN()
        {
            _sprintAD = new SprintAD();
            _userStoryAD = new UserStoryAD();
        }

        /// <summary>
        /// Valida y registra un nuevo Sprint en estado Planificado.
        /// Cumple con DoD: Previene solapamiento de fechas en el mismo proyecto.
        /// </summary>
        public bool CrearSprint(Sprint sprint, out string mensajeError)
        {
            mensajeError = string.Empty;

            // Validaciones básicas de entrada
            if (sprint == null)
            {
                mensajeError = "La entidad Sprint no puede ser nula.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(sprint.NombreSprint))
            {
                mensajeError = "El nombre del Sprint es obligatorio.";
                return false;
            }

            if (sprint.FechaInicio.Date > sprint.FechaFin.Date)
            {
                mensajeError = "La fecha de inicio no puede ser posterior a la fecha de fin.";
                return false;
            }

            // DoD: Prevenir solapamiento de fechas en el mismo proyecto (CA-8.3)
            List<Sprint> sprintsExistentes = _sprintAD.ListarPorProyecto(sprint.ProyectoId);
            bool haySolapamiento = sprintsExistentes.Any(s =>
                s.SprintId != sprint.SprintId &&
                s.Estado != "Cerrado" &&
                sprint.FechaInicio.Date <= s.FechaFin.Date &&
                sprint.FechaFin.Date >= s.FechaInicio.Date
            );

            if (haySolapamiento)
            {
                mensajeError = "Las fechas del Sprint se solapan con una iteración existente o activa en el proyecto.";
                return false;
            }

            sprint.Estado = "Planificado";
            sprint.FechaCreacion = DateTime.Now;

            return _sprintAD.Insertar(sprint, out mensajeError);
        }

        /// <summary>
        /// Valida Capacity Planning y reglas antes de iniciar formalmente la ejecución del Sprint.
        /// Cumple con DoD: Valida límite de Story Points y unicidad de Sprint activo.
        /// </summary>
        public bool IniciarSprint(int sprintId, int capacidadMaximaStoryPoints, out string mensajeError)
        {
            mensajeError = string.Empty;

            Sprint sprint = _sprintAD.ObtenerPorId(sprintId);
            if (sprint == null)
            {
                mensajeError = "El Sprint especificado no existe.";
                return false;
            }

            if (sprint.Estado == "Activo")
            {
                mensajeError = "El Sprint ya se encuentra en ejecución.";
                return false;
            }

            // CA-10.1: Verificar que no haya otro Sprint actualmente activo en el proyecto
            List<Sprint> sprintsProyecto = _sprintAD.ListarPorProyecto(sprint.ProyectoId);
            if (sprintsProyecto.Any(s => s.Estado == "Activo" && s.SprintId != sprintId))
            {
                mensajeError = "Ya existe un Sprint activo en este proyecto. Debe cerrarlo antes de iniciar uno nuevo.";
                return false;
            }

            // Obtener historias asociadas al Sprint Backlog
            List<UserStory> historiasSprint = _userStoryAD.ListarPorSprint(sprintId);

            // CA-10.1: Debe tener al menos una historia asignada
            if (historiasSprint == null || historiasSprint.Count == 0)
            {
                mensajeError = "No se puede iniciar un Sprint sin historias de usuario asignadas.";
                return false;
            }

            // DoD / CA-9.3 / US-04.3: Validar que la suma de Story Points no supere el límite de capacidad
            int totalStoryPoints = historiasSprint.Sum(us => us.StoryPoints);
            if (capacidadMaximaStoryPoints > 0 && totalStoryPoints > capacidadMaximaStoryPoints)
            {
                mensajeError = $"Capacidad excedida: El Sprint contiene {totalStoryPoints} Story Points, superando el límite de capacidad del equipo ({capacidadMaximaStoryPoints} SP).";
                return false;
            }

            // CA-10.2 / CA-10.3: Transicionar estado a Activo y fijar línea base
            sprint.Estado = "Activo";
            return _sprintAD.ActualizarEstado(sprint.SprintId, "Activo", out mensajeError);
        }

        /// <summary>
        /// Obtiene la carga total de Story Points asignados al Sprint para Capacity Planning.
        /// </summary>
        public int CalcularStoryPointsComprometidos(int sprintId)
        {
            List<UserStory> historias = _userStoryAD.ListarPorSprint(sprintId);
            return historias?.Sum(h => h.StoryPoints) ?? 0;
        }
    }
}
