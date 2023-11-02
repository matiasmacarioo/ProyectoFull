using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Proyecto.Controllers
{

    [Authorize]
    public class TareasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public TareasController(ApplicationDbContext context, ApplicationDbContext contextUsuario, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            _context = context;
            _userManager = userManager;
            _rolManager = rolManager;
        }

        public async Task<IActionResult> Index()
        {
            var tareas = _context.Tareas?.Include(a => a.Asignatura).OrderByDescending(a => a.Fecha).ToList();
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario != null)
            {
                // Encontrar alumno por su campo IdentityID igual al id del usuario identity.
                var alumno = _context.Alumnos.Where(a => a.IdentityID == usuario.Id).FirstOrDefault();
                // Cuscar tareas de la carrera del alumno que inició sesión.
                if (alumno != null)
                {
                    tareas =  _context.Tareas?.Include(a => a.Asignatura).OrderByDescending(a => a.Fecha).Where(t => t.Asignatura.CarreraID == alumno.CarreraID).ToList();
                }
            }
            var asignaturas = _context.Asignaturas?.ToList();
            ViewBag.AsignaturasList = new SelectList(asignaturas, "AsignaturaID", "Nombre");
            ViewBag.UsuariosList = new SelectList(_context.Users, "Id", "Email");

            return View(tareas);
        }

        [Authorize(Roles = "Profesor,Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create(string titulo, string descripcion, DateTime fecha, DateTime fechaVencimiento, int asignaturaID, string usuarioID)
        {
            var tarea = new Tarea { Titulo = titulo, Descripcion = descripcion, Fecha = fecha, FechaVencimiento = fechaVencimiento, AsignaturaID = asignaturaID, UsuarioID = usuarioID };
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Profesor,Administrador")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string titulo, string descripcion, DateTime fecha, DateTime fechaVencimiento, int asignaturaID, string usuarioID)
        {

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                tarea.Titulo = titulo;
                tarea.Descripcion = descripcion;
                tarea.Fecha = fecha;
                tarea.FechaVencimiento = fechaVencimiento;
                tarea.AsignaturaID = asignaturaID;
                tarea.UsuarioID = usuarioID;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Profesor,Administrador")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}