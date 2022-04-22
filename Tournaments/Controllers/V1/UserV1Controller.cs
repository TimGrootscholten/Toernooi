using Dtos;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CreateUser([FromBody] UserDto user)
        {
            _userService.CreateUser(user);
            return Ok();
        }
        
        [HttpPost("token")]
        public async Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequestDto)
        {
            return await _userService.Authenticate(authenticateRequestDto);
        }
    }
}
