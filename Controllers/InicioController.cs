using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Proyecto_Tickets.Data;

namespace Proyecto_Tickets.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUserService _userService;

        public InicioController(IUserService userService)
        {
            _userService = userService;
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
                    return RedirectToAction("Index", "Ticket");
                }
                else if(usuario.TipoUsuario == 2)
                {
                    return RedirectToAction("Index", "TicketTecnico");
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


    }
}
