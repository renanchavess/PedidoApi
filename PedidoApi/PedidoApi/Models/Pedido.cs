using PedidoApi.Enums;

namespace PedidoApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public List<PedidoItem> Itens { get; set; }
        public PedidoStatus Status { get; set; }
    }
}
