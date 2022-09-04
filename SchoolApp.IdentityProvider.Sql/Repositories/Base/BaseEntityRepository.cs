using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Sql.Context;

namespace SchoolApp.IdentityProvider.Sql.Repositories.Base;

public abstract class BaseEntityRepository<TDto, TDomain> where TDto : class
                                                          where TDomain : class
{
    protected Func<TDto, TDomain> MapToDomain { get; set; }
    protected Func<TDomain, TDto> MapToDto { get; set; }

    protected readonly SchoolAppContext _context;
    protected readonly DbSet<TDto> _dbSet;
    public BaseEntityRepository(SchoolAppContext context,
                                Func<TDto, TDomain> mapToDomain,
                                Func<TDomain, TDto> mapToDto)
    {
        _context = context;
        _dbSet = _context.Set<TDto>();
        MapToDomain = mapToDomain;
        MapToDto = mapToDto;
    }
}
