using PedidoApi.Enums;
using PedidoApi.Models;

namespace PedidoApi.DTO
{
    public class PedidoDTO
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

        public static PedidoDTO fromEntity(Pedido pedido)
        {
            List<PedidoItemDTO> itens = new List<PedidoItemDTO>();

            foreach (var item in pedido.Itens)
            {
                itens.Add(PedidoItemDTO.fromEntity(item));
            }

            return new PedidoDTO
            {
                id = pedido.Id,
                clienteId = pedido.ClienteId,
                itens = itens
            };
        }
    }
}
