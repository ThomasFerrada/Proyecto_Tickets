using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;
using System.Collections.Generic;

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
            List<EstadoTecnico> estado = new List<EstadoTecnico>();
            List<Prioridad> prob = new List<Prioridad>();
            prob = _tickets.ObtenerPrio();
            estado = _tickets.ObetnerEstTecnico();
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
                var idestado = ticket.EstadoTecnico;
                var estadoTec = estado
                    .Where(estadoTec => estadoTec.IdEstado == idestado) // Filtra los problemas cuyo Id coincide con el IdProblema del ticket
                    .Select(estadoTec => estadoTec.Estado) // Proyecta el string asociado al problema encontrado
                    .FirstOrDefault();

                ticket.EstTec = estadoTec;


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

        public IActionResult Empezar(int idTicketEmpezar)
        {
            List<EstadoTecnico> estado= new List<EstadoTecnico>();
            estado = _tickets.ObetnerEstTecnico();
            Ticket ticket = new Ticket();
            ticket = _tickets.ObtenerTicket(idTicketEmpezar);
            int estadoSeleccionado = ticket.EstadoTecnico;
            ViewBag.ListaPrio = new SelectList(estado, "IdEstado", "Estado", estadoSeleccionado);
            
            return View(ticket);
        }

        public IActionResult ActualizarEstadoTicket(int NuevoEstado, string comentario, int ticket)
        {
            var response=_tickets.UpdateTicket(NuevoEstado, comentario, ticket);
            if (response=="1")
            {
                return RedirectToAction("TicketTecnico", "Index");
            }
            else
            {
                return RedirectToAction("Inicio", "Index");
            }
        }



    }
}
