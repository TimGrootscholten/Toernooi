using System.ComponentModel.DataAnnotations;
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
    public async Task<UserInfoDto> GetUserById([Required] Guid id)
    {
        return await _userService.GetUserById(id);
    }

    [HttpPost]
    public async Task<AuthResponse> CreateUser([FromBody] UserDto user)
    {
        return await _userService.CreateUser(user);
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
    public async Task<IActionResult> AddPermissionGroups([Required] Guid userId, [Required] List<Guid> permissionGroupIds)
    {
        await _userService.AddPermissionGroups(userId, permissionGroupIds);
        return Ok();
    }

    [HttpPost("token")]
    public async Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest)
    {
        return await _userService.Authenticate(authenticateRequest);
    }

    [HttpPost("authenticate-with-refresh-token")]
    public async Task<AuthResponse> Authenticate(AuthenticateWithRefreshTokenDto authenticateWithRefreshToken)
    {
        return await _userService.AuthenticateWithRefreshToken(authenticateWithRefreshToken);
    }

    [HttpDelete("client-grant")]
    public async Task<bool> DeleteClientGrant([Required] Guid clientId)
    {
        return await _userService.DeleteClientGrant(clientId);
    }

    [HttpPost("is-unique-username")]
    public async Task<bool> IsUniqueUsername([Required] string username)
    {
        return await _userService.IsUniqueUsername(username);
    }
}