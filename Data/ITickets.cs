using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Data
{
    public interface ITickets
    {
        Ticket ObtenerTicket(int id);
        List<Ticket> ObtenerTickets(int id);
        string InsertTicket(Ticket ticket);

        List<int> Data();
        List<Prioridad> ObtenerPrio();

    }
}
