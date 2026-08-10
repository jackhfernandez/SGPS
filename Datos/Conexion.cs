using System.Configuration;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Datos;

public static class Conexion
{
    private const string NombreCadena = "SGPS_Connection";
    private const string ArchivoConfiguracionLocal = "Conexion.local.json";

    public static SqlConnection ObtenerConexion()
    {
        var conexion = new SqlConnection(ObtenerCadenaConexion());

        try
        {
            conexion.Open();
        }
        catch
        {
            conexion.Dispose();
            throw;
        }

        return conexion;
    }

    public static string ObtenerCadenaConexion()
    {
        var builder = new SqlConnectionStringBuilder(ObtenerCadenaConfigurada());

        AplicarVariablesDeEntorno(builder);
        AplicarConfiguracionLocal(builder);

        return builder.ConnectionString;
    }

    private static string ObtenerCadenaConfigurada()
    {
        var cadena = ConfigurationManager.ConnectionStrings[NombreCadena]?.ConnectionString;

        if (string.IsNullOrWhiteSpace(cadena))
        {
            throw new ConfigurationErrorsException(
                $"No se encontro la cadena de conexion '{NombreCadena}' en el App.config de la aplicacion.");
        }

        return cadena;
    }

    private static void AplicarVariablesDeEntorno(SqlConnectionStringBuilder builder)
    {
        AsignarSiTieneValor(Environment.GetEnvironmentVariable("SQLSERVER_DB_HOST"), valor => builder.DataSource = valor);
        AsignarSiTieneValor(Environment.GetEnvironmentVariable("SQLSERVER_DB_NAME"), valor => builder.InitialCatalog = valor);
        AsignarSiTieneValor(Environment.GetEnvironmentVariable("SQLSERVER_DB_USER"), valor => builder.UserID = valor);
        AsignarSiTieneValor(Environment.GetEnvironmentVariable("SQLSERVER_DB_PASS"), valor => builder.Password = valor);

        if (bool.TryParse(Environment.GetEnvironmentVariable("SQLSERVER_DB_INTEGRATED_SECURITY"), out var integrada))
        {
            builder.IntegratedSecurity = integrada;
        }
    }

    private static void AplicarConfiguracionLocal(SqlConnectionStringBuilder builder)
    {
        var configuracion = LeerConfiguracionLocal();

        if (configuracion is null)
        {
            return;
        }

        AsignarSiTieneValor(configuracion.Servidor, valor => builder.DataSource = valor);
        AsignarSiTieneValor(configuracion.BaseDatos, valor => builder.InitialCatalog = valor);

        if (configuracion.AutenticacionIntegrada.HasValue)
        {
            builder.IntegratedSecurity = configuracion.AutenticacionIntegrada.Value;
        }
    }

    private static ConfiguracionConexion? LeerConfiguracionLocal()
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, ArchivoConfiguracionLocal);

        if (!File.Exists(ruta))
        {
            ruta = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Datos", ArchivoConfiguracionLocal);
        }

        if (!File.Exists(ruta))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ConfiguracionConexion>(File.ReadAllText(ruta));
    }

    private static void AsignarSiTieneValor(string? valor, Action<string> asignar)
    {
        if (!string.IsNullOrWhiteSpace(valor))
        {
            asignar(valor);
        }
    }

    private sealed class ConfiguracionConexion
    {
        public string? Servidor { get; set; }
        public string? BaseDatos { get; set; }
        public bool? AutenticacionIntegrada { get; set; }
    }
}
