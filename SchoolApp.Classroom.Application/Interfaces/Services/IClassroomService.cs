using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Services;

public interface IClassroomService : ICrudService<Domain.Entities.Classrooms.Classroom, int>
{
    Domain.Entities.Classrooms.Classroom GetOneById(int id);
    Domain.Entities.Classrooms.Classroom GetOneById(AuthenticatedUserObject requesterUser, int id);
    IList<Domain.Entities.Classrooms.Classroom> GetAllByOwnerId(int ownerId, int top, int skip);
    IList<Domain.Entities.Classrooms.Classroom> GetAllByTeacherId(int teacherId, int top, int skip);
}
