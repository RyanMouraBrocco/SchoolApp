using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Classroom.Sql.Context;

namespace SchoolApp.Classroom.Ioc.Database;

public static class DatabaseInjection
{
    public static void AddClassroomDatabase(this IServiceCollection service)
    {
        service.AddDbContext<SchoolAppClassroomContext>(options => options.UseSqlServer("name=ConnectionStrings:Classroom"));
    }
}
