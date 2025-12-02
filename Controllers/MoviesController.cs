using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket.Models;

namespace eTickets.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieService _movieService;
        private readonly ProducerService _producerService;
        private readonly CinemaService _cinemaService;
        private readonly ActorService _actorService;

        public MovieController(MovieService movieService,
                               ProducerService producerService,
                               CinemaService cinemaService,
                               ActorService actorService)
        {
            _movieService = movieService;
            _producerService = producerService;
            _cinemaService = cinemaService;
            _actorService = actorService;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAll();
            return View(movies);
        }

        // GET: Movie/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Producers = new SelectList(await _producerService.GetAll(), "Id", "Name");
            ViewBag.Cinemas = new SelectList(await _cinemaService.GetAll(), "Id", "Name");
            ViewBag.Actors = new MultiSelectList(await _actorService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, int[] SelectedActorIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Producers = new SelectList(await _producerService.GetAll(), "Id", "Name");
                ViewBag.Cinemas = new SelectList(await _cinemaService.GetAll(), "Id", "Name");
                ViewBag.Actors = new MultiSelectList(await _actorService.GetAll(), "Id", "Name");
                return View(movie);
            }

            await _movieService.Add(movie, SelectedActorIds);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null) return NotFound();

            ViewBag.Producers = new SelectList(await _producerService.GetAll(), "Id", "Name", movie.ProducerId);
            ViewBag.Cinemas = new SelectList(await _cinemaService.GetAll(), "Id", "Name", movie.CinemaId);
            ViewBag.Actors = new MultiSelectList(await _actorService.GetAll(),
                                                 "Id", "Name",
                                                 movie.Actors_Movies?.Select(am => am.ActorId));
            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, int[] SelectedActorIds)
        {
            if (id != movie.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Producers = new SelectList(await _producerService.GetAll(), "Id", "Name", movie.ProducerId);
                ViewBag.Cinemas = new SelectList(await _cinemaService.GetAll(), "Id", "Name", movie.CinemaId);
                ViewBag.Actors = new MultiSelectList(await _actorService.GetAll(), "Id", "Name", SelectedActorIds);
                return View(movie);
            }

            await _movieService.Update(movie, SelectedActorIds);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null) return NotFound();
            return View(movie);
        }
    }
}
