using Dapper;
using Proyecto_Tickets.Models;
using System.Net.Sockets;

namespace Proyecto_Tickets.Data
{
    public class InicioSesion : IUserService
    {
        private readonly Conexion _conexion;

        public InicioSesion(Conexion conexion)
        {
            _conexion = conexion;
        }

        public Cliente GetClienteById(int id)
        {
            throw new NotImplementedException();
        }

        public Cliente ValidateCliente(string username, string password)
        {
            Cliente cliente = new Cliente();  
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@Correo", username);
                parametros.Add("@Contraseña", password);
                string sql = "SELECT * FROM Cliente WHERE correo = @Correo AND contrasena = @Contraseña";
                cliente = conexion.QuerySingleOrDefault<Cliente>(sql, parametros);
                return (cliente);

            }
        }
    }
}
