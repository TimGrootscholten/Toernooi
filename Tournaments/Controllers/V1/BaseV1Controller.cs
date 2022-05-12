using Microsoft.AspNetCore.Mvc;

namespace Tournaments.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class BaseV1Controller : ControllerBase
{
    internal bool HasError(string fieldname)
    {
        return ModelState.Keys.Where(k => k == fieldname).Select(k => ModelState[k]!.Errors[0].ErrorMessage).Any();
    }
}