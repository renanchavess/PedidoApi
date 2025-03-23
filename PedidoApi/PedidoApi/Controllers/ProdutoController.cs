using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidoApi.Interfaces;

namespace PedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoDAO _produtoDAO;

        public ProdutoController(IProdutoDAO produtoDAO)
        {
            _produtoDAO = produtoDAO;
        }

        [HttpGet]
        public IActionResult Listar(string? nome)
        {
            var produtos = _produtoDAO.Listar(nome);
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            var produto = _produtoDAO.Obter(id);
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }
    }
}
