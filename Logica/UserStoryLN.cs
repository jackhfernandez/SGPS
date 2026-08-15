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
            historia.FechaCreacion = DateTime.Now;
            historia.FechaUltimaModificacion = DateTime.Now;

            _userStoryAD.Insertar(historia);
        }

        public void Modificar(UserStory historia)
        {
            ValidarEstructuraHistoria(historia);
            historia.FechaUltimaModificacion = DateTime.Now;

            _userStoryAD.Actualizar(historia);
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
