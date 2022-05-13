using Microsoft.EntityFrameworkCore;
using Migrations;
using Models;

namespace Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly TournamentDbContext _dbContext;

    public TokenRepository(TournamentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SaveRefreshToken(Guid clientId, Guid refreshToken, string username, Guid? oldRefreshToken = null)
    {
        var token = await _dbContext.Tokens.AsNoTracking()
            .Where(x => x.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (token == null)
        {
            Token saveToken = new Token
            {
                ClientId = clientId,
                RefreshToken = refreshToken,
                RefreshTokenExpires = DateTime.UtcNow.AddDays(7),
                Username = username
            };
            await _dbContext.Tokens.AddAsync(saveToken);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        if (token.RefreshToken == oldRefreshToken)
        {
            token.RefreshToken = refreshToken;
            token.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
            await _dbContext.Tokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        if (token.ClientId == clientId)
        {
            _dbContext.Tokens.Remove(new Token {Id = token.Id});
            await _dbContext.SaveChangesAsync();
        }

        return false;
    }

    public async Task<Token> CheckRefreshToken(Guid clientId, Guid oldRefreshToken)
    {
        var token = await _dbContext.Tokens.AsQueryable()
            .Where(x => x.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (token == null) return null;
        if (token.RefreshToken == oldRefreshToken && DateTime.UtcNow < token.RefreshTokenExpires) return token;
        return null;
    }

    public async Task<bool> DeleteClientGrant(Guid clientId)
    {
        var token = await _dbContext.Tokens.AsQueryable()
            .Where(x => x.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (token != null)
        {
            _dbContext.Tokens.Remove(token);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}

public interface ITokenRepository
{
    Task<bool> SaveRefreshToken(Guid clientId, Guid refreshToken, string username, Guid? oldRefreshToken = null);
    Task<Token> CheckRefreshToken(Guid clientId, Guid oldRefreshToken);
    Task<bool> DeleteClientGrant(Guid clientId);
}