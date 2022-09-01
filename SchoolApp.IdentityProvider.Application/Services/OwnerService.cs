using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Domain.Enums;
using SchoolApp.IdentityProvider.Application.Domain.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Application.Validations;

namespace SchoolApp.IdentityProvider.Application.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public IList<Owner> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager or UserTypeEnum.Teacher => _ownerRepository.GetAll(requesterUser.AccountId, top, skip),
            UserTypeEnum.Owner => new List<Owner>() { _ownerRepository.GetOneById(requesterUser.UserId) },
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    public async Task<Owner> CreateAsync(AuthenticatedUserObject requesterUser, Owner newOwner)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);

        var duplicatedEmail = _ownerRepository.GetOneByEmail(newOwner.Email);
        if (duplicatedEmail != null)
            throw new UnauthorizedAccessException("This email has already used");

        newOwner.AccountId = requesterUser.AccountId;
        newOwner.CreationDate = DateTime.Now;
        newOwner.CreatorId = requesterUser.UserId;
        newOwner.UpdaterId = null;
        newOwner.UpdateDate = null;

        return await _ownerRepository.InsertAsync(newOwner);
    }

    public async Task<Owner> UpdateAsync(AuthenticatedUserObject requesterUser, int ownerId, Owner updatedOwner)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);

        var ownerCheck = _ownerRepository.GetOneById(ownerId);
        if (ownerCheck == null || ownerCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Owner not found");

        var duplicatedEmail = _ownerRepository.GetOneByEmail((string)updatedOwner.Email);
        if (duplicatedEmail != null && duplicatedEmail.Id != ownerId)
            throw new UnauthorizedAccessException("This email has already used");

        updatedOwner.Id = ownerId;
        updatedOwner.AccountId = requesterUser.AccountId;
        updatedOwner.CreationDate = ownerCheck.CreationDate;
        updatedOwner.CreatorId = ownerCheck.CreatorId;
        updatedOwner.UpdaterId = requesterUser.UserId;
        updatedOwner.UpdateDate = DateTime.Now;

        return await _ownerRepository.UpdateAsync(updatedOwner);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int ownerId)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);

        var ownerCheck = _ownerRepository.GetOneById(ownerId);
        if (ownerCheck == null || ownerCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Owner not found");

        ownerCheck.Id = ownerId;
        ownerCheck.UpdaterId = requesterUser.UserId;
        ownerCheck.UpdateDate = DateTime.Now;

        await _ownerRepository.UpdateAsync(ownerCheck);
        await _ownerRepository.DeleteAsync(ownerId);
    }
}
