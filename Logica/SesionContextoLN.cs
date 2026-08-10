/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo SesionContextoLN.cs basandote en las
 *    entidades Usuario y Rol del proyecto. Implementa un contexto de sesion
 *    que permita iniciar y cerrar sesion, almacenar los roles actuales y
 *    comprobar si el usuario tiene un rol solicitado, respetando la
 *    estructura actual del proyecto."
 * 3. Cambios del equipo: Se utiliza IEnumerable<Rol> como parametro en
 *    IniciarSesion en lugar de depender de una coleccion o un enumerado y esto es
 *    para recibir distintas fuentes de roles y con ToList() traerlo facil
 */

using Modelo;

namespace Logica;

public static class SesionContextoLN
{
    private static List<Rol> _rolesActuales = new();

    public static Usuario UsuarioActual { get; private set; } = null!;

    public static IReadOnlyList<Rol> RolesActuales => _rolesActuales;

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
