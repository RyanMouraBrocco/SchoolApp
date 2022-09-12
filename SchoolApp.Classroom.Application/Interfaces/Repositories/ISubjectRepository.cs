using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface ISubjectRepository : ICrudRepository<Subject>
{
    IList<Subject> GetAll(int accountId, int top, int skip);
}
