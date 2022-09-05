namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface ICrudRepository<TEntity>
{
    Task<TEntity> InsertAsync(TEntity item);
    Task<TEntity> UpdateAsync(TEntity item);
    Task DeleteAsync(int id);
    TEntity GetOneById(int id);
}
