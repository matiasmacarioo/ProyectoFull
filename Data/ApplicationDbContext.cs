using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Proyecto.Models.Carrera>? Carreras { get; set; }
    public DbSet<Proyecto.Models.Alumno>? Alumnos { get; set; }
    public DbSet<Proyecto.Models.Profesor>? Profesores { get; set; }
    public DbSet<Proyecto.Models.Asignatura>? Asignaturas { get; set; }
    public DbSet<Proyecto.Models.ProfesorAsignatura>? ProfesorAsignaturas { get; set; }
    public DbSet<Proyecto.Models.Tarea>? Tareas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

}
