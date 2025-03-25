using Microsoft.AspNetCore.Http;
using PedidoApi.DataAccess;
using PedidoApi.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace PedidoApi.Middleware
{
    public class TokenRevogadoMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenRevogadoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthDAO authDAO)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    var jwtHandler = new JwtSecurityTokenHandler();
                    
                    if (jwtHandler.CanReadToken(token))
                    {
                        var jwtToken = jwtHandler.ReadJwtToken(token);

                        var auth = authDAO.ObterToken(token);

                        if (auth == null)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Token not found.");
                            return;
                        }

                        if (auth.Revogado)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Token has been revoked.");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}