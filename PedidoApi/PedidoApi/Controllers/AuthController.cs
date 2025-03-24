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
            if (request.Email == "administrativo@gmail.com" && request.Senha == "123")
            {
                return Ok();
            }

            return Unauthorized();
        }
    }
}
