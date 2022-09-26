using SchoolApp.File.Application.Domain.Dtos;

namespace SchoolApp.File.Application.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<IList<StudentDto>> GetAllByOwnerIdAsync(int ownerId);
}
