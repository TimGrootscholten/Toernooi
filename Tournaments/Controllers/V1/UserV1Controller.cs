using Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;

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
    }
}
