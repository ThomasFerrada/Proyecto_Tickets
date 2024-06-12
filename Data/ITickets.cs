using Proyecto_Tickets.Models;

namespace Proyecto_Tickets.Data
{
    public interface ITickets
    {
        Ticket ObtenerTicket(int id);
        Notificaciones Notifificacion(int id);
        Peticion ObtenerPeticion(int id);
        List<Ticket> ObtenerTickets(int id);
        List<Ticket> AllTickets();
        List<Usuario> AllClientes();
        List<Usuario> AllAdministradores();
        List<Usuario> AllTecnicos();
        List<Usuario> AllUsuarios();
        List<Ticket> ObtenerTicketsTecnico(int id);
        List<Notificaciones> ObtenerNotificaciones(int id);
        string InsertTicket(Ticket ticket);
        string CrearUsuario(Cliente? cliente, Tecnico? tecnico, Administrador? administrador);
        string InsPeticion(Peticion peticion);
        string UpdateNotificacion(Notificaciones notificaciones);
        string UpdateEstadoNoti(int notificacion);
        string UpdateTicket(int tecnico,int estadoAnterior, int NuevoEstado, string comentario, int ticket);

        

        string UpdateEstadoAdmin(int ticket, int tecAn, int tec,  int est);
        
        List<int> Data();
        List<Prioridad> ObtenerPrio();
        List<Peticion> Peticiones();
        List<EstadoCliente> ObetnerEstCiente();
        List<EstadoTecnico> ObetnerEstTecnico();
        List<Tecnico> ObetnerTecnicos();




        

    }
}
