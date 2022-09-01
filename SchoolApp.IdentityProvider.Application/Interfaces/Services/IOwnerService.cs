using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IOwnerService
{
    IList<Owner> GetAll(AuthenticatedUserObject requesterUser, int top, int skip);
    Task<Owner> CreateAsync(AuthenticatedUserObject requesterUser, Owner newOwner);
    Task<Owner> UpdateAsync(AuthenticatedUserObject requesterUser, int ownerId, Owner updatedOwner);
    Task DeleteAsync(AuthenticatedUserObject requesterUser, int ownerId);
}
