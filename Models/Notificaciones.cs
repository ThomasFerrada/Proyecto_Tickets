namespace Proyecto_Tickets.Models
{
    public class Notificaciones
    {

        public int IdNotificacion { get; set; }
        public int IdTicket { get; set; }
        public int IdCliente { get; set; }
        public int IdTecnico { get; set; }
        public string Bitacora { get; set; }
        public bool Visto { get; set; }
    }
}
