using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;
using Microsoft.AspNetCore.Identity;

namespace Proyecto.Controllers
{
    public class ProfesorAsignaturasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public ProfesorAsignaturasController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _rolManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var ProfesorAsignaturas = _context.ProfesorAsignaturas?.Include(a => a.Profesor).Include(a => a.Asignatura).ToList();
            var carreras = _context.Carreras?.ToList();
            var asignaturas = _context.Asignaturas?.ToList();
            ViewBag.CarrerasList = new SelectList(carreras, "CarreraID", "Nombre");
            ViewBag.AsignaturasList = new SelectList(asignaturas, "AsignaturaID", "Nombre");
            return View(ProfesorAsignaturas);
        }
    }
}