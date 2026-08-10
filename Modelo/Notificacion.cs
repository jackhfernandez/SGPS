using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Notificacion
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public int? UserStoryId { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime FechaNotificacion { get; set; }

        public Notificacion()
        {
            Leido = false;
            FechaNotificacion = DateTime.Now;
        }

        public Notificacion(int usuarioId, string titulo, string mensaje, int? userStoryId = null)
            : this()
        {
            UsuarioId = usuarioId;
            Titulo = titulo;
            Mensaje = mensaje;
            UserStoryId = userStoryId;
        }
    }
}
