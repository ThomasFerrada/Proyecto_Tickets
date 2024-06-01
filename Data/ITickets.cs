using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Data
{
    public interface ITickets
    {
        Ticket ObtenerTicket(int id);
        List<Ticket> ObtenerTickets(int id);
        List<Ticket> ObtenerTicketsTecnico(int id);
        string InsertTicket(Ticket ticket);
        string UpdateTicket(int NuevoEstado, string comentario, int ticket);
        

        List<int> Data();
        List<Prioridad> ObtenerPrio();
        List<EstadoCliente> ObetnerEstCiente();
        List<EstadoTecnico> ObetnerEstTecnico();


        

    }
}
