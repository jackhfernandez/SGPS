/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo UsuarioLN.cs basandote en la entidad
 *    Usuario y en la clase UsuarioAD del proyecto. Implementa la
 *    autenticacion de usuarios, la generacion de salt y hash seguro de
 *    contrasenas, la verificacion de credenciales y la consulta de roles,
 *    respetando la estructura actual del proyecto."
 * 3. Cambios del equipo: Se implemento PBKDF2 con SHA-256.
 */

using System.Security.Cryptography;
using Datos;
using Modelo;

namespace Logica;

public class UsuarioLN
{
    private const int TamanoSaltBytes = 16;
    private const int TamanoHashBytes = 32;
    private const int Iteraciones = 100_000;
    private static readonly HashAlgorithmName AlgoritmoHash = HashAlgorithmName.SHA256;

    private readonly UsuarioAD _usuarioAD = new();

    public Usuario? Autenticar(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var usuario = _usuarioAD.ObtenerPorEmail(email.Trim());

        if (usuario is null || !usuario.EsActivo)
        {
            return null;
        }

        return VerificarPassword(password, usuario.PasswordHash, usuario.PasswordSalt)
            ? usuario
            : null;
    }

    public List<Rol> ObtenerRoles(int usuarioId) => _usuarioAD.ObtenerRolesPorUsuario(usuarioId);

    public List<Usuario> ListarUsuarios() => _usuarioAD.ListarUsuarios();

    public Dictionary<int, List<Rol>> ObtenerRolesDeTodosLosUsuarios() =>
        _usuarioAD.ObtenerRolesDeTodosLosUsuarios();

    public int CrearUsuario(Usuario nuevo, string password)
    {
        ValidarDatosUsuario(nuevo);
        ValidarPassword(password);

        if (_usuarioAD.ExisteEmail(nuevo.Email, null))
        {
            throw new InvalidOperationException($"El correo '{nuevo.Email}' ya está registrado en el sistema.");
        }

        var (hash, salt) = GenerarCredenciales(password);
        return _usuarioAD.CrearUsuario(nuevo, hash, salt);
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        ValidarDatosUsuario(usuario);

        if (_usuarioAD.ExisteEmail(usuario.Email, usuario.UsuarioId))
        {
            throw new InvalidOperationException($"El correo '{usuario.Email}' ya está en uso por otro usuario.");
        }

        _usuarioAD.ActualizarUsuario(usuario);
    }

    public void CambiarEstadoActivo(int usuarioId, bool activo)
    {
        if (!activo && SesionContextoLN.UsuarioActual?.UsuarioId == usuarioId)
        {
            throw new InvalidOperationException("No puedes desactivar tu propia cuenta.");
        }

        _usuarioAD.CambiarEstado(usuarioId, activo);
    }

    public void RestablecerPassword(int usuarioId, string nuevaPassword)
    {
        ValidarPassword(nuevaPassword);

        var (hash, salt) = GenerarCredenciales(nuevaPassword);
        _usuarioAD.ActualizarPassword(usuarioId, hash, salt);
    }

    public void AsignarRoles(int usuarioId, List<int> rolIds)
    {
        ArgumentNullException.ThrowIfNull(rolIds);

        _usuarioAD.AsignarRoles(usuarioId, rolIds);
    }

    private static void ValidarDatosUsuario(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        if (string.IsNullOrWhiteSpace(usuario.Nombres))
        {
            throw new ArgumentException("Los nombres son obligatorios.");
        }

        if (string.IsNullOrWhiteSpace(usuario.Apellidos))
        {
            throw new ArgumentException("Los apellidos son obligatorios.");
        }

        if (string.IsNullOrWhiteSpace(usuario.Email))
        {
            throw new ArgumentException("El correo electrónico es obligatorio.");
        }

        if (!usuario.Email.Contains('@'))
        {
            throw new ArgumentException("El correo electrónico no tiene un formato válido.");
        }
    }

    private static void ValidarPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
        }
    }

    public static (string Hash, string Salt) GenerarCredenciales(string password)
    {
        var salt = GenerarSalt();
        return (CalcularHash(password, salt), salt);
    }

    public static string GenerarSalt() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(TamanoSaltBytes));

    public static string CalcularHash(string password, string salt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            Convert.FromBase64String(salt),
            Iteraciones,
            AlgoritmoHash,
            TamanoHashBytes);

        return Convert.ToBase64String(hash);
    }

    public static bool VerificarPassword(string password, string hashAlmacenado, string saltAlmacenado)
    {
        if (string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(hashAlmacenado) ||
            string.IsNullOrWhiteSpace(saltAlmacenado))
        {
            return false;
        }

        byte[] hashEsperado;
        byte[] hashCalculado;

        try
        {
            hashEsperado = Convert.FromBase64String(hashAlmacenado);
            hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                password,
                Convert.FromBase64String(saltAlmacenado),
                Iteraciones,
                AlgoritmoHash,
                TamanoHashBytes);
        }
        catch (FormatException)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
    }
}
