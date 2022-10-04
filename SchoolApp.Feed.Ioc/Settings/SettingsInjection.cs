using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Feed.Ioc.Settings;

public static class SettingsInjection
{
    public static void AddFeedSettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
    }
}
