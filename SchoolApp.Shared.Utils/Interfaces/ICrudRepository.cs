namespace SchoolApp.Shared.Utils.Interfaces;

public interface ICrudRepository<TEntity, TIdentity> where TEntity : class
{
    Task<TEntity> InsertAsync(TEntity item);
    Task<TEntity> UpdateAsync(TEntity item);
    Task DeleteAsync(TIdentity id);
    TEntity GetOneById(TIdentity id);
}
