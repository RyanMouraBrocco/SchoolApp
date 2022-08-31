using SchoolApp.IdentityProvider.Application.Domain.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface ITeacherRepository : IUserRepository<Teacher>
{
}
