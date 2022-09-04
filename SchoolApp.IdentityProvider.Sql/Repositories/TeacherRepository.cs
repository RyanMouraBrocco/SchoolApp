using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.IdentityProvider.Sql.Repositories.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class TeacherRepository : BaseMainEntityRepository<TeacherDto, Teacher>, ITeacherRepository
{
    public TeacherRepository(SchoolAppContext context) : base(context, TeacherMapper.MapToDomain, TeacherMapper.MapToDto)
    {
    }

    public Teacher GetOneByEmail(string email)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Email.Equals(email) && !x.Deleted));
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
