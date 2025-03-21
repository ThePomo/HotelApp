using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using HotelPrenotazioni.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")] 
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Mostra la lista di tutti i dipendenti con i loro ruoli
    public async Task<IActionResult> Index()
    {
        var utenti = _userManager.Users.ToList(); 
        var utentiConRuoli = new List<UserWithRoles>();

        // Recupera i ruoli per ogni utente
        foreach (var user in utenti)
        {
            var roles = await _userManager.GetRolesAsync(user);
            utentiConRuoli.Add(new UserWithRoles
            {
                User = user,
                Roles = roles
            });
        }

        return View(utentiConRuoli);  // Passa la lista di utenti con i ruoli alla vista
    }

   
    public async Task<IActionResult> Create()
    {
        // Popola i ruoli disponibili dal database
        var roles = await _roleManager.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Name", "Name"); // Passa i ruoli alla vista
        return View();
    }

    // Creazione di un nuovo dipendente con ruolo
    [HttpPost]
    public async Task<IActionResult> Create(string email, string password, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            ModelState.AddModelError("", "Il ruolo selezionato non esiste.");
            return View();
        }

        var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View();
    }

    // Elimina un dipendente
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }
}

