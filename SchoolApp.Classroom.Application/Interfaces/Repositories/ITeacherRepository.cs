using SchoolApp.Classroom.Application.Domain.Dtos;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface ITeacherRepository
{
    Task<TeacherDto> GetOneByIdAsync(int id);
}
