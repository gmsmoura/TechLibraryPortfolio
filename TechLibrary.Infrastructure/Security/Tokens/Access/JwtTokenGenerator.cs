using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Infrastructure.Security.Tokens.Access
{
    public class JwtTokenGenerator
    {
        private readonly string _signingKey;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _signingKey = configuration["Jwt:SigningKey"]!;
        }

        public string Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(60),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private SymmetricSecurityKey SecurityKey()
        {
            var symmetrickey = Encoding.UTF8.GetBytes(_signingKey);
            return new SymmetricSecurityKey(symmetrickey);
        }
    }
}
