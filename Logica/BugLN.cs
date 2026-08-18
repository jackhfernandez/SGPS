using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica
{
    public class BugLN
    {
        private readonly BugAD _bugAD;
        private readonly HistorialCambioAD _historialAD;

        public BugLN()
        {
            _bugAD = new BugAD();
            _historialAD = new HistorialCambioAD();
        }

        /// <summary>
        /// Valida si una User Story tiene bugs abiertos con severidad Bloqueante o Alta.
        /// Cumple con el criterio DoD / CA-14.3 para impedir el pase a 'Done'.
        /// </summary>
        /// <param name="userStoryId">Identificador de la User Story a evaluar</param>
        /// <returns>True si la User Story está bloqueada por QA; False si está limpia</returns>
        public bool TieneBugsBloqueantesOAltos(int userStoryId)
        {
            if (userStoryId <= 0)
                throw new ArgumentException("El identificador de la Historia de Usuario no es válido.", nameof(userStoryId));

            List<Bug> listaBugs = _bugAD.ObtenerBugsPorUserStory(userStoryId);

            if (listaBugs == null || listaBugs.Count == 0)
                return false;

            // Estados no resueltos
            string[] estadosAbiertos = { "Nuevo", "En Proceso" };
            // Severidades que condicionan el pase a Done
            string[] severidadesCriticas = { "Bloqueante", "Alta" };

            return listaBugs.Any(b =>
                estadosAbiertos.Contains(b.Estado, StringComparer.OrdinalIgnoreCase) &&
                severidadesCriticas.Contains(b.Severidad, StringComparer.OrdinalIgnoreCase)
            );
        }

        /// <summary>
        /// Valida el pase a Done de una User Story verificando que no existan bloqueos de QA.
        /// </summary>
        public bool ValidarPaseADone(int userStoryId, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (TieneBugsBloqueantesOAltos(userStoryId))
            {
                mensajeError = "No se puede marcar la Historia de Usuario como 'Done' porque tiene Bugs abiertos con severidad 'Bloqueante' o 'Alta'.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Registra un nuevo defecto reportado por QA.
        /// </summary>
        public bool RegistrarBug(Bug bug, int usuarioId, out string mensajeValidacion)
        {
            mensajeValidacion = string.Empty;

            if (bug == null)
                throw new ArgumentNullException(nameof(bug));

            if (string.IsNullOrWhiteSpace(bug.Titulo))
            {
                mensajeValidacion = "El título del Bug es obligatorio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(bug.PasosReproducir))
            {
                mensajeValidacion = "Debe especificar los pasos para reproducir el defecto.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(bug.Severidad))
            {
                bug.Severidad = "Media";
            }

            bug.Estado = "Nuevo";
            bug.FechaReporte = DateTime.Now;
            bug.UsuarioReportaId = usuarioId;

            int nuevoBugId = _bugAD.Insertar(bug);
            if (nuevoBugId > 0)
            {
                _historialAD.Insertar(new HistorialCambio
                {
                    Entidad = "Bug",
                    EntidadId = nuevoBugId,
                    CampoModificado = "Creacion",
                    ValorAnterior = null,
                    ValorNuevo = $"Bug creado con severidad {bug.Severidad}",
                    UsuarioId = usuarioId,
                    FechaModificacion = DateTime.Now
                });
                return true;
            }

            mensajeValidacion = "Error al persistir el registro del Bug en la base de datos.";
            return false;
        }

        /// <summary>
        /// Lista los Bugs de un proyecto. Los filtros de estado y severidad son
        /// opcionales: en null no se aplican.
        /// </summary>
        public List<Bug> ListarBugsPorProyecto(int proyectoId, string? estado = null, string? severidad = null)
        {
            return _bugAD.ListarBugsPorProyecto(proyectoId, estado, severidad);
        }

        /// <summary>
        /// Genera el siguiente código de bug del proyecto con el formato
        /// BUG-&lt;clave&gt;-NN que exige la especificación (ej. BUG-SGPS-01),
        /// tomando el mayor correlativo existente.
        /// </summary>
        public string GenerarCodigoBug(int proyectoId, string claveProyecto)
        {
            if (string.IsNullOrWhiteSpace(claveProyecto))
                throw new ArgumentException("La clave del proyecto es obligatoria para generar el código.");

            var prefijo = $"BUG-{claveProyecto.Trim().ToUpperInvariant()}";
            var maximo = 0;

            foreach (var bug in _bugAD.ListarBugsPorProyecto(proyectoId))
            {
                var partes = bug.CodigoBug?.Split('-');

                if (partes is { Length: 3 } &&
                    string.Equals($"{partes[0]}-{partes[1]}", prefijo, StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(partes[2], out var numero) &&
                    numero > maximo)
                {
                    maximo = numero;
                }
            }

            return $"{prefijo}-{maximo + 1:00}";
        }

        /// <summary>
        /// Cambia el estado de un Bug (ej. En Proceso, Resuelto, Cerrado).
        /// </summary>
        public bool ActualizarEstadoBug(int bugId, string nuevoEstado, int usuarioId, out string mensaje)
        {
            mensaje = string.Empty;

            Bug bugExistente = _bugAD.ObtenerPorId(bugId);
            if (bugExistente == null)
            {
                mensaje = "El Bug especificado no existe.";
                return false;
            }

            string estadoAnterior = bugExistente.Estado;
            bugExistente.Estado = nuevoEstado;

            bool actualizado = _bugAD.ActualizarEstado(bugId, nuevoEstado);
            if (actualizado)
            {
                _historialAD.Insertar(new HistorialCambio
                {
                    Entidad = "Bug",
                    EntidadId = bugId,
                    CampoModificado = "Estado",
                    ValorAnterior = estadoAnterior,
                    ValorNuevo = nuevoEstado,
                    UsuarioId = usuarioId,
                    FechaModificacion = DateTime.Now
                });
                return true;
            }

            mensaje = "No se pudo actualizar el estado del Bug en la base de datos.";
            return false;
        }
    }
}
