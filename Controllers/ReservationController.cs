using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Services;
using Ticket.Models;

namespace Ticket.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ITicketMService _ticketService;
        private readonly IScreeningService _screeningService;

        public ReservationController(ITicketMService ticketService, IScreeningService screeningService)
        {
            _ticketService = ticketService;
            _screeningService = screeningService;
        }

        // GET: Reservation/Create?screeningId=5
        public async Task<IActionResult> Create(int screeningId)
        {
            var screening = await _screeningService.GetById(screeningId);
            if (screening == null) return NotFound();

            return View(screening); // On passe la séance à la vue
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int screeningId, int seatNumber)
        {
            var screening = await _screeningService.GetById(screeningId);
            if (screening == null) return NotFound();

            // Vérifier si le siège est déjà pris
            var existingTicket = await _ticketService.GetTicketBySeat(screeningId, seatNumber);
            if (existingTicket != null)
            {
                ModelState.AddModelError("", "Ce siège est déjà réservé !");
                return View(screening);
            }

            // Créer le ticket
            var ticket = new TicketM
            {
                ScreeningId = screeningId,
                SeatNumber = seatNumber,
                UserId = User.Identity.Name, // ou ton UserId
                DateAchat = DateTime.Now
            };

            await _ticketService.Add(ticket);

            return RedirectToAction("Confirmation", new { id = ticket.Id });
        }

        // GET: Reservation/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var ticket = await _ticketService.GetById(id);
            if (ticket == null) return NotFound();

            return View(ticket);
        }
    }
}
