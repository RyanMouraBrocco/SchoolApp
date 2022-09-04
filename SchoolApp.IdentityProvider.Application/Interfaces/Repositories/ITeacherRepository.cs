using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface ITeacherRepository : IUserRepository<Teacher>
{
}
