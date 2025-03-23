namespace PedidoApi.ValueObject
{
    public class Endereco
    {
        public string Rua { get; set; }
        public string Numero { get; set; }        
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

        public Endereco() { }
    }
}
