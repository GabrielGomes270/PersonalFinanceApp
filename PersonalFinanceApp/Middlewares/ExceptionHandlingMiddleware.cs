namespace PersonalFinanceApp.Middlewares;
using FluentValidation;
using PersonalFinanceApp.Responses;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }

        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado");

            await WriteResponseAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }

        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acesso não autorizado");

            await WriteResponseAsync(
                context,
                HttpStatusCode.Unauthorized,
                "Não autorizado");
        }

        catch (FluentValidation.ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new ValidationErrorResponse
            {
                Errors = errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");

            await WriteResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Erro interno no servidor");
        }


    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}

