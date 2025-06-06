using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Token
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly SymmetricSecurityKey key;

        public TokenService(UserManager<AppUser> userManager, IConfiguration configuration, ILogger<TokenService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
        }
        public async Task<string> GenerateAccessToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("PhotoUrl", user.Photos.FirstOrDefault(x => x.IsMain)?.Url ?? string.Empty), // PhotoUrl
                new Claim("Username", user.UserName),
            };

            // Add roles to the token
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds,
                Issuer = _configuration["Token:Issuer"],
                IssuedAt = DateTime.Now,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Task<string> GenerateRefreshToken(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
