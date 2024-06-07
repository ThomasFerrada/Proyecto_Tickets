namespace Proyecto_Tickets.Models
{
    public class ListaPeticion
    {
        public List<Peticion> ListaPet { get; set; }
        public List<Tecnico> ListaTec { get; set; }
        public List<EstadoTecnico> ListaEstado { get; set; }
    }
}
