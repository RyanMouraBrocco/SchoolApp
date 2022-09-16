using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IStudentRepository : ICrudRepository<Student, int>
{
    IList<Student> GetAll(int accountId, int top, int skip);
    IList<Student> GetAllByOwnerId(int ownerId, int top, int skip);
    IList<Student> GetAllByTeacherId(int teacherId, int top, int skip);
}
