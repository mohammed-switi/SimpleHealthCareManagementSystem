    using HealthCare.Dtos;
    using HealthCare.Models;
using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
using Microsoft.Extensions.Options;

namespace HealthCare.Services
{

        public class TokenService
        {
            private readonly AuthSettings _authSettings;

        public TokenService(IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public string GenerateToken(AppUserDTO user)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _authSettings.Issuer,
                    audience: _authSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
