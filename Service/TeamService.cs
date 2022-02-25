using Repositories;
using Mapster;
using Model;
using Dtos;

namespace Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
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
            Team orgTeam = team.Adapt<Team>();
            return _teamRepository.CreateTeam(orgTeam).Adapt<TeamDto>();
        }

        public TeamDto UpdateTeam(TeamDto team)
        {
            Team orgTeam = team.Adapt<Team>();
            orgTeam.SetUpdated();
            return _teamRepository.UpdateTeam(orgTeam).Adapt<TeamDto>();
        }

        public bool DeleteTeam(Guid id)
        {
            return _teamRepository.DeleteTeam(id);
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
