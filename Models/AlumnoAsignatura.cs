using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class AlumnoAsignatura
    {
        [Key]
        public int AlumnoAsignaturaID { get; set; }
        public int AlumnoID { get; set; }
        public int AsignaturaID { get; set; }
        public Alumno? Alumno { get; set; }
        public Asignatura? Asignatura { get; set; }
    }
}