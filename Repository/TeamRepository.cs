using Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TournamentDbContext _dbContext;

        public TeamRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Team GetTeamById(Guid id)
        {
            return _dbContext.Teams.Find(id);
        }

        public List<Team> GetTeams()
        {
            return _dbContext.Teams.AsQueryable().ToList();
        }

        public Team CreateTeam(Team team)
        {
            _dbContext.Teams.Add(team);
            _dbContext.SaveChanges();
            return team;
        }

        public Team UpdateTeam(Team team)
        {
            _dbContext.Teams.Update(team);
            _dbContext.SaveChanges();
            return team;
        }

        public bool DeleteTeam(Guid id)
        {
            _dbContext.Teams.Remove(new Team{Id = id});
            _dbContext.SaveChanges();
            return true;
        }
    }

    public interface ITeamRepository
    {
        Team GetTeamById(Guid id);
        List<Team> GetTeams();
        Team CreateTeam(Team team);
        Team UpdateTeam(Team team);
        bool DeleteTeam(Guid id);
    }
}
