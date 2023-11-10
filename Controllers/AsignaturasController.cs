using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models;
using Proyecto.Data;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto.Controllers
{
    public class AsignaturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignaturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var asignaturas = _context.Asignaturas?.Include(a => a.Carrera).OrderBy(a => a.Nombre).ToList();
            var carreras = _context.Carreras?.ToList();
            var alumnos = _context.Alumnos?.OrderBy(a => a.Carrera.Nombre).ThenBy(a => a.Nombre).ToList();

            ViewBag.CarrerasList = new SelectList(carreras, "CarreraID", "Nombre");
            ViewBag.AlumnosList = new SelectList(alumnos, "AlumnoID", "Nombre");

            return View(asignaturas);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(string nombre, int carreraID)
        {
            var asignatura = new Asignatura { Nombre = nombre, CarreraID = carreraID };
            _context.Asignaturas.Add(asignatura);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, string nombre, int carreraID)
        {

            var asignatura = await _context.Asignaturas.FindAsync(id);
            if (asignatura != null)
            {
                asignatura.Nombre = nombre;
                asignatura.CarreraID = carreraID;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var asignatura = await _context.Asignaturas.FindAsync(id);
            if (asignatura != null)
            {
                _context.Asignaturas.Remove(asignatura);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AsociarAlumnos(int asignaturaID, List<int> alumnos)
        {
            try
            {
                var asignatura = await _context.Asignaturas.FindAsync(asignaturaID);
                if (asignatura == null)
                {
                    return NotFound();
                }

                // Elimina las relaciones de alumnos existentes para esta asignatura.
                var relacionesAntiguas = _context.AlumnoAsignaturas.Where(pa => pa.AsignaturaID == asignaturaID);
                _context.AlumnoAsignaturas.RemoveRange(relacionesAntiguas);

                // Asocia los nuevos alumnos seleccionados.
                foreach (var alumnoID in alumnos)
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