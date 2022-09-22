using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Classroom.Application.Services;

namespace SchoolApp.Classroom.Ioc.Services;

public static class ServicesInjection
{
    public static void AddClassroomServices(this IServiceCollection service)
    {
        service.AddScoped<IStudentService, StudentService>();
        service.AddScoped<IClassroomService, ClassroomService>();
        service.AddScoped<ISubjectService, SubjectService>();
        service.AddScoped<IActivityAnswerGradeService, ActivityAnswerGradeService>();
        service.AddScoped<IClassroomStudentGradeService, ClassroomStudentGradeService>();
    }
}
