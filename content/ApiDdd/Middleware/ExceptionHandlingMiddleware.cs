using System.Net;
using System.Text.Json;
using DddApiTemplate.Domain.Exceptions;

namespace DddApiTemplate.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Regra de domínio violada");
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            status = (int)statusCode,
            title = message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
