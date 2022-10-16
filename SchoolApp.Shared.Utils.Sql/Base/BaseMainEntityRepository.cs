using Microsoft.EntityFrameworkCore;
using SchoolApp.Shared.Utils.Sql.Contexts;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.Shared.Utils.Sql.Base;

public class BaseMainEntityRepository<TDto, TDomain, TContext> : BaseCrudRepository<TDto, TDomain, TContext> where TDto : class, IIdentityEntity, IAccountEntity, ISoftDeleteEntity
                                                                                                             where TDomain : class
                                                                                                             where TContext : SchoolAppContext
{
    public BaseMainEntityRepository(TContext context,
                                    Func<TDto, TDomain> mapToDomain,
                                    Func<TDomain, TDto> mapToDto) : base(context, mapToDomain, mapToDto)
    {
    }

    public override async Task<TDomain> InsertAsync(TDomain item)
    {
        var dto = MapToDto(item);
        dto.Deleted = false;
        await _dbSet.AddAsync(dto);
        await _context.SaveChangesAsync();
        return MapToDomain(dto);
    }

    public override async Task DeleteAsync(int id)
    {
        var dto = _dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (dto != null)
        {
            dto.Deleted = true;
            _dbSet.Attach(dto);
            _context.Entry(dto).Property(x => x.Deleted).IsModified = true;
            await _context.SaveChangesAsync();
            _context.Entry(dto).State = EntityState.Detached;
        }
    }

    public override TDomain GetOneById(int id)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.Deleted));
    }

    public virtual IList<TDomain> GetAll(int accountId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Where(x => x.AccountId == accountId && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }
}
