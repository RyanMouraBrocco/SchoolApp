using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IClassroomRepository : ICrudRepository<Domain.Entities.Classrooms.Classroom>
{
}
