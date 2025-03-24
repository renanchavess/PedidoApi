namespace PedidoApi.Models
{
    public class Auth
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public bool Revogado { get; set; }
        public string Descricao { get; set; }
        public DateTime Expiracao { get; set; }
    }
}
