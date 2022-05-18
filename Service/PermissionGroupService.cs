using Dtos;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services;

public class PermissionGroupService : IPermissionGroupService
{
    private readonly ILogger _logger;
    private readonly IPermissionGroupRepository _permissionGroupRepository;

    public PermissionGroupService(
        ILogger<PermissionGroupDto> logger,
        IPermissionGroupRepository permissionGroupRepository)
    {
        _logger = logger;
        _permissionGroupRepository = permissionGroupRepository;
    }

    public async Task<PermissionGroupDto> GetPermissionGroupById(Guid id)
    {
        var permissionGroup = await _permissionGroupRepository.GetPermissionGroupById(id);
        return permissionGroup.ToDto();
    }

    public async Task<List<PermissionGroupDto>> GetPermissionGroups()
    {
        var permissionGroups = await _permissionGroupRepository.GetPermissionGroups();
        return permissionGroups.Select(x => x.ToDto()).ToList();
    }

    public async Task<PermissionGroupDto> CreatePermissionGroup(PermissionGroupDto permissionGroup)
    {
        var orgPermissionGroup = permissionGroup.ToEntity();
        var newPermissionGroup = await _permissionGroupRepository.CreatePermissionGroup(orgPermissionGroup);
        _logger.Log(LogLevel.Information, $"Created permission group with id {newPermissionGroup.Id}");
        return newPermissionGroup.ToDto();
    }

    public async Task<PermissionGroupDto> UpdatePermissionGroup(PermissionGroupDto permissionGroup)
    {
        var orgPermissionGroup = permissionGroup.ToEntity();
        orgPermissionGroup.SetUpdated();
        var newPermissionGroup = await _permissionGroupRepository.UpdatePermissionGroup(orgPermissionGroup);
        _logger.Log(LogLevel.Information, $"Updated permission group with id {newPermissionGroup.Id}");
        return newPermissionGroup.ToDto();
    }

    public async Task<bool> DeletePermissionGroup(Guid id)
    {
        var oldPermissionGroup = await _permissionGroupRepository.DeletePermissionGroup(id);
        _logger.Log(LogLevel.Information, $"Deleted permission group with id {id}");
        return oldPermissionGroup;
    }
}

public interface IPermissionGroupService
{
    Task<PermissionGroupDto> GetPermissionGroupById(Guid id);
    Task<List<PermissionGroupDto>> GetPermissionGroups();
    Task<PermissionGroupDto> CreatePermissionGroup(PermissionGroupDto permissionGroup);
    Task<PermissionGroupDto> UpdatePermissionGroup(PermissionGroupDto permissionGroup);
    Task<bool> DeletePermissionGroup(Guid id);
}