using PedidoApi.Models;

namespace PedidoApi.Interfaces
{
    public interface IPedidoDAO
    {
        void Criar(Pedido pedido);
        Pedido Obter(int id);
        void Atualizar(Pedido pedido);
        List<Pedido> Listar();
    }
}
