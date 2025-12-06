using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Common.Models;
using LaReservaBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LaReservaBackend.Application.Services;
public class JwtOptions
{
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public string Key { get; set; } = "";
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 30;
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher = new();
    private readonly JwtOptions _jwtOptions;

    public AuthService(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResultDto> AuthenticateAsync(string email, string password, string ipAddress)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) throw new UnauthorizedAccessException("Credenciales no válidas");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Credenciales no válidas");

        var AccessToken = GenerateJwtToken(user);
        var RefreshToken = GenerateRefreshToken(ipAddress, _jwtOptions.RefreshTokenDays);

        user.RefreshTokens.Add(RefreshToken);
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return new AuthResultDto
        {
            AccessToken = AccessToken.Token,
            RefreshToken = RefreshToken.Token,
            AccessTokenExpiresAt = AccessToken.ExpiresAt,
            RefreshTokenExpiresAt = RefreshToken.ExpiresAt,
            UserId = user.Id,
            Email = user.Email
        };
    }

    public async Task<AuthResultDto> RefreshTokenAsync(string token, string ipAddress)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(token);
        if (user == null) throw new UnauthorizedAccessException("Invalid token");

        var rt = user.RefreshTokens.SingleOrDefault(x => x.Token == token);
        if (rt == null || !rt.IsActive) throw new UnauthorizedAccessException("Invalid token");

        rt.Revoked = true;
        rt.RevokedAt = DateTime.UtcNow;
        rt.RevokedByIp = ipAddress;

        var newRefreshToken = GenerateRefreshToken(ipAddress, _jwtOptions.RefreshTokenDays);
        rt.ReplacedByToken = newRefreshToken.Token;
        user.RefreshTokens.Add(newRefreshToken);

        user.RefreshTokens.RemoveAll(x => !x.IsActive && x.CreatedAt < DateTime.UtcNow.AddDays(-_jwtOptions.RefreshTokenDays * 2));

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        var accessToken = GenerateJwtToken(user);

        return new AuthResultDto
        {
            AccessToken = accessToken.Token,
            RefreshToken = newRefreshToken.Token,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshTokenExpiresAt = newRefreshToken.ExpiresAt,
            UserId = user.Id,
            Email = user.Email
        };
    }

    public async Task RevokeRefreshTokenAsync(string token, string ipAddress)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(token);
        if (user == null) return;

        var rt = user.RefreshTokens.SingleOrDefault(x => x.Token == token);
        if (rt == null || rt.Revoked) return;

        rt.Revoked = true;
        rt.RevokedAt = DateTime.UtcNow;
        rt.RevokedByIp = ipAddress;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    private (string Token, DateTime ExpiresAt) GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullname", user.FullName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expires);
    }

    private RefreshToken GenerateRefreshToken(string ipAddress, int daysValid)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);

        return new RefreshToken
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(daysValid),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}
