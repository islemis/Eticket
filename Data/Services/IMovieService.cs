using Ticket.Models;

namespace Ticket.Data.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task Add(Movie movie, int[] actorIds);
        Task Update(Movie movie, int[] actorIds);
        Task Delete(int id);
    }
}
