using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OficiosYa.Web.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(int userId, string email, bool esAdmin = false, bool esProfesional = false)
    {
        var secret = _configuration["Jwt:SecretKey"] ?? "OficiosYa_SecretKey_Minimo32Caracteres_ParaProduccionCambiar";
        var issuer = _configuration["Jwt:Issuer"] ?? "OficiosYa";
        var audience = _configuration["Jwt:Audience"] ?? "OficiosYa";
        var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var min) ? min : 60 * 24 * 7; // 7 días por defecto

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };
        if (esAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        if (esProfesional)
            claims.Add(new Claim(ClaimTypes.Role, "Profesional"));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
