using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;

        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {

            _config = config;

            var signingKey = Environment.GetEnvironmentVariable("JWT_KEY");

            if(string.IsNullOrEmpty(signingKey))
            {
                throw new ArgumentNullException(nameof(signingKey));
            }

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name!),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id!),
                new Claim(ClaimTypes.Role, "User")
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = creds,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}