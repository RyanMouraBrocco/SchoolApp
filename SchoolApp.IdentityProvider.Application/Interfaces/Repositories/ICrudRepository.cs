namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface ICrudRepository<TEntity> where TEntity : class
{
    Task<TEntity> InsertAsync(TEntity item);
    Task<TEntity> UpdateAsync(TEntity item);
    Task DeleteAsync(int id);
    TEntity GetOneById(int id);
}
