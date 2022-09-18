using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.IdentityProvider.Sql.Context;

namespace SchoolApp.IdentityProvider.Ioc.Database;

public static class DatabaseInjection
{
    public static void AddIdentityProviderDatabase(this IServiceCollection service)
    {
        service.AddDbContext<SchoolAppIdentityProviderContext>(options => options.UseSqlServer("name=ConnectionStrings:IdentityProvider"));
    }
}
