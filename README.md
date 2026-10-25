# Aula Nova — Laboratorio CRUD de alumnos

**Semana 09 · Sección B · ASP.NET MVC 5 sobre .NET Framework 4.7.2 · C# · Sesiones**

Aplicación para el laboratorio con un diseño renovado: navegación superior, portada azul, ilustraciones en CSS, resumen del directorio, listado reordenado, búsqueda, ficha de alumno y formularios adaptables.

## Abrir en Visual Studio (Windows)

1. En Visual Studio Installer, habilita **ASP.NET and web development**, **.NET Framework project and item templates** y las herramientas de desarrollo de **.NET Framework 4.7.2**.
2. Extrae el ZIP completo en una carpeta local. Abre `LaboratorioCRUDAlumnos.sln`.
3. Haz clic derecho en la solución y selecciona **Restaurar paquetes NuGet**. La restauración utiliza `packages.config` y descarga cuatro dependencias MVC/WebPages.
4. Si fuese necesario, establece `LaboratorioCRUDAlumnos` como proyecto de inicio.
5. Compila la solución y pulsa **Ctrl+F5** con **IIS Express**. La URL configurada es `http://localhost:51109/`.
6. Aparecerán tres alumnos de ejemplo. Registra, consulta, edita y elimina desde el directorio.

Si falta el targeting pack 4.7.2, instálalo desde Visual Studio Installer. No cambies el proyecto a ASP.NET Core.

## Cumplimiento de la consigna

| Requisito | Implementación |
| --- | --- |
| MVC sobre .NET Framework | Proyecto clásico de Visual Studio, MVC 5.2.9, .NET Framework 4.7.2 |
| Sin autenticación | `authentication mode="None"`; IIS Express con autenticación anónima |
| Cinco campos exactos | `Id` int, `Nombres` string, `Apellidos` string, `Edad` int, `Correo` string |
| Estructura MVC | `Models/AlumnoModel.cs`, `Controllers/AlumnoController.cs`, `Views/Alumno` |
| CRUD completo | `Index`, `Details`, `Create`, `Edit` y `Delete` |
| GET / POST | GET muestra pantallas; POST crea, actualiza y elimina |
| Confirmación para eliminar | GET Delete presenta al alumno; solo POST realiza la eliminación |
| Persistencia temporal | La colección `List<AlumnoModel>` vive en `Session["alumnos"]` |
| Inicialización | Se cargan tres ejemplos solo si `Session["alumnos"] == null` |
| Restricciones | Sin base de datos, EF, archivos de alumnos ni listas estáticas |
| Entrega | Solución, código, guía de pruebas y evidencia del uso de sesiones |

## Datos y validación

El identificador se asigna automáticamente y se conserva al editar. Nombres y apellidos son obligatorios, hasta 60 caracteres; edad entre 1 y 120; correo obligatorio y con formato válido, hasta 120 caracteres. La validación principal se ejecuta en el servidor. HTML añade ayuda de validación al completar los formularios.

La búsqueda filtra la vista, sin reemplazar la colección guardada en sesión. Las tarjetas solo calculan valores a partir de los cinco campos existentes: total, mayores de edad y edad promedio.

## Cómo funciona la sesión

El controlador inicializa y recupera la lista en `ObtenerAlumnos()`. Crear, editar y eliminar vuelven a asignar la lista a `Session["alumnos"]`. La configuración es **InProc**, con 20 minutos de inactividad y cookie de sesión.

Actualizar el navegador conserva los cambios mientras la sesión siga vigente. Un navegador independiente tiene otra sesión y recibe sus propios ejemplos. Al expirar la sesión o reiniciar IIS Express se pierde el almacenamiento temporal. Borrar todos los alumnos mantiene la lista vacía; los ejemplos no se vuelven a insertar hasta iniciar otra sesión.

Consulta `docs/SESIONES.md` para la evidencia del código y `docs/PRUEBAS-Y-CAPTURAS.md` para completar las pruebas y capturas solicitadas.

## Verificación

- El proyecto C# se puede compilar con referencias de .NET Framework sin ejecutar ASP.NET.
- La ejecución web y la compilación de vistas con IIS Express requieren Windows / .NET Framework; el SDK de .NET moderno en macOS no sustituye ese entorno.
- Auditoría documental opcional, con Python 3: `python tools/validar_estructura.py`.
- Prueba HTTP opcional, después de iniciar el proyecto en Windows: `python tools/probar_crud.py http://localhost:51109`.

Las capturas de una vista previa del diseño, si se incluyen, son únicamente material visual. No acreditan el funcionamiento de las sesiones ni reemplazan capturas reales del laboratorio en IIS Express.

## Cambios respecto a la versión anterior

Esta versión ajusta el proyecto al laboratorio entregado: se retiraron ASP.NET Core, SQLite y los campos adicionales. Se recuperaron ASP.NET MVC / .NET Framework, el campo Edad y las sesiones. El diseño se renovó conservando todas las operaciones solicitadas.

Base original: [CrudAumnos](https://github.com/betuelarones-arch/CrudAumnos), revisión `89a77919a1ccf74c3eed83c3871561aa2cfc32be`. La copia local no contiene `.git`. La actualización de GitHub conserva el historial del repositorio independiente Aula Nova.
