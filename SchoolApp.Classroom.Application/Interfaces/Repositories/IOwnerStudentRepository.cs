using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IOwnerStudentRepository : ICrudRepository<OwnerStudent>
{
}
