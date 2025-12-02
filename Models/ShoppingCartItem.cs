using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public TicketM Ticket { get; set; }

        [Required]
        [Display(Name = "ID du panier")]
        public string ShoppingCartId { get; set; }
    }
}
