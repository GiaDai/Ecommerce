using System.Diagnostics;
using Serilog;

namespace Ecommerce.WebApp.FrontEnd.Server.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        // Log request body
        context.Request.EnableBuffering();
        var requestBodyStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body = requestBodyStream;

        Log.Information($"Request: {context.Request.Method} {context.Request.Path} {requestBodyText}");

        // Log response body
        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await _next(context);
        stopwatch.Stop();
        var processingTime = stopwatch.ElapsedMilliseconds;

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        await responseBodyStream.CopyToAsync(originalBodyStream);

        Log.Information("Request: {Method} {Path} {RequestBody}", context.Request.Method, context.Request.Path, requestBodyText);
        Log.Information("Response: {StatusCode} {ResponseBody} {ProcessingTime}ms", context.Response.StatusCode, responseBodyText, processingTime);
    }
}
