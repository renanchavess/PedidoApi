using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidoApi.DTO;
using PedidoApi.Interfaces;
using PedidoApi.Models;

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
        public IActionResult Criar([FromBody] ClienteDTO clienteDTO)
        {
            Cliente cliente = clienteDTO.toEntity();
            _clienteDAO.Criar(cliente);
            return StatusCode(StatusCodes.Status201Created, new { id = cliente.Id, nome = cliente.Nome, ativo = cliente.Ativo ? "Ativo" : "Desativado"});
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            var cliente = _clienteDAO.Obter(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(new { id = cliente.Id, email = cliente.Email, telefone = cliente.Telefone, ativo = cliente.Ativo ? "Ativo" : "Desativado" });
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] Models.Cliente cliente)
        {
            _clienteDAO.Atualizar(cliente);
            return Ok(new { id = cliente.Id, nome = cliente.Nome, ativo = cliente.Ativo ? "Ativo" : "Desativado" });
        }

        [HttpGet]
        public IActionResult Listar(string? nome, string? email, string? telefone, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var clientes = _clienteDAO.Listar(nome, email, telefone, page, pageSize);
            List<Object> clientesDTO = new List<Object>();
            foreach (var cliente in clientes)
            {
                clientesDTO.Add(new
                {
                    id = cliente.Id,
                    nome = cliente.Nome,
                    email = cliente.Email,
                    telefone = cliente.Telefone,
                    ativo = cliente.Ativo ? "Ativo" : "Desativado"
                });
            }

            return Ok(clientesDTO);
        }
    }
}
