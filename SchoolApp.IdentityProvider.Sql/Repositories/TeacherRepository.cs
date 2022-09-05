using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class TeacherRepository : UserRepository<TeacherDto, Teacher>, ITeacherRepository
{
    public TeacherRepository(SchoolAppContext context) : base(context, TeacherMapper.MapToDomain, TeacherMapper.MapToDto)
    {
    }

    public override Teacher GetOneById(int id)
    {
        return MapToDomain(_dbSet.AsNoTracking().Include(x => x.Formations).FirstOrDefault(x => x.Id == id && !x.Deleted));
    }

    public override IList<Teacher> GetAll(int accountId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
             .Include(x => x.Formations)
             .Where(x => x.AccountId == accountId && !x.Deleted)
             .Skip(skip)
             .Take(top)
             .Select(x => MapToDomain(x))
             .ToList();
    }
}
