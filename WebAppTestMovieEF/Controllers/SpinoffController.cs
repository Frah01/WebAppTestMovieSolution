using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTestMovieEF.Models;

namespace WebAppTestMovieEF.Controllers
{
    public class SpinoffController : Controller
    {
        private readonly CorsoAcademyContext _context;
        private readonly ILogger<HomeController> _logger;

        public SpinoffController(CorsoAcademyContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Spinoff
        public async Task<IActionResult> Index()
        {
            List<TSpinoff> listSpinoff = null;
            try
            {

                listSpinoff = await _context.TSpinoffs.Include(t => t.IdMovieNavigation).ToListAsync();
                _logger.LogDebug("SpinOff presenti {0}", listSpinoff.Count);
                _logger.LogInformation("Lista degli Spinoff");

            }
            catch (Exception ex)
            {
                string sErr = string.Empty;
                if (ex.InnerException != null)
                {
                    sErr = string.Format("Source : {0}{4}Message : {1}{4}StackTrace: {2}{4}InnerException: {3}{4}", ex.Source, ex.Message, ex.StackTrace, ex.InnerException.Message, System.Environment.NewLine);
                }
                else
                {
                    sErr = string.Format("Source : {0}{3}Message : {1}{3}StackTrace: {2}{3}", ex.Source, ex.Message, ex.StackTrace, System.Environment.NewLine);

                }
                _logger.LogError(sErr);
                throw;
            }
            return View(listSpinoff);
        }

        // GET: Spinoff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tSpinoff = await _context.TSpinoffs
                .Include(t => t.IdMovieNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tSpinoff == null)
            {
                return NotFound();
            }

            return View(tSpinoff);
        }

        // GET: Spinoff/Create
        public IActionResult Create()
        {
            ViewData["IdMovie"] = new SelectList(_context.TMovies, "Id", "Film");
            return View();
        }

        // POST: Spinoff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Spinoff,Data,IsCanonical,IdMovie")] TSpinoff tSpinoff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tSpinoff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMovie"] = new SelectList(_context.TMovies, "Id", "Film", tSpinoff.IdMovie);
            return View(tSpinoff);
        }

        // GET: Spinoff/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tSpinoff = await _context.TSpinoffs.FindAsync(id);
            if (tSpinoff == null)
            {
                return NotFound();
            }
            ViewData["IdMovie"] = new SelectList(_context.TMovies, "Id", "Film", tSpinoff.IdMovie);
            return View(tSpinoff);
        }

        // POST: Spinoff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Spinoff,Data,IsCanonical,IdMovie")] TSpinoff tSpinoff)
        {
            if (id != tSpinoff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tSpinoff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TSpinoffExists(tSpinoff.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMovie"] = new SelectList(_context.TMovies, "Id", "Film", tSpinoff.IdMovie);
            return View(tSpinoff);
        }

        // GET: Spinoff/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tSpinoff = await _context.TSpinoffs
                .Include(t => t.IdMovieNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tSpinoff == null)
            {
                return NotFound();
            }

            return View(tSpinoff);
        }

        // POST: Spinoff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tSpinoff = await _context.TSpinoffs.FindAsync(id);
            if (tSpinoff != null)
            {
                _context.TSpinoffs.Remove(tSpinoff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TSpinoffExists(int id)
        {
            return _context.TSpinoffs.Any(e => e.Id == id);
        }
    }
}
