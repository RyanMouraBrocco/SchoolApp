using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface ITeacherService : IUserService<Teacher>
{
    Teacher GetOneById(int id);
}
