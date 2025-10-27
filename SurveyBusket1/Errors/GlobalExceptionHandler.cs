using Microsoft.AspNetCore.Diagnostics;
using System;

namespace SurveyBusket1.Errors;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger
    ) : IExceptionHandler
{
    // Handles all unhandled exceptions globally in the ASP.NET Core application
    private readonly ILogger<GlobalExceptionHandler> _Logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Log the exception details (message + stack trace) for developers to review
        _Logger.LogError(exception, "Something Went wrong {Message}", exception.Message);

        // Create a standardized error response using ProblemDetails (RFC 7807 format)
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError, // HTTP 500: Internal Server Error
            Title = "Internal Server Error",                   // Short title describing the problem
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1" // Reference to HTTP spec for 500 errors
        };

        // Set the HTTP status code of the response
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // Write the problem details as a JSON response to the client
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        // Return true to indicate that the exception was handled successfully
        return true;
    }

}
