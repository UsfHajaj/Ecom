using Ecom.Core.Entites;
using Ecom.Core.Serviecs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Services
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration configuration;

        public GenerateToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser appUser)
        {
            
            List<Claim> claims = new List<Claim>()
            {
                //new Claim(ClaimTypes.NameIdentifier,appUser.Id),

                new Claim(ClaimTypes.Name,appUser.UserName),
                new Claim(ClaimTypes.Email,appUser.Email),
                
            };
            var security = configuration["Token:Secret"];
            var key = Encoding.ASCII.GetBytes(security);
            var signingCredentials=new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = configuration["Token:Issure"],
                SigningCredentials = signingCredentials,
                NotBefore= DateTime.Now,
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
            
        }
    }
}
