using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Services
{
    /// <summary>
    /// Servicio que maneja la creación de tokens JWT para usuarios y administradores.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// Constructor que inicializa el servicio de creación de tokens con la clave de firma.
        /// </summary>
        /// <exception cref="ArgumentNullException">Lanzado si la variable de ambiente <signingKey> es nula o vacía.</exception>
        public TokenService()
        {
            var signingKey = Environment.GetEnvironmentVariable("JWT_KEY");

            if (string.IsNullOrEmpty(signingKey))
            {
                throw new ArgumentNullException(nameof(signingKey));
            }

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        /// <summary>
        /// Crea un token JWT para un usuario.
        /// </summary>
        /// <param name="user">El objeto <see cref="AppUser"/> que representa al usuario para el cual se generará el token.</param>
        /// <returns>Devuelve un token JWT en formato de cadena para el usuario especificado.</returns>
        public string CreateTokenUser(AppUser user)
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

        /// <summary>
        /// Crea un token JWT para un administrador.
        /// </summary>
        /// <param name="admin">El objeto <see cref="AppUser"/> que representa al administrador para el cual se generará el token.</param>
        /// <returns>Devuelve un token JWT en formato de cadena para el administrador especificado.</returns>
        public string CreateTokenAdmin(AppUser admin)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, admin.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, admin.Name!),
                new Claim(JwtRegisteredClaimNames.NameId, admin.Id!),
                new Claim(ClaimTypes.Role, "Admin")
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
