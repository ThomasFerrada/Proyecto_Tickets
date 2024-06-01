using Microsoft.AspNetCore.Mvc;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Controllers
{
    public class TicketTecnicoController : Controller
    {
        private readonly ITickets _tickets;

        public TicketTecnicoController(ITickets tickets)
        {

            _tickets = tickets;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr=  HttpContext.Session.GetInt32("TipoUsuario");
            List<Ticket>ticketList = new List<Ticket>();
            ticketList = _tickets.ObtenerTicketsTecnico(id);
            bool bloquear = ticketList.Any(t => t.IdPrioridad == 5);
            foreach (var ticket in ticketList)
            {
                if (ticket.IdPrioridad > 4)
                {
                    bloquear = true;
                    ticket.Bloqueado = "N";
                }
                else if (ticket.IdPrioridad<5 && bloquear==true)
                {
                    ticket.Bloqueado = "S";
                }
                else {
                    ticket.Bloqueado="N";
                }
            }

            return View(ticketList);
        }

        [HttpPost]

        public IActionResult Examinar(int idTicketExaminar)
        {
            //traer ticket
            Ticket ticket= new Ticket();
            ticket = _tickets.ObtenerTicket(idTicketExaminar);
            return View(ticket);
        }



    }
}
