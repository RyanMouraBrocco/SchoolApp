using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.IdentityProvider.Sql.Repositories.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class TeacherRepository : BaseIdentityRepository<TeacherDto, Teacher>, ITeacherRepository
{
    public TeacherRepository(SchoolAppContext context) : base(context, TeacherMapper.MapToDomain, TeacherMapper.MapToDto)
    {
    }

    public Teacher GetOneByEmail(string email)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Email.Equals(email) && !x.Deleted));
    }
}
