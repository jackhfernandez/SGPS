using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public int UserStoryId { get; set; }
        public int UsuarioId { get; set; }

        // Contenido del comentario. Soporta menciones @usuario para notificaciones in-app.
        public string TextoComentario { get; set; } = string.Empty;
        public DateTime FechaComentario { get; set; }

        // Nombre completo del autor (Nombres + Apellidos de dbo.Usuarios).
        // Solo para mostrar en la lista de comentarios; no se persiste.
        public string? AutorNombre { get; set; }

        public Comentario()
        {
            FechaComentario = DateTime.Now;
        }
    }
}
