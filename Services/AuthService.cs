using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MyAuthCrudApi.Models;

namespace MyAuthCrudApi.Services;

public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var keyString = _config["Jwt:Key"]?.Trim() 
            ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
        var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing in configuration.");
        var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is missing in configuration.");

        // ✅ Decode Base64 dengan validasi tambahan
        byte[] keyBytes;
        try
        {
            keyBytes = Convert.FromBase64String(keyString);
        }
        catch (FormatException)
        {
            throw new InvalidOperationException("JWT Key in configuration is not a valid Base64 string.");
        }

        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

        // ✅ Debugging token yang dihasilkan
        Console.WriteLine($"[DEBUG] Token Generated: {keyBytes}");

        return generatedToken;
    }
}
