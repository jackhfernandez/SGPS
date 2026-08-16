/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo EpicLN.cs basandote en la entidad Epic y en
 *    EpicAD del proyecto. Implementa las validaciones de negocio y la
 *    delegacion al acceso a datos, siguiendo el estilo de RolLN y ProyectoLN,
 *    respetando la estructura actual del proyecto."
 * 3. Cambios del equipo: Se bloquea la eliminacion de un epic que todavia
 *    tenga historias asociadas, porque la FK de dbo.UserStories no tiene
 *    ON DELETE CASCADE y el error crudo de SQL Server no seria comprensible.
 */

using Datos;
using Modelo;
using System.Text.RegularExpressions;

namespace Logica;

public class EpicLN
{
    private readonly EpicAD _epicAD = new();

    public List<Epic> ListarPorProyecto(int proyectoId) => _epicAD.ListarPorProyecto(proyectoId);

    public Epic? ObtenerPorId(int epicId) => _epicAD.ObtenerPorId(epicId);

    public int CrearEpic(Epic epic)
    {
        ValidarDatosEpic(epic);

        if (_epicAD.ExisteTitulo(epic.ProyectoId, epic.Titulo))
        {
            throw new InvalidOperationException(
                $"Ya existe un epic llamado '{epic.Titulo}' en este proyecto.");
        }

        epic.FechaCreacion = DateTime.Now;

        return _epicAD.InsertarEpic(epic);
    }

    public void ActualizarEpic(Epic epic)
    {
        ValidarDatosEpic(epic);

        if (epic.EpicId <= 0)
        {
            throw new ArgumentException("El epic a modificar no tiene un identificador valido.");
        }

        if (_epicAD.ExisteTitulo(epic.ProyectoId, epic.Titulo, epic.EpicId))
        {
            throw new InvalidOperationException(
                $"Ya existe otro epic llamado '{epic.Titulo}' en este proyecto.");
        }

        _epicAD.ActualizarEpic(epic);
    }

    public void EliminarEpic(int epicId)
    {
        if (epicId <= 0)
        {
            throw new ArgumentException("El identificador del epic no es valido.");
        }

        var historias = _epicAD.ContarHistorias(epicId);

        if (historias > 0)
        {
            throw new InvalidOperationException(
                $"No se puede eliminar el epic porque tiene {historias} historia(s) de usuario asociada(s). " +
                "Reasigna o elimina esas historias primero.");
        }

        _epicAD.EliminarEpic(epicId);
    }

    /// <summary>
    /// Valida las reglas de datos maestros del epic (Proyecto, Titulo y Color).
    /// </summary>
    public void ValidarDatosEpic(Epic epic)
    {
        if (epic == null)
        {
            throw new ArgumentNullException(nameof(epic), "Los datos del epic no pueden ser nulos.");
        }

        if (epic.ProyectoId <= 0)
        {
            throw new ArgumentException("Debe seleccionar el proyecto al que pertenece el epic.");
        }

        if (string.IsNullOrWhiteSpace(epic.Titulo) || epic.Titulo.Trim().Length > 200)
        {
            throw new ArgumentException("El titulo del epic es obligatorio y no debe exceder los 200 caracteres.");
        }

        epic.Titulo = epic.Titulo.Trim();

        if (string.IsNullOrWhiteSpace(epic.ColorHex))
        {
            epic.ColorHex = "#3182CE";
        }

        epic.ColorHex = epic.ColorHex.Trim().ToUpperInvariant();

        // dbo.Epics.ColorHex es VARCHAR(7): siempre en formato #RRGGBB.
        var regexColor = new Regex("^#[0-9A-F]{6}$");

        if (!regexColor.IsMatch(epic.ColorHex))
        {
            throw new ArgumentException("El color debe estar en formato hexadecimal #RRGGBB (Ej. #3182CE).");
        }
    }
}
