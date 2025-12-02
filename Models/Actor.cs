using eTickets.Models;
using System.ComponentModel.DataAnnotations;

namespace Ticket.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Relation Many-to-Many avec Movie
        public List<Actor_Movie> Actors_Movies { get; set; }
    }
}
