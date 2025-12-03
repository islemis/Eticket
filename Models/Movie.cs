using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket.Models
{
    public class Movie
    {
        [Required]
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

        // Nouveau : Producteur
        [Required]
        [Display(Name = "Producteur")]
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }

        // Many-to-Many avec Actor
        public List<Actor_Movie> Actors_Movies { get; set; }

        // Relations avec Séances
        public List<Screening> Screenings { get; set; }

      
        [Display(Name = "Date de début")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Date de fin")]
        public DateTime EndDate { get; set; }
        [NotMapped]
        [Display(Name = "Affiche du film")]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Sélectionner les acteurs")]
        public List<int> ActorIds { get; set; }
        [NotMapped]
        public string NewProducerName { get; set; }

        [NotMapped]
        public string NewActorsNames { get; set; } // séparer par virgule
    }
}
