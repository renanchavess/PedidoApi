using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidoApi.Interfaces;

namespace PedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteDAO _clienteDAO;

        public ClienteController(IClienteDAO clienteDAO)
        {
            _clienteDAO = clienteDAO;
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Models.Cliente cliente)
        {
            _clienteDAO.Criar(cliente);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            var cliente = _clienteDAO.Obter(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] Models.Cliente cliente)
        {
            _clienteDAO.Atualizar(cliente);
            return Ok();
        }

        [HttpGet]
        public IActionResult Listar(string? nome, string? email, string? telefone)
        {
            var clientes = _clienteDAO.Listar(nome, email, telefone);
            return Ok(clientes);
        }
    }
}
