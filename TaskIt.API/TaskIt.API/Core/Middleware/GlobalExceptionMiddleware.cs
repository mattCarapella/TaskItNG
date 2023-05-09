using System.Diagnostics;

namespace TaskIt.API.Core.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not proceess request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            await Results.Problem(
                title: "An error occurred but we're working on it!",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    { "traceId", Activity.Current?.Id }
                }
            ).ExecuteAsync(httpContext);
        }
    }

}