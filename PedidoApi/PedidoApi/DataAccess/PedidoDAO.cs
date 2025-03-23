using PedidoApi.Enums;
using PedidoApi.Interfaces;
using PedidoApi.Models;

namespace PedidoApi.DataAccess
{
    public class PedidoDAO : IPedidoDAO
    {
        public void Atualizar(Pedido pedido)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                connection.BeginTransaction();
                command.CommandText = "UPDATE Pedidos SET statua = @Status, data = @Data, valor = @Valor WHERE id = @Id";
                command.Parameters.AddWithValue("@Status", pedido.Status);
                command.Parameters.AddWithValue("@Id", pedido.Id);
                command.ExecuteNonQuery();
            }
        }

        public void Criar(Pedido pedido)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "INSERT INTO Pedidos (cliente_id) VALUES (@ClienteId); SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    foreach (var item in pedido.Itens)
                    {
                        command.CommandText = "INSERT INTO ItensPedido (pedido_id, produto_id, quantidade) VALUES (@PedidoId, @ProdutoId, @Quantidade)";
                        command.Parameters.AddWithValue("@PedidoId", id);
                        command.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                        command.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Commit();
                }
                
            }
        }

        public List<Pedido> Listar()
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Pedidos";
                var reader = command.ExecuteReader();
                var pedidos = new List<Pedido>();
                while (reader.Read())
                {
                    var pedido = new Pedido
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ClienteId = Convert.ToInt32(reader["cliente_id"]),
                        Status = (PedidoStatus)Convert.ToInt32(reader["status"])
                    };
                    pedidos.Add(pedido);
                }
                return pedidos;
            }
        }

        public Pedido Obter(int id)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Pedidos WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    return new Pedido
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ClienteId = Convert.ToInt32(reader["cliente_id"]),
                        Status = (PedidoStatus)Convert.ToInt32(reader["status"])
                    };
                }

                return null;
            }
        }
    }
}
