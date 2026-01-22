using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Base;
using Freaks.WebApi.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Freaks.WebApi.Common.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ExceptionHandlerMiddleware" />.
    /// </summary>
    /// <param name="next">Делегат для вызова следующего middleware в конвейере.</param>
    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, ILogger<ExceptionHandlerMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleException(context, ex, env);
        }
    }

    private static async Task HandleException(HttpContext context, Exception ex, IWebHostEnvironment env)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "UNKNOWN_EXCEPTION";
        var message = "Неизвестная ошибка";
        var errors = new Dictionary<string, string[]>();

        if (ex is ValidationApiException validationApiException)
        {
            statusCode = validationApiException.StatusCode;
            errorCode = validationApiException.ErrorCode;
            message = validationApiException.Message;
            errors = validationApiException.Errors.ToDictionary(e => e.Key, e => e.Value.ToArray());
        }
        else if (ex is BaseApiException baseApiException)
        {
            statusCode = baseApiException.StatusCode;
            errorCode = baseApiException.ErrorCode;
            message = baseApiException.Message;
        }

        var stackTrace = string.Empty;
        if (env.IsDevelopment()
            || env.IsCompose())
        {
            stackTrace = ex.StackTrace;
        }

        var exceptionResponse =
            new
            {
                Message = message,
                ErrorCode = errorCode,
                Errors = errors,
                StackTrace = stackTrace,
            };

        context.Response.StatusCode = statusCode.GetHashCode();
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(exceptionResponse, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
    }
}