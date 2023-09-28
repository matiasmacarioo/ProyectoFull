using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Tarea
    {
        [Key]
        public int TareaID { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        
        [NotMapped]
        public string FechaString { get => Fecha.ToString("yyyy-MM-dd");}

        public DateTime FechaVencimiento { get; set; }

        [NotMapped]
        public string FechaVencimientoString { get => FechaVencimiento.ToString("yyyy-MM-dd");}

        // public bool Realizada { get; set; }
        // public enum Prioridad { Baja, Media, Alta }
        public string? UsuarioID { get; set; }
        public IdentityUser? Usuario { get; set; }

        [ForeignKey("Asignatura")]
        public int AsignaturaID { get; set; }
        public Asignatura? Asignatura { get; set; }
    }
}