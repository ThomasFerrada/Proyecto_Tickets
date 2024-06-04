namespace Proyecto_Tickets.Models
{
    public class Peticion
    {
        public int IdTicket { get; set; } 
        public int IdTecnico { get; set; } 
        public int EstadoTicket { get; set; } 
        public string Mensaje { get; set; } 
    }
}
