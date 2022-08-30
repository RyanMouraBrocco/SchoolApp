using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Interfaces;

namespace SchoolApp.IdentityProvider.Sql.Repositories.Base;

public abstract class BaseIdentityRepository<TDto, TDomain> where TDto : class, IIdentityEntity
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

    public TDomain GetOneById(int id)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id));
    }
}
