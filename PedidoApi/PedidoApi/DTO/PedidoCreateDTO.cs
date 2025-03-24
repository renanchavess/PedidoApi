using PedidoApi.Enums;
using PedidoApi.Models;

namespace PedidoApi.DTO
{
    public class PedidoCreateDTO
    {
        public int id { get; set; }
        public int clienteId { get; set; }
        public List<PedidoItemDTO> itens { get; set; }

        public Pedido toEntity()
        {
            return new Pedido
            {
                Id = this.id,
                ClienteId = this.clienteId,
                Itens = this.itens.Select(i => i.toEntity()).ToList(),
                Status = PedidoStatus.EmProcessamento
            };
        }
    }
}
