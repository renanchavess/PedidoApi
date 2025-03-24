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
                    command.CommandText = "INSERT INTO Auth (token, revogado, descricao, expiracao) VALUES (@Token, @Revogado, @Descricao, @Expiracao)";
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
                command.CommandText = "SELECT * FROM Auth WHERE token = @Token";
                command.Parameters.AddWithValue("@Token", token);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Auth
                    {
                        Token = reader.GetString(0),
                        Revogado = reader.GetBoolean(1),
                        Descricao = reader.GetString(2),
                        Expiracao = reader.GetDateTime(3)
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
                    command.CommandText = "UPDATE Auth SET revogado = @Revogado WHERE token = @Token";
                    command.Parameters.AddWithValue("@Revogado", true);
                    command.Parameters.AddWithValue("@Token", token.Token);
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

        public bool TokenRenogado(string token)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT revogado FROM Auth WHERE token = @Token";
                command.Parameters.AddWithValue("@Token", token);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetBoolean(0);
                }
                return true;
            }
        }
    }

}
