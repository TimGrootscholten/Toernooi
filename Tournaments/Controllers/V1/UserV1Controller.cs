using Dtos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Security;
using Services.User;

namespace Tournaments.Controllers.V1
{
    [Route("api/v1/users")]
    [Produces("application/json")]
    public class UserV1Controller : BaseV1Controller
    {
        private readonly IUserService _userService;

        public UserV1Controller(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            await _userService.CreateUser(user);
            return Ok();
        }

        [Scope]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserEditDto user)
        {
            await _userService.UpdateUser(user);
            return Ok();
        }

        [Scope]
        [HttpPut("addPermissionGroups")]
        public async Task<IActionResult> AddPermissionGroups(Guid userId, List<Guid> permissionGroupIds)
        {
            await _userService.AddPermissionGroups(userId, permissionGroupIds);
            return Ok();
        }
        
        [HttpPost("token")]
        public async Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequestDto)
        {
            return await _userService.Authenticate(authenticateRequestDto);
        }
    }
}
