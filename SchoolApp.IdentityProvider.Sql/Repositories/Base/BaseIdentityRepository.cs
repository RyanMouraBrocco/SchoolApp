using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Interfaces;

namespace SchoolApp.IdentityProvider.Sql.Repositories.Base;

public abstract class BaseIdentityRepository<TDto, TDomain> where TDto : class, IIdentityEntity, ISoftDeleteEntity, IAccountEntity
                                                   where TDomain : class
{
    protected Func<TDto, TDomain> MapToDomain { get; set; }
    protected Func<TDomain, TDto> MapToDto { get; set; }

    private readonly SchoolAppContext _context;
    protected readonly DbSet<TDto> _dbSet;
    public BaseIdentityRepository(SchoolAppContext context,
                                  Func<TDto, TDomain> mapToDomain,
                                  Func<TDomain, TDto> mapToDto)
    {
        _context = context;
        _dbSet = _context.Set<TDto>();
        MapToDomain = mapToDomain;
        MapToDto = mapToDto;
    }

    public async Task<TDomain> InsertAsync(TDomain item)
    {
        var dto = MapToDto(item);
        dto.Deleted = false;
        await _dbSet.AddAsync(dto);
        await _context.SaveChangesAsync();
        return MapToDomain(dto);
    }

    public async Task<TDomain> UpdateAsync(TDomain item)
    {
        var dto = MapToDto(item);
        _dbSet.Update(dto);
        await _context.SaveChangesAsync();
        _context.Entry(dto).State = EntityState.Detached;
        return MapToDomain(dto);
    }

    public async Task DeleteAsync(int id)
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

    public TDomain GetOneById(int id)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.Deleted));
    }

    public IList<TDomain> GetAll(int accountId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Where(x => x.AccountId == accountId && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }
}
