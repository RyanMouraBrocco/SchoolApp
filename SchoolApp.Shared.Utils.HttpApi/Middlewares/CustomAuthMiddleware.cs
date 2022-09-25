using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SchoolApp.Shared.Utils.Authentication;

namespace SchoolApp.Shared.Utils.HttpApi.Middlewares;

public class CustomAuthMiddleware
{
    private CustomAuthenticationSettings Settings { get; set; }
    private readonly RequestDelegate _next;

    public CustomAuthMiddleware(RequestDelegate next, IOptions<CustomAuthenticationSettings> settings)
    {
        _next = next;
        Settings = settings.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path != "/" && context.Request.Path != "/swagger/index.html")
        {
            if (context.Request.Headers.TryGetValue("Internal-Key", out var values))
            {
                var internalKey = values.FirstOrDefault();
                if (internalKey != Settings.Key)
                    throw new UnauthorizedAccessException();

                await _next(context);
            }
            else
                throw new UnauthorizedAccessException();
        }
        else
            await _next(context);
    }
}
