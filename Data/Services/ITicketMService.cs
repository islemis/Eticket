using System.Collections.Generic;
using System.Threading.Tasks;
using Ticket.Models;

namespace Ticket.Data.Services
{
    public interface ITicketMService
    {
        Task<List<TicketM>> GetAll();
        Task<TicketM> GetById(int id);
        Task<List<TicketM>> GetByScreeningId(int screeningId);
        Task Add(TicketM ticket);
        Task Update(TicketM ticket);
        Task Delete(int id);
    }
}