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
    public class AlumnoAsignaturasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public AlumnoAsignaturasController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _rolManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var AlumnoAsignaturas = _context.AlumnoAsignaturas?.Include(a => a.Alumno).Include(a => a.Asignatura).ToList();
            return View(AlumnoAsignaturas);
        }
    }
}