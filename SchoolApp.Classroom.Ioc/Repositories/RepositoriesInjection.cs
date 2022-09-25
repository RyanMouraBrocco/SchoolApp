using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Http.Repositories;
using SchoolApp.Classroom.Sql.Repositories;

namespace SchoolApp.Classroom.Ioc.Repositories;

public static class RepositoriesInjection
{
    public static void AddClassroomRepositories(this IServiceCollection service)
    {
        service.AddScoped<IClassroomRepository, ClassroomRepository>();
        service.AddScoped<IClassroomStudentRepository, ClassroomStudentRepository>();
        service.AddScoped<IOwnerStudentRepository, OwnerStudentRepository>();
        service.AddScoped<IOwnerTypeRepository, OwnerTypeRepository>();
        service.AddScoped<IStudentRepository, StudentRepository>();
        service.AddScoped<ISubjectRepository, SubjectRepository>();
        service.AddHttpClient<ITeacherRepository, TeacherRepository>();
        service.AddHttpClient<IActivityAnswerRepository, AcitivityAnswerRepository>();
        service.AddScoped<IActivityAnswerGradeRepository, ActivityAnswerGradeRepository>();
        service.AddScoped<IClassroomStudentGradeRepository, ClassroomStudentGradeRepository>();
    }
}
