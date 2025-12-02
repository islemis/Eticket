using System.ComponentModel.DataAnnotations;

namespace Ticket.Models
{

    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Affiche du film")]
        public string ImageURL { get; set; }

        [Range(30, 240)]
        public int DurationMinutes { get; set; }

        // 🔗 Nouveau : Producteur
        [Required]
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }

        // 🔗 Nouveau : Many-to-Many avec Actor
        public List<Actor_Movie> Actors_Movies { get; set; }

        // Relation avec Séances
        public List<Screening> Screenings { get; set; }
    }
}
