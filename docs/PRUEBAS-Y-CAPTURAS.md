# Guía de pruebas y capturas — Semana 09

Las evidencias funcionales deben obtenerse ejecutando `LaboratorioCRUDAlumnos.sln` en Windows, con Visual Studio e IIS Express. Esta guía no afirma que las pruebas web se hayan ejecutado en macOS.

## Secuencia para completar la entrega

| Paso | Acción | Resultado esperado | Nombre sugerido de la captura |
| --- | --- | --- | --- |
| 1 | Inicia el proyecto en una sesión nueva | Se ven Ana María, Carlos y Lucía | `01-primer-ingreso.png` |
| 2 | Pulsa Registrar alumno, completa todos los campos y guarda | Mensaje de confirmación y cuarto alumno en el directorio | `02-crear-alumno.png` |
| 3 | Abre los detalles del alumno nuevo | La ficha muestra exactamente Id, Nombres, Apellidos, Edad y Correo | `03-detalles.png` |
| 4 | Entra a Editar, modifica edad o correo y guarda | El directorio refleja el cambio | `04-editar-alumno.png` |
| 5 | Actualiza el navegador con F5 | El alumno y sus cambios se mantienen | `05-persistencia-sesion.png` |
| 6 | Pulsa Eliminar | Pantalla con el alumno y los botones conservar/eliminar | `06-confirmar-eliminacion.png` |
| 7 | Confirma la eliminación | El alumno desaparece del directorio | `07-eliminado.png` |
| 8 | Abre AlumnoController.cs en Visual Studio | Se ve el método ObtenerAlumnos con Session["alumnos"] | `08-codigo-sesiones.png` |

En cada captura deja visible el navegador y su URL local. Puedes usar Win+Shift+S y guardar la imagen PNG. Coloca las imágenes reales en una carpeta `Capturas` junto a la solución antes de comprimir tu entrega final.

## Casos adicionales

- Edad 0 o 121: debe rechazarse y conservar lo que ya había en sesión.
- Nombre vacío o correo incorrecto: debe aparecer un error de validación.
- GET de Delete sin confirmar: el registro debe seguir existiendo.
- Abrir una ventana privada: debe recibir una colección independiente con los tres ejemplos.
- Borrar los tres ejemplos: el listado queda vacío y sigue vacío al actualizar.
- Registrar desde un listado vacío: debe funcionar y asignar un Id válido.
- Consultar un Id que no existe: debe devolver HTTP 404.
- Reiniciar IIS Express: al tratarse de sesiones InProc, los cambios temporales se pierden y se cargan de nuevo los ejemplos.

## Prueba HTTP opcional

Con Python 3 instalado y la aplicación ya iniciada:

```powershell
python tools/probar_crud.py http://localhost:51109
```

El script utiliza cookies propias y comprueba el CRUD, el refresco, la separación de sesiones y la lista vacía. No utiliza ni modifica la sesión de tu navegador. No genera capturas; estas se deben tomar en el navegador.

## Revisar los 20 puntos de la rúbrica

- [ ] Proyecto clásico MVC / .NET Framework y solución Visual Studio (2).
- [ ] Modelo con los cinco campos y estructura MVC (3).
- [ ] Listado y detalles (3).
- [ ] Registro válido e inválido (3).
- [ ] Edición y actualización (3).
- [ ] Confirmación y eliminación (3).
- [ ] Sesión inicializada una vez, cambios tras F5 y evidencia del código (3).

Solo marca cada casilla después de comprobarla en Windows. Conserva las capturas reales con el ZIP del proyecto.
