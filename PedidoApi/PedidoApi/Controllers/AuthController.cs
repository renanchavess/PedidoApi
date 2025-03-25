using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidoApi.DTO;
using PedidoApi.Services;

namespace PedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult LoginAdministrativo([FromBody] AuthLoginDTO request)
        {
            if (request.Email != "admin" || request.Senha != "password123")
            {
                return Unauthorized();
            }
            var tokenBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.Email+":"+request.Senha));
            return Ok(tokenBase64);
        }
    }
}
