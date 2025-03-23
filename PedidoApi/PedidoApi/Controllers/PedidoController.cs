using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public PedidoController(IPedidoDAO pedidoDAO)
        {
            _pedidoDAO = pedidoDAO;
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Pedido pedido)
        {
            _pedidoDAO.Criar(pedido);
            return StatusCode(StatusCodes.Status201Created);
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

            switch(status)
            {
                case "Em Processamento":
                    pedido.Status = PedidoStatus.EmProcessamento;
                    break;
                case "Finalizado":
                    pedido.Status = PedidoStatus.Finalizado;
                    break;                
                default:
                    return BadRequest();
            }

            _pedidoDAO.Atualizar(pedido);
            return Ok();
        }

    }
}
