using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Asignatura
    {
        [Key]
        public int AsignaturaID { get; set; }
        public string? Nombre { get; set; }

        [ForeignKey("Carrera")]
        public int CarreraID { get; set; }
        public Carrera? Carrera { get; set; }
    }
}