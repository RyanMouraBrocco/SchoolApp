using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IClassroomRepository : ICrudRepository<Domain.Entities.Classrooms.Classroom>
{
    IList<Domain.Entities.Classrooms.Classroom> GetAll(int accountId, int top, int skip);
    public IList<Application.Domain.Entities.Classrooms.Classroom> GetAllByOwnerId(int ownerId, int top, int skip);
    public IList<Application.Domain.Entities.Classrooms.Classroom> GetAllByTeacherId(int teacherId, int top, int skip);
}
