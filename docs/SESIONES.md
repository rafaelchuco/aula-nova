# Evidencia de inicialización y uso de sesiones

El código está en `LaboratorioCRUDAlumnos/Controllers/AlumnoController.cs`.

## Inicialización, una sola vez por sesión

```csharp
private List<AlumnoModel> ObtenerAlumnos()
{
    if (Session["alumnos"] == null)
    {
        Session["alumnos"] = new List<AlumnoModel>
        {
            new AlumnoModel { Id = 1, Nombres = "Ana María", Apellidos = "García López", Edad = 19, Correo = "ana.garcia@example.com" },
            new AlumnoModel { Id = 2, Nombres = "Carlos", Apellidos = "Ramírez Torres", Edad = 21, Correo = "carlos.ramirez@example.com" },
            new AlumnoModel { Id = 3, Nombres = "Lucía", Apellidos = "Fernández Ruiz", Edad = 20, Correo = "lucia.fernandez@example.com" }
        };
    }
    return (List<AlumnoModel>)Session["alumnos"];
}
```

La condición revisa `null`, no la cantidad de alumnos. Por eso no sobrescribe cambios ni vuelve a insertar ejemplos cuando se borra el último alumno.

## Lectura y escritura

Cada acción recupera la colección con `ObtenerAlumnos()`. Después de cada operación POST válida, se conserva la lista:

```csharp
// Create
alumno.Id = alumnos.Count == 0 ? 1 : alumnos.Max(a => a.Id) + 1;
alumnos.Add(alumno);
Session["alumnos"] = alumnos;

// Edit
existente.Nombres = alumno.Nombres;
existente.Apellidos = alumno.Apellidos;
existente.Edad = alumno.Edad;
existente.Correo = alumno.Correo;
Session["alumnos"] = alumnos;

// DeleteConfirmed
alumnos.Remove(alumno);
Session["alumnos"] = alumnos;
```

Las tres acciones están marcadas con `[HttpPost]` y `[ValidateAntiForgeryToken]`. Los formularios emiten `@Html.AntiForgeryToken()`.

## Configuración de ASP.NET

En `LaboratorioCRUDAlumnos/Web.config`:

```xml
<sessionState mode="InProc" timeout="20" cookieless="UseCookies" />
```

Los alumnos permanecen en memoria, asociados a la sesión del visitante. No se utilizan archivos, variables estáticas, Entity Framework ni conexiones a bases de datos para almacenar la colección.
