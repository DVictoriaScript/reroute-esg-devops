using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ReRoute.Api.Middleware;

/// <summary>
/// Middleware global de tratamento de exceções.
/// Converte erros internos em respostas JSON padronizadas para o cliente (Postman, Swagger, app mobile).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

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
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado na API ReRoute.");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno. Consulte os logs da aplicação.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode status, string detail)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = status.ToString(),
            Detail = detail
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
