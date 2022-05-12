using Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Security;
using Services.User;

namespace Tournaments.Controllers.V1;

[Route("api/v1/users")]
[Produces("application/json")]
public class UserV1Controller : BaseV1Controller
{
    private readonly IUserService _userService;

    public UserV1Controller(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    [Scope]
    public async Task<UserInfoDto> GetUserById(Guid id)
    {
        return await _userService.GetUserById(id);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto user)
    {
        await _userService.CreateUser(user);
        return Ok();
    }

    [HttpPut]
    [Scope]
    public async Task<IActionResult> UpdateUser([FromBody] UserEditDto user)
    {
        await _userService.UpdateUser(user);
        return Ok();
    }

    [HttpPut("add-permission-groups")]
    [Scope]
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

    [HttpPost("is-unique-username")]
    public async Task<bool> IsUniqueUsername(string username)
    {
        return await _userService.IsUniqueUsername(username);
    }
}