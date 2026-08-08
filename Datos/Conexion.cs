using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class Conexion
    {
        public static SqlConnection getConexion()
        {
            var builder = new SqlConnectionStringBuilder
            {
                // Crear variables de entorno o actualizar con variables locales
                DataSource = Environment.GetEnvironmentVariable("SQLSERVER_DB_HOST") ?? @"LC201\SQLEXPRESS",
                InitialCatalog = "SGPS_DB",
                UserID = Environment.GetEnvironmentVariable("SQLSERVER_DB_USER") ?? "sa",
                Password = Environment.GetEnvironmentVariable("SQLSERVER_DB_PASS") ?? "epici",
                TrustServerCertificate = true
            };

            string cadena = builder.ConnectionString;

            return new SqlConnection(cadena);
        }
    }
}
