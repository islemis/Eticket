using System.ComponentModel.DataAnnotations;

namespace Ticket.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email client")]
        public string Email { get; set; }

        [Display(Name = "Utilisateur")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Date de commande")]
        public DateTime OrderDate { get; set; }

        // Initialisation pour éviter null
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
