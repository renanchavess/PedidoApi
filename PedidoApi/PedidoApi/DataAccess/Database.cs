using System.Data.SqlClient;

namespace PedidoApi.DataAccess
{
    public class Database
    {
        private readonly string _connection = @"Server=localhost,1433;Database=Pedidos;User Id=sa;Password=StrongPassword123!;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connection);
        }
    }
}
