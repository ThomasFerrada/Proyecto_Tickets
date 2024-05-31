using Dapper;
using Proyecto_Tickets.Models;

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
                list.Add(ticket.idTicket+1);

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
            throw new NotImplementedException();
        }

        public string InsertTicket(Ticket ticket)
        {
            var respuesta = "";
            using (var conexion = _conexion.ObtenerConexion())
            {

                var parametros = new DynamicParameters();
                parametros.Add("@idTicket", ticket.idTicket);
                parametros.Add("@descripcion", ticket.Descripcion);
                parametros.Add("@estado", ticket.Estado);
                parametros.Add("@idCliente", ticket.IdCliente);
                parametros.Add("@idTecnico", ticket.IdTecnico);
                parametros.Add("@idPrioridad", ticket.IdPrioridad);
                parametros.Add("@bloqueado", ticket.Bloqueado);
                string sql = "INSERT INTO Ticket (idTicket, descripcion, estado, idCliente, idTecnico, idPrioridad, bloqueado) " +
                             "VALUES (@idTicket, @descripcion, @estado, @idCliente, @idTecnico, @idPrioridad, @bloqueado)";
                var response = conexion.Execute(sql, parametros);

                var parametros2 = new DynamicParameters();
                parametros2.Add("@aumentoCargaTrabajo", 1);
                parametros2.Add("@idTecnico", ticket.IdTecnico);
                
                 sql= "UPDATE Tecnico SET cargaTrabajo = cargaTrabajo + @aumentoCargaTrabajo WHERE idTecnico = @idTecnico";
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
    }
}
