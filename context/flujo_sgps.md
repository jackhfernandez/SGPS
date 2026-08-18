

# Flujo de Vida Completo de un Proyecto en SGPS
### Desde la Creación hasta el Cierre y Despliegue en Producción

El ciclo de vida de un proyecto en SGPS (*Sistema de Gestión de Proyectos de Software*) sigue una metodología ágil rigurosa (Scrum / Kanban) integrada con control de accesos basado en roles (RBAC) y persistencia en SQL Server (`SGPS_DB`). A continuación, se detallan las 5 fases secuenciales del flujo operativo.

## Flujograma General del Proyecto

```markdown
┌──────────────┐     ┌──────────────────┐     ┌──────────────────────┐     ┌──────────────────────┐     ┌──────────────┐
│  1. INICIO   │ ──► │ 2. BACKLOG & MVP │ ──► │ 3. CICLO DE SPRINTS  │ ──► │ 4. QA, UAT & RELEASE │ ──► │  5. CIERRE   │
└──────────────┘     └──────────────────┘     └──────────────────────┘     └──────────────────────┘     └──────────────┘
• PM/PO crea Prj     • PO crea Epics          • Planning (SM/PO)           • Pruebas Cliente (UAT)      • 100% "Done"
• Asigna equipo      • Redacta US             • Kanban Execution (Dev)     • Certificación de Bugs      • Estado Archivado
• Configura clave    • Prioriza (WSJF/MVP)    • QA Testing & Bugs          • Generación Release Notes   • Solo Lectura
• Define accesos     • Estimación Fibonacci   • Feedback de Cliente        • Despliegue a Producción    • Auditoría final
```

## FASE 1: Configuración e Inicio (Kick-off)

### 1.1 Creación del Proyecto por el PM / PO
1. **Autenticación:** El usuario con rol **Product Owner (PO)** o **Project Manager (PM)** inicia sesión en la aplicación WinForms mediante `Login.cs` (`UsuarioLN.Autenticar` -> `UsuarioAD.ObtenerPorEmail`).
2. **Acceso al Módulo:** Ingresa al módulo de administración y abre el formulario `ProyectoCreacion.cs`.
3. **Registro de Datos Maestros:**
   * **Nombre del Proyecto:** Ej. *Sistema de Facturación Electrónica*.
   * **Clave Única (Prefix):** Ej. `SGPS` o `FAC` (utilizado automáticamente como prefijo para los tickets de historias y bugs: `SGPS-101`, `BUG-SGPS-01`).
   * **Metodología:** Selecciona *Scrum*, *Kanban* o *Híbrido*.
   * **Fechas Estimadas:** Fecha de Inicio y Fecha de Fin estimada.
4. **Validación y Persistencia:** `ProyectoLN.cs` valida que la clave no exista previamente y contenga entre 3 y 10 caracteres en mayúsculas, enviando la entidad `Proyecto.cs` hacia `ProyectoAD.cs` para su inserción en `dbo.Proyectos`.

### 1.2 Asignación del Equipo y Roles RBAC
1. **Búsqueda de Integrantes:** En la pestaña de asignación o formulario `ProyectoMiembros.cs`, el PM busca usuarios registrados en `dbo.Usuarios`.
2. **Asignación de Roles (`dbo.ProyectoMiembros`):**
   * **Scrum Master (SM):** Encargado de coordinar iteraciones, capacidad y métricas.
   * **Desarrolladores (Dev):** Asignados a la ejecución de tareas técnicas y avance en el tablero Kanban.
   * **QA Testers:** Asignados al control de calidad, reporte y re-prueba de fallos.
   * **Cliente (Stakeholder):** Asignado con permisos restringidos de supervisión y comentarios.
3. **Notificación:** El sistema activa los permisos RBAC correspondientes y despacha una notificación in-app registrada en `dbo.Notificaciones` a cada miembro asignado.

## FASE 2: Definición de Requerimientos y Construcción del Backlog

### 2.1 Desglose de Requerimientos (PO)
1. **Creación de Epics (`EpicGestion.cs`):** El PO define los módulos funcionales principales (ej. `EP-01: Autenticación`, `EP-02: Emisión de Comprobantes`) que se persisten en `dbo.Epics`.
2. **Redacción de User Stories (`UserStoryEdicion.cs`):** Registro de historias bajo la estructura estándar:
   > *Como [usuario], quiero [funcionalidad] para [beneficio]*
   * Se asigna el **Valor de Negocio** (*Alto, Medio, Bajo*) y los **Criterios de Aceptación**.
3. **Persistencia:** Se insertan las entidades `UserStory.cs` en la tabla `dbo.UserStories` con un correlativo incremental.

### 2.2 Priorización y Estimación (Lean & Planning Poker)
1. **Priorización WSJF:** El PO ordena el Product Backlog en `ProductBacklogGestion.cs` mediante *Drag & Drop*. La capa `UserStoryLN.cs` actualiza la columna `OrdenPrioridad` en `dbo.UserStories`.
2. **Estimación Técnica:** En sesión de refinamiento, el equipo técnico asigna **Story Points** utilizando la escala Fibonacci (1, 2, 3, 5, 8, 13, 21) a cada tarjeta.
3. **Definición del MVP:** Se seleccionan y marcan las historias prioritarias de alto valor que conformarán la primera entrega funcional del sistema.
4. **Verificación DoR (*Definition of Ready*):** Al contar con estimación, criterios claros y ausencia de bloqueos, la historia pasa al estado **Ready** (*Lista para Sprint*).

## FASE 3: Ejecución Iterativa (Ciclo de Sprints - Scrum Engine)

*(Esta fase se repite iterativamente en ciclos de 2 a 4 semanas hasta completar el alcance del proyecto)*

```markdown
                 ┌────────────────────────────────────────┐
                 │            CICLO DEL SPRINT            │
                 └───────────────────┬────────────────────┘
                                     │
             ┌───────────────────────┴───────────────────────┐
             ▼                                               ▼
  ┌─────────────────────┐                         ┌─────────────────────┐
  │   SPRINT PLANNING   │                         │  SPRINT EXECUTION   │
  │ ─────────────────── │                         │ ─────────────────── │
  │ • SM crea Sprint    │                         │ • Dev toma tarea en │
  │ • PO pasa historias │                         │   Kanban (To Do ->  │
  │   del Backlog       │                         │   In Progress)      │
  │ • Valida Capacidad  │                         │ • Registra horas    │
  │   (SprintLN)        │                         │ • Menciones @user   │
  └─────────────────────┘                         └──────────┬──────────┘
                                                             │
                                                             ▼
                                                  ┌─────────────────────┐
                                                  │ QA & BUG TRACKING   │
                                                  │ ─────────────────── │
                                                  │ • Mueve a Testing   │
                                                  │ • QA valida criterio│
                                                  │ • ¿Hay fallos?      │
                                                  └──────────┬──────────┘
                                                             │
                                   ┌─────────────────────────┴─────────────────────────┐
                                   ▼ [SÍ]                                              ▼ [NO]
                   ┌───────────────────────────────────┐               ┌───────────────────────────────────┐
                   │ • Registra BUG en BugReporte.cs   │               │ • Pasa a columna "Done"           │
                   │ • Bloquea US y vuelve a Dev       │               │ • Aprobación de criterios al 100% │
                   └───────────────────────────────────┘               └───────────────────────────────────┘
```

### 3.1 Sprint Planning (Apertura de Iteración)
1. El Scrum Master accede a `SprintPlanificacion.cs` y crea un nuevo Sprint en `dbo.Sprints` (ej. *Sprint 1 - Módulo Base*).
2. Arrastra las historias desde el Product Backlog hacia el Sprint Backlog.
3. `SprintLN.cs` valida que la suma de Story Points no exceda la capacidad del equipo ni existan traslapes de fechas.
4. Al hacer clic en **Iniciar Sprint**:
   * Se congela la línea base en SQL Server (`dbo.Sprints.Estado = 'Activo'`).
   * Se inicializan los datos para el gráfico de avance (`BurndownChartVista.cs`).

### 3.2 Ejecución Diaria en el Tablero Kanban (Dev Team)
1. Cada desarrollador accede a `TableroKanban.cs` en su cliente WinForms.
2. Visualiza las tarjetas instanciadas a través del control `UcTarjetaKanban.cs`.
3. Arrastra las tarjetas de **To Do** a **In Progress**.
4. Accede a `TareaEdicion.cs` para desglosar actividades técnicas en `dbo.Tareas`, imputando horas estimadas y reales.
5. Colabora mediante comentarios con menciones `@usuario` persistidas en `dbo.Comentarios` y notificadas vía `dbo.Notificaciones`.

### 3.3 Control de Calidad (QA & Bug Tracking)
1. Al finalizar la codificación y pruebas unitarias, el Dev mueve la tarjeta a **In Testing**.
2. El QA Tester recibe la alerta in-app y abre `BugReporte.cs` / `BugGestion.cs`:
   * **Con Incidencias:** Registra el bug en `dbo.Bugs` (Severidad: *Bloqueante, Alta, Media, Baja*) vinculado a la User Story (`UserStoryId`). `BugLN.cs` bloquea la historia impidiendo su paso a Done y la regresa a **In Progress**.
   * **Sin Incidencias (Criterios al 100%):** El QA aprueba la historia y la traslada a **Done**.

### 3.4 Feedback Continuo del Cliente (Módulo Cliente)
1. El usuario con rol Cliente ingresa mediante la vista protegida `ClientePortal.cs`.
2. Monitorea en tiempo real el porcentaje de avance general (0% a 100%) y consulta las historias entregadas.
3. Ingresa observaciones y comentarios directos sobre las entregas para revisión del PO.

### 3.5 Cierre del Sprint y Demo (Sprint Review & Retrospective)
1. El SM expone el incremento de software funcional al PO y Cliente.
2. En `SprintPlanificacion.cs`, el SM ejecuta la acción **Cerrar Sprint**:
   * Las historias en estado **Done** se consolidan formalmente como avance completado.
   * Historias inconclusas retornan al Product Backlog o se mueven al siguiente Sprint.
   * Se recalculan las métricas ágiles en `MetricasAgilesVista.cs` (*Velocity Chart*).

## FASE 4: Control de Calidad Global, UAT y Release

Al culminar las iteraciones planificadas para la versión o MVP:
1. **Pruebas de Aceptación del Usuario (UAT):** El Cliente y el PO llevan a cabo pruebas End-to-End sobre el compilado de staging.
2. **Certificación de Defectos:** `BugLN.cs` consulta en `dbo.Bugs` y valida que **no existan** incidencias activas con severidad *Bloqueante* o *Alta*.
3. **Generación de Release Notes:** Se vinculan las historias finalizadas a la versión oficial (ej. `v1.0.0-PROD`) exportando la bitácora de cambios.
4. **Despliegue a Producción:** Distribución del ejecutable cliente WinForms y ejecución de scripts DDL/DML sobre la base centralizada `SGPS_DB`.

## FASE 5: Cierre Formal e Historización del Proyecto

```markdown
┌──────────────────────┐     ┌──────────────────────┐     ┌──────────────────────┐     ┌──────────────────────┐
│  100% Backlog Done   │ ──► │  Cierre de Sprints   │ ──► │   Auditoría Final    │ ──► │ Estado: 'Archivado'  │
└──────────────────────┘     └──────────────────────┘     └──────────────────────┘     └──────────┬───────────┘
                                                                                                  │
                                                                                                  ▼
                                                                                       ┌──────────────────────┐
                                                                                       │ SQL: EsActivo = 0    │
                                                                                       │ • Bloqueo de edición │
                                                                                       │ • Acceso Read-Only   │
                                                                                       │ • Trazabilidad total │
                                                                                       └──────────────────────┘
```

1. **Verificación de Completitud:** En `ProyectoCreacion.cs` / `Principal.cs`, el PM valida que el **100%** de las Historias de Usuario en `dbo.UserStories` se encuentren en estado **Done** o hayan sido descartadas formalmente por el PO.
2. **Generación de Reportes Finales:**
   * Balance de horas estimadas vs. horas reales trabajadas (`dbo.Tareas`).
   * Trazabilidad completa e historial inmutable de auditoría (`dbo.HistorialCambios`).
   * Reporte consolidado de velocidad y densidad de defectos.
3. **Cierre de Proyecto en SQL Server:**
   * El PM o Administrador ejecuta la acción **Archivar / Finalizar Proyecto**.
   * `ProyectoLN.cs` invoca a `ProyectoAD.cs` para actualizar el registro en la base de datos:
   ```sql
   UPDATE dbo.Proyectos
   SET EsActivo = 0, FechaFinReal = GETDATE()
   WHERE ProyectoId = @ProyectoId;

4. **Modo Solo Lectura (Read-Only):**
* El proyecto se oculta de los tableros activos (`TableroKanban.cs`) y de las listas de planificación de sprints.
* Se transfiere a la vista de **Histórico de Portafolio**.
* Todos los miembros del equipo mantienen permisos de solo lectura para auditorías y cumplimiento normativo (ISO/IEC 27001).