using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket.Models
{
    public class TicketM
    {
        public int Id { get; set; }

        [Range(1, 200)]
        [Display(Name = "Numéro de siège")]
        public int SeatNumber { get; set; }

        [Display(Name = "Réservé ?")]
        public bool IsReserved { get; set; }

        // 🔗 Relation Screening
        [Required]
        [Display(Name = "Séance")]
        public int ScreeningId { get; set; }

        [ForeignKey("ScreeningId")]
        public Screening Screening { get; set; }

        // 🔗 Relation avec l'utilisateur
        [Required]
        [Display(Name = "Utilisateur")]
        public string UserId { get; set; }

        // 🔗 Date d'achat du ticket
        [Required]
        [Display(Name = "Date d'achat")]
        public DateTime DateAchat { get; set; }
    }
}
