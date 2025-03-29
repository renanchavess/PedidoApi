using PedidoApi.Interfaces;
using PedidoApi.Models;
using PedidoApi.ValueObject;
using System.Data.SqlClient;

namespace PedidoApi.DataAccess
{
    public class ClienteDAO : IClienteDAO
    {
        public void Criar(Cliente cliente)
{
    using (var connection = new Database().GetConnection())
    {
        connection.Open();
        var command = connection.CreateCommand();
        var transaction = connection.BeginTransaction();
        command.Transaction = transaction;

        try
        {
            command.CommandText = "INSERT INTO Clientes (nome, email, telefone, ativo) VALUES (@Nome, @Email, @Telefone, @Ativo); SELECT SCOPE_IDENTITY();";
            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            command.Parameters.AddWithValue("@Ativo", cliente.Ativo);

            cliente.Id = Convert.ToInt32(command.ExecuteScalar());

            command.CommandText = "INSERT INTO Enderecos (rua, numero, cidade, estado, cep, cliente_id) VALUES (@Rua, @Numero, @Cidade, @Estado, @Cep, @ClienteId)";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Rua", cliente.Endereco.Rua);
            command.Parameters.AddWithValue("@Numero", cliente.Endereco.Numero);
            command.Parameters.AddWithValue("@Cidade", cliente.Endereco.Cidade);
            command.Parameters.AddWithValue("@Estado", cliente.Endereco.Estado);
            command.Parameters.AddWithValue("@Cep", cliente.Endereco.Cep);
            command.Parameters.AddWithValue("@ClienteId", cliente.Id);
            command.ExecuteNonQuery();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }
}

        public void Atualizar(Cliente cliente)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var command = connection.CreateCommand();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "UPDATE Clientes SET nome = @Nome, email = @Email, telefone = @Telefone, ativo = @Ativo WHERE id = @Id";
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                    command.Parameters.AddWithValue("@Id", cliente.Id);
                    command.Parameters.AddWithValue("@Ativo", cliente.Ativo);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public Cliente Obter(int id)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, email, telefone, ativo FROM Clientes WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Cliente
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Telefone = reader.GetString(3),
                        Ativo = reader.GetBoolean(4)
                    };
                }
                return null;
            }
        }

        public List<Cliente> Listar(string? nome, string? email, string? telefone, int page, int pageSize)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var query = "SELECT c.id, c.nome, c.email, c.telefone, c.ativo, e.rua, e.numero, e.cidade, e.estado, e.cep " +
                            "FROM Clientes as c " +
                            "INNER JOIN Enderecos as e ON e.cliente_id = c.id";

                if (!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(telefone))
                {
                    query += " WHERE ";

                    if (!string.IsNullOrEmpty(nome))
                    {
                        query += "c.nome LIKE @Nome AND ";
                        command.Parameters.AddWithValue("@Nome", "%" + nome + "%");
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        query += "c.email LIKE @Email AND ";
                        command.Parameters.AddWithValue("@Email", "%" + email + "%");
                    }

                    if (!string.IsNullOrEmpty(telefone))
                    {
                        query += "c.telefone LIKE @Telefone AND ";
                        command.Parameters.AddWithValue("@Telefone", "%" + telefone + "%");
                    }

                    query = query.Substring(0, query.Length - 5);
                }

                query += " ORDER BY c.id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                command.CommandText = query;
                command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                var reader = command.ExecuteReader();
                var clientes = new List<Cliente>();

                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Telefone = reader.GetString(reader.GetOrdinal("telefone")),
                        Ativo = reader.GetBoolean(reader.GetOrdinal("ativo")),
                        Endereco = new Endereco
                        {
                            Rua = reader.GetString(reader.GetOrdinal("rua")),
                            Numero = reader.GetString(reader.GetOrdinal("numero")),
                            Cidade = reader.GetString(reader.GetOrdinal("cidade")),
                            Estado = reader.GetString(reader.GetOrdinal("estado")),
                            Cep = reader.GetString(reader.GetOrdinal("cep"))
                        }
                    });
                }

                return clientes;
            }
        }
    }
}
