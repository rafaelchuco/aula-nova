PRAGMA journal_mode=WAL;
CREATE TABLE IF NOT EXISTS alumnos (
id INTEGER PRIMARY KEY AUTOINCREMENT,
nombres TEXT NOT NULL CHECK(length(trim(nombres)) BETWEEN 1 AND 60),
apellidos TEXT NOT NULL CHECK(length(trim(apellidos)) BETWEEN 1 AND 60),
documento TEXT NOT NULL UNIQUE CHECK(length(documento) BETWEEN 8 AND 12 AND documento NOT GLOB '*[^0-9]*'),
correo TEXT NOT NULL COLLATE NOCASE UNIQUE CHECK(length(correo) BETWEEN 3 AND 120),
telefono TEXT,
carrera TEXT NOT NULL CHECK(carrera IN ('Ingeniería de Sistemas','Administración','Contabilidad','Diseño Digital','Marketing')),
ciclo INTEGER NOT NULL CHECK(ciclo BETWEEN 1 AND 12),
estado TEXT NOT NULL DEFAULT 'Activo' CHECK(estado IN ('Activo','Egresado','Inactivo')),
observaciones TEXT CHECK(length(observaciones)<=300),
fecha_registro TEXT NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ','now')));
CREATE INDEX IF NOT EXISTS ix_alumnos_carrera ON alumnos(carrera);
CREATE INDEX IF NOT EXISTS ix_alumnos_estado ON alumnos(estado);
PRAGMA user_version=1;
