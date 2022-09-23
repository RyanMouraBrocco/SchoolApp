using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Activity.Http.Settings;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Activity.Ioc.Settings;

public static class SettingsInjection
{
    public static void AddActivitySettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        service.Configure<ClassroomServiceApiSettings>(configuration.GetSection(nameof(ClassroomServiceApiSettings)));
    }
}
