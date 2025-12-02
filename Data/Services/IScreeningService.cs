using Ticket.Models;
namespace Ticket.Data.Services
{


    public interface IScreeningService
    {
        Task<List<Screening>> GetAll();
        Task<Screening> GetById(int id);
        Task Add(Screening screening);
        Task Update(Screening screening);
        Task Delete(int id);
        Task<List<Screening>> GetByMovieId(int movieId);
    }


}
