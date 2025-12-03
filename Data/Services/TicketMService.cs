using Microsoft.EntityFrameworkCore;
using Ticket.Models;

namespace Ticket.Data.Services
{
    public class TicketMService : ITicketMService
    {
        private readonly AppDbContext _context;

        public TicketMService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TicketM>> GetAll()
        {
            return await _context.TicketMs
                                 .Include(t => t.Screening)
                                     .ThenInclude(s => s.Movie)
                                         .Include(t => t.User) 

                                 .ToListAsync();
        }

        public async Task<TicketM> GetById(int id)
        {
            return await _context.TicketMs
                                 .Include(t => t.Screening)
                                     .ThenInclude(s => s.Movie)
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TicketM>> GetByScreeningId(int screeningId)
        {
            return await _context.TicketMs
                                 .Where(t => t.ScreeningId == screeningId)
                                 .Include(t => t.Screening)
                                     .ThenInclude(s => s.Movie)
                                 .ToListAsync();
        }

        public async Task Add(TicketM ticket)
        {
            await _context.TicketMs.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TicketM ticket)
        {
            _context.TicketMs.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ticket = await _context.TicketMs.FindAsync(id);
            if (ticket != null)
            {
                _context.TicketMs.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TicketM> GetTicketBySeat(int ScreeningId, int SeatNumber)
        
            {
                return await _context.TicketMs
                                     .FirstOrDefaultAsync(t => t.ScreeningId == ScreeningId && t.SeatNumber == SeatNumber);
            }
        public async Task<List<TicketM>> GetTicketsByUser(string userId)
        {
            return await _context.TicketMs
                .Where(t => t.UserId == userId)
                .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
                .ToListAsync();
        

    }


}
}
