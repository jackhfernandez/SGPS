using Datos;
using Modelo;

namespace Logica;

public class RolLN
{
    private readonly RolAD _rolAD = new();

    public List<Rol> ListarRoles() => _rolAD.ListarRoles();

    public Dictionary<int, int> ObtenerConteoUsuariosPorRol() =>
        _rolAD.ObtenerConteoUsuariosPorRol();

    public int CrearRol(Rol rol)
    {
        ArgumentNullException.ThrowIfNull(rol);

        var nombre = rol.NombreRol?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del rol es obligatorio.");
        }

        if (_rolAD.ExisteNombre(nombre, null))
        {
            throw new InvalidOperationException($"El rol '{nombre}' ya existe en el sistema.");
        }

        rol.NombreRol = nombre;

        return _rolAD.CrearRol(rol);
    }

    public void ActualizarRol(Rol rol)
    {
        ArgumentNullException.ThrowIfNull(rol);

        var nombre = rol.NombreRol?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del rol es obligatorio.");
        }

        if (_rolAD.ExisteNombre(nombre, rol.RolId))
        {
            throw new InvalidOperationException($"El rol '{nombre}' ya existe en el sistema.");
        }

        rol.NombreRol = nombre;

        _rolAD.ActualizarRol(rol);
    }

    public void EliminarRol(int rolId)
    {
        if (_rolAD.TieneUsuariosAsignados(rolId))
        {
            throw new InvalidOperationException("No se puede eliminar un rol que está asignado a uno o más usuarios.");
        }

        _rolAD.EliminarRol(rolId);
    }
}