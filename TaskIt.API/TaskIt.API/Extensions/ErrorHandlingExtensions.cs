using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace TaskIt.API.Extensions;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExtensionHandler(this IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionDetails?.Error;

            logger.LogError(exception, "Could not proceess request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            await Results.Problem(
                title: "An error occurred but we're working on it.",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    { "traceId", Activity.Current?.Id }
                }
            ).ExecuteAsync(context);
        });
    }
}