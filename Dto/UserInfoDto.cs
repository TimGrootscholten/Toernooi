using System.Diagnostics.CodeAnalysis;
using Models;

namespace Dtos
{
    public class UserInfoDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PermissionGroupDto> PermissionGroups { get; set; }
    }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    public static class UserInfoDtoExtension
    {
        public static UserInfoDto ToInfoDto(this User entity)
        {
            return new UserInfoDto
            {
                Id = entity.Id,
                Username = entity.Username,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PermissionGroups = entity.PermissionGroups != null ? entity.PermissionGroups.Select(x => x.ToDto()).ToList() : new List<PermissionGroupDto>(),
                Created = entity.Created,
                Updated = entity.Updated
            };
        }
    }
}
