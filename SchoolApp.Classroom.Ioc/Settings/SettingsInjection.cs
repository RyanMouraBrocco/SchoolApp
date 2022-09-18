using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Classroom.Http.Settings;

namespace SchoolApp.Classroom.Ioc.Settings;

public static class SettingsInjection
{
    public static void AddClassroomSettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<IdentityProviderServiceApiSettings>(configuration.GetSection(nameof(IdentityProviderServiceApiSettings)));
    }
}
