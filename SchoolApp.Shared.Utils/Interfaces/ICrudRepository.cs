namespace SchoolApp.Shared.Utils.Interfaces;

public interface ICrudRepository<TEntity> where TEntity : class
{
    Task<TEntity> InsertAsync(TEntity item);
    Task<TEntity> UpdateAsync(TEntity item);
    Task DeleteAsync(int id);
    TEntity GetOneById(int id);
}
