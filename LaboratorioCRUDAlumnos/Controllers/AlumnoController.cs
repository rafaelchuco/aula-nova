using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LaboratorioCRUDAlumnos.Models;

namespace LaboratorioCRUDAlumnos.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class AlumnoController : Controller
    {
        public ActionResult Index(string buscar)
        {
            var alumnos = ObtenerAlumnos();
            ViewBag.Total = alumnos.Count;
            ViewBag.Mayores = alumnos.Count(a => a.Edad >= 18);
            ViewBag.Promedio = alumnos.Count == 0 ? 0 : alumnos.Average(a => a.Edad);
            ViewBag.Buscar = buscar;

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var texto = buscar.Trim();
                return View(alumnos.Where(a =>
                    (a.Nombres + " " + a.Apellidos).IndexOf(texto, System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                    a.Correo.IndexOf(texto, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList());
            }

            return View(alumnos);
        }

        public ActionResult Details(int id)
        {
            var alumno = ObtenerAlumnos().FirstOrDefault(a => a.Id == id);
            if (alumno == null) return HttpNotFound();
            return View(alumno);
        }

        public ActionResult Create()
        {
            ObtenerAlumnos();
            return View(new AlumnoModel { Edad = 18 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombres,Apellidos,Edad,Correo")] AlumnoModel alumno)
        {
            var alumnos = ObtenerAlumnos();
            if (!ModelState.IsValid) return View(alumno);

            alumno.Id = alumnos.Count == 0 ? 1 : alumnos.Max(a => a.Id) + 1;
            Normalizar(alumno);
            alumnos.Add(alumno);
            Session["alumnos"] = alumnos;
            TempData["Mensaje"] = "El alumno fue registrado correctamente.";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var alumno = ObtenerAlumnos().FirstOrDefault(a => a.Id == id);
            if (alumno == null) return HttpNotFound();
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Nombres,Apellidos,Edad,Correo")] AlumnoModel alumno)
        {
            var alumnos = ObtenerAlumnos();
            var existente = alumnos.FirstOrDefault(a => a.Id == id);
            if (existente == null) return HttpNotFound();
            alumno.Id = id;
            if (!ModelState.IsValid) return View(alumno);

            Normalizar(alumno);
            existente.Nombres = alumno.Nombres;
            existente.Apellidos = alumno.Apellidos;
            existente.Edad = alumno.Edad;
            existente.Correo = alumno.Correo;
            Session["alumnos"] = alumnos;
            TempData["Mensaje"] = "Los datos del alumno fueron actualizados.";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var alumno = ObtenerAlumnos().FirstOrDefault(a => a.Id == id);
            if (alumno == null) return HttpNotFound();
            return View(alumno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var alumnos = ObtenerAlumnos();
            var alumno = alumnos.FirstOrDefault(a => a.Id == id);
            if (alumno == null) return HttpNotFound();

            alumnos.Remove(alumno);
            Session["alumnos"] = alumnos;
            TempData["Mensaje"] = "El alumno fue eliminado del directorio.";
            return RedirectToAction("Index");
        }

        private void Normalizar(AlumnoModel alumno)
        {
            alumno.Nombres = alumno.Nombres.Trim();
            alumno.Apellidos = alumno.Apellidos.Trim();
            alumno.Correo = alumno.Correo.Trim();
        }

        // RF03: los ejemplos se cargan una sola vez por sesión.
        // La lista vacía también es una sesión inicializada: no se repuebla al borrar todo.
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
    }
}
