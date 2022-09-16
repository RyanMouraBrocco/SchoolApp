using SchoolApp.Shared.Authentication;

namespace SchoolApp.Shared.Utils.Interfaces;

public interface ICrudService<TEntity, TIdentity> where TEntity : class
{
    IList<TEntity> GetAll(AuthenticatedUserObject requesterUser, int top, int skip);
    Task<TEntity> CreateAsync(AuthenticatedUserObject requesterUser, TEntity newEntity);
    Task<TEntity> UpdateAsync(AuthenticatedUserObject requesterUser, TIdentity itemId, TEntity updatedEntity);
    Task DeleteAsync(AuthenticatedUserObject requesterUser, TIdentity itemId);
}
