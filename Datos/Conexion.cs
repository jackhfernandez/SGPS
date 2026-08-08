using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Text.Json;

namespace Datos
{
    public class Conexion
    {
        public static SqlConnection getConexion()
        {
            var configuracion = ObtenerConfiguracionLocal();
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = configuracion?.Servidor
                    ?? Environment.GetEnvironmentVariable("SQLSERVER_DB_HOST")
                    ?? @"LC201\SQLEXPRESS",
                InitialCatalog = configuracion?.BaseDatos
                    ?? Environment.GetEnvironmentVariable("SQLSERVER_DB_NAME")
                    ?? "SGPS_DB",
                IntegratedSecurity = configuracion?.AutenticacionIntegrada
                    ?? bool.TryParse(Environment.GetEnvironmentVariable("SQLSERVER_DB_INTEGRATED_SECURITY"), out var integrada) && integrada,
                TrustServerCertificate = true
            };

            if (!builder.IntegratedSecurity)
            {
                builder.UserID = Environment.GetEnvironmentVariable("SQLSERVER_DB_USER") ?? "sa";
                builder.Password = Environment.GetEnvironmentVariable("SQLSERVER_DB_PASS") ?? "epici";
            }

            return new SqlConnection(builder.ConnectionString);
        }

        private static ConfiguracionConexion? ObtenerConfiguracionLocal()
        {
            const string archivo = "Conexion.local.json";
            var ruta = Path.Combine(AppContext.BaseDirectory, archivo);

            if (!File.Exists(ruta))
            {
                ruta = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Datos", archivo);
            }

            if (!File.Exists(ruta))
            {
                return null;
            }

            return JsonSerializer.Deserialize<ConfiguracionConexion>(File.ReadAllText(ruta));
        }

        private sealed class ConfiguracionConexion
        {
            public string Servidor { get; set; } = string.Empty;
            public string BaseDatos { get; set; } = string.Empty;
            public bool AutenticacionIntegrada { get; set; }
        }
    }
}
