using Dtos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Services.Security;

namespace Tournaments.Controllers.V1;

[Route("api/v1/permissiongroup")]
[Produces("application/json")]
public class PermissionGroupV1Controller : BaseV1Controller
{
    private readonly IPermissionGroupService _permissionGroupService;

    public PermissionGroupV1Controller(IPermissionGroupService permissionGroupService)
    {
        _permissionGroupService = permissionGroupService;
    }

    [HttpGet("{id:guid}")]
    [Scope(TournamentPermissions.PermissionGroupRead)]
    public async Task<PermissionGroupDto> GetPermissionGroupById(Guid id)
    {
        return await _permissionGroupService.GetPermissionGroupById(id);
    }

    [HttpGet]
    [Scope(TournamentPermissions.PermissionGroupRead)]
    public async Task<List<PermissionGroupDto>> GetPermissionGroups()
    {
        return await _permissionGroupService.GetPermissionGroups();
    }

    [HttpPost]
    [Scope(TournamentPermissions.PermissionGroupCreate)]
    public async Task<PermissionGroupDto> CreatePermissionGroup(PermissionGroupDto permissionGroup)
    {
        return await _permissionGroupService.CreatePermissionGroup(permissionGroup);
    }

    [HttpPut]
    [Scope(TournamentPermissions.PermissionGroupUpdate)]
    public async Task<PermissionGroupDto> UpdatePermissionGroup(PermissionGroupDto permissionGroup)
    {
        return await _permissionGroupService.UpdatePermissionGroup(permissionGroup);
    }

    [HttpDelete("{id:guid}")]
    [Scope(TournamentPermissions.PermissionGroupDelete)]
    public async Task<bool> DeletePermissionGroup(Guid id)
    {
        return await _permissionGroupService.DeletePermissionGroup(id);
    }
}