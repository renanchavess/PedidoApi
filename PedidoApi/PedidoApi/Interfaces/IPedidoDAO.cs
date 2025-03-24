using PedidoApi.Enums;
using PedidoApi.Models;

namespace PedidoApi.Interfaces
{
    public interface IPedidoDAO
    {
        Pedido Criar(Pedido pedido);
        Pedido Obter(int id);
        void Atualizar(Pedido pedido);
        List<Pedido> Listar(Cliente? cliente, PedidoStatus? status);
    }
}
