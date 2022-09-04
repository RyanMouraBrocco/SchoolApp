using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Formation;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.IdentityProvider.Sql.Repositories.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class TeacherFormationRepository : BaseSimpleEntityRepository<TeacherFormationDto, TeacherFormation>, ITeacherFormationRepository
{
    public TeacherFormationRepository(SchoolAppContext context) : base(context, TeacherFormationMapper.MapToDomain, TeacherFormationMapper.MapToDto)
    {
    }

    public async Task DeleteAllByTeacherIdAsync(int teacherId)
    {
        var allFormations = _dbSet.AsNoTracking().Where(x => x.TeacherId == teacherId).ToList();
        _dbSet.RemoveRange(allFormations);
        await _context.SaveChangesAsync();
    }
}
