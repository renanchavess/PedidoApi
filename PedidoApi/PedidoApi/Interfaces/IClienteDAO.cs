using PedidoApi.Models;

namespace PedidoApi.Interfaces
{
    public interface IClienteDAO
    {
        void Criar(Cliente cliente);
        Cliente Obter(int id);
        void Atualizar(Cliente cliente);
        List<Cliente> Listar(string? nome, string? email, string? telefone);
    }
}
