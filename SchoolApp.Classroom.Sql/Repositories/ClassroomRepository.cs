using Microsoft.EntityFrameworkCore;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Mappers.Classrooms;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class ClassroomRepository : BaseMainEntityRepository<ClassroomDto, Application.Domain.Entities.Classrooms.Classroom, SchoolAppClassroomContext>, IClassroomRepository
{
    public ClassroomRepository(SchoolAppClassroomContext context) : base(context, ClassroomMapper.MapToDomain, ClassroomMapper.MapToDto)
    {
    }

    public override IList<Application.Domain.Entities.Classrooms.Classroom> GetAll(int accountId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Include(x => x.Students)
                     .Where(x => x.AccountId == accountId && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }

    public IList<Application.Domain.Entities.Classrooms.Classroom> GetAllByOwnerId(int ownerId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Include(x => x.Students)
                     .Where(x => x.Students.Any(x => x.Student.Owners.Any(x => x.OwnerId == ownerId)) && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }

    public IList<Application.Domain.Entities.Classrooms.Classroom> GetAllByTeacherId(int teacherId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Include(x => x.Students)
                     .Where(x => x.TeacherId == teacherId && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }
}
