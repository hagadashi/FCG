using System.Net;
using System.Text.Json;
using Datadog.Trace;
using FCG.Application.Settings;
using FCG.Domain.Exceptions;

namespace FCG.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Ocorreu uma exceção não tratada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ErrorResponse();

        switch (exception)
        {
            case DomainException domainEx:
                response.Message = domainEx.Message;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Details = "Erro de regra de negócio";
                break;

            case UnauthorizedAccessException:
                response.Message = "Acesso não autorizado";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Details = "Credenciais inválidas ou token expirado";
                break;

            case KeyNotFoundException:
                response.Message = "Recurso não encontrado";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Details = exception.Message;
                break;

            case ArgumentException argEx:
                response.Message = "Parâmetros inválidos";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Details = argEx.Message;
                break;

            case InvalidOperationException invalidOpEx:
                response.Message = "Operação inválida";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Details = invalidOpEx.Message;
                break;

            default:
                response.Message = "Erro interno do servidor";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Details = "Entre em contato com o suporte técnico";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var span = Tracer.Instance.ActiveScope?.Span;
        if (span is not null)
        {
            var errorStatusCodes = new[] { 400, 500, 502, 503, 504 };
            if (errorStatusCodes.Contains(context.Response.StatusCode))
            {
                span.Error = true;
            }

            span.SetTag("error.msg", exception.Message);
            span.SetTag("error.type", exception.GetType().Name);
            span.SetTag("http.status_code", context.Response.StatusCode.ToString());
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

