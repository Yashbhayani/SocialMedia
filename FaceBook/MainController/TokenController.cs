using FaceBookApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FaceBookApp.Controllers
{
    public class TokenController : Controller
    {
        public IConfiguration _configuration;

        public TokenController(IConfiguration config)
        {
            _configuration = config;
        }


        public static string GetToken(string email, string Department, string Role, IConfiguration _configuration)
        {
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Email", email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:ValidIssuer"],
                _configuration["Jwt:ValidAudience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public static GetTokenClass GetEmailMethode(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            var validationParameters = GetValidationParameters();
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            var Email = principal.Identities.ToList()[0].Claims.ToList()[3];
            GetTokenClass getTokenClass = new GetTokenClass()
            {
                Email = Email.Value,
            };

            return getTokenClass;

        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "JWTAuthenticationServer",
                ValidAudience = "JWTServicePostmanClient",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YashBhayani1873@gmail.com"))
            };
        }
    }
}
