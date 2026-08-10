/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Crea una clase de apoyo para devolver la serie del Burndown
 *    Chart desde SprintAD.ObtenerPuntosPendientesBurndown, siguiendo el estilo
 *    de las demas clases de la capa Modelo."
 * 3. Cambios del equipo: La clase guarda solo la fecha y los puntos
 *    pendientes. La linea guia ideal no se persiste ni se calcula en la base
 *    de datos: es una recta que la capa de negocio deduce del primer y ultimo
 *    punto de esta serie, asi que guardarla seria informacion duplicada.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    /// <summary>
    /// Punto de la serie del Burndown Chart: los Story Points que seguian
    /// pendientes al cierre de un dia concreto del Sprint.
    /// La "linea guia ideal" no se guarda: se calcula en la capa de negocio
    /// a partir del primer y ultimo punto de esta serie.
    /// </summary>
    public class PuntoBurndown
    {
        public DateTime Fecha { get; set; }
        public int PuntosPendientes { get; set; }

        public PuntoBurndown() { }

        public PuntoBurndown(DateTime fecha, int puntosPendientes)
        {
            Fecha = fecha;
            PuntosPendientes = puntosPendientes;
        }
    }
}
