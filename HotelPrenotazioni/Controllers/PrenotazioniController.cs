using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelPrenotazioni.Data;
using HotelPrenotazioni.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelPrenotazioni.Controllers
{
    [Authorize]
    public class PrenotazioniController : Controller
    {
        private readonly HotelDbContext _context;

        public PrenotazioniController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Prenotazioni
        public async Task<IActionResult> Index()
        {
            var prenotazioni = _context.Prenotazioni.Include(p => p.Camera).Include(p => p.Cliente);
            return View(await prenotazioni.ToListAsync());
        }

        // GET: Prenotazioni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Camera)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.PrenotazioneId == id);

            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }

        // GET: Prenotazioni/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "Numero");
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "Cognome");
            return View();
        }

        // POST: Prenotazioni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("PrenotazioneId,ClienteId,CameraId,DataInizio,DataFine,Stato")] Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prenotazione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "Numero", prenotazione.CameraId);
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "Cognome", prenotazione.ClienteId);
            return View(prenotazione);
        }

        // GET: Prenotazioni/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione == null) return NotFound();

            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "Numero", prenotazione.CameraId);
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "Cognome", prenotazione.ClienteId);
            return View(prenotazione);
        }

        // POST: Prenotazioni/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("PrenotazioneId,ClienteId,CameraId,DataInizio,DataFine,Stato")] Prenotazione prenotazione)
        {
            if (id != prenotazione.PrenotazioneId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prenotazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrenotazioneExists(prenotazione.PrenotazioneId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "Numero", prenotazione.CameraId);
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "Cognome", prenotazione.ClienteId);
            return View(prenotazione);
        }

        // GET: Prenotazioni/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Camera)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.PrenotazioneId == id);

            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }

        // POST: Prenotazioni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione != null)
            {
                _context.Prenotazioni.Remove(prenotazione);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PrenotazioneExists(int id)
        {
            return _context.Prenotazioni.Any(e => e.PrenotazioneId == id);
        }
    }
}
