using System.ComponentModel.DataAnnotations;

namespace LaboratorioCRUDAlumnos.Models
{
    public class AlumnoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresa los nombres del alumno.")]
        [StringLength(60, ErrorMessage = "Los nombres no deben superar los 60 caracteres.")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Ingresa los apellidos del alumno.")]
        [StringLength(60, ErrorMessage = "Los apellidos no deben superar los 60 caracteres.")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120 años.")]
        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "Ingresa el correo electrónico.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
        [StringLength(120, ErrorMessage = "El correo no debe superar los 120 caracteres.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }
    }
}
