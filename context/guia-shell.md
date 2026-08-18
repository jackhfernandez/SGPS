# Capa de Presentación — guía del shell

`Principal.cs` es un **shell**: una barra lateral fija más un panel de contenido
donde los módulos se incrustan en sitio. No hay MDI ni ventanas flotantes.

```
┌──────────────────┬───────────────────────────────────────────┐
│  (S) SGPS        │  SGPS / BACKLOG / EPICS      ← breadcrumb │
│                  ├───────────────────────────────────────────┤
│  ▸ Resumen       │                                           │
│  ▾ Backlog       │   panelContenido                          │
│      Epics    ◀  │   · arranca en la página Resumen          │
│      Historias   │   · aquí se incrusta el módulo activo     │
│  ▸ Sprints …     │                                           │
│  ⚙ Administración│                                           │
│  (AQ) Ana Quispe │                                           │
└──────────────────┴───────────────────────────────────────────┘
   260px #0F3B33                    #F5F1E8 con tarjetas blancas
```

---

## Añadir un módulo nuevo

Son **3 pasos**. Ni la barra lateral ni las tarjetas de la página de Resumen se
tocan a mano: ambas se generan recorriendo el mapa de navegación.

### Paso 1 — Crear el formulario

Un `Form` normal, con el diseñador de siempre. **No** hay que convertirlo a
`UserControl` ni heredar de nada especial. Colócalo en la subcarpeta de su
módulo (`Backlog/`, `Kanban/`, `Sprint/`…).

Como el formulario acabará incrustado, ten en cuenta dos cosas al maquetarlo:

- Usa `Dock`/`Anchor` en los contenedores raíz para que estire con el panel.
  Si dejas todo en posición absoluta, quedará arrinconado arriba a la izquierda.
- Valida el permiso en `OnLoad` (defensa en profundidad), como hacen los demás:

  ```csharp
  protected override void OnLoad(EventArgs e)
  {
      try
      {
          PermisoLN.ValidarLectura(Modulo.EpicGestion);
      }
      catch (PermisoDenegadoException ex)
      {
          MessageBox.Show(ex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          Close();
          return;
      }

      base.OnLoad(e);
      // … carga de datos
  }
  ```

### Paso 2 — Declarar el permiso

En `Logica/PermisoLN.cs`: añade el valor al `enum Modulo` y su fila en
`CrearMatriz()`.

```csharp
public enum Modulo
{
    // …
    ReleaseNotes            // ← nuevo
}
```

```csharp
Fila(Modulo.ReleaseNotes,
    (RolSistema.Administrador, NivelAcceso.Total),
    (RolSistema.ProductOwner,  NivelAcceso.Total),
    (RolSistema.ScrumMaster,   NivelAcceso.Lectura));
```

Los roles que no aparecen en la fila quedan en `SinAcceso` y no verán el módulo.
Actualiza también la tabla de `permisos.md`, que es la documentación de la matriz.

### Paso 3 — Añadir la línea al mapa de navegación

En `Ui/MapaNavegacion.cs`, array `Grupos`. **Esta es la única línea que escribes
para que aparezcan el ítem de la barra lateral y la tarjeta del Resumen.**

Si el módulo entra en un grupo existente, añade un `ItemNav`:

```csharp
new("Sprints", Tema.Glifos.Cronometro, "Planificación y cierre de iteraciones",
[
    new("Planificación", Modulo.SprintPlanificacion, () => new SprintPlanificacion(), false),
    new("Ejecución",     Modulo.SprintEjecucion,     () => new SprintEjecucion(),     false),
    new("Release notes", Modulo.ReleaseNotes,        () => new ReleaseNotes(),        true)  // ← nuevo
]),
```

Si necesitas un grupo nuevo (= **una tarjeta nueva** en el Resumen), añade un
`GrupoNav` al array:

```csharp
new("Releases", Tema.Glifos.Carpeta, "Versiones y notas de entrega",
[
    new("Release notes", Modulo.ReleaseNotes, () => new ReleaseNotes(), true)
]),
```

Firmas de los dos records:

| Record     | Argumentos                                                          |
|------------|---------------------------------------------------------------------|
| `GrupoNav` | `Texto`, `Glifo`, `Descripcion` (línea gris de la tarjeta), `Items` |
| `ItemNav`  | `Texto`, `Modulo`, `Crear` (fábrica del form), `Construido`         |

---

## El interruptor `Construido`

Es el último `bool` de `ItemNav`. Hoy 5 de los 16 ítems del mapa siguen siendo
plantillas vacías (QA, Métricas y Cliente), así que están en `false`.

| Valor   | Tarjeta del Resumen                    | Al pulsar el ítem                       |
|---------|----------------------------------------|-----------------------------------------|
| `false` | atenuada, chip «En construcción»        | muestra la página `PaginaEnConstruccion` |
| `true`  | normal, chip con el nivel de acceso     | incrusta el formulario real              |

Cuando termines de implementar un módulo, **cambia su `false` por `true`**. Eso
es todo: la página «Módulo pendiente» indica en pantalla qué archivo falta, así
que sirve de lista de tareas viva.

---

## Cómo funciona la incrustación

Todo ocurre en `Principal.cs`.

`Navegar(grupo, item)` revalida el permiso y decide qué mostrar; luego
`PrepararIncrustado` convierte el `Form` en algo que puede vivir dentro de un
panel:

```csharp
formulario.TopLevel = false;                        // deja de ser ventana propia
formulario.FormBorderStyle = FormBorderStyle.None;  // sin barra de título
formulario.ControlBox = false;
formulario.AutoScroll = false;                      // que no herede scroll
formulario.Dock = DockStyle.Fill;
```

y `MostrarEnHost` lo coloca:

```csharp
panelContenido.Controls.Add(contenido);
if (contenido is Form formulario) formulario.Show();
```

Dos detalles que rompen la app si se invierten:

- **`TopLevel = false` debe asignarse ANTES de `Controls.Add`.** Al revés,
  WinForms lanza excepción.
- **El `Show()` final no es opcional**: un `Form` con `TopLevel = false` se queda
  invisible sin él.

### Formularios que llaman a `Close()`

Los módulos se escribieron como diálogos y su botón «Cancelar» hace `Close()`.
Incrustado, eso dejaría el panel vacío, así que el shell escucha `FormClosed` y
vuelve automáticamente a la página de Resumen. **No hace falta tocar los
formularios** por este motivo.

### Diálogos de verdad

Las pantallas que son realmente modales (editar una historia, imputar horas en
una tarea) **no** deben ir al mapa de navegación: se abren con `ShowDialog()`
desde su pantalla padre, como cualquier diálogo. El mapa es solo para pantallas
de primer nivel.

Por el mismo motivo `Modulo.UcTarjetaKanban` no está en el mapa: es un control
de tarjeta que instancia `TableroKanban`, no una pantalla.

---

## Patrón de pantalla de gestión (referencia Sprint)

`SprintPlanificacion` y `SprintEjecucion` comparten un patrón de pantalla de
gestión que puedes copiar para los módulos que falten: lista a la izquierda,
detalle a la derecha y un selector de proyecto arriba.

### Flujo en tres capas

1. **Script** — `Datos/scripts/SGPS_pa_<Tabla>.sql`. Procedimientos idempotentes
   (`IF OBJECT_ID(...) IS NOT NULL DROP` + `CREATE`). Los de escritura usan
   `SET NOCOUNT ON` y terminan con `SELECT @@ROWCOUNT AS FilasAfectadas;`
   porque con `SET NOCOUNT ON` `ExecuteNonQuery` devolvería -1.
2. **Datos** — `<Tabla>AD.cs`. Un método por procedimiento. Los de escritura leen
   el conteo con `ExecuteScalar` (helper `LeerFilasAfectadas`). El `try/catch`
   relanza con prefijo `"Error en la capa datos (...)..."`.
3. **Lógica** — `<Tabla>LN.cs`. Valida reglas de negocio y devuelve
   `out string mensajeError` en los métodos que pueden fallar con un mensaje para
   el usuario. El formulario nunca habla con `AD` directamente.

> **Conexión ya abierta:** `Conexion.ObtenerConexion()` devuelve la conexión
> abierta. No llames `cn.Open()` después o lanzará *"The connection was not
> closed. The connection's current state is open."* (era un bug en
> `SprintAD.Insertar/ObtenerPorId/ActualizarEstado`).

### Maquetación tipo

```
┌ pnlSelector   (Dock Top, 56px)    combo de proyecto + etiqueta
├ splitVertical (Dock Fill, vertical)
│  ├ Panel1 (FixedPanel.Panel1, Panel1MinSize)  grid de la lista + lblResumen (Bottom)
│  └ Panel2                                  detalle: grid + acciones + resumen
```

En el detalle, el orden de `Controls.Add` importa (z-order del `Dock`):

```csharp
pnlDetalle.Controls.Add(dgvHistorias);      // 1º el Fill
pnlDetalle.Controls.Add(lblTitulo);         // 2º los Top, en orden vertical
pnlDetalle.Controls.Add(pnlAcciones);       // …
pnlDetalle.Controls.Add(lblResumenSprint);  // último el Bottom
```

### Trampas de WinForms

- **`SplitterDistance` no se toca en `InitializeComponent`.** El contenedor aún
  no tiene ancho real y el setter valida contra el tamaño por defecto (~150px),
  lanzando `ArgumentOutOfRangeException`. Se ajusta en `OnLoad`:

  ```csharp
  private void AjustarSplit()
  {
      try
      {
          var maximo = splitVertical.Width - splitVertical.Panel2MinSize - splitVertical.SplitterWidth;
          if (maximo > splitVertical.Panel1MinSize)
          {
              splitVertical.SplitterDistance = Math.Clamp(
                  (int)(splitVertical.Width * 0.30),
                  splitVertical.Panel1MinSize,
                  maximo);
          }
      }
      catch (Exception) { /* contenedor aún sin ancho: se deja el reparto por defecto */ }
  }
  ```

- **En el diseñador, fija `Size` ANTES que `Panel1MinSize`/`Panel2MinSize`.**
  El setter de `Panel1MinSize` re-ajusta `SplitterDistance` internamente y lo
  valida contra el ancho actual; si aún es el default (150px) lanza la misma
  excepción. Este fue el bug real de *"No se pudo abrir 'Planificación'"*.
- **Colisión de nombres:** dentro de `namespace Presentacion.Sprint`, el tipo
  `Modelo.Sprint` colisiona con el namespace. Usa `Modelo.Sprint` totalmente
  calificado; `using Modelo;` no es suficiente.
- **Flag `_cargando`:** `SelectedIndexChanged`/`SelectionChanged` también se
  disparan al rellenar la lista desde código. Pon `_cargando = true` mientras
  rellenas y retorna al inicio del handler si está activo.

### Estado de la UI

Un único método `ActualizarEstadoBotones()` centraliza el `Enabled` de las
acciones: permiso (`PermisoLN.TieneAcceso(...)`) + estado de la entidad
seleccionada + selección vigente. Llámalo desde los handlers y desde la carga.

---

## Patrón de tablero (referencia Kanban)

`TableroKanban` no usa el patrón lista/detalle: son cuatro columnas de estado
(`To Do`, `In Progress`, `In Testing`, `Done`) generadas en código sobre un
`TableLayoutPanel` del diseñador. Cada columna es un `PanelTarjeta` con una
etiqueta de encabezado (`Dock.Top`) y un `FlowLayoutPanel` desplazable
(`Dock.Fill`, `AutoScroll = true`) que contiene las tarjetas.

- **Las columnas se generan en código** porque son la misma pieza repetida y
  porque el diseñador no puede cablear el arrastre entre ellas.
- **Las tarjetas (`UcTarjetaKanban`) se pintan a mano.** Una tarjeta compuesta
  de labels no se puede arrastrar de una pieza: el `MouseDown` lo captura el
  hijo, no el control. Con `UserPaint` toda la tarjeta es una superficie única.
- **El arrastre solo se inicia al superar `SystemInformation.DragSize`**, para
  no convertir en arrastre cualquier clic con un temblor de ratón.
- **Las tarjetas no tienen `AllowDrop`**: lo tienen la columna y su lista, así
  el soltar sobre una tarjeta cae en la columna que la contiene.
- **El ancho de las tarjetas se recalcula en el `Resize` de la lista**, porque
  la barra de desplazamiento aparece y desaparece según cuántas haya.

El cambio de estado no lo decide el formulario: lo aplica
`TareaLN.CambiarEstadoKanbanUserStory(...)`, que valida la transición del flujo,
la Definition of Done (no hay paso a `Done` con tareas técnicas pendientes ni
con bugs Bloqueante/Alta abiertos), autoasigna la historia al pasar a
`In Progress` y registra el cambio en `dbo.HistorialCambios`. El formulario solo
captura `InvalidOperationException` y muestra su mensaje.

`TareaEdicion` tiene **dos modos** con el mismo código:

| Constructor                  | Modo                | Selector de proyecto/historia |
|------------------------------|---------------------|-------------------------------|
| `new TareaEdicion()`         | pantalla del shell  | visible                       |
| `new TareaEdicion(historia)` | diálogo del tablero | oculto (`pnlSelector`)        |

> **Conexión ya abierta (otra vez):** el mismo bug de `cn.Open()` doble que
> tenía `SprintAD` estaba en `TareaAD`, `BugAD` y `HistorialCambioAD`, y hacía
> caer el arrastre a `Done`. Quedan instancias en `ProyectoAD`
> (líneas ~229, ~275, ~305, ~369) pendientes para quien lleve ese módulo.

### Verificación

1. `dotnet build SGPS.slnx`
2. **Prueba headless del diseñador** para cazar excepciones de layout: construir
   el `Form` sin mostrarlo a varios anchos
   (`form.ClientSize = ...; form.CreateControl(); form.PerformLayout();`) en un
   hilo `[STAThread]`.
3. **Prueba de integración contra la BD** del ciclo completo usando las capas
   `LN`/`AD` (crear → asignar → iniciar → avanzar estados → cerrar) y limpia los
   datos al final.
4. Para ejecutar un script SQL contra la BD local:

   ```
   sqlcmd -S "localhost\SQLEXPRESS" -E -C -d SGPS_DB -i Datos\scripts\SGPS_pa_Sprint.sql -b
   ```

   (el `-E` es autenticación de Windows; las credenciales reales están
   comentadas en `App.config` y en runtime se aplican por las variables
   `SQLSERVER_DB_*`).

---

## Estilos: `Ui/Tema.cs`

Fuente de verdad de la identidad visual. **No escribas `Color.FromArgb(...)` a
mano en formularios nuevos**; usa `Tema`.

### Colores

| Constante                            | Uso                                       |
|--------------------------------------|-------------------------------------------|
| `VerdeProfundo` `#0F3B33`            | barra lateral, tarjetas oscuras           |
| `VerdeHover` `#174A41`               | hover/activo dentro de la barra lateral   |
| `Teal` `#0C6E63`                     | botón primario, ítem activo               |
| `TealHover` / `TealPresionado`       | estados del botón primario                |
| `TealSuave` `#498F84`                | texto secundario sobre verde profundo     |
| `Crema` `#F5F1E8` / `CremaHover`     | fondo del área de contenido               |
| `Blanco`                             | tarjetas                                  |
| `Borde` `#DAD4C4`                    | bordes y separadores sobre crema          |
| `TextoOscuro` / `TextoCuerpo` / `TextoTenue` | jerarquía de texto                |
| `Dorado` / `Coral` / `Oliva`         | acentos y colores de rol                  |
| `SeleccionSuave`                     | fila seleccionada en grids                |

### Fuentes

`TituloHero`, `TituloSeccion`, `TituloTarjeta`, `Etiqueta` (Consolas, para
mayúsculas tipo `ACCESO`), `Cuerpo`, `CuerpoSemi`, `Boton`, `Numero`, `Marca`,
`Icono`, `IconoGrande`.

### Helpers

```csharp
Tema.AplicarEstiloGrid(dgvHistorias);   // quita bordes y aplica cabecera crema
Tema.EstiloBotonPrimario(btnGuardar);
Tema.EstiloBotonSecundario(btnCancelar);

Tema.ColorDeRol(rol);              // tono oscuro, para fondos claros
Tema.ColorDeRolSobreOscuro(rol);   // tono claro, para la barra lateral
Tema.ContrasteSobre(color);        // blanco o verde según luminancia
Tema.TextoNivel(nivel);            // "Total" / "Edición" / "Lectura"
Tema.Iniciales(nombreCompleto);    // "AQ"
```

### Iconos

`Tema.Glifos` expone los códigos de **Segoe MDL2 Assets**: `Inicio`, `Carpeta`,
`Lista`, `Cronometro`, `Tablero`, `Alerta`, `Grafico`, `Contacto`, `Ajustes`,
`Personas`, `Buscar`, `Mas`, `ChevronAbajo`, `ChevronDerecha`, `Construccion`.

Consúltalos siempre tras comprobar `Tema.HayIconos`; si la fuente no está
instalada, los controles omiten el glifo en lugar de dibujar cuadros vacíos.

---

## Controles propios (`Ui/`)

| Archivo                  | Qué es                                                        |
|--------------------------|---------------------------------------------------------------|
| `Tema.cs`                | paleta, fuentes, glifos y helpers                             |
| `MapaNavegacion.cs`      | tabla declarativa de la navegación                            |
| `PanelTarjeta.cs`        | `Panel` con esquinas redondeadas y borde                      |
| `TarjetaModulo.cs`       | la tarjeta grande de la página de Resumen                     |
| `ItemNavegacion.cs`      | ítem de la barra lateral (grupo o sub-ítem)                   |
| `PaginaResumen.cs`       | página de inicio                                              |
| `PaginaEnConstruccion.cs`| marcador para módulos aún no implementados                    |

### Regla al pintar

**Nunca asignes propiedades del control dentro de `OnPaint`.** Los setters
llaman a `Invalidate()`, y eso encadena un repintado infinito que satura la CPU
y deja media interfaz sin dibujar. Calcula el valor en una variable local o
asígnalo en el constructor.

---

## Sin barra de scroll

Es un requisito del diseño. Se sostiene sobre cuatro cosas:

- `Principal`: `WindowState = Maximized`, `AutoScroll = false`, `MinimumSize`.
- Barra lateral: `TableLayoutPanel` de 4 filas (logo / navegación / administración
  / usuario). Se usa tabla y no `Dock` apilado porque el orden de `Dock` depende
  del z-order de la colección y es frágil.
- **Acordeón de expansión única**: al abrir un grupo se cierran los demás, así el
  alto máximo de la navegación queda acotado.
- Páginas de contenido con `TableLayoutPanel` en porcentajes.

Si un módulo necesita scroll interno (una lista larga), ponlo en un contenedor
propio con `AutoScroll = true`, no en el formulario entero.

---

## Probar

```
dotnet build SGPS.slnx
```

Requiere `SGPS_DB` en `localhost` (ver `App.config`). Si no está, ejecuta en
orden `Datos/scripts/SGPS_crea.sql`, `SGPS_procedimientos.sql` y
`SGPS_datos_prueba.sql`.

Usuarios de prueba, contraseña `Sgps.2026`. La barra lateral debe mostrar
exactamente estos grupos:

| Usuario               | Rol           | Grupos visibles                                              |
|-----------------------|---------------|--------------------------------------------------------------|
| `admin@sgps.local`    | Administrador | todos, incluido Administración                               |
| `po@sgps.local`       | ProductOwner  | todos menos Administración                                   |
| `sm@sgps.local`       | ScrumMaster   | todos menos Administración                                   |
| `dev@sgps.local`      | Developer     | sin Administración ni Cliente                                |
| `qa@sgps.local`       | QA            | sin Administración ni Cliente                                |
| `cliente@sgps.local`  | Cliente       | solo Resumen, Métricas y Cliente — arranca en el Portal      |

`permisos_debug.log`, en la carpeta de salida, registra el diagnóstico de roles
y el estado de la navegación en cada inicio de sesión. Si un usuario solo ve
«Resumen», revisa ahí si su rol de BD quedó sin mapear y añádelo a
`Logica/permisos.roles.json`.
