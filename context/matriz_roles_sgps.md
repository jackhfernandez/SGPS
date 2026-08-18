# Matriz de Roles y Accesos por Módulo (SGPS)

El **Sistema de Gestión de Proyectos de Software (SGPS)** utiliza un diseño conceptual centralizado. Todos los perfiles de usuario se conectan de forma simultánea a un **servidor de base de datos único (SGPS_DB) bajo tecnología SQL Server**.

```
                                            ┌──────────────────────────────────────────┐
                                            │          SERVIDOR DE BD ÚNICO            │
                                            │               (SGPS_DB)                  │
                                            └────────────────────▲─────────────────────┘
                                                                 │
        ┌─────────────────────┬────────────────────────┬──────────────────────┬─────────────────────┬────────────────────┐            
        │                     │                        │                      │                     │                    |
┌───────┴──────┐      ┌───────┴──────┐         ┌───────┴──────┐       ┌───────┴──────┐      ┌───────┴──────┐    ┌────────┴────────┐
│ PC: Admin    │      │ PC: PO / PM  │         │ PC: Scrum M. │       │   PC: Dev    │      │   PC: QA     │    │  PC: Cliente    │
│ (Seguridad)  │      │  (Backlog)   │         │   (Sprints)  │       │   (Kanban)   │      │    (Bugs)    │    │(Portal Cliente) │
└───────┬──────┘      └───────┬──────┘         └───────┬──────┘       └───────┬──────┘      └───────┬──────┘    └────────┬────────┘
        │                     │                        │                      │                     │                    │
 - Usuarios            - Proyectos              - Planificar Sprints   - Tablero Kanban       - Reporte Bugs     - Solo lectura avance %
 - Roles / RBAC        - Epics / Backlog        - Burndown Chart       - Imputar Horas        - Re-prueba QA     - Dejar Comentarios
 - Logs Auditoría      - Priorización           - Capacidad Equipo     - Desglose Tareas      - Bloqueo US
                                           
```

---

## 1. Niveles de Permiso (CRUD)
Para mayor claridad en la matriz, se definen las siguientes siglas de acceso:
* **C (Create):** Crear nuevos registros.
* **R (Read):** Leer o visualizar información.
* **U (Update):** Actualizar o modificar registros existentes.
* **D (Delete):** Eliminar o dar de baja registros.
* **N/A:** Sin acceso al módulo.

---

## 2. Matriz RBAC (Control de Acceso Basado en Roles)

| Módulo Crítico | Administrador | Product Owner / PM | Scrum Master | Desarrollador | QA Tester | Cliente |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: |
| **Seguridad y RBAC** | CRUD | R | R | N/A | N/A | N/A |
| **Logs de Auditoría** | CRUD | N/A | N/A | N/A | N/A | N/A |
| **Gestión de Proyectos** | CRUD | CRUD | R | R | R | R (Solo lectura) |
| **Epics y Product Backlog** | CRUD | CRUD | RU | R | R | R (Solo lectura) |
| **Planificación de Sprints**| CRUD | RU | CRUD | R | R | N/A |
| **Tablero Kanban / Tareas** | CRUD | R | RU | CRUD | RU | R (Solo lectura) |
| **Imputación de Horas** | CRUD | R | R | CRUD | CRUD | N/A |
| **Control de Erreurs / Bugs**| CRUD | R | RU | RU | CRUD | N/A |
| **Portal del Cliente** | CRUD | RU | R | N/A | N/A | RU (Comentarios) |

---

## 3. Detalle Operativo por Perfil

### 🛡️ Administrador (Seguridad)
* **Usuarios y Roles:** Alta, baja y modificación de cuentas de usuario y perfiles RBAC.
* **Auditoría:** Monitoreo exclusivo de logs de transacciones, accesos ilegítimos y cambios de configuración.
* **Mantenimiento:** Soporte global sobre la consistencia de la base de datos `SGPS_DB`.

### 👑 Product Owner / Project Manager (Backlog & Proyectos)
* **Proyectos:** Definición de objetivos, presupuestos de horas y alcance inicial del software.
* **Product Backlog:** Creación, refinamiento y priorización estricta de Epics e Historias de Usuario (US).
* **Validación:** Aceptación final de las funcionalidades entregadas en el sprint.

### 🔄 Scrum Master (Sprints)
* **Ciclos de Trabajo:** Creación y apertura de Sprints, asignación de la capacidad del equipo en horas.
* **Métricas:** Monitoreo activo de gráficos de rendimiento como el *Burndown Chart* y velocidad del equipo.
* **Facilitación:** Modificación de prioridades operativas en el tablero en coordinación con el PO.

### 💻 Desarrollador (Kanban & Ejecución)
* **Tablero Kanban:** Autoasignación y arrastre de tareas (*To Do*, *In Progress*, *Done*).
* **Desglose Técnico:** Fragmentación de Historias de Usuario en subtareas técnicas reutilizables.
* **Esfuerzo:** Registro diario de horas invertidas (imputación horaria) para el control de desviaciones.

### 🧪 QA Tester (Calidad & Bugs)
* **Ciclo de Defectos:** Apertura de reportes de bugs con evidencias, criticidad y pasos de reproducción.
* **Re-prueba:** Validación y cierre definitivo de errores corregidos por desarrollo.
* **Bloqueos:** Facultad de bloquear el paso a producción de Historias de Usuario que no cumplan criterios de aceptación.

### 🤝 Cliente (Portal Externo)
* **Visibilidad:** Visualización transparente del porcentaje de avance general del proyecto.
* **Feedback:** Adición de comentarios puntuales dentro de los entregables liberados en su portal.
