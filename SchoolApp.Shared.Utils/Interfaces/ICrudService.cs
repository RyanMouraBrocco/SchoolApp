using SchoolApp.Shared.Authentication;

namespace SchoolApp.Shared.Utils.Interfaces;

public interface ICrudService<TEntity> where TEntity : class
{
    IList<TEntity> GetAll(AuthenticatedUserObject requesterUser, int top, int skip);
    Task<TEntity> CreateAsync(AuthenticatedUserObject requesterUser, TEntity newEntity);
    Task<TEntity> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, TEntity updatedEntity);
    Task DeleteAsync(AuthenticatedUserObject requesterUser, int itemId);
}
