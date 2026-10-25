# Estado de verificación

## Verificaciones realizadas

- Auditoría de código y configuración: pasó `tools/validar_estructura.py`.
- Compilación del proyecto C# clásico: pasó MSBuild con referencias oficiales de .NET Framework 4.7.2 y dependencias MVC 5.2.9.
- Análisis de las nueve plantillas `.cshtml`: pasó con el motor Razor 3.2.9, el utilizado por MVC 5. Para este control se transformó la directiva MVC `@model` en el tipo base equivalente antes de generar C#.
- Compilación del C# generado de esas nueve plantillas: pasó contra las referencias de .NET Framework y MVC 5.
- Revisión visual de la vista previa del diseño: realizada por separado de la aplicación.

## Verificaciones pendientes en Windows

Este equipo es macOS y no cuenta con Windows, .NET Framework ni IIS Express. Por ello no se ejecutó el CRUD web real ni se obtuvieron capturas de operaciones reales de ASP.NET en este entorno. Compilar C# y Razor no certifica el ciclo de vida de sesiones en IIS.

La guía `PRUEBAS-Y-CAPTURAS.md` incluye la secuencia para validar y capturar cada operación. `tools/probar_crud.py` comprueba la aplicación por HTTP cuando ya esté iniciada en IIS Express.

Las imágenes en `Vistas-previas` están identificadas como **VISTA PREVIA**. Muestran el diseño con ejemplos; no son evidencia de CRUD funcionando. Deben mantenerse separadas de las capturas reales solicitadas por el laboratorio.
