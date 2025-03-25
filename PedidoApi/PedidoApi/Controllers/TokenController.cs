using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PedidoApi.DTO;
using PedidoApi.Interfaces;
using PedidoApi.Models;
using PedidoApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthDAO _authDAO;

        public TokenController(IAuthDAO authDAO)
        {
            _authDAO = authDAO;
        }

        [HttpPost]
        public IActionResult CriarToken([FromBody] TokenCreate request)
        {            
            var token = new TokenService().GerarToken(request.Descricao, request.Expiracao);

            Auth auth = new Auth
            {
                Token = token,
                Revogado = false,
                Descricao = request.Descricao,
                Expiracao = request.Expiracao
            };

            _authDAO.CriarToken(auth);

            return Ok(new { token = token });
        }

        [HttpDelete("{id}")]
        public IActionResult RevokeToken(int id)
        {
            var auth = _authDAO.ObterPorId(id);

            if (auth == null)
            {
                return NotFound();
            }

            auth.Revogado = true;
            _authDAO.RevogarToken(auth);
            return Ok();
        }

        [HttpGet]
        public IActionResult ListarTokens()
        {
            return Ok(_authDAO.Listar());
        }
    }
}
