using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Models;

namespace eTickets.Data.Services
{
    public class ProducerService
    {
        private readonly AppDbContext _context;

        public ProducerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producer>> GetAll()
        {
            return await _context.Producers.ToListAsync();
        }
    }
}
