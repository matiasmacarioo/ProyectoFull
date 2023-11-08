using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;
using Microsoft.AspNetCore.Identity;

namespace TuProyecto.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public AlumnosController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _rolManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = _context.Alumnos?.Include(a => a.Carrera).OrderBy(a => a.Carrera.Nombre).ThenBy(a => a.Nombre).ToList();
            var carreras = _context.Carreras?.ToList();
            var asignaturas = _context.Asignaturas?.ToList();

            ViewBag.AsignaturasList = new SelectList(asignaturas, "AsignaturaID", "Nombre");
            ViewBag.CarrerasList = new SelectList(carreras, "CarreraID", "Nombre");
            return View(alumnos);
        }
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, DateTime fechaNacimiento, int carreraID, string direccion, int documento, string email)
        {
            var alumnoExiste = await _context.Alumnos.AnyAsync(a => a.Documento == documento);
            var idRelacionar = string.Empty;

            if(!alumnoExiste){
                var alumno = new Alumno { Nombre = nombre, FechaNacimiento = fechaNacimiento, CarreraID = carreraID, Direccion = direccion, Documento = documento, Email = email };

                 // Verificar si el usuario de Identity ya existe
                var usuarioExiste = await _userManager.FindByEmailAsync(alumno.Email);
                if (usuarioExiste == null)
                {
                    // El usuario no existe, así que lo creamos
                    var usuario = new IdentityUser { UserName = alumno.Email, Email = alumno.Email, EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(usuario, "alumno");
                    idRelacionar = usuario.Id;
                    if (result.Succeeded)
                    {
                        // Asignar el rol de "Alumno" al nuevo alumno
                        await _userManager.AddToRoleAsync(usuario, "alumno");
                    }
                }

                 if (usuarioExiste != null)
                {
                    idRelacionar = usuarioExiste.Id;
                }

                alumno.IdentityID = idRelacionar;

                _context.Alumnos.Add(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else{
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombre, DateTime fechaNacimiento, int carreraID, string direccion, int documento, string email)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno != null)
            {
                alumno.Nombre = nombre;
                alumno.FechaNacimiento = fechaNacimiento;
                alumno.CarreraID = carreraID;
                alumno.Direccion = direccion;
                alumno.Documento = documento;
                alumno.Email = email;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno != null)
            {
                _context.Alumnos.Remove(alumno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

         [HttpPost]
        public async Task<IActionResult> AsociarAsignaturas(int alumnoID, List<int> asignaturas)
        {
            try
            {
                var alumno = await _context.Alumnos.FindAsync(alumnoID);
                if (alumno == null)
                {
                    return NotFound();
                }

                // Elimina las relaciones de asignaturas existentes para este alumno.
                var relacionesAntiguas = _context.AlumnoAsignaturas.Where(pa => pa.AlumnoID == alumnoID);
                _context.AlumnoAsignaturas.RemoveRange(relacionesAntiguas);

                // Asocia las nuevas asignaturas seleccionadas.
                foreach (var asignaturaID in asignaturas)
                {
                    var alumnoAsignatura = new AlumnoAsignatura
                    {
                        AlumnoID = alumnoID,
                        AsignaturaID = asignaturaID
                    };

                    // Agrega la nueva relación AlumnoAsignatura a la base de datos.
                    _context.AlumnoAsignaturas.Add(alumnoAsignatura);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}