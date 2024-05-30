using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Data
{
    public interface IUserService
    {
        Cliente ValidateCliente(string username, string password);
        Cliente GetClienteById(int id);
    }
}
