using System.Diagnostics;
using System.Text;

namespace FCG.API.Middlewares;

public class RequestLoggingMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;


    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString();

        // Adiciona correlation ID no header da resposta
        context.Response.Headers.Append("X-Correlation-ID", correlationId);

        // Log da requisição de entrada
        await LogRequest(context, correlationId);

        // Captura o body da resposta
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            // Log da resposta
            await LogResponse(context, correlationId, stopwatch.ElapsedMilliseconds);

            // Restaura o body original
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task LogRequest(HttpContext context, string correlationId)
    {
        var request = context.Request;

        var requestBody = string.Empty;
        if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        _logger.LogInformation("Incoming Request: {Method} {Path} | CorrelationId: {CorrelationId} | ContentType: {ContentType} | Body: {Body}",
            request.Method,
            request.Path,
            correlationId,
            request.ContentType ?? "N/A",
            string.IsNullOrEmpty(requestBody) ? "N/A" : requestBody);
    }

    private async Task LogResponse(HttpContext context, string correlationId, long elapsedMilliseconds)
    {
        var response = context.Response;

        var responseBody = string.Empty;
        if (response.Body.CanSeek && response.ContentType?.Contains("application/json") == true)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            responseBody = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
        }

        var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

        _logger.Log(logLevel, "Outgoing Response: {StatusCode} | CorrelationId: {CorrelationId} | Duration: {Duration}ms | ContentType: {ContentType} | Body: {Body}",
            response.StatusCode,
            correlationId,
            elapsedMilliseconds,
            response.ContentType ?? "N/A",
            string.IsNullOrEmpty(responseBody) ? "N/A" : responseBody);
    }

}