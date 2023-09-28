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
            var asignaturas = _context.Asignaturas?.ToList();
            ViewBag.CarrerasList = new SelectList(carreras, "CarreraID", "Nombre");
            ViewBag.AsignaturasList = new SelectList(asignaturas, "AsignaturaID", "Nombre");
            return View(profesores);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombre, DateTime fechaNacimiento, int carreraID, string direccion, int documento, string email)
        {
            var profesorExiste = await _context.Profesores.AnyAsync(a => a.Documento == documento);
            if (!profesorExiste)
            {
                var profesor = new Profesor { Nombre = nombre, FechaNacimiento = fechaNacimiento, CarreraID = carreraID, Direccion = direccion, Documento = documento, Email = email };
                _context.Profesores.Add(profesor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombre, DateTime fechaNacimiento, int carreraID, string direccion, int documento, string email)
        {

            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor != null)
            {
                profesor.Nombre = nombre;
                profesor.FechaNacimiento = fechaNacimiento;
                profesor.CarreraID = carreraID;
                profesor.Direccion = direccion;
                profesor.Email = email;
                var profesorExiste = await _context.Profesores.AnyAsync(a => a.Documento == documento);
                if (!profesorExiste)
                {
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

        [HttpPost]
        public async Task<IActionResult> AsociarAsignaturas(int profesorID, List<int> asignaturas)
        {
            try
            {
                var profesor = await _context.Profesores.FindAsync(profesorID);
                if (profesor == null)
                {
                    return NotFound();
                }

                // Elimina las relaciones de asignaturas existentes para este profesor.
                var relacionesAntiguas = _context.ProfesorAsignaturas.Where(pa => pa.ProfesorID == profesorID);
                _context.ProfesorAsignaturas.RemoveRange(relacionesAntiguas);

                // Asocia las nuevas asignaturas seleccionadas.
                foreach (var asignaturaID in asignaturas)
                {
                    var profesorAsignatura = new ProfesorAsignatura
                    {
                        ProfesorID = profesorID,
                        AsignaturaID = asignaturaID
                    };

                    // Agrega la nueva relación ProfesorAsignatura a la base de datos.
                    _context.ProfesorAsignaturas.Add(profesorAsignatura);
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