using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica
{
    public class TareaLN
    {
        private readonly TareaAD _tareaAD;
        private readonly UserStoryAD _userStoryAD;
        private readonly BugAD _bugAD;
        private readonly HistorialCambioAD _historialCambioAD;

        public TareaLN()
        {
            _tareaAD = new TareaAD();
            _userStoryAD = new UserStoryAD();
            _bugAD = new BugAD();
            _historialCambioAD = new HistorialCambioAD();
        }

        #region Métodos CRUD y Gestión de Tareas Técnicas

        public int RegistrarTarea(Tarea tarea)
        {
            if (tarea == null)
                throw new ArgumentNullException(nameof(tarea), "Los datos de la tarea no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(tarea.TituloTarea))
                throw new ArgumentException("El título de la tarea técnica es obligatorio.");

            if (tarea.HorasEstimadas < 0)
                throw new ArgumentException("Las horas estimadas no pueden ser negativas.");

            // Estado inicial por defecto en SGPS_DB
            if (string.IsNullOrWhiteSpace(tarea.Estado))
                tarea.Estado = TareaEstadoConstantes.Pendiente;

            return _tareaAD.Insertar(tarea);
        }

        /// <summary>
        /// Tarea tecnica por identificador, o null si no existe.
        /// </summary>
        public Tarea? ObtenerTarea(int tareaId) => _tareaAD.ObtenerPorId(tareaId);

        /// <summary>
        /// Guarda los cambios del formulario de desglose tecnico: titulo,
        /// horas, estado y responsable de una tarea ya registrada.
        /// </summary>
        public bool ActualizarTarea(Tarea tarea)
        {
            if (tarea == null)
                throw new ArgumentNullException(nameof(tarea), "Los datos de la tarea no pueden ser nulos.");

            if (tarea.TareaId <= 0)
                throw new ArgumentException("La tarea a modificar no tiene un identificador válido.");

            if (string.IsNullOrWhiteSpace(tarea.TituloTarea))
                throw new ArgumentException("El título de la tarea técnica es obligatorio.");

            if (tarea.HorasEstimadas < 0 || tarea.HorasTrabajadas < 0)
                throw new ArgumentException("Las horas no pueden tener valores negativos.");

            if (!TareaEstadoConstantes.Orden.Contains(tarea.Estado))
                throw new InvalidOperationException($"Estado '{tarea.Estado}' no es válido para tareas técnicas.");

            return _tareaAD.Actualizar(tarea);
        }

        /// <summary>
        /// Estado que sigue en el avance de una tarea tecnica
        /// (Pendiente → En Proceso → Completado), o null si ya es el ultimo.
        /// </summary>
        public static string? SiguienteEstadoTarea(string estadoActual)
        {
            int indice = Array.IndexOf(TareaEstadoConstantes.Orden, estadoActual);

            return indice >= 0 && indice < TareaEstadoConstantes.Orden.Length - 1
                ? TareaEstadoConstantes.Orden[indice + 1]
                : null;
        }

        public bool ActualizarImputacionHoras(int tareaId, decimal horasTrabajadas, decimal horasEstimadas)
        {
            if (horasTrabajadas < 0 || horasEstimadas < 0)
                throw new ArgumentException("Las horas no pueden tener valores negativos.");

            Tarea tarea = _tareaAD.ObtenerPorId(tareaId);
            if (tarea == null)
                throw new KeyNotFoundException("La tarea especificada no existe.");

            tarea.HorasTrabajadas = horasTrabajadas;
            tarea.HorasEstimadas = horasEstimadas;

            return _tareaAD.Actualizar(tarea);
        }

        public bool CambiarEstadoTareaTecnica(int tareaId, string nuevoEstado)
        {
            if (!TareaEstadoConstantes.Orden.Contains(nuevoEstado))
                throw new InvalidOperationException($"Estado '{nuevoEstado}' no es válido para tareas técnicas.");

            Tarea tarea = _tareaAD.ObtenerPorId(tareaId);
            if (tarea == null)
                throw new KeyNotFoundException("La tarea técnica no existe.");

            tarea.Estado = nuevoEstado;
            return _tareaAD.Actualizar(tarea);
        }

        public List<Tarea> ListarTareasPorUserStory(int userStoryId)
        {
            return _tareaAD.ListarPorUserStory(userStoryId);
        }

        #endregion

        #region Flujo de Transición de Estados Kanban & Reglas de Validación

        /// <summary>
        /// Valida y actualiza el flujo de estados Kanban para una User Story.
        /// Flujo permitido: To Do -> In Progress -> In Testing -> Done
        /// </summary>
        public bool CambiarEstadoKanbanUserStory(int userStoryId, string nuevoEstado, int usuarioId)
        {
            UserStory us = _userStoryAD.ObtenerPorId(userStoryId);
            if (us == null)
                throw new KeyNotFoundException($"No se encontró la User Story con ID {userStoryId}.");

            string estadoActual = us.Estado;

            // Validar transiciones de estado secuenciales válidas
            ValidarTransicionKanban(estadoActual, nuevoEstado);

            // Regla DoD: Validación antes de pasar a "Done"
            if (nuevoEstado.Equals("Done", StringComparison.OrdinalIgnoreCase))
            {
                ValidarReglasPasoADone(userStoryId);
            }

            // Actualización de estado en User Story
            us.Estado = nuevoEstado;
            us.FechaUltimaModificacion = DateTime.Now;

            // Autoasignación si pasa a "In Progress" y no tiene responsable (CA-12.2)
            if (nuevoEstado.Equals("In Progress", StringComparison.OrdinalIgnoreCase) && !us.UsuarioAsignadoId.HasValue)
            {
                us.UsuarioAsignadoId = usuarioId;
            }

            bool actualizado = _userStoryAD.ActualizarBool(us);

            if (actualizado)
            {
                // Registrar Audit Trail (dbo.HistorialCambios)
                _historialCambioAD.Insertar(new HistorialCambio
                {
                    Entidad = "UserStory",
                    EntidadId = userStoryId,
                    CampoModificado = "Estado",
                    ValorAnterior = estadoActual,
                    ValorNuevo = nuevoEstado,
                    UsuarioId = usuarioId,
                    FechaModificacion = DateTime.Now
                });
            }

            return actualizado;
        }

        /// <summary>
        /// Regla de Negocio / DoD:
        /// 1. No permite mover a 'Done' si existen tareas técnicas pendientes o en proceso.
        /// 2. No permite mover a 'Done' si existen Bugs Bloqueantes o Altos sin resolver.
        /// </summary>
        public void ValidarReglasPasoADone(int userStoryId)
        {
            // 1. Validar tareas técnicas pendientes
            List<Tarea> tareas = _tareaAD.ListarPorUserStory(userStoryId);
            bool tieneTareasPendientes = tareas.Any(t => !t.Estado.Equals("Completado", StringComparison.OrdinalIgnoreCase));

            if (tieneTareasPendientes)
            {
                throw new InvalidOperationException("No se puede mover la historia a 'Done' porque aún existen tareas técnicas pendientes o en proceso.");
            }

            // 2. Validar Bugs no resueltos (CA-14.3)
            List<Bug> bugs = _bugAD.ListarPorUserStory(userStoryId);
            bool tieneBugsBloqueantes = bugs.Any(b =>
                (b.Severidad.Equals("Bloqueante", StringComparison.OrdinalIgnoreCase) ||
                 b.Severidad.Equals("Alta", StringComparison.OrdinalIgnoreCase)) &&
                !b.Estado.Equals("Resuelto", StringComparison.OrdinalIgnoreCase) &&
                !b.Estado.Equals("Cerrado", StringComparison.OrdinalIgnoreCase));

            if (tieneBugsBloqueantes)
            {
                throw new InvalidOperationException("No se puede mover la historia a 'Done' porque tiene defectos vinculados de severidad Bloqueante/Alta sin resolver.");
            }
        }

        private void ValidarTransicionKanban(string estadoActual, string nuevoEstado)
        {
            if (estadoActual.Equals(nuevoEstado, StringComparison.OrdinalIgnoreCase))
                return;

            bool esValido = false;

            switch (estadoActual)
            {
                case "To Do":
                    esValido = nuevoEstado == "In Progress";
                    break;
                case "In Progress":
                    esValido = nuevoEstado == "In Testing" || nuevoEstado == "To Do";
                    break;
                case "In Testing":
                    // Retorno a In Progress por fallo QA o avance a Done
                    esValido = nuevoEstado == "Done" || nuevoEstado == "In Progress";
                    break;
                case "Done":
                    // Reabrir en caso de desestimación
                    esValido = nuevoEstado == "In Testing" || nuevoEstado == "In Progress";
                    break;
            }

            if (!esValido)
            {
                throw new InvalidOperationException($"Transición de estado no permitida en el flujo Kanban: de '{estadoActual}' a '{nuevoEstado}'.");
            }
        }

        #endregion
    }
}

