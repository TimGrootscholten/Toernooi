using Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Tournaments.Controllers.V1
{
    [Route("api/v1/teams")]
    [Produces("application/json")]
    public class TeamV1Controller : BaseV1Controller
    {
        private readonly ITeamService _teamService;

        public TeamV1Controller(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("{id:guid}")]
        public TeamDto GetTeamById(Guid id)
        {
            return _teamService.GetTeamById(id);
        }

        [HttpGet]
        public List<TeamDto> GetTeams()
        {
            return _teamService.GetTeams();
        }

        [HttpPost]
        public TeamDto CreateTeam([FromBody] TeamDto team)
        {
            return _teamService.CreateTeam(team);
        }

        [HttpPut]
        public TeamDto UpdateTeam([FromBody] TeamDto team)
        {
            return _teamService.UpdateTeam(team);
        }

        [HttpDelete("{id:guid}")]
        public bool DeleteTeam(Guid id)
        {
            return _teamService.DeleteTeam(id);
        }
    }
}