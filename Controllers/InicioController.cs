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
            var cliente=_userService.ValidateCliente(correo, pwd);
            
            if (cliente != null)
            {
                HttpContext.Session.SetInt32("UserId", cliente.IdCliente);

                // Redirige a la vista de inicio o a otra vista
                return RedirectToAction("Index", "Ticket");
            }
            else
            {
                return View();
            }
            
        }


    }
}
