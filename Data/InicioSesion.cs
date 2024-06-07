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

        //public Cliente ValidateCliente(string username, string password)
        //{
        //    Cliente cliente = new Cliente();  
        //    using (var conexion = _conexion.ObtenerConexion())
        //    {
        //        var parametros = new DynamicParameters();
        //        parametros.Add("@Correo", username);
        //        parametros.Add("@Contraseña", password);
        //        string sql = "SELECT * FROM Cliente WHERE correo = @Correo AND contrasena = @Contraseña";
        //        cliente = conexion.QuerySingleOrDefault<Cliente>(sql, parametros);
        //        return (cliente);

        //    }
        //}

        public Usuario ValidateUsuario(string username, string password)
        {
            Usuario usuario = new Usuario();
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@Correo", username);
                parametros.Add("@Contraseña", password);
                string sql = "SELECT * FROM Cliente WHERE correo = @Correo AND contrasena = @Contraseña";
                var response = conexion.QuerySingleOrDefault<Cliente>(sql, parametros);
                if (response == null)
                {
                    sql = "SELECT * FROM Tecnico WHERE correo = @Correo AND contrasena = @Contraseña";
                    var response2 = conexion.QuerySingleOrDefault<Tecnico>(sql, parametros);

                    if (response2==null)
                    {
                        sql = "SELECT * FROM Administrador WHERE correo = @Correo AND contrasena = @Contraseña";
                        var response3 = conexion.QuerySingleOrDefault<Administrador>(sql, parametros);
                        if (response3==null)
                        {
                            usuario = null;
                        }
                        else
                        {
                            usuario.Id = response3.IdAdmin;
                            usuario.TipoUsuario = 3;
                            usuario.Correo = response3.Correo;
                            usuario.contraseña = response3.Contraseña;
                        }
                    }
                    else
                    {
                        usuario.Id = response2.IdTecnico;
                        usuario.TipoUsuario = 2;
                        usuario.Correo = response2.Correo;
                        usuario.contraseña = response2.Contraseña;
                    }
                }
                else
                {
                    usuario.Id = response.IdCliente;
                    usuario.TipoUsuario = 1;
                    usuario.Correo = response.Correo;
                    usuario.contraseña = response.Contraseña;
                }

            }
            return usuario;
        }
    }
}
