namespace Proyecto_Tickets.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int TipoUsuario { get; set; }
        public string nombre { get; set; }
        public string Correo { get; set; }
        public string contraseña { get; set; }
    }
}
