using Dtos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Services.Security;

namespace Tournaments.Controllers.V1;

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
    [Scope(TournamentPermissions.TeamRead)]
    public TeamDto GetTeamById(Guid id)
    {
        return _teamService.GetTeamById(id);
    }

    [HttpGet]
    [Scope(TournamentPermissions.TeamRead)]
    public List<TeamDto> GetTeams()
    {
        return _teamService.GetTeams();
    }

    [HttpPost]
    [Scope(TournamentPermissions.TeamCreate)]
    public TeamDto CreateTeam([FromBody] TeamDto team)
    {
        return _teamService.CreateTeam(team);
    }

    [HttpPut]
    [Scope(TournamentPermissions.TeamUpdate)]
    public TeamDto UpdateTeam([FromBody] TeamDto team)
    {
        return _teamService.UpdateTeam(team);
    }

    [HttpDelete("{id:guid}")]
    [Scope(TournamentPermissions.TeamDelete)]
    public bool DeleteTeam(Guid id)
    {
        return _teamService.DeleteTeam(id);
    }
}