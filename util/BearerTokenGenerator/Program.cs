using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>();

var configuration = builder.Build();

var bearerSigningKey = configuration["BearerSigningKey"] ?? throw new ArgumentException("Unable to find the bearer signing key.");

var claims = new List<Claim> { new("role", "admin") };        
var identity = new ClaimsIdentity(claims, "Test");
var user = new ClaimsPrincipal(identity);

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerSigningKey));
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

var securityToken = new JwtSecurityToken(
    claims: user.Claims,
    signingCredentials: credentials,
    expires: DateTime.UtcNow.AddMinutes(60)
);

var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
Console.WriteLine(token);