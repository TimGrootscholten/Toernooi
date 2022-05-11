using System.ComponentModel.DataAnnotations;
using Models;

namespace Dtos;

public class PermissionGroupDto : BaseDto
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public List<int> Permissions { get; set; }
}

public static class PermissionGroupDtoExtension
{
    public static PermissionGroup ToEntity(this PermissionGroupDto dto)
    {
        return new PermissionGroup
        {
            Id = dto.Id,
            Name = dto.Name,
            Permissions = dto.Permissions.ToCommaSeparateString(),
            Created = dto.Created,
            Updated = dto.Updated
        };
    }

    public static PermissionGroupDto ToDto(this PermissionGroup entity)
    {
        return new PermissionGroupDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Permissions = entity.Permissions.CommaSeparateStringToList(),
                Created = entity.Created,
                Updated = entity.Updated
            };
    }
}