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
    public class ProfesoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public ProfesoresController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _rolManager = roleManager;
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
            var idRelacionar = string.Empty;
            var profesor = new Profesor { Nombre = nombre, FechaNacimiento = fechaNacimiento, CarreraID = carreraID, Direccion = direccion, Documento = documento, Email = email };
            
            var profesorExiste = await _context.Profesores.AnyAsync(a => a.Documento == documento);
            
            if (!profesorExiste)
            {

                // Verificar si el usuario de Identity ya existe
                var usuarioExiste = await _userManager.FindByEmailAsync(profesor.Email);
                if (usuarioExiste == null)
                {
                    // El usuario no existe, así que lo creamos
                    var usuario = new IdentityUser { UserName = profesor.Email, Email = profesor.Email, EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(usuario, "profesor");
                    idRelacionar = usuario.Id;
                    if (result.Succeeded)
                    {
                        // Asignar el rol de "Profesor" al nuevo profesor
                        await _userManager.AddToRoleAsync(usuario, "profesor");
                    }
                }

                 if (usuarioExiste != null)
                {
                    idRelacionar = usuarioExiste.Id;
                }

                profesor.IdentityID = idRelacionar;

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
            var idRelacionar = string.Empty;
            var profesor = await _context.Profesores.FindAsync(id);
            var usuario = await _userManager.FindByEmailAsync(profesor.Email);
            
            // Verificar si el correo nuevo ya existe en "Profesores"
            var correoExisteEnProfesores = await _context.Profesores.AnyAsync(c => c.Email == email);

            // Verificar si el correo electrónico ha cambiado y no existe otro Profesor con ese correo 
            if (profesor.Email != email || !correoExisteEnProfesores)
            {
                if (usuario != null)
                {
                    // Actualizar Usuario en Identity
                    usuario.Email = email;
                    usuario.UserName = email;
                    usuario.EmailConfirmed = true;
                    await _userManager.UpdateAsync(usuario);
                }
            }

            if (usuario != null)
            {
                idRelacionar = usuario.Id;
            }

            if (idRelacionar != string.Empty)
            {
                profesor.IdentityID = idRelacionar;
            }

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