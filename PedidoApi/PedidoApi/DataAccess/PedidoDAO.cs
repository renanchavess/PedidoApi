﻿using PedidoApi.Enums;
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
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "UPDATE Pedidos SET status = @Status WHERE id = @Id";
                    command.Parameters.AddWithValue("@Status", pedido.Status);
                    command.Parameters.AddWithValue("@Id", pedido.Id);
                    try
                    {
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public Pedido Criar(Pedido pedido)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "INSERT INTO Pedidos (cliente_id, status) VALUES (@ClienteId, @Status); SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);
                    command.Parameters.AddWithValue("@Status", pedido.Status);

                    int id = Convert.ToInt32(command.ExecuteScalar());
                    pedido.Id = id;

                    foreach (var item in pedido.Itens)
                    {
                        item.PedidoId = id;
                        command.CommandText = "INSERT INTO ItensPedidos (pedido_id, produto_id, quantidade, preco) VALUES (@PedidoId, @ProdutoId, @Quantidade, @Preco)";
                        command.Parameters.AddWithValue("@PedidoId", id);
                        command.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                        command.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        command.Parameters.AddWithValue("@Preco", item.Preco);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    
                    connection.Close();                    
                }

                return pedido;
            }
        }

        public List<Pedido> Listar(Cliente? cliente, PedidoStatus? status, int page, int pageSize)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var query = "SELECT * FROM Pedidos WHERE 1=1";

                if (cliente != null)
                {
                    query += " AND cliente_id = @ClienteId";
                    command.Parameters.AddWithValue("@ClienteId", cliente.Id);
                }

                if (status != null)
                {
                    query += " AND status = @Status";
                    command.Parameters.AddWithValue("@Status", (int)status);
                }

                query += " ORDER BY id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                command.CommandText = query;
                command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

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

                reader.Close();

                foreach (var pedido in pedidos)
                {
                    var itemCommand = connection.CreateCommand();
                    itemCommand.CommandText = "SELECT * FROM ItensPedidos WHERE pedido_id = @PedidoId";
                    itemCommand.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    var itemReader = itemCommand.ExecuteReader();
                    var itens = new List<PedidoItem>();

                    while (itemReader.Read())
                    {
                        var item = new PedidoItem
                        {
                            PedidoId = pedido.Id,
                            ProdutoId = Convert.ToInt32(itemReader["produto_id"]),
                            Quantidade = Convert.ToInt32(itemReader["quantidade"]),
                            Preco = Convert.ToDecimal(itemReader["preco"])
                        };
                        itens.Add(item);
                    }

                    itemReader.Close();
                    pedido.Itens = itens;
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
                    var pedido = new Pedido
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ClienteId = Convert.ToInt32(reader["cliente_id"]),
                        Status = (PedidoStatus)Convert.ToInt32(reader["status"])
                    };

                    reader.Close();

                    command.CommandText = "SELECT * FROM ItensPedidos WHERE pedido_id = @PedidoId";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@PedidoId", id);
                    reader = command.ExecuteReader();
                    var itens = new List<PedidoItem>();
                    while (reader.Read())
                    {
                        var item = new PedidoItem
                        {
                            ProdutoId = Convert.ToInt32(reader["produto_id"]),
                            Quantidade = Convert.ToInt32(reader["quantidade"]),
                            Preco = Convert.ToDecimal(reader["preco"])
                        };
                        itens.Add(item);
                    }

                    pedido.Itens = itens;
                    return pedido;
                }

                return null;
            }
        }
    }
}
