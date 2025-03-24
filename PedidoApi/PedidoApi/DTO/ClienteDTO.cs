using PedidoApi.Models;
using PedidoApi.ValueObject;

namespace PedidoApi.DTO
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }

        public Cliente toEntity(bool ativo = true)
        {
            return new Cliente
            {
                Id = this.Id,
                Nome = this.Nome,
                Email = this.Email,
                Telefone = this.Telefone,
                Endereco = this.Endereco,
                Ativo = ativo
            };
        }
    }
}
