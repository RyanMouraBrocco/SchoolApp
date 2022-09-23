using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Activity.Application.Services;

namespace SchoolApp.Activity.Ioc.Services;

public static class ServicesInjection
{
    public static void AddActivityServices(this IServiceCollection service)
    {
        service.AddScoped<IActivityAnswerService, ActivityAnswerService>();
        service.AddScoped<IActivityService, ActivityService>();
    }
}
