using PedidoApi.Interfaces;
using PedidoApi.Models;

namespace PedidoApi.DataAccess
{
    public class ProdutoDAO : IProdutoDAO
    {
        public List<Produto> Listar(string? nome)
        {
            using (var connection = new Database().GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Produtos";

                if (!String.IsNullOrEmpty(nome))
                {
                    command.CommandText += " WHERE nome LIKE @Nome";
                    command.Parameters.AddWithValue("@Nome", "%" + nome + "%");
                }

                var reader = command.ExecuteReader();
                var produtos = new List<Produto>();
                while (reader.Read())
                {
                    produtos.Add(new Produto
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        Estoque = reader.GetInt32(reader.GetOrdinal("estoque")),
                        Preco = reader.GetDecimal(reader.GetOrdinal("preco"))
                    });
                }

                return produtos;
            }
        }

        public Produto Obter(int id)
        {
            using ( var connection = new Database().GetConnection()) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Produtos WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Produto
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        Estoque = reader.GetInt32(reader.GetOrdinal("estoque")),
                        Preco = reader.GetDecimal(reader.GetOrdinal("preco"))
                    };
                }

                return null;
            }
        }
    }
}
