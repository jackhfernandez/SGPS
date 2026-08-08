using System.Security.Cryptography;
using Datos;
using SGPS.Entidades;

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
