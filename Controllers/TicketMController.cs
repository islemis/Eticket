using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket.Data.Services;
using Ticket.Models;

namespace eTickets.Controllers
{
    public class TicketMController : Controller
    {
        private readonly ITicketMService _ticketService;
        private readonly IScreeningService _screeningService;

        public TicketMController(ITicketMService ticketService, IScreeningService screeningService)
        {
            _ticketService = ticketService;
            _screeningService = screeningService;
        }

        // GET: Ticket
        public async Task<IActionResult> Index()
           {
            var tickets = await _ticketService.GetAll();
            return View(tickets);
        }
        // GET: Ticket/MyTickets
        public async Task<IActionResult> MyTickets()
        {
            // Récupérer l'ID de l'utilisateur connecté
            var userId = User.Identity.Name; // ou User.FindFirst(ClaimTypes.NameIdentifier).Value selon ton Auth
            var tickets = await _ticketService.GetTicketsByUser(userId);
            return View(tickets);
        }

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
