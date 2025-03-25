using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeBook.Api.Services
{
    public interface ITokenService
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token);
            if (jwtToken == null)
                throw new Exception("Invalid JWT token");

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User identifier not found in token");

            return userIdClaim.Value;
        }
    }
}
