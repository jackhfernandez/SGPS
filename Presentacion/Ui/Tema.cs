/*
 * 1. Reporte de uso de IA
 * 2. Prompt: "Extrae la paleta y las fuentes que Login.Designer.cs tiene
 *    repartidas en literales Color.FromArgb y centralizalas en una clase
 *    estatica Tema, agregando helpers para dar estilo a DataGridView y
 *    botones, respetando la estructura actual del proyecto."
 * 3. Cambios del equipo: Se agrego la deteccion de la fuente de iconos
 *    Segoe MDL2 Assets con degradado elegante, para que la interfaz siga
 *    siendo legible en equipos donde esa fuente no este instalada.
 */

using Logica;

namespace Presentacion.Ui;

/// <summary>
/// Fuente de verdad de la identidad visual del SGPS. Los colores provienen
/// de la pantalla de Login, que fue el primer modulo con diseno definitivo.
/// </summary>
public static class Tema
{
    // ---------------------------------------------------------------- Colores

    /// <summary>Verde profundo del panel hero del Login. Fondo de la barra lateral.</summary>
    public static readonly Color VerdeProfundo = Color.FromArgb(15, 59, 51);

    /// <summary>Verde un paso mas claro, para hover y seleccion dentro de la barra lateral.</summary>
    public static readonly Color VerdeHover = Color.FromArgb(23, 74, 65);

    /// <summary>Teal de accion: botones primarios e indicador de item activo.</summary>
    public static readonly Color Teal = Color.FromArgb(12, 110, 99);

    public static readonly Color TealHover = Color.FromArgb(14, 124, 113);
    public static readonly Color TealPresionado = Color.FromArgb(10, 92, 83);

    /// <summary>Teal desaturado, para reglas y textos secundarios sobre verde profundo.</summary>
    public static readonly Color TealSuave = Color.FromArgb(73, 143, 132);

    /// <summary>Crema del panel de acceso del Login. Fondo del area de contenido.</summary>
    public static readonly Color Crema = Color.FromArgb(245, 241, 232);

    public static readonly Color Blanco = Color.White;

    /// <summary>Borde de tarjetas y separadores sobre crema.</summary>
    public static readonly Color Borde = Color.FromArgb(218, 212, 196);

    public static readonly Color TextoOscuro = Color.FromArgb(18, 32, 28);
    public static readonly Color TextoCuerpo = Color.FromArgb(44, 51, 47);
    public static readonly Color TextoTenue = Color.FromArgb(138, 133, 122);

    public static readonly Color Dorado = Color.FromArgb(239, 198, 92);
    public static readonly Color Coral = Color.FromArgb(226, 114, 91);
    public static readonly Color Oliva = Color.FromArgb(124, 138, 62);
    public static readonly Color SombraSuave = Color.FromArgb(164, 195, 187);

    /// <summary>Crema levemente oscurecida, para hover de superficies claras.</summary>
    public static readonly Color CremaHover = Color.FromArgb(236, 231, 219);

    /// <summary>Seleccion de fila en grids.</summary>
    public static readonly Color SeleccionSuave = Color.FromArgb(226, 236, 233);

    // ---------------------------------------------------------------- Fuentes

    public static readonly Font TituloHero = new("Georgia", 30F, FontStyle.Bold);
    public static readonly Font TituloSeccion = new("Georgia", 20F, FontStyle.Bold);
    public static readonly Font TituloTarjeta = new("Georgia", 13F, FontStyle.Bold);
    public static readonly Font Etiqueta = new("Consolas", 8.5F);
    public static readonly Font Cuerpo = new("Segoe UI", 9.75F);
    public static readonly Font CuerpoSemi = new("Segoe UI Semibold", 9.5F);
    public static readonly Font Boton = new("Segoe UI", 10.5F, FontStyle.Bold);
    public static readonly Font Numero = new("Segoe UI Light", 28F);
    public static readonly Font Marca = new("Georgia", 15F, FontStyle.Bold);

    /// <summary>
    /// Fuente de iconos. Si "Segoe MDL2 Assets" no esta instalada se cae a
    /// Segoe UI y <see cref="HayIconos"/> queda en false para que los
    /// controles omitan el glifo en lugar de dibujar cuadros vacios.
    /// </summary>
    public static readonly Font Icono;

    public static readonly Font IconoGrande;

    /// <summary>true si la fuente de iconos esta disponible en el equipo.</summary>
    public static readonly bool HayIconos;

    static Tema()
    {
        const string familiaIconos = "Segoe MDL2 Assets";

        HayIconos = FontFamily.Families.Any(f =>
            string.Equals(f.Name, familiaIconos, StringComparison.OrdinalIgnoreCase));

        Icono = HayIconos ? new Font(familiaIconos, 14F) : new Font("Segoe UI", 14F);
        IconoGrande = HayIconos ? new Font(familiaIconos, 22F) : new Font("Segoe UI", 22F);
    }

    /// <summary>
    /// Codigos de Segoe MDL2 Assets usados por la navegacion. Se consultan
    /// siempre a traves de <see cref="HayIconos"/>; si la fuente falta, los
    /// controles no dibujan el glifo.
    /// </summary>
    public static class Glifos
    {
        public const string Inicio = "";          // Home
        public const string Carpeta = "";         // Folder
        public const string Lista = "";           // BulletedList
        public const string Cronometro = "";      // Stopwatch
        public const string Tablero = "";         // ViewAll
        public const string Alerta = "";          // Warning
        public const string Grafico = "";         // Diagnostic
        public const string Contacto = "";        // Contact
        public const string Ajustes = "";         // Settings
        public const string Personas = "";        // People
        public const string Buscar = "";          // Search
        public const string Mas = "";             // More
        public const string ChevronAbajo = "";    // ChevronDown
        public const string ChevronDerecha = "";  // ChevronRight
        public const string Construccion = "";    // Repair
    }

    // ---------------------------------------------------------------- Helpers

    /// <summary>
    /// Color de rol para fondos claros (crema o blanco), coherente con los
    /// botones de acceso rapido del Login.
    /// </summary>
    public static Color ColorDeRol(RolSistema rol) => rol switch
    {
        RolSistema.Administrador => TextoCuerpo,
        RolSistema.ProductOwner => Color.FromArgb(176, 130, 40),
        RolSistema.ScrumMaster => Teal,
        RolSistema.Developer => Coral,
        RolSistema.QA => Oliva,
        RolSistema.Cliente => TextoTenue,
        _ => TextoTenue
    };

    /// <summary>
    /// Variante clara del color de rol, para usarse sobre el verde profundo de
    /// la barra lateral. Los tonos de <see cref="ColorDeRol"/> son oscuros por
    /// diseno y ahi resultarian ilegibles.
    /// </summary>
    public static Color ColorDeRolSobreOscuro(RolSistema rol) => rol switch
    {
        RolSistema.Administrador => Dorado,
        RolSistema.ProductOwner => Color.FromArgb(217, 142, 95),
        RolSistema.ScrumMaster => Color.FromArgb(109, 186, 172),
        RolSistema.Developer => Coral,
        RolSistema.QA => Color.FromArgb(163, 179, 86),
        RolSistema.Cliente => Color.FromArgb(185, 179, 165),
        _ => TealSuave
    };

    /// <summary>
    /// Color de texto legible sobre <paramref name="fondo"/>, elegido por
    /// luminancia relativa.
    /// </summary>
    public static Color ContrasteSobre(Color fondo)
    {
        var luminancia = (0.299 * fondo.R + 0.587 * fondo.G + 0.114 * fondo.B) / 255.0;

        return luminancia > 0.55 ? VerdeProfundo : Blanco;
    }

    /// <summary>Etiqueta corta del nivel de acceso, para los chips de las tarjetas.</summary>
    public static string TextoNivel(NivelAcceso nivel) => nivel switch
    {
        NivelAcceso.Total => "Total",
        NivelAcceso.Edicion => "Edición",
        NivelAcceso.Lectura => "Lectura",
        _ => "Sin acceso"
    };

    /// <summary>Iniciales de un nombre completo, para los avatares circulares.</summary>
    public static string Iniciales(string? nombreCompleto)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
        {
            return "?";
        }

        var partes = nombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return partes.Length == 1
            ? partes[0][..1].ToUpperInvariant()
            : (partes[0][..1] + partes[1][..1]).ToUpperInvariant();
    }

    public static void EstiloBotonPrimario(Button boton)
    {
        boton.BackColor = Teal;
        boton.ForeColor = Blanco;
        boton.Font = Boton;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.FlatAppearance.MouseOverBackColor = TealHover;
        boton.FlatAppearance.MouseDownBackColor = TealPresionado;
        boton.UseVisualStyleBackColor = false;
        boton.Cursor = Cursors.Hand;
    }

    public static void EstiloBotonSecundario(Button boton)
    {
        boton.BackColor = Crema;
        boton.ForeColor = TextoCuerpo;
        boton.Font = CuerpoSemi;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderColor = Borde;
        boton.FlatAppearance.MouseOverBackColor = CremaHover;
        boton.UseVisualStyleBackColor = false;
        boton.Cursor = Cursors.Hand;
    }

    /// <summary>Aplica el tema a un DataGridView y lo deja sin bordes ni cabeceras del sistema.</summary>
    public static void AplicarEstiloGrid(DataGridView grid)
    {
        grid.BackgroundColor = Blanco;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = Borde;
        grid.EnableHeadersVisualStyles = false;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToResizeRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.RowTemplate.Height = 34;

        grid.ColumnHeadersDefaultCellStyle.BackColor = Crema;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextoTenue;
        grid.ColumnHeadersDefaultCellStyle.Font = Etiqueta;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Crema;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextoTenue;
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        grid.DefaultCellStyle.BackColor = Blanco;
        grid.DefaultCellStyle.ForeColor = TextoCuerpo;
        grid.DefaultCellStyle.Font = Cuerpo;
        grid.DefaultCellStyle.SelectionBackColor = SeleccionSuave;
        grid.DefaultCellStyle.SelectionForeColor = TextoOscuro;
        grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
    }
}
