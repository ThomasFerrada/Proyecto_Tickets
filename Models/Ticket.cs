namespace Proyecto_Tickets.Models
{
    public class Ticket
    {
        public int IdTicket { get; set; }
        public string Descripcion { get; set; }
        public int IdCliente { get; set; }
        public int IdTecnico { get; set; }
        public int IdPrioridad { get; set; }
        public string Bloqueado { get; set; }
        public int EstadoCliente { get; set; }
        public int EstadoTecnico { get; set; }
        public string? Tecnico { get; set; }
        public string? EstCli { get; set; }
        public string? EstTec { get; set; }
        public string? ComentarioTec { get; set; }
        public string? Problema { get; set; }

        


    }
}
