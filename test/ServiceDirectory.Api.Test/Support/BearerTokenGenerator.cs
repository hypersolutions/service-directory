using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServiceDirectory.Api.Test.Support;

public static class BearerTokenGenerator
{
    public static string CreateTestToken(string bearerSigningKey, string role = "admin")
    {
        var claims = new List<Claim> { new("role", role) };        
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerSigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var securityToken = new JwtSecurityToken(
            claims: principal.Claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(60)
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(securityToken);
    }
}