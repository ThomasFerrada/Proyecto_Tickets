using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly ITickets _ticket;
        public AdministradorController(ITickets ticket)
        {
            _ticket = ticket;
        }

        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            if (id != 0 && tipoUsr == 3)
            {
                var view = new ListaPeticion()
                {
                    ListaPet = _ticket.Peticiones(),
                    ListaTec = _ticket.ObetnerTecnicos(),
                    ListaEstado = _ticket.ObetnerEstTecnico()
                };
                return View(view);
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                

            
        }

        public IActionResult Examinar(int idTicketExaminar) {

            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            if (id != 0 && tipoUsr == 3)
            {
                var peticion = _ticket.ObtenerPeticion(idTicketExaminar);
                List<Tecnico> tecnicos = new List<Tecnico>();
                tecnicos = _ticket.ObetnerTecnicos();
                var tecSele = tecnicos
                        .Where(tec => tec.IdTecnico == peticion.IdTecnico) // Filtra los problemas cuyo Id coincide con el IdProblema del ticket
                        .Select(tecSele => tecSele.Nombre) // Proyecta el string asociado al problema encontrado
                        .FirstOrDefault();
                ViewBag.ListTec = new SelectList(tecnicos, "IdTecnico", "Nombre", peticion.IdTecnico);
                ViewBag.Tec = peticion.IdTecnico;

                List<EstadoTecnico> estado = new List<EstadoTecnico>();
                estado = _ticket.ObetnerEstTecnico();
                var estSele = estado
                        .Where(est => est.IdEstado == peticion.EstadoTicket) // Filtra los problemas cuyo Id coincide con el IdProblema del ticket
                        .Select(estSele => estSele.Estado) // Proyecta el string asociado al problema encontrado
                        .FirstOrDefault();
                ViewBag.ListaEst = new SelectList(estado, "IdEstado", "Estado", peticion.EstadoTicket);
                return View(peticion);
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                
        }

        public IActionResult Actualizar(int ticket, int tecAn, int tec, int est)
        {

            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            if (id != 0 && tipoUsr == 3)
            {
                var response = _ticket.UpdateEstadoAdmin(ticket, tecAn, tec, est);



                return View();
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                
        }
    }
}
