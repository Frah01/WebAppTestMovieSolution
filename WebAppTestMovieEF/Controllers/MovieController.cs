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
    public class MovieController : Controller
    {
        private readonly CorsoAcademyContext _context;
        private readonly ILogger<HomeController> _logger;

        public MovieController(CorsoAcademyContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
         {
            List<TMovie> listFilm = null;
            try
            {

                listFilm = await _context.TMovies.Include(t => t.IdGenreNavigation).ToListAsync();
                _logger.LogDebug("Film presenti {0}", listFilm.Count);
                _logger.LogInformation("Lista dei Film");
                
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
            return View(listFilm);
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies
                .Include(t => t.IdGenreNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tMovie == null)
            {
                return NotFound();
            }

            return View(tMovie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            ViewData["IdGenre"] = new SelectList(_context.TGenres, "Id", "Genere");
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Film,Data,IsAdult,IdGenre")] TMovie tMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGenre"] = new SelectList(_context.TGenres, "Id", "Genere", tMovie.IdGenre);
            return View(tMovie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies.FindAsync(id);
            if (tMovie == null)
            {
                return NotFound();
            }
            ViewData["IdGenre"] = new SelectList(_context.TGenres, "Id", "Genere", tMovie.IdGenre);
            return View(tMovie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Film,Data,IsAdult,IdGenre")] TMovie tMovie)
        {
            if (id != tMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TMovieExists(tMovie.Id))
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
            ViewData["IdGenre"] = new SelectList(_context.TGenres, "Id", "Genere", tMovie.IdGenre);
            return View(tMovie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies
                .Include(t => t.IdGenreNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tMovie == null)
            {
                return NotFound();
            }

            return View(tMovie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tMovie = await _context.TMovies.FindAsync(id);
            if (tMovie != null)
            {
                _context.TMovies.Remove(tMovie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TMovieExists(int id)
        {
            return _context.TMovies.Any(e => e.Id == id);
        }
    }
}
