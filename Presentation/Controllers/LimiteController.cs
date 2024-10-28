using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LimiteController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        return StatusCode(200);
    }
}