using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Proyecto_Tickets.Data;
using System.Diagnostics.Eventing.Reader;
using Proyecto_Tickets.Models;
using Microsoft.AspNetCore.Http.Timeouts;

namespace Proyecto_Tickets.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITickets _tickets;

        public InicioController(IUserService userService, ITickets tickets)
        {
            _userService = userService;
            _tickets = tickets;
        }

        public ActionResult Index()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string pwd)
        {
            var usuario=_userService.ValidateUsuario(correo, pwd);
            
            if (usuario != null)
            {
                HttpContext.Session.SetInt32("UserId", usuario.Id);
                HttpContext.Session.SetInt32("TipoUsuario", usuario.TipoUsuario);
                if (usuario.TipoUsuario==1)
                {
                    var nombre = _userService.GetUsuario(1, usuario.Id);

                    
                    HttpContext.Session.SetString("Nombre", nombre);
                    ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
                    return RedirectToAction("misTickets", "Ticket");
                }
                else if(usuario.TipoUsuario == 2)
                {
                    var nombre = _userService.GetUsuario(2, usuario.Id);
                    
                    HttpContext.Session.SetString("Nombre", nombre);
                    ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
                    return RedirectToAction("Index", "TicketTecnico");
                }
                else if (usuario.TipoUsuario == 3)
                {
                    var nombre = _userService.GetUsuario(3, usuario.Id);
                    
                    HttpContext.Session.SetString("Nombre", nombre);
                    ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre");
                    return RedirectToAction("Index", "Administrador");
                }
                else
                
                {
                    return RedirectToAction("Index", "Ticket");
                }

                // Redirige a la vista de inicio o a otra vista
                
            }
            else
            {
                return View();
            }
            
        }

        public IActionResult CrearCliente(string? nombre, string? correo, string? pwd, string? empresa)
        {
            if (empresa == null && nombre == null && correo == null && pwd == null)
            {
                return View();
            }
            
            
            
                Cliente cliente = new Cliente();
                cliente.Nombre = nombre;
                cliente.Correo = correo;
                cliente.Contraseña = pwd;
                cliente.NomEmpresa = empresa;

               var response = _tickets.CrearUsuario(cliente, null, null);
            
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            // Redirigir al usuario a la página de inicio de sesión o a otra página
            return RedirectToAction("Index", "Inicio");
        }

    }
}
