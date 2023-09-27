using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;

namespace Proyecto.Controllers
{
    public class ProfesoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfesoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var profesores = _context.Profesores?.Include(a => a.Carrera).OrderBy(a => a.Nombre).ToList();
            var carreras = _context.Carreras?.ToList();
            ViewBag.CarrerasList = new SelectList(carreras, "CarreraID", "Nombre");
            return View(profesores);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombre, DateTime fechaNacimiento, int carreraID, string direccion, int documento)
        {
            var profesorExiste = await _context.Profesores.AnyAsync(a => a.Documento == documento);
            if (!profesorExiste){
                var profesor = new Profesor { Nombre = nombre, FechaNacimiento = fechaNacimiento, CarreraID = carreraID, Direccion = direccion, Documento = documento };
                _context.Profesores.Add(profesor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else{
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombre, DateTime fechaNacimiento, int carreraID, string direccion, int documento)
        {

            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor != null)
            {
                profesor.Nombre = nombre;
                profesor.FechaNacimiento = fechaNacimiento;
                profesor.CarreraID = carreraID;
                profesor.Direccion = direccion;
                var profesorExiste = await _context.Profesores.AnyAsync(a => a.Documento == documento);
                if (!profesorExiste){
                    profesor.Documento = documento;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor != null)
            {
                _context.Profesores.Remove(profesor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}