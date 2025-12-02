using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Models;

namespace eTickets.Data.Services
{
    public class ActorService
    {
        private readonly AppDbContext _context;

        public ActorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Actor>> GetAll()
        {
            return await _context.Actors.ToListAsync();
        }
    }
}
