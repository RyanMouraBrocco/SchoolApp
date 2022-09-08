using Microsoft.EntityFrameworkCore;
using SchoolApp.Shared.Utils.Interfaces;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.Shared.Utils.Sql.Base;

public class BaseCrudRepository<TDto, TDomain, TContext> : BaseRepository<TDto, TDomain, TContext>, ICrudRepository<TDomain> where TDto : class, IIdentityEntity
                                                                                                                             where TDomain : class
                                                                                                                             where TContext : DbContext
{
    protected BaseCrudRepository(TContext context,
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
