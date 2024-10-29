using Application.UseCases.CreateLimite;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LimiteController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateLimiteCommand command,
        [FromServices] IValidator<CreateLimiteCommand> validator,
        [FromServices] ICreateLimiteHandler handler,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return StatusCode(412, validationResult.Errors);

        var response = await handler.Handle(command, cancellationToken);

        return response.IsSuccess ? StatusCode(200, response.Value) : StatusCode(404);
    }
}