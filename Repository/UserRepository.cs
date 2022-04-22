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

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users.AsQueryable()
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();
        }

        public User CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
    }

    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        User CreateUser(User user);
    }
}
