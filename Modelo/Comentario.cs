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
        public string TextoComentario { get; set; }
        public DateTime FechaComentario { get; set; }

        public Comentario()
        {
            FechaComentario = DateTime.Now;
        }
    }
}
