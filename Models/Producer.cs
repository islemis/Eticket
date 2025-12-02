using System.ComponentModel.DataAnnotations;

namespace Ticket.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
