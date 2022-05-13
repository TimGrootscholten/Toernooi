using Migrations;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories;

public class UserRepository : IUserRepository
{
    private readonly TournamentDbContext _dbContext;

    public UserRepository(TournamentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _dbContext.Users.AsQueryable()
            .Where(x => x.Id == id)
            .Include("PermissionGroups")
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _dbContext.Users.AsQueryable()
            .Where(x => x.Username == username)
            .Include("PermissionGroups")
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _dbContext.Users.AsQueryable()
            .Include("PermissionGroups")
            .ToListAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
        _dbContext.Entry(user).Property(x => x.Username).IsModified = false;
        _dbContext.Entry(user).Property(x => x.Password).IsModified = false;
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> IsUsernameUnique(string username)
    {
        return await _dbContext.Users.AsQueryable()
            .Where(x => x.Username == username)
            .AnyAsync();
    }
}

public interface IUserRepository
{
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByUsername(string username);
    Task<List<User>> GetUsers();
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task<bool> IsUsernameUnique(string username);
}