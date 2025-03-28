﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidoApi.Interfaces;

namespace PedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoDAO _produtoDAO;

        public ProdutoController(IProdutoDAO produtoDAO)
        {
            _produtoDAO = produtoDAO;
        }

        [HttpGet]
        public IActionResult Listar(string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var produtos = _produtoDAO.Listar(nome, page, pageSize);
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
