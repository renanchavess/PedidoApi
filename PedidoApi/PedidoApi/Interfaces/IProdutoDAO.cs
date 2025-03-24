using PedidoApi.Models;

namespace PedidoApi.Interfaces
{
    public interface IProdutoDAO
    {
        Produto Obter(int id);
        List<Produto> Listar(string? nome, int page, int pageSize);
    }
}
