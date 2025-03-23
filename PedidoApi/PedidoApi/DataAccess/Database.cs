using System.Data.SqlClient;

namespace PedidoApi.DataAccess
{
    public class Database
    {
        private readonly string _connection = @"Server=localhost;Database=Pedido;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connection);
        }
    }
}
