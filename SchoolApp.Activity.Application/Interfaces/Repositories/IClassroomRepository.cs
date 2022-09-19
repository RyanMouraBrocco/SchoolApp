using SchoolApp.Activity.Application.Domain.Dtos;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IClassroomRepository
{
    Task<ClassroomDto> GetOneByIdAsync(int id);
}
