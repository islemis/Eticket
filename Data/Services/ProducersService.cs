using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Models;

namespace Ticket.Data.Services
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
        public async Task Add(Producer producer)
        {
            _context.Producers.Add(producer);
            await _context.SaveChangesAsync();
        }

    }
}
