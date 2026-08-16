# Permisos y Accesos por Módulo

Matriz de accesos por rol/módulo del SGPS. La fuente de verdad está en
`Logica/PermisoLN.cs` (matriz `PermisoLN.Matriz`).

La barra lateral de `Presentacion/Principal.cs` se genera a partir de la tabla
declarativa `Presentacion/Ui/MapaNavegacion.cs`: cada entrada apunta a un valor
de `Modulo`, y se muestra solo si `PermisoLN.PuedeVer(modulo)` es cierto (un
grupo se ve si alguno de sus sub-ítems se ve). Al navegar, el módulo vuelve a
validar el permiso antes de abrirse (defensa en profundidad).

> Para dar de alta una pantalla nueva basta con añadir un `ItemNav` en
> `MapaNavegacion.cs` y su fila en `PermisoLN.Matriz`; la barra lateral y las
> tarjetas de la página de Resumen se actualizan solas.

## Niveles de acceso

| Nivel           | Significado                                                    |
|-----------------|----------------------------------------------------------------|
| `Total`         | Lectura + creación + edición + eliminación (control total)     |
| `Edición`       | Lectura + creación + edición                                   |
| `Lectura`       | Solo consulta / visualización                                  |
| `Sin acceso`    | El módulo no se muestra ni se puede abrir                      |

## Matriz de acceso por rol y módulo

> Roles según `Datos/scripts/SGPS_datos_prueba.sql`: `Administrador`,
> `ProductOwner`, `ScrumMaster`, `Developer`, `QA`, `Cliente`.

| Módulo / Formulario             | Administrador | ProductOwner | ScrumMaster | Developer | QA | Cliente |
|---------------------------------|---------------|--------------|-------------|-----------|----|---------|
| `Login.cs`                      | Sí            | Sí           | Sí          | Sí        | Sí | Sí      |
| `Principal.cs` (MDI)            | Sí            | Sí           | Sí          | Sí        | Sí | Sí      |
| `UsuarioGestion.cs`             | Total         | Sin acceso   | Sin acceso  | Sin acceso| Sin acceso | Sin acceso |
| `RolGestion.cs`                 | Total         | Sin acceso   | Sin acceso  | Sin acceso| Sin acceso | Sin acceso |
| `ProyectoCreacion.cs`           | Total         | Total        | Lectura     | Lectura   | Lectura | Sin acceso |
| `ProyectoMiembros.cs`           | Total         | Total        | Lectura     | Lectura   | Lectura | Sin acceso |
| `ProductBacklogGestion.cs`      | Total         | Total        | Lectura     | Lectura   | Lectura | Sin acceso |
| `EpicGestion.cs`                | Total         | Total        | Lectura     | Lectura   | Lectura | Sin acceso |
| `UserStoryEdicion.cs`           | Total         | Total        | Lectura     | Lectura   | Lectura | Sin acceso |
| `SprintPlanificacion.cs`        | Total         | Edición      | Total       | Lectura   | Lectura | Sin acceso |
| `SprintEjecucion.cs`            | Total         | Edición      | Total       | Lectura   | Lectura | Sin acceso |
| `TableroKanban.cs`              | Total         | Lectura      | Lectura     | Total     | Lectura | Sin acceso |
| `TareaEdicion.cs`               | Total         | Lectura      | Lectura     | Total     | Lectura | Sin acceso |
| `UcTarjetaKanban.cs`            | Total         | Lectura      | Lectura     | Total     | Lectura | Sin acceso |
| `BugReporte.cs` / `BugGestion.cs`| Total        | Lectura      | Lectura     | Edición   | Total | Sin acceso |
| `ClientePortal.cs`              | Lectura       | Lectura      | Lectura     | Sin acceso| Sin acceso | Solo Lectura + Comentarios |
| `BurndownChartVista.cs`         | Total         | Total        | Total       | Lectura   | Lectura | Lectura |
| `MetricasAgilesVista.cs`        | Total         | Total        | Total       | Lectura   | Lectura | Lectura |

> Notas:
> - `RolGestion`, `ProyectoMiembros`, `EpicGestion`, `UserStoryEdicion`,
>   `SprintEjecucion`, `TareaEdicion`, `UcTarjetaKanban` y
>   `MetricasAgilesVista` no estaban en la tabla original; se les asignó el
>   mismo acceso que su módulo equivalente (Seguridad, Proyectos, Backlog,
>   Sprint, Kanban y Reportes).
> - `UcTarjetaKanban` es un control de tarjeta, no una pantalla: conserva su
>   fila en la matriz (con el acceso de Kanban) pero **no aparece en la barra
>   lateral**, porque se instancia desde `TableroKanban`.
> - El acceso del Cliente al `ClientePortal` se modela como `Lectura` en
>   `PermisoLN.cs` (la UI a futuro restringirá comentarios).
> - Si un usuario tiene varios roles, se toma el mayor nivel de acceso.
> - Los nombres de rol se comparan de forma tolerante (mayúsculas/minúsculas,
>   espacios, paréntesis y sinónimos): `ProductOwner`, `Product Owner`,
>   `Product Owner (PO)` o `PO` se tratan como el mismo rol; igual para
>   `ScrumMaster`, `QA`/`QA Tester`, `Admin`/`Administrador`, `Cliente`.
> - El mapeo nombre de rol → rol del sistema es **configurable** en
>   `Logica/permisos.roles.json` (se copia a la carpeta de salida). Si en la
>   BD existen roles con nombres personalizados (p. ej. `dev01`, `qa01`),
>   agrégalos ahí sin recompilar:
>
>   ```json
>   {
>     "MapeoRoles": {
>       "dev01": "Developer",
>       "qa01": "QA"
>     }
>   }
>   ```
>
>   Si tras iniciar sesión solo aparece "Archivo > Cerrar sesión", revisa en la
>   barra de estado el nombre exacto del rol y añádelo a ese archivo
>   (o usa los nombres estándar del script de datos de prueba).

## Usuarios registrados (datos de prueba) y módulos accesibles

Usuarios de `SGPS_datos_prueba.sql` (contraseña: `Sgps.2026`).

### Ana Quispe — `admin@sgps.local` — Administrador

| Módulo                       | Nivel de acceso |
|------------------------------|-----------------|
| UsuarioGestion               | Total           |
| RolGestion                   | Total           |
| ProyectoCreacion             | Total           |
| ProyectoMiembros             | Total           |
| ProductBacklogGestion        | Total           |
| EpicGestion / UserStoryEdicion | Total         |
| SprintPlanificacion/Ejecucion| Total           |
| TableroKanban / TareaEdicion | Total           |
| BugReporte / BugGestion      | Total           |
| ClientePortal                | Lectura         |
| BurndownChartVista / Metricas | Total         |

### Bruno Salazar — `po@sgps.local` — ProductOwner

| Módulo                       | Nivel de acceso |
|------------------------------|-----------------|
| UsuarioGestion / RolGestion  | Sin acceso      |
| ProyectoCreacion / Miembros  | Total           |
| ProductBacklogGestion        | Total           |
| EpicGestion / UserStoryEdicion | Total         |
| SprintPlanificacion/Ejecucion| Edición         |
| TableroKanban / TareaEdicion | Lectura         |
| BugReporte / BugGestion      | Lectura         |
| ClientePortal                | Lectura         |
| BurndownChartVista / Metricas | Total         |

### Carla Mendoza — `sm@sgps.local` — ScrumMaster

| Módulo                       | Nivel de acceso |
|------------------------------|-----------------|
| UsuarioGestion / RolGestion  | Sin acceso      |
| ProyectoCreacion / Miembros  | Lectura         |
| ProductBacklogGestion        | Lectura         |
| EpicGestion / UserStoryEdicion | Lectura       |
| SprintPlanificacion/Ejecucion| Total           |
| TableroKanban / TareaEdicion | Lectura         |
| BugReporte / BugGestion      | Lectura         |
| ClientePortal                | Lectura         |
| BurndownChartVista / Metricas | Total         |

### Diego Rojas — `dev@sgps.local` — Developer

| Módulo                       | Nivel de acceso |
|------------------------------|-----------------|
| UsuarioGestion / RolGestion  | Sin acceso      |
| ProyectoCreacion / Miembros  | Lectura         |
| ProductBacklogGestion        | Lectura         |
| EpicGestion / UserStoryEdicion | Lectura       |
| SprintPlanificacion/Ejecucion| Lectura         |
| TableroKanban / TareaEdicion | Total           |
| BugReporte / BugGestion      | Edición         |
| ClientePortal                | Sin acceso      |
| BurndownChartVista / Metricas | Lectura       |

### Elena Vargas — `qa@sgps.local` — QA

| Módulo                       | Nivel de acceso |
|------------------------------|-----------------|
| UsuarioGestion / RolGestion  | Sin acceso      |
| ProyectoCreacion / Miembros  | Lectura         |
| ProductBacklogGestion        | Lectura         |
| EpicGestion / UserStoryEdicion | Lectura       |
| SprintPlanificacion/Ejecucion| Lectura         |
| TableroKanban / TareaEdicion | Lectura         |
| BugReporte / BugGestion      | Total           |
| ClientePortal                | Sin acceso      |
| BurndownChartVista / Metricas | Lectura       |

### Fabio Herrera — `cliente@sgps.local` — Cliente

| Módulo                       | Nivel de acceso |
|------------------------------|-----------------|
| UsuarioGestion / RolGestion  | Sin acceso      |
| ProyectoCreacion / Miembros  | Sin acceso      |
| ProductBacklogGestion        | Sin acceso      |
| EpicGestion / UserStoryEdicion | Sin acceso    |
| SprintPlanificacion/Ejecucion| Sin acceso      |
| TableroKanban / TareaEdicion | Sin acceso      |
| BugReporte / BugGestion      | Sin acceso      |
| ClientePortal                | Solo Lectura + Comentarios |
| BurndownChartVista / Metricas | Lectura       |

> Gloria Paredes — `inactivo@sgps.local` — tiene rol Developer pero está
> desactivada (`EsActivo = 0`) y no puede iniciar sesión.
