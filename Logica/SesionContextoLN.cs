using SGPS.Entidades;

namespace Logica;

public static class SesionContextoLN
{
    private static List<Rol> _rolesActuales = new();

    public static Usuario UsuarioActual { get; private set; } = null!;

    public static void IniciarSesion(Usuario usuario, IEnumerable<Rol> roles)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(roles);

        UsuarioActual = usuario;
        _rolesActuales = roles.ToList();
    }

    public static void CerrarSesion()
    {
        UsuarioActual = null!;
        _rolesActuales.Clear();
    }

    public static bool TieneRol(params string[] roles)
    {
        if (UsuarioActual is null || roles is null || roles.Length == 0)
        {
            return false;
        }

        return _rolesActuales.Any(rolActual => roles.Any(rolSolicitado =>
            !string.IsNullOrWhiteSpace(rolSolicitado) &&
            string.Equals(rolActual.NombreRol, rolSolicitado, StringComparison.OrdinalIgnoreCase)));
    }
}
