using Ticket.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket.Models;

namespace Ticket.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ProducerService _producerService;
        private readonly ActorService _actorService;

        public MovieController(IMovieService movieService,
                               ProducerService producerService,
                               ActorService actorService)
        {
            _movieService = movieService;
            _producerService = producerService;
            _actorService = actorService;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAll();
            return View(movies);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Toujours initialiser même si vide
            var actors = await _actorService.GetAll() ?? new List<Actor>();
            ViewBag.Actors = new MultiSelectList(actors, "Id", "FullName");

            var producers = await _producerService.GetAll() ?? new List<Producer>();
            ViewBag.Producers = new SelectList(producers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var actors = await _actorService.GetAll() ?? new List<Actor>();
                ViewBag.Actors = new MultiSelectList(actors, "Id", "FullName");

                var producers = await _producerService.GetAll() ?? new List<Producer>();
                ViewBag.Producers = new SelectList(producers, "Id", "FullName");

                return View(movie);
            }

            // 1️⃣ Gestion du producteur
            if (!string.IsNullOrWhiteSpace(movie.NewProducerName))
            {
                var newProducer = new Producer { Name = movie.NewProducerName };
                await _producerService.Add(newProducer);
                movie.ProducerId = newProducer.Id;
            }

            // 2️⃣ Acteurs existants
            var actorIds = movie.ActorIds ?? new List<int>();

            // 3️⃣ Nouveaux acteurs
            if (!string.IsNullOrWhiteSpace(movie.NewActorsNames))
            {
                var newActorNames = movie.NewActorsNames
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var name in newActorNames)
                {
                    var newActor = new Actor { Name = name };
                    await _actorService.Add(newActor);
                    actorIds.Add(newActor.Id);
                }
            }

            // 4️⃣ Upload image
            if (movie.ImageFile != null && movie.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(movie.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await movie.ImageFile.CopyToAsync(stream);
                }
                movie.ImageURL = "/images/movies/" + fileName;
            }

            // 5️⃣ Ajouter le film avec les acteurs
            await _movieService.Add(movie, actorIds.ToArray());

            return RedirectToAction(nameof(Index));
        }


        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null) return NotFound();

            ViewBag.Producers = new SelectList(await _producerService.GetAll(), "Id", "Name", movie.ProducerId);
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
