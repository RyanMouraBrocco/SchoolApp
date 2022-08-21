using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;

namespace SchoolApp.IdentityProvider.Sql.Context;

public class SchoolAppContext : DbContext
{
    public DbSet<UserDto> User { get; set; }

    public SchoolAppContext()
    {

    }
}
