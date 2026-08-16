/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo ProyectoMiembroLN.cs basandote en la entidad
 *    ProyectoMiembro y en la clase ProyectoMiembroAD del proyecto. Implementa
 *    la gestion de miembros de un proyecto (asignar, quitar, cambiar rol y
 *    listar) respetando la estructura actual del proyecto."
 * 3. Cambios del equipo: Se valida el rol contra la lista permitida
 *    (PO, SM, Developer, QA, Cliente). Se protege la regla de negocio de
 *    'al menos un Product Owner' del DoD: no se permite quitar al unico PO
 *    ni cambiarle el rol de PO sin que otro miembro conserve el rol.
 */

using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    public class ProyectoMiembroLN
    {
        private static readonly string[] RolesValidos = { "PO", "SM", "Developer", "QA", "Cliente" };

        private readonly ProyectoMiembroAD _proyectoMiembroAD;

        public ProyectoMiembroLN()
        {
            _proyectoMiembroAD = new ProyectoMiembroAD();
        }

        /// <summary>
        /// Lista los miembros del proyecto con el nombre y correo de cada usuario.
        /// </summary>
        public List<ProyectoMiembro> ListarPorProyecto(int proyectoId)
        {
            if (proyectoId <= 0)
            {
                throw new ArgumentException("El ID del proyecto no es válido.");
            }

            return _proyectoMiembroAD.ListarPorProyecto(proyectoId);
        }

        /// <summary>
        /// Lista los usuarios activos que aún no pertenecen al proyecto.
        /// </summary>
        public List<Usuario> ListarDisponibles(int proyectoId)
        {
            if (proyectoId <= 0)
            {
                throw new ArgumentException("El ID del proyecto no es válido.");
            }

            return _proyectoMiembroAD.ListarDisponibles(proyectoId);
        }

        /// <summary>
        /// Asigna un usuario al proyecto con el rol indicado.
        /// </summary>
        public void Asignar(int proyectoId, int usuarioId, string rolEnProyecto)
        {
            if (proyectoId <= 0)
            {
                throw new ArgumentException("El ID del proyecto no es válido.");
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException("El usuario seleccionado no es válido.");
            }

            string rol = ValidarRol(rolEnProyecto);

            _proyectoMiembroAD.Asignar(proyectoId, usuarioId, rol);
        }

        /// <summary>
        /// Cambia el rol de un miembro del proyecto.
        /// </summary>
        public void CambiarRol(int proyectoId, int usuarioId, string rolEnProyecto)
        {
            if (proyectoId <= 0)
            {
                throw new ArgumentException("El ID del proyecto no es válido.");
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException("El usuario seleccionado no es válido.");
            }

            string rol = ValidarRol(rolEnProyecto);

            ProyectoMiembro miembro = ListarPorProyecto(proyectoId)
                .FirstOrDefault(m => m.UsuarioId == usuarioId)
                ?? throw new InvalidOperationException("El miembro seleccionado ya no forma parte del proyecto.");

            bool esPOActual = EsPO(miembro.RolEnProyecto);
            bool seguiraSiendoPO = EsPO(rol);

            if (esPOActual && !seguiraSiendoPO)
            {
                ValidarExisteOtroPO(proyectoId, usuarioId);
            }

            _proyectoMiembroAD.CambiarRol(proyectoId, usuarioId, rol);
        }

        /// <summary>
        /// Quita a un miembro del proyecto.
        /// </summary>
        public void Quitar(int proyectoId, int usuarioId)
        {
            if (proyectoId <= 0)
            {
                throw new ArgumentException("El ID del proyecto no es válido.");
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException("El usuario seleccionado no es válido.");
            }

            ProyectoMiembro miembro = ListarPorProyecto(proyectoId)
                .FirstOrDefault(m => m.UsuarioId == usuarioId)
                ?? throw new InvalidOperationException("El miembro seleccionado ya no forma parte del proyecto.");

            if (EsPO(miembro.RolEnProyecto))
            {
                ValidarExisteOtroPO(proyectoId, usuarioId);
            }

            _proyectoMiembroAD.Quitar(proyectoId, usuarioId);
        }

        private static string ValidarRol(string rolEnProyecto)
        {
            if (string.IsNullOrWhiteSpace(rolEnProyecto) ||
                !RolesValidos.Contains(rolEnProyecto, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Debe seleccionar un rol válido: PO, SM, Developer, QA o Cliente.");
            }

            return RolesValidos.First(r =>
                string.Equals(r, rolEnProyecto, StringComparison.OrdinalIgnoreCase));
        }

        private static bool EsPO(string rolEnProyecto) =>
            string.Equals(rolEnProyecto, "PO", StringComparison.OrdinalIgnoreCase);

        private void ValidarExisteOtroPO(int proyectoId, int usuarioIdQueSale)
        {
            int otrosPO = _proyectoMiembroAD.ListarPorProyecto(proyectoId)
                .Count(m => EsPO(m.RolEnProyecto) && m.UsuarioId != usuarioIdQueSale);

            if (otrosPO == 0)
            {
                throw new InvalidOperationException(
                    "El proyecto debe tener al menos un Product Owner (PO). Asigna el rol PO a otro miembro antes de continuar.");
            }
        }
    }
}
