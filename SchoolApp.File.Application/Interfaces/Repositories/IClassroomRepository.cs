using SchoolApp.File.Application.Domain.Dtos;

namespace SchoolApp.File.Application.Interfaces.Repositories;

public interface IClassroomRepository
{
    Task<ClassroomDto> GetOneByIdAsync(int id);
}
