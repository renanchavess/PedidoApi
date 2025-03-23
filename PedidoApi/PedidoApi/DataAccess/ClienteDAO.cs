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
                    command.CommandText = "INSERT INTO Clientes (nome, email, telefone) VALUES (@Nome, @Email, @Telefone); SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@Telefone", cliente.Telefone);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = "INSERT INTO Enderecos (rua, numero, cidade, estado, cep, cliente_id) VALUES (@Rua, @Numero, @Cidade, @Estado, @Cep, @ClienteId)";
                    command.Parameters.AddWithValue("@Rua", cliente.Endereco.Rua);
                    command.Parameters.AddWithValue("@Numero", cliente.Endereco.Numero);                    
                    command.Parameters.AddWithValue("@Cidade", cliente.Endereco.Cidade);
                    command.Parameters.AddWithValue("@Estado", cliente.Endereco.Estado);
                    command.Parameters.AddWithValue("@Cep", cliente.Endereco.Cep);
                    command.Parameters.AddWithValue("@ClienteId", id);
                    command.ExecuteNonQuery();
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

        public void Atualizar(Cliente cliente)
        {
            using ( var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                connection.BeginTransaction();
                command.CommandText = "UPDATE Clientes SET nome = @Nome, email = @Email, telefone = @Telefone WHERE id = @Id";
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                command.Parameters.AddWithValue("@Id", cliente.Id);
                command.ExecuteNonQuery();
            }
        }

        public Cliente Obter(int id)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Clientes WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Cliente
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Telefone = reader.GetString(3)
                    };
                }
                return null;
            }
        }

        public List<Cliente> Listar(string? nome, string? email, string? telefone)
        {
            using (var connnection = new Database().GetConnection())
            {
                connnection.Open();
                var cmd = connnection.CreateCommand();
                cmd.CommandText = "SELECT c.id, c.nome, c.email, c.telefone, e.rua, e.numero, e.cidade, e.estado, e.cep " +
                    "FROM Clientes as c " +
                    "INNER JOIN Enderecos as e ON e.cliente_id = c.id";
                
                if (!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(telefone))
                {
                    cmd.CommandText += " WHERE ";

                    if (!string.IsNullOrEmpty(nome))
                    {
                        cmd.CommandText += "nome LIKE @Nome AND ";
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        cmd.CommandText += "email LIKE @Email AND ";
                    }

                    if (!string.IsNullOrEmpty(telefone))
                    {
                        cmd.CommandText += "telefone LIKE @Telefone AND ";
                    }

                    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 5);
                }                

                cmd.Parameters.AddWithValue("@Nome", "%" + nome + "%");
                cmd.Parameters.AddWithValue("@Email", "%" + email + "%");
                cmd.Parameters.AddWithValue("@Telefone", "%" + telefone + "%");

                var reader = cmd.ExecuteReader();

                var clientes = new List<Cliente>();

                while (reader.Read())
                {                    
                    clientes.Add(new Cliente
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Telefone = reader.GetString(reader.GetOrdinal("telefone")),
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
