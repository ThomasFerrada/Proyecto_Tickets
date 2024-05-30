namespace Proyecto_Tickets.Models
{
    public class Ticket
    {
        public int idTicket { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int IdCliente { get; set; }
        public int IdTecnico { get; set; }
        public int IdPrioridad { get; set; }
        public string Bloqueado { get; set; }
        public string? Tecnico { get; set; }

        


    }
}
