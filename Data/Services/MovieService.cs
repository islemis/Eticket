using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Models;

namespace Ticket.Data.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAll()
        {
            return await _context.Movies
                .Include(m => m.Producer)
                .Include(m => m.Actors_Movies)
                    .ThenInclude(am => am.Actor)
                .Include(m => m.Screenings)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies
                .Include(m => m.Producer)
                .Include(m => m.Actors_Movies)
                    .ThenInclude(am => am.Actor)
                .Include(m => m.Screenings)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Add(Movie movie, int[] actorIds)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync(); // récupère Id du film

            if (actorIds != null)
            {
                foreach (var actorId in actorIds)
                {
                    _context.Actors_Movies.Add(new Actor_Movie
                    {
                        MovieId = movie.Id,
                        ActorId = actorId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Movie movie, int[] actorIds)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            var existingActors = _context.Actors_Movies.Where(am => am.MovieId == movie.Id);
            _context.Actors_Movies.RemoveRange(existingActors);
            await _context.SaveChangesAsync();

            if (actorIds != null)
            {
                foreach (var actorId in actorIds)
                {
                    _context.Actors_Movies.Add(new Actor_Movie
                    {
                        MovieId = movie.Id,
                        ActorId = actorId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
