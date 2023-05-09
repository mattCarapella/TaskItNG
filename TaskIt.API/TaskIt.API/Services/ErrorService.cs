using Microsoft.AspNetCore.Mvc;

namespace TaskIt.API.Services;

public class ErrorService
{
    private readonly ILogger<ErrorService> _logger;

    public ErrorService(ILogger<ErrorService> logger)
    {
        _logger = logger;
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public IActionResult NotFoundProblemDetails(int id)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = $"Item with ID {id} was not found.",
            Status = StatusCodes.Status404NotFound
        };

        return new NotFoundObjectResult(problemDetails);
    }


    public IActionResult InvalidModelProblemDetails()
    {
        var problemDetails = new ProblemDetails
        {
            Detail = $"Bad Request: Invalid model.",
            Status = StatusCodes.Status400BadRequest
        };

        return new BadRequestObjectResult(problemDetails);
    }


    public IActionResult InvalidNameProblemDetails(string name)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = $"An item with with the name \"{name}\" already exists.",
            Status = StatusCodes.Status422UnprocessableEntity
        };

        return new UnprocessableEntityObjectResult(problemDetails);
    }


}