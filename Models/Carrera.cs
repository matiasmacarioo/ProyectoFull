using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Proyecto.Models
{
    public class Carrera
    {
        [Key]
        public int CarreraID { get; set; }
        public string? Nombre { get; set; }
        public int Duracion { get; set; }
    }
}