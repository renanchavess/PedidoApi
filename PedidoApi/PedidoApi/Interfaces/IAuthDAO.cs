using PedidoApi.Models;

namespace PedidoApi.Interfaces
{
    public interface IAuthDAO
    {
        void RevogarToken(Auth token);
        void CriarToken(Auth token);
        Auth ObterToken(string token);
        List<Auth> Listar();        
    }
}
