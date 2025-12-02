using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket.Models
{
    public class Screening
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Heure de début")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(5, 50)]
        [Display(Name = "Prix")]
        public double Price { get; set; }

        // 🔗 Relation avec Movie
        [Required]
        [Display(Name = "Film")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        // Tickets liés à cette séance
        public List<TicketM> Tickets { get; set; }
    }
}
