using Repositories;
using Mapster;
using Models;
using Dtos;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class TeamService : ITeamService
    {
        private readonly ILogger _logger;
        private readonly ITeamRepository _teamRepository;

        public TeamService(
            ILogger<TeamService> logger,
            ITeamRepository teamRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
        }

        public TeamDto GetTeamById(Guid id)
        {
            return _teamRepository.GetTeamById(id).Adapt<TeamDto>();
        }

        public List<TeamDto> GetTeams()
        {
            return _teamRepository.GetTeams().Adapt<List<TeamDto>>();
        }

        public TeamDto CreateTeam(TeamDto team)
        {
            var orgTeam = team.Adapt<Team>();
            var newTeam = _teamRepository.CreateTeam(orgTeam).Adapt<TeamDto>();
            _logger.Log(LogLevel.Information, $"Created team with id {newTeam.Id}");
            return newTeam;
        }

        public TeamDto UpdateTeam(TeamDto team)
        {
            var orgTeam = team.Adapt<Team>();
            orgTeam.SetUpdated();
            var newTeam = _teamRepository.UpdateTeam(orgTeam).Adapt<TeamDto>();
            _logger.Log(LogLevel.Information, $"Updated team with id {newTeam.Id}");
            return newTeam;
        }

        public bool DeleteTeam(Guid id)
        {
            var oldTeam = _teamRepository.DeleteTeam(id);
            _logger.Log(LogLevel.Information, $"Deleted team with id {id}");
            return oldTeam;
        }
    }

    public interface ITeamService
    {
        TeamDto GetTeamById(Guid id);
        List<TeamDto> GetTeams();
        TeamDto CreateTeam(TeamDto team);
        TeamDto UpdateTeam(TeamDto team);
        bool DeleteTeam(Guid id);
    }
}
