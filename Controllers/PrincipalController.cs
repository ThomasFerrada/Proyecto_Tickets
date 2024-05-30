using Microsoft.AspNetCore.Mvc;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly ITickets _ticket;

        public PrincipalController(ITickets ticket)
        {
            _ticket = ticket;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Index(string id)
        {
            Ticket ticket = new Ticket();
            ticket = _ticket.ObtenerTicket(Convert.ToInt32(id));
            return View(ticket);
            
        }

        
    }
}
