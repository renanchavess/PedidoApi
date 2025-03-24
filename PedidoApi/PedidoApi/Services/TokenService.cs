using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PedidoApi.Services
{
    public class TokenService
    {
        public string GerarToken(string descricao, DateTime expiracao)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("cApG5jY6c2EJcApG5jY6c2EJcApG5jY6c2EJ"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: descricao,
                audience: descricao,
                claims: new List<Claim>(),
                expires: expiracao,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
