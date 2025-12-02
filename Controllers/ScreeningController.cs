using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket.Data.Services;
using Ticket.Models;

namespace eTickets.Controllers
{
    public class ScreeningController : Controller
    {
        private readonly ScreeningService _screeningService;
        private readonly Data.Services.MovieService _movieService;

        public ScreeningController(ScreeningService screeningService, Data.Services.MovieService movieService)
        {
            _screeningService = screeningService;
            _movieService = movieService;
        }

        // GET: Screening
        public async Task<IActionResult> Index()
        {
            var screenings = await _screeningService.GetAll();
            return View(screenings);
        }

        // GET: Screening/Create
        public async Task<IActionResult> Create()
        {
            var movies = await _movieService.GetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Title");
            return View();
        }

        // POST: Screening/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Screening screening)
        {
            if (!ModelState.IsValid)
            {
                var movies = await _movieService.GetAll();
                ViewBag.Movies = new SelectList(movies, "Id", "Title");
                return View(screening);
            }

            await _screeningService.Add(screening);
            return RedirectToAction(nameof(Index));
        }

        // GET: Screening/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var screening = await _screeningService.GetById(id);
            if (screening == null) return NotFound();

            var movies = await _movieService.GetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Title", screening.MovieId);
            return View(screening);
        }

        // POST: Screening/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Screening screening)
        {
            if (id != screening.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var movies = await _movieService.GetAll();
                ViewBag.Movies = new SelectList(movies, "Id", "Title", screening.MovieId);
                return View(screening);
            }

            await _screeningService.Update(screening);
            return RedirectToAction(nameof(Index));
        }

        // GET: Screening/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var screening = await _screeningService.GetById(id);
            if (screening == null) return NotFound();
            return View(screening);
        }

        // POST: Screening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _screeningService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Screening/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var screening = await _screeningService.GetById(id);
            if (screening == null) return NotFound();
            return View(screening);
        }
    }
}
