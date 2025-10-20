using BCrypt.Net;
using HRSystem.Csharp.Domain.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Helpers
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public JwtService(IConfiguration configuration, AppDbContext appDbContext = null)
        {
            _configuration = configuration;
            _appDbContext = appDbContext;
        }

        public string GenerateJwtToken(string username, string email, string employeeCode)
        {
            var jwtId = Ulid.NewUlid().ToString();
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim("UserCode", employeeCode)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JwtSecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "HRSystem.com",
                audience: "HRSystem.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedPassword);
        }

        public string getJwtIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenValidation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "HRSystem.com",
                ValidAudience = "HRSystem.com",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JwtSecretKey"]!))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidation, out SecurityToken securityToken);

            if (!IsJwtWithValidSecurityAlgorithm(securityToken)) return null;

            return principal; 
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        => validatedToken is JwtSecurityToken jwtSecurityToken
            && jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    
    }
}
