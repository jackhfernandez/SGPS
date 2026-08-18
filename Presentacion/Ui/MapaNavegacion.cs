/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Convierte la navegacion por menus de Principal.cs en una tabla
 *    declarativa de grupos y sub-items, cada uno asociado a un valor del
 *    enum Modulo y a una fabrica del formulario, de forma que la
 *    visibilidad se derive de PermisoLN."
 * 3. Cambios del equipo: Se agrego la bandera Construido porque 13 de los 20
 *    formularios todavia son plantillas vacias; asi el shell muestra una
 *    pagina "en construccion" en vez de una pantalla gris de 800x450.
 */

using Logica;
using Presentacion.Backlog;
using Presentacion.Cliente;
using Presentacion.Kanban;
using Presentacion.Proyectos;
using Presentacion.QA;
using Presentacion.Reporte;
using Presentacion.Seguridad;
using Presentacion.Sprint;

namespace Presentacion.Ui;

/// <summary>Una pantalla concreta dentro de un grupo de la barra lateral.</summary>
/// <param name="Texto">Nombre visible en la navegacion y en el breadcrumb.</param>
/// <param name="Modulo">Modulo de <see cref="PermisoLN"/> que gobierna su visibilidad.</param>
/// <param name="Crear">Fabrica del formulario a incrustar.</param>
/// <param name="Construido">false mientras el formulario siga siendo una plantilla vacia.</param>
public sealed record ItemNav(
    string Texto,
    Modulo Modulo,
    Func<Form> Crear,
    bool Construido);

/// <summary>Grupo de la barra lateral. Se ve si alguno de sus items se ve.</summary>
public sealed record GrupoNav(
    string Texto,
    string Glifo,
    string Descripcion,
    ItemNav[] Items)
{
    /// <summary>Grupo sin sub-items: navega directo (caso de Resumen).</summary>
    public bool EsDirecto => Items.Length == 0;
}

/// <summary>
/// Tabla declarativa de la navegacion. Para dar de alta un modulo nuevo basta
/// con agregar un <see cref="ItemNav"/> aqui: la barra lateral, la pagina de
/// Resumen y el filtrado por rol se actualizan solos.
/// </summary>
public static class MapaNavegacion
{
    /// <summary>Grupos del cuerpo de la barra lateral, en orden de aparicion.</summary>
    public static readonly GrupoNav[] Grupos =
    [
        new("Proyectos", Tema.Glifos.Carpeta, "Portafolio y equipos de trabajo",
        [
            new("Portafolio", Modulo.ProyectoCreacion, () => new ProyectoCreacion(), true),
            new("Equipo", Modulo.ProyectoMiembros, () => new ProyectoMiembros(), true)
        ]),

        new("Backlog", Tema.Glifos.Lista, "Epics, historias y priorización",
        [
            new("Product Backlog", Modulo.ProductBacklogGestion, () => new ProductBacklogGestion(), true),
            new("Epics", Modulo.EpicGestion, () => new EpicGestion(), true),
            new("Historias", Modulo.UserStoryEdicion, () => new UserStoryEdicion(), true)
        ]),

        new("Sprints", Tema.Glifos.Cronometro, "Planificación y cierre de iteraciones",
        [
            new("Planificación", Modulo.SprintPlanificacion, () => new SprintPlanificacion(), true),
            new("Ejecución", Modulo.SprintEjecucion, () => new SprintEjecucion(), true)
        ]),

        new("Kanban", Tema.Glifos.Tablero, "Tablero del sprint activo",
        [
            new("Tablero", Modulo.TableroKanban, () => new TableroKanban(), true),
            new("Tareas", Modulo.TareaEdicion, () => new TareaEdicion(), true)
        ]),

        new("QA", Tema.Glifos.Alerta, "Defectos y re-prueba",
        [
            new("Bugs", Modulo.BugGestion, () => new BugGestion(), false),
            new("Reportar bug", Modulo.BugReporte, () => new BugReporte(), true)
        ]),

        new("Métricas", Tema.Glifos.Grafico, "Burndown y velocidad del equipo",
        [
            new("Burndown", Modulo.BurndownChartVista, () => new BurndownChartVista(), false),
            new("Velocity", Modulo.MetricasAgilesVista, () => new MetricasAgilesVista(), false)
        ]),

        new("Cliente", Tema.Glifos.Contacto, "Portal de avance y comentarios",
        [
            new("Portal", Modulo.ClientePortal, () => new ClientePortal(), false)
        ])
    ];

    /// <summary>Grupo anclado al pie de la barra lateral.</summary>
    public static readonly GrupoNav Administracion =
        new("Administración", Tema.Glifos.Ajustes, "Usuarios, roles y accesos",
        [
            new("Usuarios", Modulo.UsuarioGestion, () => new UsuarioGestion(), true),
            new("Roles", Modulo.RolGestion, () => new RolGestion(), true)
        ]);

    /// <summary>Todos los grupos, incluido Administración. Lo usa la página de Resumen.</summary>
    public static IEnumerable<GrupoNav> TodosLosGrupos => Grupos.Append(Administracion);

    /// <summary>Un item se ve si el usuario tiene al menos lectura sobre su módulo.</summary>
    public static bool EsVisible(ItemNav item) => PermisoLN.PuedeVer(item.Modulo);

    /// <summary>Un grupo se ve si alguno de sus items se ve.</summary>
    public static bool EsVisible(GrupoNav grupo) => grupo.Items.Any(EsVisible);

    /// <summary>Items visibles de un grupo, en orden.</summary>
    public static IEnumerable<ItemNav> ItemsVisibles(GrupoNav grupo) => grupo.Items.Where(EsVisible);

    /// <summary>
    /// Mayor nivel de acceso que el usuario tiene sobre cualquier módulo del
    /// grupo. Es lo que muestra el chip de la tarjeta en la página de Resumen.
    /// </summary>
    public static NivelAcceso NivelDelGrupo(GrupoNav grupo)
    {
        var maximo = NivelAcceso.SinAcceso;

        foreach (var item in grupo.Items)
        {
            var nivel = PermisoLN.NivelAccesoUsuario(item.Modulo);

            if (nivel > maximo)
            {
                maximo = nivel;
            }
        }

        return maximo;
    }

    /// <summary>true si al menos un módulo visible del grupo ya está implementado.</summary>
    public static bool GrupoConstruido(GrupoNav grupo) => ItemsVisibles(grupo).Any(i => i.Construido);
}
