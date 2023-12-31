﻿using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // Obtener la lista de carreras para el array de etiquetas de carreras.
        var carreras = await _contexto.Carreras
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Duracion)
            .Select(c => c.Nombre) // Solo se selecciona el nombre de la carrera.
            .ToListAsync();

        
        // Obtener la lista de carreras desde la base de datos, esta no se pasa a la vista
        var carrerasList = await _contexto.Carreras
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Duracion)
            .ToListAsync();

        var alumnosList = await _contexto.Alumnos
            .OrderBy(c => c.Nombre)
            .ToListAsync();


        var cantidadAlumnosPorCarrera = new List<int>();
        foreach (var carrera in carrerasList)
        {
            var cantidad = await _contexto.Alumnos
                .Where(a => a.Carrera.Nombre == carrera.Nombre)
                .CountAsync();
            cantidadAlumnosPorCarrera.Add(cantidad);
        }

        // Pasar la lista de cantidades de alumnos al modelo
        ViewData["CantidadAlumnosPorCarrera"] = cantidadAlumnosPorCarrera;
        
        ViewData["Alumnos"] = alumnosList;

        // Pasar la lista de carreras al modelo
        ViewData["Carreras"] = carreras;

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

        var profesoresList = await _contexto.Profesores.ToListAsync();

        if (profesoresList.Count > 0)
        {
            var idRelacionar = string.Empty;

            foreach (var profesor in profesoresList)
            {
                var usuarioProfesor = await _userManager.FindByEmailAsync(profesor.Email);
                if (usuarioProfesor == null)
                {
                    // El usuario no existe, así que lo creamos
                    var nuevoUsuarioProfesor = new IdentityUser { UserName = profesor.Email, Email = profesor.Email, EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(nuevoUsuarioProfesor, "profesor");
                    idRelacionar = usuario.Id;
                    if (result.Succeeded)
                    {
                        // Asignar el rol de "Profesor" al nuevo profesor
                        await _userManager.AddToRoleAsync(nuevoUsuarioProfesor, "profesor");
                    }
                }
                // Si el usuario ya existe relacionarlo con el profesor
                 if (usuarioProfesor != null)
                {
                    idRelacionar = usuarioProfesor.Id;
                }
                profesor.IdentityID = idRelacionar;

                await _contexto.SaveChangesAsync();
            }
        }

        var alumnosList = await _contexto.Alumnos.ToListAsync();

        if (alumnosList.Count > 0)
        {
            var idRelacionar = string.Empty;

            foreach (var alumno in alumnosList)
            {
                var usuarioAlumno = await _userManager.FindByEmailAsync(alumno.Email);
                if (usuarioAlumno == null)
                {
                    // El usuario no existe, así que lo creamos
                    var nuevoUsuarioAlumno = new IdentityUser { UserName = alumno.Email, Email = alumno.Email, EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(nuevoUsuarioAlumno, "alumno"); //contraseña: alumno
                    idRelacionar = usuario.Id;
                    if (result.Succeeded)
                    {
                        // Asignar el rol de "Alumno" al nuevo alumno
                        await _userManager.AddToRoleAsync(nuevoUsuarioAlumno, "alumno");
                    }
                }
                // Si el usuario ya existe relacionarlo con el alumno
                 if (usuarioAlumno != null)
                {
                    idRelacionar = usuarioAlumno.Id;
                }
                alumno.IdentityID = idRelacionar;

                await _contexto.SaveChangesAsync();
            }
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
