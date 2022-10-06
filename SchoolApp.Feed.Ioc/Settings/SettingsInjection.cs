using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Feed.Queue.Settings;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Feed.Ioc.Settings;

public static class SettingsInjection
{
    public static void AddFeedSettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        service.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));
    }
}
