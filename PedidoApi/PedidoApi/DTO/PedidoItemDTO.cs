using PedidoApi.Models;

namespace PedidoApi.DTO
{
    public class PedidoItemDTO
    {
        public int quantidade { get; set; }
        public int produtoId { get; set; }
        public decimal preco { get; set; }

        public PedidoItem toEntity()
        {
            return new PedidoItem
            {
                ProdutoId = this.produtoId,
                Quantidade = this.quantidade,
                Preco = this.preco
            };
        }
    }
}
