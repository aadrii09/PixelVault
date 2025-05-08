using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiVault.Models;
using ApiVault.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiVault.Utilidades
{
    public class JwtHelper
    {
        private readonly ApiSettings _settings;
        public JwtHelper(IOptions<ApiSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            // Agregar el rol "Admin" si el usuario es administrador
            if (usuario.EsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secreta));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Emisor,
                audience: _settings.Audiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiraEnMinutos),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
