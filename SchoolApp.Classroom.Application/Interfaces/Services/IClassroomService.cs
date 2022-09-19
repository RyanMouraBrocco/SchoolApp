using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Services;

public interface IClassroomService : ICrudService<Domain.Entities.Classrooms.Classroom, int>
{
    Domain.Entities.Classrooms.Classroom GetOneById(int id);
}
