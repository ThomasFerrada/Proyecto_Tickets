using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Tickets.Data;
using Proyecto_Tickets.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
            //

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
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");

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
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");

            var id = HttpContext.Session.GetInt32("UserId") ?? 0;
            var tipoUsr = HttpContext.Session.GetInt32("TipoUsuario");
            if (id != 0 && tipoUsr == 3)
            {
                var response = _ticket.UpdateEstadoAdmin(ticket, tecAn, tec, est);



                return RedirectToAction("Index", "Administrador");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
                
        }

        public IActionResult CrearUsuario(int? tipo, string? nombre,string? correo, string? pwd) {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");

            if (tipo == null && nombre==null&& correo == null && pwd == null)
            {
                return View();
            }
            var response = "";
            if (tipo == 1)
            {
                Administrador administrador = new Administrador();
                administrador.Nombre = nombre;
                administrador.Correo = correo;
                administrador.Contraseña = pwd;
                response=_ticket.CrearUsuario(null,null,administrador);
            }
            if (tipo == 2)
            {
                Tecnico tecnico = new Tecnico();
                tecnico.Nombre = nombre;
                tecnico.Correo = correo;
                tecnico.Contraseña= pwd;
                response = _ticket.CrearUsuario(null,tecnico,null);
            }
            return View();

        }
    }
}
