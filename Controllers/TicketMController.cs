using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket.Data.Services;
using Ticket.Models;

namespace eTickets.Controllers
{
    public class TicketMController : Controller
    {
        private readonly TicketMService _ticketService;
        private readonly ScreeningService _screeningService;

        public TicketMController(TicketMService ticketService, ScreeningService screeningService)
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

        // GET: Ticket/Create
        public async Task<IActionResult> Create()
        {
            var screenings = await _screeningService.GetAll();
            ViewBag.Screenings = new SelectList(screenings, "Id", "StartTime");
            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketM ticket)
        {
            if (!ModelState.IsValid)
            {
                var screenings = await _screeningService.GetAll();
                ViewBag.Screenings = new SelectList(screenings, "Id", "StartTime");
                return View(ticket);
            }

            // Vérifier si le siège est déjà réservé
            var existingTicket = await _ticketService.GetTicketBySeat(ticket.ScreeningId, ticket.SeatNumber);
            if (existingTicket != null)
            {
                ModelState.AddModelError("", "Ce siège est déjà réservé !");
                var screenings = await _screeningService.GetAll();
                ViewBag.Screenings = new SelectList(screenings, "Id", "StartTime");
                return View(ticket);
            }

            await _ticketService.Add(ticket);
            return RedirectToAction(nameof(Index));
        }

        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetById(id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetById(id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }
    }
}
