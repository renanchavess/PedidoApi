using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidoApi.DTO;
using PedidoApi.Enums;
using PedidoApi.Interfaces;
using PedidoApi.Models;

namespace PedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoDAO _pedidoDAO;
        private readonly IClienteDAO _clienteDAO;

        public PedidoController(IPedidoDAO pedidoDAO, IClienteDAO clienteDAO)
        {
            _pedidoDAO = pedidoDAO;
            _clienteDAO = clienteDAO;
        }

        [HttpPost]
        public IActionResult Criar([FromBody] PedidoCreateDTO pedidoDTO)
        {
            var pedido = _pedidoDAO.Criar(pedidoDTO.toEntity());
            return StatusCode(StatusCodes.Status201Created, new { pedido_id = pedido.Id, cliente_id = pedido.ClienteId, status = pedido.Status.ToString() });
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            var pedido = _pedidoDAO.Obter(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        [HttpPut("{id}/status")]
        public IActionResult AtualizarStatus(int id, [FromBody] string status)
        {
            Pedido pedido = _pedidoDAO.Obter(id);

            if (pedido == null)
            {
                return NotFound();
            }

            switch (status)
            {
                case "Em Processamento":
                    pedido.Status = PedidoStatus.EmProcessamento;
                    break;
                case "Finalizado":
                    pedido.Status = PedidoStatus.Finalizado;
                    break;

                case "Cancelado":
                    pedido.Status = PedidoStatus.Cancelado;
                    break;
                default:
                    return BadRequest();
            }

            _pedidoDAO.Atualizar(pedido);
            return Ok();
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] int? clienteId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            Cliente cliente = null;
            PedidoStatus? pedidoStatus = null;

            if (clienteId is not null)
            {
                cliente = _clienteDAO.Obter(clienteId.Value);

                if (cliente == null)
                {
                    return BadRequest();
                }
            }

            if (status is not null)
            {
                if (!Enum.TryParse(status, true, out PedidoStatus statusEnum))
                {
                    return BadRequest();
                }
                pedidoStatus = statusEnum;
            }

            var pedidos = _pedidoDAO.Listar(cliente, pedidoStatus, page, pageSize);
            return Ok(pedidos);
        }

    }
}
