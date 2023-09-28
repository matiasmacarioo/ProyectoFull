using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class ProfesorAsignatura
    {
        [Key]
        public int ProfesorAsignaturaID { get; set; }
        public int ProfesorID { get; set; }
        public int AsignaturaID { get; set; }
        public Profesor? Profesor { get; set; }
        public Asignatura? Asignatura { get; set; }
    }
}