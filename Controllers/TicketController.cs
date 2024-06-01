﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITickets _ticket;
        private readonly IUserService _userService;
        public TicketController(ITickets ticket, IUserService userService) {
            
            _ticket = ticket;
            _userService = userService;
        }

      

        public IActionResult Index()
        {
            var idCliente = HttpContext.Session.GetInt32("UserId");
            if (idCliente != null) {
                List<Prioridad> list = new List<Prioridad>();
                list = _ticket.ObtenerPrio();
                ViewBag.ListaPrio = new SelectList(list, "IdPrioridad", "Categoria");
                return View();
            }
            else
            {
                return RedirectToAction("Inicio", "Index");
            }

            
        }

        public ActionResult misTickets()
        {
            var idCliente = HttpContext.Session.GetInt32("UserId") ?? 0;
            List<Ticket> list = new List<Ticket>();
            List<EstadoCliente> estado = new List<EstadoCliente>();
            List<Prioridad> prob = new List<Prioridad>();
            prob = _ticket.ObtenerPrio();
            estado =_ticket.ObetnerEstCiente();
            list = _ticket.ObtenerTickets(idCliente);
            foreach (var item in list)
            {
                var id = item.IdPrioridad;
                var problemaEncontrado = prob
                    .Where(Categoria => Categoria.IdPrioridad == id) // Filtra los problemas cuyo Id coincide con el IdProblema del ticket
                    .Select(Categoria => Categoria.Categoria) // Proyecta el string asociado al problema encontrado
                    .FirstOrDefault();
                item.Problema=problemaEncontrado;

                var idestado = item.EstadoCliente;
                var estadocl = estado
                    .Where(estcl => estcl.IdEstado == idestado) // Filtra los problemas cuyo Id coincide con el IdProblema del ticket
                    .Select(estadocl => estadocl.Estado) // Proyecta el string asociado al problema encontrado
                    .FirstOrDefault();

                item.EstCli = estadocl;

            }
            
            return View(list);
        }

        [HttpPost]
        public ActionResult ArmarTicket(string descripcion, int prioridad)
        {
            Ticket ticket= new Ticket();
            var idTecnico = 0;
            var idTicket = 0;
            var data = _ticket.Data();
            for (var i = 0; i < data.Count; i++) {
                if (i==0)
                {
                    idTecnico = data[i];
                }
                else
                {
                    idTicket = data[i];
                }
            }
            var idCliente = HttpContext.Session.GetInt32("UserId");
            var idPrioridad = prioridad;
            ticket.IdTicket = idTicket;
            ticket.Descripcion = descripcion;
            ticket.IdCliente = idCliente ?? 0;
            ticket.IdTecnico=idTecnico;
            ticket.IdPrioridad = idPrioridad;
            ticket.Bloqueado = "N";
            ticket.EstadoCliente= 1;
            ticket.EstadoTecnico = 1;

            var response= _ticket.InsertTicket(ticket);
            if (response=="OK")
            {
                return RedirectToAction("Ticket", "Index");
            }
            else
            {
                return RedirectToAction("Ticket", "Index");
            }

            
        }

        


    }
}
