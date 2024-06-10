using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Data
{
    public interface IUserService
    {
        Usuario ValidateUsuario(string username, string password);
        Cliente GetClienteById(int id);

        string GetUsuario(int? tipo,int? id);

    }
}
