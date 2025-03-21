using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelPrenotazioni.Data;
using HotelPrenotazioni.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelPrenotazioni.Controllers
{
    [Authorize]
    public class CameraController : Controller
    {
        private readonly HotelDbContext _context;

        public CameraController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Camera 
        public async Task<IActionResult> Index()
        {
            return View(await _context.Camere.ToListAsync());
        }

        // GET: Camera/Details/5 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var camera = await _context.Camere
                .FirstOrDefaultAsync(m => m.CameraId == id);

            if (camera == null)
                return NotFound();

            return View(camera);
        }

        // GET: Camera/Create 
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camera/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CameraId,Numero,Tipo,Prezzo")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        // GET: Camera/Edit/5 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var camera = await _context.Camere.FindAsync(id);
            if (camera == null)
                return NotFound();

            return View(camera);
        }

        // POST: Camera/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("CameraId,Numero,Tipo,Prezzo")] Camera camera)
        {
            if (id != camera.CameraId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraExists(camera.CameraId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        // GET: Camera/Delete/5 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var camera = await _context.Camere
                .FirstOrDefaultAsync(m => m.CameraId == id);

            if (camera == null)
                return NotFound();

            return View(camera);
        }

        // POST: Camera/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camera = await _context.Camere.FindAsync(id);
            if (camera != null)
            {
                _context.Camere.Remove(camera);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CameraExists(int id)
        {
            return _context.Camere.Any(e => e.CameraId == id);
        }
    }
}
