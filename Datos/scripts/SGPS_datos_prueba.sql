-- ============================================================================
-- SISTEMA DE GESTIÓN DE PROYECTOS DE SOFTWARE (SGPS)
-- Script de Datos de Prueba: Roles y Usuarios
-- Requisito previo: ejecutar SGPS_crea.sql
-- El script es re-ejecutable (no duplica filas si ya existen).
--
-- SOLO PARA ENTORNOS LOCALES DE DESARROLLO Y PRUEBAS.
-- Todos los usuarios comparten la contraseña: Sgps.2026
-- Los valores de PasswordHash y PasswordSalt se generaron con
-- Logica.UsuarioLN (PBKDF2-SHA256, 100000 iteraciones, salt de 16 bytes,
-- hash de 32 bytes, ambos en Base64). Cada usuario tiene su propio salt,
-- por eso dos usuarios con la misma contraseña tienen hashes distintos.
-- ============================================================================

USE SGPS_DB;
GO

-- ----------------------------------------------------------------------------
-- Roles del sistema
-- Los nombres 'Cliente' y 'Developer' los usa el menú dinámico de Principal.cs.
-- ----------------------------------------------------------------------------
INSERT INTO dbo.Roles (NombreRol, Descripcion)
SELECT v.NombreRol, v.Descripcion
FROM (VALUES
    ('Administrador', 'Administra usuarios, proyectos y configuración del sistema'),
    ('ProductOwner',  'Gestiona el Product Backlog y prioriza las historias de usuario'),
    ('ScrumMaster',   'Facilita el proceso Scrum y administra los sprints'),
    ('Developer',     'Implementa historias de usuario y tareas técnicas'),
    ('QA',            'Verifica historias y reporta bugs'),
    ('Cliente',       'Consulta el avance del proyecto desde el portal de cliente')
) AS v (NombreRol, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Roles AS r WHERE r.NombreRol = v.NombreRol);
GO

-- ----------------------------------------------------------------------------
-- Usuarios de prueba (contraseña: Sgps.2026)
-- 'inactivo@sgps.local' tiene EsActivo = 0 para probar el rechazo de cuentas
-- desactivadas en el inicio de sesión.
-- ----------------------------------------------------------------------------
INSERT INTO dbo.Usuarios (Nombres, Apellidos, Email, PasswordHash, PasswordSalt, EsActivo)
SELECT v.Nombres, v.Apellidos, v.Email, v.PasswordHash, v.PasswordSalt, v.EsActivo
FROM (VALUES
    ('Ana',    'Quispe',   'admin@sgps.local',    'zg/21l6KM9832IH42Qt90HmGFNl6TGo4ktJ6B4bfNuE=', '7+9J4+QfjKUBcJ8WrXRMVw==', 1),
    ('Bruno',  'Salazar',  'po@sgps.local',       '/BJbYViejHLkaebIyfuYYtQ6Kxhm7Hg0nOsKHXX2OhM=', 'PGr3dhQyBbshnRGztuSgeg==', 1),
    ('Carla',  'Mendoza',  'sm@sgps.local',       'io2s79hYFA2YHq0un7dPqJu7o/Vdfr2mepymXWgJxuM=', 'PYtJJpjNsSRyZthSiVnwlA==', 1),
    ('Diego',  'Rojas',    'dev@sgps.local',      'jV8moRmLwSf4liD1op74Wzks/kN/c7Gdra37o2urKyM=', 'k7OwB04wwXZoasSRF2IArg==', 1),
    ('Elena',  'Vargas',   'qa@sgps.local',       '+CbaE1fUlxoAtE2FXK+mx45UDPwDb03ogSTAuaWFESk=', 'IK25m7HaxGGzpyiMDbcbIg==', 1),
    ('Fabio',  'Herrera',  'cliente@sgps.local',  'X5hkZsMOpRfWjoFNC5JjpVlNw/BDRUeb6GqRVu+uIkc=', '0nTntGVfjNsIf3QDtGCMDA==', 1),
    ('Gloria', 'Paredes',  'inactivo@sgps.local', 'C4bDV06C1BgOEVH0bi42sQQksVY7YQ45Sapk0Cu3VPs=', 'EHuN2v8CFz2pKWASrc7ykA==', 0)
) AS v (Nombres, Apellidos, Email, PasswordHash, PasswordSalt, EsActivo)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Usuarios AS u WHERE u.Email = v.Email);
GO

-- ----------------------------------------------------------------------------
-- Asignación de roles a los usuarios de prueba
-- ----------------------------------------------------------------------------
INSERT INTO dbo.UsuarioRoles (UsuarioId, RolId)
SELECT u.UsuarioId, r.RolId
FROM (VALUES
    ('admin@sgps.local',    'Administrador'),
    ('po@sgps.local',       'ProductOwner'),
    ('sm@sgps.local',       'ScrumMaster'),
    ('dev@sgps.local',      'Developer'),
    ('qa@sgps.local',       'QA'),
    ('cliente@sgps.local',  'Cliente'),
    ('inactivo@sgps.local', 'Developer')
) AS v (Email, NombreRol)
INNER JOIN dbo.Usuarios AS u ON u.Email = v.Email
INNER JOIN dbo.Roles AS r ON r.NombreRol = v.NombreRol
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.UsuarioRoles AS ur
    WHERE ur.UsuarioId = u.UsuarioId AND ur.RolId = r.RolId
);
GO
