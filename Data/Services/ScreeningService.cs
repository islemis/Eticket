using Microsoft.EntityFrameworkCore;
using Ticket.Models;

namespace Ticket.Data.Services
{
    public class ScreeningService : IScreeningService
    {
        private readonly AppDbContext _context;

        public ScreeningService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Screening>> GetAll()
        {
            return await _context.Screenings
                                 .Include(s => s.Movie)
                                 .ToListAsync();
        }

        public async Task<Screening> GetById(int id)
        {
            return await _context.Screenings
                                 .Include(s => s.Movie)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Add(Screening screening)
        {
            await _context.Screenings.AddAsync(screening);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Screening screening)
        {
            _context.Screenings.Update(screening);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening != null)
            {
                _context.Screenings.Remove(screening);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Screening>> GetByMovieId(int movieId)
        {
            return await _context.Screenings
                                 .Where(s => s.MovieId == movieId)
                                 .Include(s => s.Movie)
                                 .ToListAsync();
        }
    }
}
