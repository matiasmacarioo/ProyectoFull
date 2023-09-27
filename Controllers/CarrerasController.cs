using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;

namespace TuProyecto.Controllers
{
    public class CarrerasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarrerasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var carreras = await _context.Carreras.OrderBy(c => c.Nombre).ThenBy(c => c.Duracion).ToListAsync();
            return View(carreras);
        }

         [HttpPost]
        public async Task<IActionResult> Create(string nombre, int duracion)
        {
            var carrera = new Carrera { Nombre = nombre, Duracion = duracion };
            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombre, int duracion)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera != null)
            {
                carrera.Nombre = nombre;
                carrera.Duracion = duracion;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera != null)
            {
                _context.Carreras.Remove(carrera);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}