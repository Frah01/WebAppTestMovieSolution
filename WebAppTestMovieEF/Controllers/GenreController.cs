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
    public class GenreController : Controller
    {
        private readonly CorsoAcademyContext _context;

        private readonly ILogger<HomeController> _logger;

        public GenreController(CorsoAcademyContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Genre
        public async Task<IActionResult> Index()
        {
            List<TGenre> listGeneri = null;
            try
            {

                listGeneri = await _context.TGenres.ToListAsync();
                _logger.LogDebug("Generi presenti {0}", listGeneri.Count);
                _logger.LogInformation("Lista dei Generi");
                
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
            return View(listGeneri);
        }
    

        // GET: Genre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGenre = await _context.TGenres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tGenre == null)
            {
                return NotFound();
            }

            return View(tGenre);
        }

        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Genere,IsAdult")] TGenre tGenre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tGenre);
        }

        // GET: Genre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGenre = await _context.TGenres.FindAsync(id);
            if (tGenre == null)
            {
                return NotFound();
            }
            return View(tGenre);
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Genere,IsAdult")] TGenre tGenre)
        {
            if (id != tGenre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TGenreExists(tGenre.Id))
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
            return View(tGenre);
        }

        // GET: Genre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGenre = await _context.TGenres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tGenre == null)
            {
                return NotFound();
            }

            return View(tGenre);
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tGenre = await _context.TGenres.FindAsync(id);
            if (tGenre != null)
            {
                _context.TGenres.Remove(tGenre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TGenreExists(int id)
        {
            return _context.TGenres.Any(e => e.Id == id);
        }
    }
}
