using FluentValidation.Results;
using Microsoft.AspNetCore.Http.Features;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Responses;
using System.Text.Json;

namespace N5Challenge.Api.Middlewares;

public class HandlingExceptionMiddleware(RequestDelegate next, ILogger<HandlingExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    private readonly ILogger<HandlingExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        string url = $"{context?.Request?.Path}{context?.Request?.QueryString}";
        context?.Request?.EnableBuffering();

        var responseFeature = context?.Response?.HttpContext?.Features?.Get<IHttpResponseFeature>();
        var method = context?.Request?.Method;

        string reasonPhrase = "Internal Server Error";
        int statusCode = 500;
        ErrorResponse errorResponse = new("An unexpected error occurred.");

        switch (ex)
        {
            case OperationCanceledException:
                reasonPhrase = "Client Closed Request";
                statusCode = 499;
                _logger.LogInformation(ex, "{url} :: {reasonPhrase} :: Request was cancelled", url, reasonPhrase);
                break;

            case FluentValidation.ValidationException validationEx:
                reasonPhrase = "Validation Failed";
                statusCode = 400;
                errorResponse = new ErrorResponse(
                    "Validation failed.",
                    validationEx.Errors.Select(e => e.ErrorMessage ?? "").ToArray()
                );
                _logger.LogWarning(validationEx, "{url} :: {reasonPhrase} :: {message}\nErrors:\n{errors}",
                    url, reasonPhrase, validationEx.Message,
                    string.Join("\n", validationEx.Errors.Select(e => $"\t> {e.ErrorMessage}"))
                );
                break;

            case EntityNotFoundException:
                reasonPhrase = "Entity Not Found";
                statusCode = 404;
                errorResponse = new ErrorResponse("The requested resource could not be found.");
                _logger.LogError(ex, "{url} :: {reasonPhrase} :: {message}", url, reasonPhrase, ex.Message);
                break;

            case RelatedEntityNotFoundException:
                reasonPhrase = "Related Entity Not Found";
                statusCode = 400;
                errorResponse = new ErrorResponse("A related resource required for this operation does not exist.");
                _logger.LogError(ex, "{url} :: {reasonPhrase} :: {message}", url, reasonPhrase, ex.Message);
                break;

            default:
                reasonPhrase = "Internal Server Error";
                statusCode = 500;
                errorResponse = new ErrorResponse("An unexpected error occurred.");
                _logger.LogError(ex, "{url} :: {reasonPhrase} :: {message}", url, reasonPhrase, ex.Message);
                break;
        }

        responseFeature!.ReasonPhrase = reasonPhrase;
        context.Response.StatusCode = statusCode;

        var responseBody = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(responseBody);
    }
}
