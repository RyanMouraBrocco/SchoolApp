using Microsoft.Extensions.DependencyInjection;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Application.Services;

namespace SchoolApp.IdentityProvider.Ioc.Services;

public static class ServicesInjection
{
    public static void AddIdentityProviderServices(this IServiceCollection service)
    {
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        service.AddScoped<IOwnerService, OwnerService>();
        service.AddScoped<ITeacherService, TeacherService>();
        service.AddScoped<IManagerService, ManagerService>();
        service.AddScoped<IFunctionService, FunctionService>();
    }
}
