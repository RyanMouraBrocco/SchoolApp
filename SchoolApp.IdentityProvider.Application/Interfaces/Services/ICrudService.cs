using SchoolApp.IdentityProvider.Application.Domain.Authentication;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface ICrudService<TEntity> where TEntity : class
{
    IList<TEntity> GetAll(AuthenticatedUserObject requesterUser, int top, int skip);
    Task<TEntity> CreateAsync(AuthenticatedUserObject requesterUser, TEntity newEntity);
    Task<TEntity> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, TEntity updatedEntity);
    Task DeleteAsync(AuthenticatedUserObject requesterUser, int itemId);
}
