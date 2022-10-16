using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Sql.Dtos.Formation;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;
using SchoolApp.IdentityProvider.Sql.Dtos.MessageAllowedPermissions;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.Shared.Utils.Sql.Contexts;

namespace SchoolApp.IdentityProvider.Sql.Context;

public class SchoolAppIdentityProviderContext : SchoolAppContext
{
    public DbSet<TeacherDto> Teacher { get; set; }
    public DbSet<OwnerDto> Owner { get; set; }
    public DbSet<ManagerDto> Manager { get; set; }
    public DbSet<TeacherFormationDto> TeacherFormation { get; set; }
    public virtual DbSet<FunctionDto> Function { get; set; }
    public DbSet<MessageAllowedClassroomDto> MessageAllowedClassroom { get; set; }
    public DbSet<MessageAllowedStudentDto> MessageAllowedStudent { get; set; }

    public SchoolAppIdentityProviderContext(DbContextOptions<SchoolAppIdentityProviderContext> options) : base(options)
    {

    }
}
