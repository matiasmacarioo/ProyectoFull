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
}
