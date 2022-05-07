using Migrations;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TournamentDbContext _dbContext;

        public UserRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users.AsQueryable()
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();
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
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }

    public interface IUserRepository
    {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsername(string username);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
    }
}
