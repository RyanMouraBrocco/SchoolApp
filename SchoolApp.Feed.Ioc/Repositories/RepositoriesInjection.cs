using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.NoSql.Repositories;
using SchoolApp.Feed.Queue.Repositories;

namespace SchoolApp.Feed.Ioc.Repositories;

public static class RepositoriesInjection
{
    public static void AddFeedRepositories(this IServiceCollection service)
    {
        service.AddScoped<IMessageRepository, MessageRepository>();
        service.AddScoped<IMessageAllowedPermissionRepository, MessageAllowedPermissionRepository>();
    }
}
