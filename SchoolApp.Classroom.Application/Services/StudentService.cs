using System.Reflection.PortableExecutable;
using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Classroom.Application.Services;

public class StudentService : IStudentService
{
    public StudentService()
    {

    }

    public Task<Student> CreateAsync(AuthenticatedUserObject requesterUser, Student newEntity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(AuthenticatedUserObject requesterUser, int itemId)
    {
        throw new NotImplementedException();
    }

    public IList<Student> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Owner => new List<Student>(),
            
            _ => new List<Student>()
        };
    }

    public Task<Student> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, Student updatedEntity)
    {
        throw new NotImplementedException();
    }
}
