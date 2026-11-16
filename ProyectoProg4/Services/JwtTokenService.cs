using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ProyectoProg4.Services;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public TokenResult GenerateToken(User user)
    {
        // Secret del appsettings, convertido a bytes
        var secret = _config["Jwt:Key"]
            ?? throw new InvalidOperationException("Falta la configuración Jwt:Key.");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        // Firma con el secreto y el algoritmo de hashing
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Claims personalizadas que se suman al payload
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("role", user.Role?.Name ?? "Common")
        };

        // Fechas en UTC
        var minutes = int.Parse(_config["Jwt:ExpirationMinutes"] ?? "60");
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(minutes);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new TokenResult(jwt, expires);
    }
}