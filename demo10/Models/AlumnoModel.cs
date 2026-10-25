using System.ComponentModel.DataAnnotations;
namespace AulaNova.Models;

public class AlumnoModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ingresa los nombres."), StringLength(60), Display(Name = "Nombres")]
    public string Nombres { get; set; } = "";
    [Required(ErrorMessage = "Ingresa los apellidos."), StringLength(60), Display(Name = "Apellidos")]
    public string Apellidos { get; set; } = "";
    [Required, RegularExpression(@"[0-9]{8,12}", ErrorMessage = "Usa entre 8 y 12 dígitos."), Display(Name = "Documento de identidad")]
    public string Documento { get; set; } = "";
    [Required, EmailAddress(ErrorMessage = "Ingresa un correo válido."), StringLength(120), Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = "";
    [RegularExpression(@"[+0-9 ()-]{7,20}", ErrorMessage = "Ingresa un teléfono válido."), Display(Name = "Teléfono")]
    public string? Telefono { get; set; }
    [Required, Display(Name = "Carrera")]
    public string Carrera { get; set; } = "Ingeniería de Sistemas";
    [Range(1, 12), Display(Name = "Ciclo académico")]
    public int Ciclo { get; set; } = 1;
    [Required, RegularExpression("^(Activo|Egresado|Inactivo)$"), Display(Name = "Estado")]
    public string Estado { get; set; } = "Activo";
    [StringLength(300), Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }
    public string FechaRegistro { get; set; } = "";
    public static readonly string[] Carreras = ["Ingeniería de Sistemas", "Administración", "Contabilidad", "Diseño Digital", "Marketing"];
}
public class AlumnosIndexModel
{
    public IReadOnlyList<AlumnoModel> Alumnos { get; set; } = [];
    public string? Buscar { get; set; }
    public string? Carrera { get; set; }
    public string? Estado { get; set; }
    public int Total { get; set; }
    public int Activos { get; set; }
    public int Egresados { get; set; }
    public int CarrerasRegistradas { get; set; }
}
