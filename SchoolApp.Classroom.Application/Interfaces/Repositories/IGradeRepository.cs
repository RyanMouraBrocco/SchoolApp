using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IGradeRepository<TEntity> : ICrudRepository<TEntity, int> where TEntity : Grade
{
}
