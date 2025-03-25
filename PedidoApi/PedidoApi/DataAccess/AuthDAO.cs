using PedidoApi.Interfaces;
using PedidoApi.Models;

namespace PedidoApi.DataAccess
{
    public class AuthDAO : IAuthDAO
    {
        public void CriarToken(Auth token)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "INSERT INTO AuthTokens (token, revogado, descricao, expiracao) VALUES (@Token, @Revogado, @Descricao, @Expiracao)";
                    command.Parameters.AddWithValue("@Token", token.Token);
                    command.Parameters.AddWithValue("@Revogado", token.Revogado);
                    command.Parameters.AddWithValue("@Descricao", token.Descricao);
                    command.Parameters.AddWithValue("@Expiracao", token.Expiracao);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public Auth ObterToken(string token)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, token, revogado, descricao, expiracao FROM AuthTokens WHERE token = @Token";
                command.Parameters.AddWithValue("@Token", token);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Auth
                    {
                        Id = reader.GetInt32(0),
                        Token = reader.GetString(1),
                        Revogado = reader.GetBoolean(2),
                        Descricao = reader.GetString(3),
                        Expiracao = reader.GetDateTime(4)
                    };
                }
                return null;
            }
        }

        public Auth ObterPorId(int id)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, token, revogado, descricao, expiracao FROM AuthTokens WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Auth
                    {
                        Id = reader.GetInt32(0),
                        Token = reader.GetString(1),
                        Revogado = reader.GetBoolean(2),
                        Descricao = reader.GetString(3),
                        Expiracao = reader.GetDateTime(4)
                    };
                }
                return null;
            }
        }

        public void RevogarToken(Auth token)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "UPDATE AuthTokens SET revogado = @Revogado WHERE token = @Token";
                    command.Parameters.AddWithValue("@Revogado", token.Revogado);
                    command.Parameters.AddWithValue("@Token", token.Token);
                    command.ExecuteNonQuery();
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
                    
            }
        }

        public List<Auth> Listar()
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, token, revogado, descricao, expiracao FROM AuthTokens";
                var reader = command.ExecuteReader();
                var tokens = new List<Auth>();
                while (reader.Read())
                {
                    tokens.Add(new Auth
                    {
                        Id = reader.GetInt32(0),
                        Token = reader.GetString(1),
                        Revogado = reader.GetBoolean(2),
                        Descricao = reader.GetString(3),
                        Expiracao = reader.GetDateTime(4)
                    });
                }
                return tokens;
            }
        }
    }

}
