using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Shared.Utils.Sql.Base;

public abstract class BaseRepository<TDto, TDomain, TContext> where TDto : class where TDomain : class where TContext : DbContext
{
    protected Func<TDto, TDomain> MapToDomain { get; set; }
    protected Func<TDomain, TDto> MapToDto { get; set; }

    protected readonly TContext _context;
    protected readonly DbSet<TDto> _dbSet;
    public BaseRepository(TContext context,
                                Func<TDto, TDomain> mapToDomain,
                                Func<TDomain, TDto> mapToDto)
    {
        _context = context;
        _dbSet = _context.Set<TDto>();
        MapToDomain = mapToDomain;
        MapToDto = mapToDto;
    }
}
