using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;
using System.Collections.Generic;
using System.Net.Sockets;

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
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
            if (id!=0 && tipoUsr==2)
            {
                List<Ticket> ticketList = new List<Ticket>();
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
                    else if (ticket.IdPrioridad < 5 && bloquear == true)
                    {
                        ticket.Bloqueado = "S";
                    }
                    else
                    {
                        ticket.Bloqueado = "N";
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
            else
            {
                return RedirectToAction("Index", "Inicio");
            }

            
        }

        [HttpPost]

        public IActionResult Examinar(int idTicketExaminar)
        {
            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
            if (id != 0 && tipoUsr == 2)
            {
                var estado = "";
                Ticket ticket = new Ticket();
                ticket = _tickets.ObtenerTicket(idTicketExaminar);
                if (ticket.IdTecnico == 1)
                {
                    estado = "Nuevo";
                }
                else if (ticket.IdTecnico == 2)
                {
                    estado = "Desarrollo";
                }
                else if (ticket.IdTecnico == 3)
                {
                    estado = "Pausa";
                }
                else if (ticket.IdTecnico == 4)
                {
                    estado = "Finalizado";
                }
                ViewBag.estado = estado;
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                
        }

        public IActionResult Empezar(int idTicketEmpezar)
        {
            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
            if (id != 0 && tipoUsr == 2)
            {
                List<EstadoTecnico> estado = new List<EstadoTecnico>();
                estado = _tickets.ObetnerEstTecnico();
                Ticket ticket = new Ticket();
                ticket = _tickets.ObtenerTicket(idTicketEmpezar);
                int estadoSeleccionado = ticket.EstadoTecnico;
                ViewBag.ListaPrio = new SelectList(estado, "IdEstado", "Estado", estadoSeleccionado);

                return View(ticket);
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                
        }

        public IActionResult ActualizarEstadoTicket(int estadoAnterior, int NuevoEstado, string comentario, int ticket, string? mensaje)
        {
            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
            if (id != 0 && tipoUsr == 2)
            {
                Ticket ticket1 = new Ticket();
                Notificaciones notificaciones = new Notificaciones();
                ticket1 = _tickets.ObtenerTicket(ticket);
                if (NuevoEstado == 4)
                {
                    notificaciones.IdTicket = ticket;
                    notificaciones.IdCliente = ticket1.IdCliente;
                    notificaciones.IdTecnico = ticket1.IdTecnico;
                    mensaje = "Su Ticket N° " + ticket + " ha sido finalizado, comentario del tecnico: " + mensaje;
                    notificaciones.Bitacora = mensaje;
                    notificaciones.Visto = false;
                    var response2 = _tickets.UpdateNotificacion(notificaciones);
                }
                var response = _tickets.UpdateTicket(id,estadoAnterior, NuevoEstado, comentario, ticket);
                if (response == "1")
                {
                    
                    if (mensaje==null && NuevoEstado==2)
                    {
                        notificaciones.IdTicket = ticket;
                        notificaciones.IdCliente = ticket1.IdCliente;
                        notificaciones.IdTecnico = ticket1.IdTecnico;
                        mensaje = "Su ticket N° "+ticket+" esta siendo atendido";
                        notificaciones.Bitacora = mensaje;
                        notificaciones.Visto = false;
                        var response2 = _tickets.UpdateNotificacion(notificaciones);
                    }
                    
                    
                    return RedirectToAction("Index", "TicketTecnico");
                }
                else
                {
                    return RedirectToAction("Inicio", "Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                
        }

        public IActionResult Peticion(int idTicket, string motivo)
        {
            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
            if (id != 0 && tipoUsr == 2)
            {
                var ticket = new Ticket();
                ticket = _tickets.ObtenerTicket(idTicket);
                var peticion = new Peticion();
                peticion.IdTicket = idTicket;
                peticion.IdTecnico = ticket.IdTecnico;
                peticion.EstadoTicket = ticket.EstadoTecnico;
                peticion.Mensaje = motivo;
                var response = _tickets.InsPeticion(peticion);

                return RedirectToAction("Index", "TicketTecnico");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
               
        }



    }
}
