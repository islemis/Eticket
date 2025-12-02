using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Models;

namespace eTickets.Data.Services
{
    public class CinemaService
    {
        private readonly AppDbContext _context;

        public CinemaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cinema>> GetAll()
        {
            return await _context.Cinemas.ToListAsync();
        }
    }
}
