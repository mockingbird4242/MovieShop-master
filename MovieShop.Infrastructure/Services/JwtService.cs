using MovieShop.Core.Models.Response;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace MovieShop.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJWT(LoginResponseModel user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToShortDateString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),



            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name.ToString()));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenSettings:PrivateKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var expires = DateTime.UtcNow.AddHours(_config.GetValue<double>("TokenSettings:ExpirationHours"));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenObject = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _config["TokenSettings:Issuer"],
                Audience = _config["TokenSettings:Audience"]
            };

            var encodedJwt = tokenHandler.CreateToken(tokenObject);
            return tokenHandler.WriteToken(encodedJwt);
        }
    }
    
}
