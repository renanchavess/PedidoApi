﻿namespace PedidoApi.Models
{
    public class PedidoItem
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
