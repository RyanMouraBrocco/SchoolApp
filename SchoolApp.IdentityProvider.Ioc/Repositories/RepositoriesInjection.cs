using Microsoft.Extensions.DependencyInjection;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Repositories;

namespace SchoolApp.IdentityProvider.Ioc.Repositories;

public static class RepositoriesInjection
{
    public static void AddIdentityProviderRepositories(this IServiceCollection service)
    {
        service.AddScoped<ITeacherRepository, TeacherRepository>();
        service.AddScoped<IManagerRepository, ManagerRepository>();
        service.AddScoped<IOwnerRepository, OwnerRepository>();
        service.AddScoped<IFunctionRepository, FunctionRepository>();
        service.AddScoped<ITeacherFormationRepository, TeacherFormationRepository>();
        service.AddScoped<IMessageAllowedClassroomRepository, MessageAllowedClassroomRepository>();
        service.AddScoped<IMessageAllowedStudentRepository, MessageAllowedStudentRepository>();
    }
}
