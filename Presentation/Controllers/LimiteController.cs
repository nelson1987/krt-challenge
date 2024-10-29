using Application.UseCases.CreateLimite;
using Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LimiteController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromBody] GetLimiteQuery query,
        [FromServices] ILimiteRepository repository,
        CancellationToken cancellationToken = default)
    {
        var response = await repository.GetAsync(query.Documento, query.Agencia, query.Conta, cancellationToken);

        return response is null ? StatusCode(404) : StatusCode(200, response.Value);
    }

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

        return response.IsSuccess ? StatusCode(201, response.Value) : StatusCode(400);
    }

    [HttpPut]
    public async Task<IActionResult> Put(
        [FromBody] ChangeLimiteCommand command,
        [FromServices] IValidator<ChangeLimiteCommand> validator,
        [FromServices] IChangeLimiteHandler handler,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return StatusCode(412, validationResult.Errors);

        var response = await handler.Handle(command, cancellationToken);

        return response.IsSuccess ? StatusCode(204) : StatusCode(400);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteLimiteCommand command,
        [FromServices] IValidator<DeleteLimiteCommand> validator,
        [FromServices] IDeleteLimiteHandler handler,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return StatusCode(412, validationResult.Errors);

        var response = await handler.Handle(command, cancellationToken);

        return response.IsSuccess ? StatusCode(204) : StatusCode(400);
    }
}