using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Data
{
    public interface ITickets
    {
        Ticket ObtenerTicket(int id);
        Peticion ObtenerPeticion(int id);
        List<Ticket> ObtenerTickets(int id);
        List<Ticket> ObtenerTicketsTecnico(int id);
        string InsertTicket(Ticket ticket);
        string CrearUsuario(Cliente? cliente,Tecnico? tecnico,Administrador? administrador);
        string InsPeticion(Peticion peticion);
        string UpdateTicket(int NuevoEstado, string comentario, int ticket);

        string UpdateEstadoAdmin(int ticket, int tecAn, int tec,  int est);
        
        List<int> Data();
        List<Prioridad> ObtenerPrio();
        List<Peticion> Peticiones();
        List<EstadoCliente> ObetnerEstCiente();
        List<EstadoTecnico> ObetnerEstTecnico();
        List<Tecnico> ObetnerTecnicos();




        

    }
}
