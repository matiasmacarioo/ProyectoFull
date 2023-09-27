using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;

namespace TuProyecto.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlumnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = _context.Alumnos?.Include(a => a.Carrera).OrderBy(a => a.Carrera.Nombre).ThenBy(a => a.Nombre).ToList();
            var carreras = _context.Carreras?.ToList();
            ViewBag.CarrerasList = new SelectList(carreras, "CarreraID", "Nombre");
            return View(alumnos);
        }
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, DateTime fechaNacimiento, int carreraID)
        {
            var alumno = new Alumno { Nombre = nombre, FechaNacimiento = fechaNacimiento, CarreraID = carreraID };
            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombre, DateTime fechaNacimiento, int carreraID)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno != null)
            {
                alumno.Nombre = nombre;
                alumno.FechaNacimiento = fechaNacimiento;
                alumno.CarreraID = carreraID;
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
    }
}