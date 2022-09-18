using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.IdentityProvider.Application.Settings;

namespace SchoolApp.IdentityProvider.Ioc.Settings;

public static class SettingsInjection
{
    public static void AddIdentityProviderSettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));
    }
}
