using System.Data.SqlClient;

namespace Proyecto_Tickets.Data
{
    public class Conexion
    {
        private readonly string _connectionString;
        public Conexion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection ObtenerConexion()
        {

            var conexion = new SqlConnection(_connectionString);
            conexion.Open();
            return conexion;
        }
    }
}
