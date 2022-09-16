using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IClassroomStudentRepository : ICrudRepository<ClassroomStudent, int>
{
    Task DeleteAllByClassroomIdAsync(int classroomId);
}
