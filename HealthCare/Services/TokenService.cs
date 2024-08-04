using HealthCare.Dtos;
using HealthCare.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using HealthCare.Repositories;

namespace HealthCare.Services
{

    public class TokenService
        {
            private readonly AuthSettings _authSettings;
        private readonly AppUserRepository _appUserRepository;

        public TokenService(IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public string GenerateToken(long id)
            {

            AppUser user = _appUserRepository.GetAppUserById(id).Result;
                
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.userId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.firstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.lastName),
                new Claim(JwtRegisteredClaimNames.Email, user.email)
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
