using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.User;

public class TokenService : ITokenService
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IApiExceptionService _apiExceptionService;

    public TokenService(ITokenRepository tokenRepository,
        IApiExceptionService apiExceptionService)
    {
        _tokenRepository = tokenRepository;
        _apiExceptionService = apiExceptionService;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tournaments@3102"));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: "tournaments.nl",
            audience: "tournaments.nl",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<bool> SaveRefreshToken(Guid clientId, Guid refreshToken, string username, Guid? oldRefreshToken = null)
    {
        var saveRefreshToken = await _tokenRepository.SaveRefreshToken(clientId, refreshToken, username, oldRefreshToken);
        if (!saveRefreshToken) throw _apiExceptionService.Create(HttpStatusCode.BadRequest, "Failed to authenticate");
        return saveRefreshToken;
    }
}

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    Task<bool> SaveRefreshToken(Guid clientId, Guid refreshToken, string username, Guid? oldRefreshToken = null);
}