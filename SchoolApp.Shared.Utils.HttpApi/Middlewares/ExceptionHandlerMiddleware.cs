using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SchoolApp.Shared.Utils.HttpApi.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            context = await SetResponseErrorFromExceptionAsync(context, e);
        }
    }

    private async Task<HttpContext> SetResponseErrorFromExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            NotImplementedException => HttpStatusCode.NotAcceptable,
            UnauthorizedAccessException => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { errorMessage = exception.Message }));

        return context;
    }
}
