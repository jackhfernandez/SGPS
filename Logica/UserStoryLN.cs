using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica
{
    public class UserStoryLN
    {
        private readonly UserStoryAD _userStoryAD;

        // Escala Fibonacci estándar según especificación DoD (1, 2, 3, 5, 8, 13, 21)
        private static readonly HashSet<int> EscalaFibonacciValida = new HashSet<int> { 1, 2, 3, 5, 8, 13, 21 };

        public UserStoryLN()
        {
            _userStoryAD = new UserStoryAD();
        }

        public void ValidarEstructuraHistoria(UserStory historia)
        {
            if (historia == null)
                throw new ArgumentNullException(nameof(historia), "La historia de usuario no puede ser nula.");

            if (string.IsNullOrWhiteSpace(historia.Titulo))
                throw new InvalidOperationException("El título de la historia de usuario es obligatorio.");

            // Validación del formato estándar ágil: Como / Quiero / Para
            if (string.IsNullOrWhiteSpace(historia.ComoUsuario))
                throw new InvalidOperationException("El campo 'Como [tipo de usuario]' es obligatorio.");

            if (string.IsNullOrWhiteSpace(historia.QuieroFuncionalidad))
                throw new InvalidOperationException("El campo 'Quiero [funcionalidad]' es obligatorio.");

            if (string.IsNullOrWhiteSpace(historia.ParaBeneficio))
                throw new InvalidOperationException("El campo 'Para [beneficio]' es obligatorio.");

            // Validación del rango de Valor de Negocio
            var valoresValidos = new[] { "Alto", "Medio", "Bajo" };
            if (string.IsNullOrWhiteSpace(historia.ValorNegocio) || !valoresValidos.Contains(historia.ValorNegocio))
                throw new InvalidOperationException("El Valor de Negocio debe ser 'Alto', 'Medio' o 'Bajo'.");

            // Restricción de estimaciones Story Points a la escala Fibonacci
            if (!EscalaFibonacciValida.Contains(historia.StoryPoints))
            {
                throw new InvalidOperationException(
                    $"Estimación de Story Points inválida ({historia.StoryPoints}). " +
                    $"Debe pertenecer a la escala Fibonacci permitida: {string.Join(", ", EscalaFibonacciValida)}.");
            }
        }

        public void Crear(UserStory historia)
        {
            ValidarEstructuraHistoria(historia);

            if (string.IsNullOrWhiteSpace(historia.CodigoTicket))
                throw new InvalidOperationException("El código de ticket de la historia es obligatorio.");

            historia.CodigoTicket = historia.CodigoTicket.Trim().ToUpperInvariant();

            // dbo.UserStories.CodigoTicket es UNIQUE: se valida aquí para no
            // exponer el error crudo de SQL Server.
            if (_userStoryAD.ExisteCodigoTicket(historia.CodigoTicket))
                throw new InvalidOperationException($"Ya existe una historia con el código '{historia.CodigoTicket}'.");

            historia.FechaCreacion = DateTime.Now;
            historia.FechaUltimaModificacion = DateTime.Now;

            _userStoryAD.Insertar(historia);
        }

        public void Modificar(UserStory historia)
        {
            ValidarEstructuraHistoria(historia);

            if (historia.UserStoryId <= 0)
                throw new InvalidOperationException("La historia a modificar no tiene un identificador válido.");

            historia.FechaUltimaModificacion = DateTime.Now;

            _userStoryAD.Actualizar(historia);
        }

        public void Eliminar(int userStoryId)
        {
            if (userStoryId <= 0)
                throw new InvalidOperationException("El identificador de la historia no es válido.");

            // La FK de dbo.Bugs no tiene ON DELETE CASCADE, a diferencia de
            // Tareas y Comentarios.
            var bugs = _userStoryAD.ContarBugs(userStoryId);

            if (bugs > 0)
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar la historia porque tiene {bugs} bug(s) asociado(s). " +
                    "Cierra o reasigna esos bugs primero.");
            }

            _userStoryAD.Eliminar(userStoryId);
        }

        public UserStory ObtenerPorId(int userStoryId) => _userStoryAD.ObtenerPorId(userStoryId);

        /// <summary>
        /// Genera el siguiente código de ticket del proyecto con el formato
        /// CLAVE-N que exige la especificación (ej. SGPS-101), tomando el
        /// mayor correlativo existente.
        /// </summary>
        public string GenerarCodigoTicket(int proyectoId, string claveProyecto)
        {
            if (string.IsNullOrWhiteSpace(claveProyecto))
                throw new ArgumentException("La clave del proyecto es obligatoria para generar el código.");

            var prefijo = claveProyecto.Trim().ToUpperInvariant();
            var maximo = 100;

            foreach (var historia in _userStoryAD.ListarPorProyectoOrdenado(proyectoId))
            {
                var partes = historia.CodigoTicket?.Split('-');

                if (partes is { Length: 2 } &&
                    string.Equals(partes[0], prefijo, StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(partes[1], out var numero) &&
                    numero > maximo)
                {
                    maximo = numero;
                }
            }

            return $"{prefijo}-{maximo + 1}";
        }

        public void ActualizarOrdenWSJF(int proyectoId, List<int> userStoryIdsOrdenados)
        {
            if (userStoryIdsOrdenados == null || !userStoryIdsOrdenados.Any())
                throw new ArgumentException("La lista de identificadores para reordenar no puede estar vacía.");

            // Asignación secuencial de OrdenPrioridad persistida en dbo.UserStories
            for (int i = 0; i < userStoryIdsOrdenados.Count; i++)
            {
                int storyId = userStoryIdsOrdenados[i];
                int nuevoOrden = i + 1;
                _userStoryAD.ActualizarOrdenPrioridad(storyId, nuevoOrden);
            }
        }

        public List<UserStory> ObtenerProductBacklogPriorizado(int proyectoId)
        {
            return _userStoryAD.ListarPorProyectoOrdenado(proyectoId);
        }
    }
}
