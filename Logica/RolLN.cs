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

    public void CambiarEstado(int rolId, bool esActivo)
    {
        if (rolId <= 0)
        {
            throw new ArgumentException("El identificador del rol no es válido.");
        }

        _rolAD.CambiarEstado(rolId, esActivo);
    }

    public bool TieneUsuariosAsignados(int rolId) => _rolAD.TieneUsuariosAsignados(rolId);
}
