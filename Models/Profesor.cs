using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Profesor
    {
        [Key]
        public int ProfesorID { get; set; }
        public string? Nombre { get; set; }
        public int? Documento { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public DateTime FechaNacimiento { get; set; }

        [NotMapped]
        public string FechaString { get => FechaNacimiento.ToString("yyyy-MM-dd");}
    
        [ForeignKey("Carrera")]
        public int CarreraID { get; set; }
        public Carrera? Carrera { get; set; }
        public ICollection<ProfesorAsignatura>? ProfesorAsignaturas { get; set; }
        public string? IdentityID { get; set; }
        public IdentityUser? IdentityUser { get; set; } // Propiedad de navegación al usuario de Identity

    }
}