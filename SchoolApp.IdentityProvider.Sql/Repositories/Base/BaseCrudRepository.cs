using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Interfaces;

namespace SchoolApp.IdentityProvider.Sql.Repositories.Base;

public abstract class BaseCrudRepository<TDto, TDomain> : BaseRepository<TDto, TDomain>, ICrudRepository<TDomain> where TDto : class, IIdentityEntity
                                                                                                                  where TDomain : class
{
    protected BaseCrudRepository(SchoolAppContext context,
                                 Func<TDto, TDomain> mapToDomain,
                                 Func<TDomain, TDto> mapToDto) : base(context, mapToDomain, mapToDto)
    {
    }

    public virtual async Task<TDomain> InsertAsync(TDomain item)
    {
        var dto = MapToDto(item);
        await _dbSet.AddAsync(dto);
        await _context.SaveChangesAsync();
        return MapToDomain(dto);
    }

    public virtual async Task<TDomain> UpdateAsync(TDomain item)
    {
        var dto = MapToDto(item);
        _dbSet.Update(dto);
        await _context.SaveChangesAsync();
        _context.Entry(dto).State = EntityState.Detached;
        return MapToDomain(dto);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var dto = _dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (dto != null)
        {
            _dbSet.Remove(dto);
            await _context.SaveChangesAsync();
            _context.Entry(dto).State = EntityState.Detached;
        }
    }

    public virtual TDomain GetOneById(int id)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id));
    }
}
