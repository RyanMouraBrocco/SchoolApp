using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface ITeacherFormationRepository
{
    Task<TeacherFormation> InsertAsync(TeacherFormation item);
    Task DeleteAllByTeacherIdAsync(int teacherId);
}
