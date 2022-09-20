using SchoolApp.Activity.Application.Domain.Dtos;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<IList<StudentDto>> GetAllByOwnerIdAsync(int ownerId);
}
