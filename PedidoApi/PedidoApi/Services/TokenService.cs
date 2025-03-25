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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("cApG5jY6c2EJcApG5jY6c2EJcApG5jY6c2EJ");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, descricao)                    
                }),
                Expires = expiracao,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static readonly HashSet<string> RevokedTokens = new HashSet<string>();

        public void RevogarToken(string token)
        {
            RevokedTokens.Add(token);
        }

        public bool IsTokenRevoked(string token)
        {
            return RevokedTokens.Contains(token);
        }
    }
}
