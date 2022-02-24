using Microsoft.AspNetCore.Mvc;

namespace Tournaments.Controllers.V1
{
    [Route("api/v1/teams")]
    [Produces("application/json")]
    public class TeamV1Controller : BaseV1Controller
    {

        public TeamV1Controller()
        {
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetTeam()
        {
            return Ok("Pizza");
        }
    }
}