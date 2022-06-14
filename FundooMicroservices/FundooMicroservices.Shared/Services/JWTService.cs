using FundooMicroservices.Models;
using FundooMicroservices.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace FundooMicroservices.Shared.Services
{
    public class JWTService : IJWTService
    {
        public string GenerateJWT(string email, string userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", email),
                    new Claim("ID", userID)
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public JWTResponse ValidateToken(HttpRequest request)
        {
            JWTResponse response = new JWTResponse();
            var headers = request.Headers.ToDictionary(q => q.Key, q => (string)q.Value);
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = headers["Authorization"].Split(' ').Last().ToString();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = mySecurityKey,
                    RequireExpirationTime = true            // might cause error, remove if it does
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                response.IsAuthorized = false;
                return response;
            }
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var userId = securityToken.Claims.First(claim => claim.Type == "ID").Value;
            var email = securityToken.Claims.First(claim => claim.Type == "Email").Value;
            response.IsAuthorized = true;
            response.userID = userId;
            response.Email = email;
            return response;
        }
    }
}
