using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Services;

public interface IStudentService : ICrudService<Student, int>
{
    IList<Student> GetAllByOwnerId(int ownerId, int top, int skip);
}
