using SchoolApp.Activity.Application.Domain.Dtos;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IClassroomRepository
{
    Task<ClassroomDto> GetOneByIdAsync(int id);
    Task<IList<ClassroomDto>> GetAllByTeacherIdAsync(int teacherId);
    Task<IList<ClassroomDto>> GetAllByOwnerIdAsync(int ownerId);
}
