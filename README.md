# Aula Nova

Sistema de gestión de alumnos en español: ASP.NET Core MVC (.NET 10) y SQLite. Diseño adaptable con directorio, indicadores, búsqueda, filtros y fichas académicas.

## Ejecutar

Requiere el SDK de .NET 10. Desde la raíz del proyecto:

```sh
dotnet restore demo10/demo10.csproj
dotnet run --project demo10/demo10.csproj
```

Abre http://localhost:5088. La base de datos se crea automáticamente en `demo10/App_Data/aula-nova.db`. Los datos persisten al cerrar el navegador y reiniciar el servidor. El sistema inicia vacío.

Para cargar seis alumnos ficticios de demostración, con la aplicación iniciada al menos una vez y el comando `sqlite3` instalado:

```sh
sqlite3 demo10/App_Data/aula-nova.db < demo10/Database/seed.sql
```

## Funciones y campos

- Crear, consultar, editar y eliminar alumnos con confirmación.
- Nombres, apellidos, documento, correo, teléfono, carrera, ciclo, estado y observaciones.
- Documento de 8 a 12 dígitos y correo únicos; ciclos del 1 al 12.
- Estados Activo, Egresado e Inactivo. Catálogo de cinco carreras.
- Búsqueda por nombre, documento o correo; filtros combinables por carrera y estado.
- Indicadores globales de alumnos, activos, egresados y carreras con alumnos.
- Validación en servidor, consultas parametrizadas y tokens antifalsificación en las operaciones que modifican datos.

## Base de datos

El esquema versionado está en `demo10/Database/001_initial.sql`. `seed.sql` es opcional y usa exclusivamente datos ficticios. Los archivos de datos locales se excluyen de GitHub. La carpeta de datos debe tener permisos de escritura.

Puedes cambiar la ruta con la variable `Database__Path` o la configuración `Database:Path` de `appsettings.json`. Usa una ruta persistente para que un despliegue no borre la base de datos. Para hacer una copia de seguridad consistente con la aplicación en ejecución:

```sh
sqlite3 demo10/App_Data/aula-nova.db ".backup '/ruta/segura/aula-nova-backup.db'"
```

## Verificar

```sh
dotnet build demo10/demo10.csproj -c Release
python3 tests/smoke.py
```

La prueba usa una base de datos temporal independiente y comprueba el CRUD por HTTP, persistencia tras reiniciar, duplicados, validación, filtros, CSRF, codificación HTML y respuestas 404. GitHub Actions ejecuta estas verificaciones en cada cambio.

## Publicar la aplicación

```sh
dotnet publish demo10/demo10.csproj -c Release -o publish
cd publish
ASPNETCORE_URLS=http://127.0.0.1:5088 dotnet AulaNova.dll
```

El esquema SQL se incluye en la publicación. Para un dominio distinto de localhost, configura `AllowedHosts` con el dominio real y usa HTTPS mediante el servidor de alojamiento. Esta versión es un CRUD local de demostración sin autenticación: antes de usar datos reales en un servidor accesible públicamente, añade acceso de usuarios y autorización.

## Origen y cambios

Adaptado del proyecto [CrudAumnos](https://github.com/betuelarones-arch/CrudAumnos), revisión `89a77919a1ccf74c3eed83c3871561aa2cfc32be`. Se eliminó el `.git` del clon y se preparó un proyecto independiente con historial nuevo. Se conserva la referencia de origen.

Se migró ASP.NET MVC 5 / .NET Framework 4.7.2 a ASP.NET Core MVC / .NET 10 para ejecutarlo en Windows, macOS y Linux. El almacenamiento temporal en sesión fue reemplazado por una base de datos SQLite. Se sustituyó el campo edad por un perfil académico más completo y se renovaron las vistas, estilos, navegación y formularios.

Referencia técnica del proveedor: [Microsoft.Data.Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/).
