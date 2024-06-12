using Dapper;
using Proyecto_Tickets.Models;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Proyecto_Tickets.Data
{
    public class ImpleTicket : ITickets
    {
        private readonly Conexion _conexion;

        public ImpleTicket(Conexion conexion)
        {
            _conexion = conexion;
        }

        public List<int> Data()
        {
            List<int> list = new List<int>();
            Tecnico tecnico = new Tecnico();
            Ticket ticket = new Ticket();
            using (var conexion = _conexion.ObtenerConexion())
            {
                string sql = "SELECT * FROM Tecnico ORDER BY cargaTrabajo ASC";
                tecnico = conexion.QueryFirstOrDefault<Tecnico>(sql);
                list.Add(tecnico.IdTecnico);
                sql = "SELECT TOP 1 * FROM Ticket ORDER BY idTicket DESC"; // Suponiendo que el identificador único se llama ID
                ticket = conexion.QueryFirstOrDefault<Ticket>(sql);
                list.Add(ticket.IdTicket+1);

            }
            return list;
        }

        public List<Prioridad> ObtenerPrio()
        {
            List<Prioridad> listaPrio = new List<Prioridad>();

            using (var conexion = _conexion.ObtenerConexion())
            {
                string sql = "SELECT * FROM Prioridad";
                listaPrio = conexion.Query<Prioridad>(sql).ToList();
            }
            return listaPrio;
        }

        public Ticket ObtenerTicket(int id)
        {
            Ticket ticket = new Ticket();
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@idTicket",id);
                string sql = "SELECT * FROM Ticket WHERE idTicket = @idTicket";
                ticket= conexion.QuerySingleOrDefault<Ticket>(sql, parametros);
                return (ticket);

            }
            
        }

        public string InsertTicket(Ticket ticket)
        {
            var respuesta = "";
            using (var conexion = _conexion.ObtenerConexion())
            {

                var parametros = new DynamicParameters();
                parametros.Add("@idTicket", ticket.IdTicket);
                parametros.Add("@descripcion", ticket.Descripcion);
                parametros.Add("@idCliente", ticket.IdCliente);
                parametros.Add("@idTecnico", ticket.IdTecnico);
                parametros.Add("@idPrioridad", ticket.IdPrioridad);
                parametros.Add("@bloqueado", ticket.Bloqueado);
                parametros.Add("@EstadoCliente", ticket.EstadoCliente);
                parametros.Add("@EstadoTecnico", ticket.EstadoTecnico);
                string sql = "INSERT INTO Ticket (idTicket, descripcion, idCliente, idTecnico, idPrioridad, bloqueado, estadoCliente, estadoTecnico) " +
                             "VALUES (@idTicket, @descripcion, @idCliente, @idTecnico, @idPrioridad, @bloqueado, @EstadoCliente,@EstadoTecnico)";
                var response = conexion.Execute(sql, parametros);

                var parametros2 = new DynamicParameters();
                parametros2.Add("@aumentoCargaTrabajo", 1);
                parametros2.Add("@idTecnico", ticket.IdTecnico);
                
                 sql= "UPDATE Tecnico SET cargaTrabajo = ISNULL(cargaTrabajo, 0) + @aumentoCargaTrabajo WHERE idTecnico = @idTecnico";
                var response2= conexion.Execute(sql, parametros2);
                if (response==1 && response2==1)
                {
                    respuesta = "OK";
                }
            }
            return respuesta;
        }

        public List<Ticket> ObtenerTickets(int id)
        {

            List<Ticket> tickets = new List<Ticket>();

            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdCliente", id);

                string sql = "SELECT * FROM Ticket WHERE idCliente = @IdCliente";

                var result = conexion.Query<Ticket>(sql, parametros);

                foreach (var ticket in result)
                {
                    var parametros2 = new DynamicParameters();
                    parametros2.Add("@IdTecnico", ticket.IdTecnico);
                    string sql2 = "SELECT * FROM Tecnico WHERE idTecnico = @IdTecnico";

                    var result2 = conexion.QuerySingleOrDefault<Tecnico>(sql2, parametros2);
                    // Realiza alguna lógica aquí si es necesario
                    // Por ejemplo:
                    ticket.Tecnico = result2.Nombre;

                    // Agrega el ticket modificado a la lista
                    tickets.Add(ticket);
                }
            }

            return tickets;
        }

        public List<Ticket> ObtenerTicketsTecnico(int id)
        {
            List<Ticket> tickets = new List<Ticket>();

            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdTecnico", id);

                string sql = "SELECT * FROM Ticket WHERE idTecnico = @IdTecnico";

                tickets = conexion.Query<Ticket>(sql,parametros).ToList();

            }
            return tickets;
        }

        public List<EstadoCliente> ObetnerEstCiente()
        {
            List<EstadoCliente> listaCliente = new List<EstadoCliente>();

            using (var conexion = _conexion.ObtenerConexion())
            {
                string sql = "SELECT * FROM EstadoCliente";
                listaCliente = conexion.Query<EstadoCliente>(sql).ToList();
            }
            return listaCliente;
        }

        public List<EstadoTecnico> ObetnerEstTecnico()
        {
            List<EstadoTecnico> listaTecnico = new List<EstadoTecnico>();

            using (var conexion = _conexion.ObtenerConexion())
            {
                string sql = "SELECT * FROM EstadoTecnico";
                listaTecnico = conexion.Query<EstadoTecnico>(sql).ToList();
            }
            return listaTecnico;
        }

        public string UpdateTicket(int tecnico, int estadoAnterior, int NuevoEstado, string comentario, int ticket)
        {

            
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@NuevoEstado", NuevoEstado);
                parametros.Add("@Comentario", comentario);
                parametros.Add("@IdTicket", ticket);
                string sql = "";
                if (estadoAnterior == 1)
                {
                    var parametros2 = new DynamicParameters();
                    parametros2.Add("@idTecnico", tecnico);
                    string query = "UPDATE Ticket SET estadoTecnico = 3 WHERE idTecnico = @idTecnico AND idPrioridad < 5 AND estadoTecnico = 2 ";
                    var response2 = conexion.Execute(query, parametros2);
                }


                sql = "UPDATE Ticket SET estadoTecnico = @NuevoEstado, ComentarioTec = @Comentario";

                // Si el NuevoEstado no es igual a 3, también actualizamos el EstadoClie
                if (NuevoEstado != 3)
                {
                    sql += ", estadoCliente = @NuevoEstado";
                }

                sql += " WHERE idTicket = @IdTicket";
                var response = conexion.Execute(sql, parametros);

                
                if (NuevoEstado==4)
                {
                    var parametros2 = new DynamicParameters();
                    parametros2.Add("@idTicket", ticket);
                    string query = "DELETE FROM Ticket WHERE idTicket = @idTicket";
                    var response2 = conexion.Execute(query, parametros2);
                }

                
                
                return response.ToString();
            }
            

        }

        public List<Peticion> Peticiones()
        {
            List<Peticion> peticiones = new List<Peticion>();

            using (var conexion = _conexion.ObtenerConexion())
            {
        

                string sql = "SELECT * FROM Peticion";

                peticiones = conexion.Query<Peticion>(sql).ToList();
 
            }
            return peticiones;
        }

        public List<Tecnico> ObetnerTecnicos()
        {
            List<Tecnico> tecnicos = new List<Tecnico>();

            using (var conexion = _conexion.ObtenerConexion())
            {


                string sql = "SELECT * FROM Tecnico";

                tecnicos = conexion.Query<Tecnico>(sql).ToList();

            }
            return tecnicos;
        }

        public string InsPeticion(Peticion peticion)
        {
            var respuesta = "";
            using (var conexion = _conexion.ObtenerConexion())
            {

                var parametros = new DynamicParameters();
                parametros.Add("@idTicket", peticion.IdTicket);
                parametros.Add("@idTecnico", peticion.IdTecnico);
                parametros.Add("@estadoTicket", peticion.EstadoTicket);
                parametros.Add("@mensaje", peticion.Mensaje);
                string sql = "INSERT INTO Peticion (idTicket,idTecnico, estadoTicket, mensaje) " +
                             "VALUES (@idTicket,@idTecnico, @estadoTicket, @mensaje)";
                var response = conexion.Execute(sql, parametros);
                if (response == 1)
                {
                    respuesta = "OK";
                }
            }
            return respuesta;
        }

        public Peticion ObtenerPeticion(int id)
        {
            Peticion peticion = new Peticion();
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@idTicket", id);
                string sql = "SELECT * FROM Peticion WHERE idTicket = @idTicket";
                peticion = conexion.QuerySingleOrDefault<Peticion>(sql, parametros);
                return (peticion);

            }
        }

        public string UpdateEstadoAdmin(int ticket, int tecAn, int tec, int est)
        {
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                bool cambio = false;
                parametros.Add("@idTicket", ticket);
                if (tecAn!=tec)
                {
                    parametros.Add("@idTecnico", tec);
                    cambio = true;
                }
                else
                {
                    parametros.Add("@idTecnico", tecAn);
                }
                
                parametros.Add("@estadoTecnico", est);

                string sql = "UPDATE Ticket SET idTecnico = @idTecnico, estadoTecnico = @estadoTecnico";

                sql += " WHERE idTicket = @IdTicket";

                var response = conexion.Execute(sql, parametros);
                
                
                
                var parametros2 = new DynamicParameters();
                var parametros3 = new DynamicParameters();
                if (cambio==true)
                {
                    parametros2.Add("@idTecnico", tec);
                    parametros2.Add("@Valor", 1);
                    sql = "UPDATE Tecnico SET cargaTrabajo = CargaTrabajo + @Valor WHERE IdTecnico = @IdTecnico";
                    var response2 = conexion.Execute(sql, parametros2);

                    parametros3.Add("@idTecnico", tecAn);
                    parametros3.Add("@Valor", 1);
                    sql = "UPDATE Tecnico SET cargaTrabajo = CargaTrabajo - @Valor WHERE IdTecnico = @IdTecnico";
                    var response3 = conexion.Execute(sql, parametros3);
                }

                var parametros4 = new DynamicParameters();

                parametros4.Add("@idTicket", ticket);

                sql = "DELETE FROM Peticion WHERE idTicket = @idTicket";
                var response4 = conexion.Execute(sql, parametros4);

                return response.ToString();
            }

        }

        public string CrearUsuario(Cliente? cliente, Tecnico? tecnico, Administrador? administrador)
        {
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                string tabla = string.Empty;
                string sqlObtenerUltimoId = string.Empty;
                string sqlInsertarUsuario = string.Empty;
                string campoId = string.Empty;
                int nuevoId;

                // Determinar el tipo de usuario y la tabla correspondiente
                if (cliente != null)
                {
                    tabla = "Cliente";
                    parametros.Add("@Nombre", cliente.Nombre);
                    parametros.Add("@Correo", cliente.Correo);
                    parametros.Add("@Contraseña", cliente.Contraseña);
                    parametros.Add("@Empresa", cliente.NomEmpresa);
                    campoId = "idCliente";
                }
                else if (tecnico != null)
                {
                    tabla = "Tecnico";
                    parametros.Add("@Nombre", tecnico.Nombre);
                    parametros.Add("@Correo", tecnico.Correo);
                    parametros.Add("@Contraseña", tecnico.Contraseña);
                    campoId = "idTecnico";
                }
                else if (administrador != null)
                {
                    tabla = "Administrador";
                    parametros.Add("@Nombre", administrador.Nombre);
                    parametros.Add("@Correo", administrador.Correo);
                    parametros.Add("@Contraseña", administrador.Contraseña);
                    campoId = "idAdmin";
                }

                if (string.IsNullOrEmpty(tabla))
                {
                    throw new ArgumentException("No se ha proporcionado un usuario válido.");
                }

                // Obtener el último ID de la tabla correspondiente
                sqlObtenerUltimoId = $"SELECT ISNULL(MAX({campoId}), 0) FROM {tabla}";
                nuevoId = conexion.QuerySingle<int>(sqlObtenerUltimoId) + 1;

                // Insertar el nuevo usuario en la tabla correspondiente
                int filasAfectadas = 0;
                if (cliente!=null)
                {
                    parametros.Add("@Id", nuevoId);
                    sqlInsertarUsuario = $"INSERT INTO {tabla} ({campoId}, nombre, correo, contrasena,nomEmpresa) VALUES (@Id, @Nombre, @Correo, @Contraseña,@Empresa)";
                    filasAfectadas = conexion.Execute(sqlInsertarUsuario, parametros);

                }
                else
                {
                    parametros.Add("@Id", nuevoId);
                    sqlInsertarUsuario = $"INSERT INTO {tabla} ({campoId}, nombre, correo, contrasena) VALUES (@Id, @Nombre, @Correo, @Contraseña)";

                    filasAfectadas = conexion.Execute(sqlInsertarUsuario, parametros);
                }
                

                if (filasAfectadas > 0)
                {
                    return "Usuario creado con éxito.";
                }
                else
                {
                    return "Error al crear el usuario.";
                }
            }
        }

        public string UpdateNotificacion(Notificaciones notificaciones)
        {
            using (var conexion = _conexion.ObtenerConexion())
            {

                var parametros = new DynamicParameters();
                parametros.Add("@idTicket", notificaciones.IdTicket);
                parametros.Add("@idCliente", notificaciones.IdCliente);
                parametros.Add("@idTecnico", notificaciones.IdTecnico);
                parametros.Add("@bitacora", notificaciones.Bitacora);
                parametros.Add("@visto", notificaciones.Visto);
                
                string sql = "INSERT INTO Notificaciones (idTicket, idCliente, idTecnico, bitacora, visto) " +
                             "VALUES (@idTicket,@idCliente, @idTecnico, @bitacora, @visto)";
                var response = conexion.Execute(sql, parametros);

                
               
               return response.ToString();
            }
        }

        public List<Notificaciones> ObtenerNotificaciones(int id)
        {
            List<Notificaciones> notificaciones = new List<Notificaciones>();

            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdCliente", id);

                string sql = "SELECT * FROM Notificaciones WHERE idCliente = @IdCliente AND visto=0";

                 notificaciones = conexion.Query<Notificaciones>(sql, parametros).ToList();



            }
            return notificaciones;
        }

        public Notificaciones Notifificacion(int id)
        {
            Notificaciones notificaciones = new Notificaciones();

            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IdNotificacion", id);

                string sql = "SELECT * FROM Notificaciones WHERE idNotificacion = @IdNotificacion";

                notificaciones = conexion.QuerySingleOrDefault<Notificaciones>(sql, parametros);



            }
            return notificaciones;

        }

        public string UpdateEstadoNoti(int notificacion)
        {
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                parametros.Add("@idNotificacion", notificacion);
                string query = "UPDATE Notificaciones SET visto = 1   WHERE idNotificacion = @idNotificacion";
                var response2 = conexion.Execute(query, parametros);
                return response2.ToString();
            }
        }

        public List<Ticket> AllTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            using (var conexion = _conexion.ObtenerConexion())
            {
                var parametros = new DynamicParameters();
                string sql = "SELECT * FROM Ticket";
                tickets = conexion.Query<Ticket>(sql, parametros).ToList();
            }
            return tickets;
        }

        public List<Usuario> AllClientes()
        {
            var usuario = new List<Usuario>();
            string query = "SELECT idCliente, nombre, correo, contrasena FROM Cliente";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.Add(new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("idCliente")),
                            nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            contraseña = reader.GetString(reader.GetOrdinal("contrasena"))
                        });
                    }
                }
            }

            return usuario;
        }

        public List<Usuario> AllTecnicos()
        {
            var usuario = new List<Usuario>();
            string query = "SELECT idTecnico, nombre, correo, contrasena FROM Tecnico";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.Add(new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("idTecnico")),
                            nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            contraseña = reader.GetString(reader.GetOrdinal("contrasena"))
                        });
                    }
                }
            }

            return usuario;
        }

        public List<Usuario> AllAdministradores()
        {
            var usuario = new List<Usuario>();
            string query = "SELECT idAdmin, nombre, correo, contrasena FROM Administrador";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.Add(new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("idAdmin")),
                            nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            contraseña = reader.GetString(reader.GetOrdinal("contrasena"))
                        });
                    }
                }
            }

            return usuario;
        }

        public List<Usuario> AllUsuarios()
        {
            var allTickets = new List<Usuario>();

            allTickets.AddRange(AllClientes());
            allTickets.AddRange(AllTecnicos());
            allTickets.AddRange(AllAdministradores());

            return allTickets;
        }

        public Cliente GetClienteById(int id)
        {
            Cliente cliente = null;
            string query = "SELECT idCliente, nombre, correo, contrasena FROM Cliente WHERE idCliente = @id";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            IdCliente = reader.GetInt32(reader.GetOrdinal("idCliente")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            Contraseña = reader.GetString(reader.GetOrdinal("contrasena"))
                        };
                    }
                }
            }

            return cliente;
        }

        public void DeleteCliente(int id)
        {
            string query = "DELETE FROM Cliente WHERE idCliente = @id";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public Tecnico GetTecnicoById(int id)
        {
            Tecnico tecnico = null;
            string query = "SELECT idTecnico, nombre, correo, contrasena FROM Tecnico WHERE idTecnico = @id";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tecnico = new Tecnico
                        {
                            IdTecnico = reader.GetInt32(reader.GetOrdinal("idTecnico")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            Contraseña = reader.GetString(reader.GetOrdinal("contrasena"))
                        };
                    }
                }
            }

            return tecnico;
        }

        public void DeleteTecnico(int id)
        {
            string query = "DELETE FROM Tecnico WHERE idTecnico = @id";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public Administrador GetAdministradorById(int id)
        {
            Administrador administrador = null;
            string query = "SELECT idAdmin, nombre, correo, contrasena FROM Administrador WHERE idAdmin = @id";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        administrador = new Administrador
                        {
                            IdAdmin = reader.GetInt32(reader.GetOrdinal("idAdmin")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            Contraseña = reader.GetString(reader.GetOrdinal("contrasena"))
                        };
                    }
                }
            }

            return administrador;
        }

        public void DeleteAdministrador(int id)
        {
            string query = "DELETE FROM Administrador WHERE idAdmin = @id";

            using (var connection = _conexion.ObtenerConexion())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
