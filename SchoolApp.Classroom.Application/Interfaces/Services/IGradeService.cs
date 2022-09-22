using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Classroom.Application.Interfaces.Services;

public interface IGradeService<TEntity> : ICrudService<TEntity, int> where TEntity : Grade
{
}
