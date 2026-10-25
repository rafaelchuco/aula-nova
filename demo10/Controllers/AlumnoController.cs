using AulaNova.Data;
using AulaNova.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
namespace AulaNova.Controllers;

public class AlumnoController(AlumnoRepository repository) : Controller
{
    public IActionResult Index(string? buscar, string? carrera, string? estado)
    {
        var all = repository.All();
        var filtered = all.Where(a => (string.IsNullOrWhiteSpace(buscar) || $"{a.Nombres} {a.Apellidos} {a.Documento} {a.Correo}".Contains(buscar.Trim(), StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(carrera) || a.Carrera == carrera) && (string.IsNullOrEmpty(estado) || a.Estado == estado));
        return View(new AlumnosIndexModel { Alumnos = filtered.ToList(), Buscar = buscar, Carrera = carrera, Estado = estado, Total = all.Count, Activos = all.Count(a => a.Estado == "Activo"), Egresados = all.Count(a => a.Estado == "Egresado"), CarrerasRegistradas = all.Select(a => a.Carrera).Distinct().Count() });
    }
    public IActionResult Details(int id) => Show("Details", id);
    public IActionResult Create() => View(new AlumnoModel());
    public IActionResult Edit(int id) => Show("Edit", id);
    public IActionResult Delete(int id) => Show("Delete", id);
    private IActionResult Show(string view, int id) { var a = repository.Find(id); return a == null ? NotFound() : View(view, a); }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(AlumnoModel alumno) => Save(alumno, false);
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(int id, AlumnoModel alumno) { if (repository.Find(id) == null) return NotFound(); alumno.Id = id; return Save(alumno, true); }
    private IActionResult Save(AlumnoModel alumno, bool update)
    {
        if (!AlumnoModel.Carreras.Contains(alumno.Carrera)) ModelState.AddModelError("Carrera", "Selecciona una carrera de la lista.");
        if (ModelState.IsValid)
        {
            try { if (!repository.Save(alumno, update)) return NotFound(); TempData["Success"] = update ? "Los datos del alumno se actualizaron." : "Alumno registrado correctamente."; return RedirectToAction(nameof(Index)); }
            catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 2067) { ModelState.AddModelError("", "Ya existe un alumno con ese documento o correo electrónico."); }
        }
        return View(update ? "Edit" : "Create", alumno);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) { if (!repository.Delete(id)) return NotFound(); TempData["Success"] = "El registro del alumno se eliminó."; return RedirectToAction(nameof(Index)); }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View("~/Views/Shared/Error.cshtml");
}
