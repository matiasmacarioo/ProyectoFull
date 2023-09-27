using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _contexto;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext contexto, ApplicationDbContext contextUsuario, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
    {
        _logger = logger;
        _contexto = contexto;
        _userManager = userManager;
        _rolManager = rolManager;
    }

    public async Task<IActionResult> IndexAsync()
    {
        await InicializarPermisosUsuario();
        return View();
    }

    public async Task<JsonResult> InicializarPermisosUsuario()
    {
        bool creado = false;

        //CREAR ROL "Administrador"
        var AdministradorExiste = _contexto.Roles.Any(r => r.Name == "Administrador");
        if (!AdministradorExiste)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("Administrador"));
        }

        // CREAR ROL "Profesor"
        var ProfesorExiste = _contexto.Roles.Any(r => r.Name == "Profesor");
        if (!ProfesorExiste)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("Profesor"));
        }

        // CREAR ROL "Alumno"
        var AlumnoExiste = _contexto.Roles.Any(r => r.Name == "Alumno");
        if (!AlumnoExiste)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("Alumno"));
        }


        //CREAR USUARIO de Administrador
        //VALIDAMOS SI YA EXISTE
        var usuario = await _userManager.FindByEmailAsync("administrador@carreras.com");
        if (usuario == null)
        {
            var user = new IdentityUser { UserName = "administrador@carreras.com", Email = "administrador@carreras.com", EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, "administrador");

            await _userManager.AddToRoleAsync(user, "Administrador");
            creado = result.Succeeded;
        }

        return Json(creado);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
