using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Http.Repositories;
using SchoolApp.Activity.NoSql.Repositories;

namespace SchoolApp.Activity.Ioc.Repositories;

public static class RepositoriesInjection
{
    public static void AddActivityRepositories(this IServiceCollection service)
    {
        service.AddHttpClient<IClassroomRepository, ClassroomRepository>();
        service.AddHttpClient<IStudentRepository, StudentRepository>();
        service.AddScoped<IActivityAnswerRepository, ActivityAnswerRepository>();
        service.AddScoped<IActivityAnswerVersionRepository, ActivityAnswerVersionRepository>();
        service.AddScoped<IActivityRepository, ActivityRepository>();
    }
}
