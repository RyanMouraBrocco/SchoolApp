using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;

namespace SchoolApp.IdentityProvider.Sql.Context;

public class SchoolAppIdentityProviderContext : DbContext
{
    public DbSet<TeacherDto> Teacher { get; set; }
    public DbSet<OwnerDto> Owner { get; set; }
    public DbSet<ManagerDto> Manager { get; set; }

    public SchoolAppIdentityProviderContext(DbContextOptions<SchoolAppIdentityProviderContext> options) : base(options)
    {

    }
}
