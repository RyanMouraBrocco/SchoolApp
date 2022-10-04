using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Feed.Application.Interfaces.Services;
using SchoolApp.Feed.Application.Services;

namespace SchoolApp.Feed.Ioc.Services;

public static class ServicesInjection
{
    public static void AddFeedServices(this IServiceCollection service)
    {
        service.AddScoped<IMessageService, MessageService>();
    }
}
