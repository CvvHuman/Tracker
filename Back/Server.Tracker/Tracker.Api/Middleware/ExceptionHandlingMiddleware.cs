using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Tracker.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private const string DefaultErrorMessage = "Внутренняя ошибка сервера. Попробуйте позже.";
    private const string UnauthorizedMessage = "Доступ запрещен. Авторизуйтесь.";
    private const string ValidationErrorMessage = "Ошибка валидации данных.";
    private const string NotFoundMessage = "Запрашиваемый ресурс не найден.";
    private const string BadRequestMessage = "Некорректный запрос.";

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла необработанная ошибка: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, responseBody) = exception switch
        {
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, CreateResponse(UnauthorizedMessage)),

            ValidationException valEx =>
                (HttpStatusCode.BadRequest, CreateResponse(ValidationErrorMessage, FormatValidationErrors(valEx))),

            KeyNotFoundException =>
                (HttpStatusCode.NotFound, CreateResponse(NotFoundMessage)),

            ArgumentException =>
                (HttpStatusCode.BadRequest, CreateResponse(BadRequestMessage)),

            _ =>
                (HttpStatusCode.InternalServerError, CreateResponse(DefaultErrorMessage))
        };

        context.Response.StatusCode = (int)statusCode;

        await JsonSerializer.SerializeAsync(context.Response.Body, responseBody, SerializerOptions);
    }

    private static object CreateResponse(string message, Dictionary<string, string[]>? errors = null) =>
        new { message, errors };

    private static Dictionary<string, string[]> FormatValidationErrors(ValidationException exception) =>
        exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
}
