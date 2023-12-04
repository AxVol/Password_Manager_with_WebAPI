using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Domain.Entity;
using WebApi.Service.Interfaces;

namespace WebApi.Service.Implementations
{
    public class JWTService : IJWTService
    {
        private readonly ConfigurationManager config;

        public JWTService(ConfigurationManager configuration)
        {
            config = configuration;
        }

        public async Task<string> GetToken(User user)
        {
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login)
            };

            JwtSecurityToken jwt = new JwtSecurityToken
                (
                    issuer: config["JwtSettings:Issure"],
                    audience: config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.Add(TimeSpan.FromHours(12)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)), SecurityAlgorithms.HmacSha256)
                );
            string encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJWT;
        }
    }
}
