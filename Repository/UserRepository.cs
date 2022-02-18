using Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TournamentDbContext _dbContext;

        public UserRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext;
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
        User CreateUser(User user);
    }
}
