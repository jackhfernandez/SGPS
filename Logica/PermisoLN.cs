/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Genera el archivo PermisoLN.cs basandote en la matriz de
 *    permisos de permisos.md y en la entidad Rol del proyecto. Implementa
 *    la matriz de acceso por rol y modulo, la normalizacion de nombres de
 *    rol (alias configurables en permisos.roles.json) y la validacion de
 *    acceso en la capa de logica, respetando la estructura actual del
 *    proyecto."
 * 3. Cambios del equipo: La matriz se define en codigo como fuente de
 *    verdad a prueba de manipulacion; permisos.roles.json solo mapea
 *    nombres de rol personalizados de la BD a los roles del sistema.
 *    Si el usuario tiene varios roles, se toma el mayor nivel de acceso.
 */

using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Logica;

public enum NivelAcceso
{
    SinAcceso = 0,
    Lectura = 1,
    Edicion = 2,
    Total = 3
}

public enum RolSistema
{
    Administrador,
    ProductOwner,
    ScrumMaster,
    Developer,
    QA,
    Cliente
}

public enum Modulo
{
    UsuarioGestion,
    RolGestion,
    ProyectoCreacion,
    ProyectoMiembros,
    ProductBacklogGestion,
    EpicGestion,
    UserStoryEdicion,
    SprintPlanificacion,
    SprintEjecucion,
    TableroKanban,
    TareaEdicion,
    UcTarjetaKanban,
    BugReporte,
    BugGestion,
    ClientePortal,
    BurndownChartVista,
    MetricasAgilesVista
}

public sealed class PermisoDenegadoException : UnauthorizedAccessException
{
    public PermisoDenegadoException(string message) : base(message) { }
}

public static class PermisoLN
{
    private const string NombreArchivoAlias = "permisos.roles.json";
    private const string ClaveMapeoRoles = "MapeoRoles";

    private static readonly IReadOnlyDictionary<Modulo, IReadOnlyDictionary<RolSistema, NivelAcceso>> Matriz =
        CrearMatriz();

    private static readonly IReadOnlyDictionary<string, RolSistema> AliasRoles = CargarAliasRoles();

    private static IReadOnlyDictionary<Modulo, IReadOnlyDictionary<RolSistema, NivelAcceso>> CrearMatriz()
    {
        var matriz = new Dictionary<Modulo, IReadOnlyDictionary<RolSistema, NivelAcceso>>();

        Fila(Modulo.UsuarioGestion, (RolSistema.Administrador, NivelAcceso.Total));
        Fila(Modulo.RolGestion, (RolSistema.Administrador, NivelAcceso.Total));

        var proyectos = new[]
        {
            (RolSistema.Administrador, NivelAcceso.Total),
            (RolSistema.ProductOwner, NivelAcceso.Total),
            (RolSistema.ScrumMaster, NivelAcceso.Lectura),
            (RolSistema.Developer, NivelAcceso.Lectura),
            (RolSistema.QA, NivelAcceso.Lectura)
        };
        Fila(Modulo.ProyectoCreacion, proyectos);
        Fila(Modulo.ProyectoMiembros, proyectos);
        Fila(Modulo.ProductBacklogGestion, proyectos);
        Fila(Modulo.EpicGestion, proyectos);
        Fila(Modulo.UserStoryEdicion, proyectos);

        var sprints = new[]
        {
            (RolSistema.Administrador, NivelAcceso.Total),
            (RolSistema.ProductOwner, NivelAcceso.Edicion),
            (RolSistema.ScrumMaster, NivelAcceso.Total),
            (RolSistema.Developer, NivelAcceso.Lectura),
            (RolSistema.QA, NivelAcceso.Lectura)
        };
        Fila(Modulo.SprintPlanificacion, sprints);
        Fila(Modulo.SprintEjecucion, sprints);

        var kanban = new[]
        {
            (RolSistema.Administrador, NivelAcceso.Total),
            (RolSistema.ProductOwner, NivelAcceso.Lectura),
            (RolSistema.ScrumMaster, NivelAcceso.Lectura),
            (RolSistema.Developer, NivelAcceso.Total),
            (RolSistema.QA, NivelAcceso.Lectura)
        };
        Fila(Modulo.TableroKanban, kanban);
        Fila(Modulo.TareaEdicion, kanban);
        Fila(Modulo.UcTarjetaKanban, kanban);

        var bugs = new[]
        {
            (RolSistema.Administrador, NivelAcceso.Total),
            (RolSistema.ProductOwner, NivelAcceso.Lectura),
            (RolSistema.ScrumMaster, NivelAcceso.Lectura),
            (RolSistema.Developer, NivelAcceso.Edicion),
            (RolSistema.QA, NivelAcceso.Total)
        };
        Fila(Modulo.BugReporte, bugs);
        Fila(Modulo.BugGestion, bugs);

        Fila(Modulo.ClientePortal,
            (RolSistema.Administrador, NivelAcceso.Lectura),
            (RolSistema.ProductOwner, NivelAcceso.Lectura),
            (RolSistema.ScrumMaster, NivelAcceso.Lectura),
            (RolSistema.Cliente, NivelAcceso.Lectura));

        var reportes = new[]
        {
            (RolSistema.Administrador, NivelAcceso.Total),
            (RolSistema.ProductOwner, NivelAcceso.Total),
            (RolSistema.ScrumMaster, NivelAcceso.Total),
            (RolSistema.Developer, NivelAcceso.Lectura),
            (RolSistema.QA, NivelAcceso.Lectura),
            (RolSistema.Cliente, NivelAcceso.Lectura)
        };
        Fila(Modulo.BurndownChartVista, reportes);
        Fila(Modulo.MetricasAgilesVista, reportes);

        return matriz;

        void Fila(Modulo modulo, params (RolSistema Rol, NivelAcceso Nivel)[] acceso)
        {
            var fila = new Dictionary<RolSistema, NivelAcceso>();

            foreach (var (rol, nivel) in acceso)
            {
                fila[rol] = nivel;
            }

            matriz[modulo] = fila;
        }
    }

    private static IReadOnlyDictionary<string, RolSistema> CargarAliasRoles()
    {
        var alias = new Dictionary<string, RolSistema>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var ruta = Path.Combine(AppContext.BaseDirectory, NombreArchivoAlias);

            if (!File.Exists(ruta))
            {
                return alias;
            }

            using var documento = JsonDocument.Parse(File.ReadAllText(ruta));

            if (!documento.RootElement.TryGetProperty(ClaveMapeoRoles, out var mapeo))
            {
                return alias;
            }

            foreach (var propiedad in mapeo.EnumerateObject())
            {
                var destino = propiedad.Value.GetString();

                if (string.IsNullOrWhiteSpace(destino) ||
                    !Enum.TryParse(destino, ignoreCase: true, out RolSistema rolSistema))
                {
                    continue;
                }

                alias[NormalizarTexto(propiedad.Name)] = rolSistema;
            }
        }
        catch (JsonException)
        {
            // Si el archivo esta corrupto se ignora y se usan solo los sinonimos estandar.
        }
        catch (IOException)
        {
        }

        return alias;
    }

    public static RolSistema? NormalizarRol(string? nombreRol)
    {
        if (string.IsNullOrWhiteSpace(nombreRol))
        {
            return null;
        }

        var normalizado = NormalizarTexto(nombreRol);

        if (AliasRoles.TryGetValue(normalizado, out var porAlias))
        {
            return porAlias;
        }

        var sinonimo = normalizado switch
        {
            "administrador" or "admin" => RolSistema.Administrador,
            "productowner" or "po" => RolSistema.ProductOwner,
            "scrummaster" or "sm" => RolSistema.ScrumMaster,
            "developer" or "desarrollador" or "dev" => RolSistema.Developer,
            "qa" or "qatester" => RolSistema.QA,
            "cliente" => RolSistema.Cliente,
            _ => (RolSistema?)null
        };

        return sinonimo ?? NormalizarPorTokens(nombreRol);
    }

    private static RolSistema? NormalizarPorTokens(string nombreRol)
    {
        var tokens = ObtenerTokens(nombreRol).ToArray();

        if (tokens.Contains("admin") || tokens.Contains("administrador"))
        {
            return RolSistema.Administrador;
        }

        if (tokens.Contains("sm"))
        {
            return RolSistema.ScrumMaster;
        }

        if (tokens.Contains("po"))
        {
            return RolSistema.ProductOwner;
        }

        if (tokens.Contains("qa") || tokens.Contains("tester") || tokens.Contains("test"))
        {
            return RolSistema.QA;
        }

        if (tokens.Contains("cliente") || tokens.Contains("client"))
        {
            return RolSistema.Cliente;
        }

        if (tokens.Contains("scrum") && tokens.Contains("master"))
        {
            return RolSistema.ScrumMaster;
        }

        if (tokens.Contains("product") && tokens.Contains("owner"))
        {
            return RolSistema.ProductOwner;
        }

        if (tokens.Contains("dev") || tokens.Contains("developer") || tokens.Contains("desarrollador"))
        {
            return RolSistema.Developer;
        }

        return null;
    }

    private static IEnumerable<string> ObtenerTokens(string texto)
    {
        var token = new StringBuilder();

        foreach (var caracter in texto)
        {
            if (char.IsLetterOrDigit(caracter))
            {
                token.Append(char.ToLowerInvariant(caracter));
            }
            else if (token.Length > 0)
            {
                yield return token.ToString();
                token.Clear();
            }
        }

        if (token.Length > 0)
        {
            yield return token.ToString();
        }
    }

    public static string Diagnostico()
    {
        var sb = new StringBuilder();

        foreach (var rol in SesionContextoLN.RolesActuales)
        {
            var rolSistema = NormalizarRol(rol.NombreRol);
            sb.AppendLine($"  Rol BD: '{rol.NombreRol}' -> {rolSistema?.ToString() ?? "NO RECONOCIDO"}");
        }

        sb.AppendLine($"  UsuarioGestion: {NivelAccesoUsuario(Modulo.UsuarioGestion)}");
        sb.AppendLine($"  TableroKanban:  {NivelAccesoUsuario(Modulo.TableroKanban)}");
        sb.AppendLine($"  ClientePortal:  {NivelAccesoUsuario(Modulo.ClientePortal)}");

        return sb.ToString();
    }

    public static NivelAcceso NivelAccesoParaRol(Modulo modulo, RolSistema rol)
    {
        return Matriz.TryGetValue(modulo, out var fila) &&
               fila.TryGetValue(rol, out var nivel)
            ? nivel
            : NivelAcceso.SinAcceso;
    }

    public static NivelAcceso NivelAccesoUsuario(Modulo modulo)
    {
        var maximo = NivelAcceso.SinAcceso;

        foreach (var rol in SesionContextoLN.RolesActuales)
        {
            var rolSistema = NormalizarRol(rol.NombreRol);

            if (rolSistema is null)
            {
                continue;
            }

            var nivel = NivelAccesoParaRol(modulo, rolSistema.Value);

            if (nivel > maximo)
            {
                maximo = nivel;
            }
        }

        return maximo;
    }

    public static bool TieneAcceso(Modulo modulo, NivelAcceso nivelMinimo) =>
        NivelAccesoUsuario(modulo) >= nivelMinimo;

    public static bool PuedeVer(Modulo modulo) =>
        NivelAccesoUsuario(modulo) > NivelAcceso.SinAcceso;

    public static void ValidarAcceso(Modulo modulo, NivelAcceso nivelMinimo)
    {
        if (!TieneAcceso(modulo, nivelMinimo))
        {
            throw new PermisoDenegadoException(
                $"No tiene permisos para acceder a '{modulo}'. Se requiere nivel {nivelMinimo}.");
        }
    }

    public static void ValidarLectura(Modulo modulo) =>
        ValidarAcceso(modulo, NivelAcceso.Lectura);

    public static void ValidarEdicion(Modulo modulo) =>
        ValidarAcceso(modulo, NivelAcceso.Edicion);

    public static void ValidarTotal(Modulo modulo) =>
        ValidarAcceso(modulo, NivelAcceso.Total);

    private static string NormalizarTexto(string texto)
    {
        var sinAcentos = RemoverDiacriticos(texto).ToLowerInvariant();
        var resultado = new StringBuilder(sinAcentos.Length);

        foreach (var caracter in sinAcentos)
        {
            if (char.IsLetterOrDigit(caracter))
            {
                resultado.Append(caracter);
            }
        }

        return resultado.ToString();
    }

    private static string RemoverDiacriticos(string texto)
    {
        var normalizado = texto.Normalize(NormalizationForm.FormD);
        var resultado = new StringBuilder(normalizado.Length);

        foreach (var caracter in normalizado)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(caracter) != UnicodeCategory.NonSpacingMark)
            {
                resultado.Append(caracter);
            }
        }

        return resultado.ToString().Normalize(NormalizationForm.FormC);
    }
}
