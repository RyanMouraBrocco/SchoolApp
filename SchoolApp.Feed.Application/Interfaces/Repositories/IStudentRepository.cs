using SchoolApp.Feed.Application.Domain.Dtos;

namespace SchoolApp.Feed.Application.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<StudentDto> GetOneByIdAsync(int id);
    Task<IList<StudentDto>> GetAllByTeacherIdAsync(int teacherId);
    Task<IList<StudentDto>> GetAllByOwnerIdAsync(int ownerId);
}
