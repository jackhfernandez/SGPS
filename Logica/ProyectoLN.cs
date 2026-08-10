using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Logica
{
    public class ProyectoLN
    {
        private readonly ProyectoAD _proyectoAD;

        public ProyectoLN()
        {
            _proyectoAD = new ProyectoAD();
        }

        /// <summary>
        /// Registra un nuevo proyecto con sus miembros iniciales tras validar las reglas de negocio maestros y DoD.
        /// </summary>
        /// <param name="proyecto">Entidad del proyecto a crear.</param>
        /// <param name="miembrosIniciales">Lista de miembros asignados al proyecto.</param>
        /// <returns>El ID del proyecto creado en SQL Server.</returns>
        public int CrearProyecto(Proyecto proyecto, List<ProyectoMiembro> miembrosIniciales)
        {
            // 1. Validaciones de la entidad Proyecto
            ValidarDatosMaestrosProyecto(proyecto);

            // 2. Validación de Unicidad de Clave (DoD / CA-3.2)
            if (_proyectoAD.ExisteClaveProyecto(proyecto.ClaveProyecto))
            {
                throw new InvalidOperationException($"La clave del proyecto '{proyecto.ClaveProyecto}' ya existe en el sistema. Debe ser única.");
            }

            // 3. Regla DoD: Al menos un Product Owner asignado por proyecto (CA-4.2 / CA-4.3)
            ValidarAsignacionProductOwner(miembrosIniciales);

            // 4. Persistencia en la capa de datos (dbo.Proyectos y dbo.ProyectoMiembros)
            return _proyectoAD.InsertarProyectoConMiembros(proyecto, miembrosIniciales);
        }

        /// <summary>
        /// Valida las reglas de datos maestros sobre el proyecto (Clave, Nombre y Metodología).
        /// </summary>
        public void ValidarDatosMaestrosProyecto(Proyecto proyecto)
        {
            if (proyecto == null)
            {
                throw new ArgumentNullException(nameof(proyecto), "Los datos del proyecto no pueden ser nulos.");
            }

            // Validación de Nombre del Proyecto
            if (string.IsNullOrWhiteSpace(proyecto.NombreProyecto) || proyecto.NombreProyecto.Trim().Length > 150)
            {
                throw new ArgumentException("El nombre del proyecto es obligatorio y no debe exceder los 150 caracteres.");
            }

            // DoD: ClaveProyecto tenga entre 3 y 10 caracteres en mayúsculas (CA-3.1)
            if (string.IsNullOrWhiteSpace(proyecto.ClaveProyecto))
            {
                throw new ArgumentException("La clave del proyecto es obligatoria.");
            }

            proyecto.ClaveProyecto = proyecto.ClaveProyecto.Trim();

            // Expresión regular: Solo letras mayúsculas de 3 a 10 caracteres
            Regex regexClave = new Regex(@"^[A-Z]{3,10}$");
            if (!regexClave.IsMatch(proyecto.ClaveProyecto))
            {
                throw new ArgumentException("La clave del proyecto debe contener entre 3 y 10 caracteres en letras mayúsculas únicamente (Ej. SGPS, PRJ).");
            }

            // Validación de Metodología (Scrum, Kanban, Híbrido) (CA-3.3)
            string[] metodologiasValidas = { "Scrum", "Kanban", "Híbrido" };
            if (string.IsNullOrWhiteSpace(proyecto.Metodologia) ||
                !metodologiasValidas.Contains(proyecto.Metodologia, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Debe seleccionar una metodología válida: Scrum, Kanban o Híbrido.");
            }
        }

        /// <summary>
        /// Verifica la regla de negocio que exige al menos un Product Owner asignado al proyecto.
        /// </summary>
        public void ValidarAsignacionProductOwner(List<ProyectoMiembro> miembros)
        {
            if (miembros == null || miembros.Count == 0)
            {
                throw new InvalidOperationException("El proyecto debe tener al menos un miembro asignado.");
            }

            // Comprueba la existencia del rol 'PO' o 'Product Owner'
            bool tienePO = miembros.Any(m =>
                string.Equals(m.RolEnProyecto, "PO", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(m.RolEnProyecto, "Product Owner", StringComparison.OrdinalIgnoreCase));

            if (!tienePO)
            {
                throw new InvalidOperationException("Regla de negocio no cumplida: Debe haber al menos un Product Owner ('PO') asignado al proyecto.");
            }
        }

        /// <summary>
        /// Obtiene un proyecto por su ID.
        /// </summary>
        public Proyecto ObtenerPorId(int proyectoId)
        {
            if (proyectoId <= 0)
            {
                throw new ArgumentException("El ID del proyecto no es válido.");
            }

            return _proyectoAD.ObtenerPorId(proyectoId);
        }
    }
}