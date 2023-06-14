using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TemplateCQRS.Api.Security;

public static class TokenHelper
{
    public static string GenerateToken(TokenGenerationRequest request, IConfiguration config, int expirationMinutes)
    {
        var jwtSettings = config.GetSection("JwtSettings");

        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "");
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, request.Email),
            new (JwtRegisteredClaimNames.Email, request.Email),
            new ("userid", request.UserId)
        };

        claims.AddRange(request.CustomClaims.Select(claim => new Claim(claim.Key, claim.Value.ToString())));

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}