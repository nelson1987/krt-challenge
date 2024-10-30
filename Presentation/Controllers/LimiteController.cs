using Application.UseCases.ChangeLimite;
using Application.UseCases.CreateLimite;
using Application.UseCases.DeleteLimite;
using Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LimiteController(ILogger<LimiteController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string documento,
        [FromQuery] string conta,
        [FromServices] ILimiteRepository repository,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(Get)}");
        var response = await repository.GetAsync(documento, conta, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(Get)}");
        return response is null ? StatusCode(404) : StatusCode(200, response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateLimiteCommand command,
        [FromServices] IValidator<CreateLimiteCommand> validator,
        [FromServices] ICreateLimiteHandler handler,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(Post)}");
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(Post)}");
            return StatusCode(412, validationResult.Errors);
        }
        var response = await handler.HandleAsync(command, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(Post)}");
        return response.IsSuccess ? StatusCode(201, response.Value) : StatusCode(400);
    }

    [HttpPut]
    public async Task<IActionResult> Put(
        [FromBody] ChangeLimiteCommand command,
        [FromServices] IValidator<ChangeLimiteCommand> validator,
        [FromServices] IChangeLimiteHandler handler,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(Put)}");
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(Put)}");
            return StatusCode(412, validationResult.Errors);
        }
        var response = await handler.HandleAsync(command, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(Put)}");
        return response.IsSuccess ? StatusCode(204) : StatusCode(400);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteLimiteCommand command,
        [FromServices] IValidator<DeleteLimiteCommand> validator,
        [FromServices] IDeleteLimiteHandler handler,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(Delete)}");
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(Delete)}");
            return StatusCode(412, validationResult.Errors);
        }
        var response = await handler.HandleAsync(command, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(Delete)}");
        return response.IsSuccess ? StatusCode(204) : StatusCode(400);
    }
}