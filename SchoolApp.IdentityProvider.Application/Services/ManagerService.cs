using System.Runtime.ConstrainedExecution;
using System.Reflection.PortableExecutable;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Domain.Enums;
using SchoolApp.IdentityProvider.Application.Validations;
using SchoolApp.IdentityProvider.Application.Helpers;

namespace SchoolApp.IdentityProvider.Application.Services;

public class ManagerService : IManagerService
{
    private readonly IManagerRepository _managerRepository;
    private readonly IFunctionRepository _functionRepository;

    public ManagerService(IManagerRepository managerRepository,
                          IFunctionRepository functionRepository)
    {
        _managerRepository = managerRepository;
        _functionRepository = functionRepository;
    }

    public IList<Manager> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager => _managerRepository.GetAll(requesterUser.AccountId, top, skip),
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    public async Task<Manager> CreateAsync(AuthenticatedUserObject requesterUser, Manager newManager)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);
        UserValidation.IsSecurityPassword(newManager.Password);

        var duplicatedEmail = _managerRepository.GetOneByEmail(newManager.Email);
        if (duplicatedEmail != null)
            throw new UnauthorizedAccessException("This email has already used");

        var functionCheck = _functionRepository.GetOneById(newManager.FunctionId);
        if (functionCheck == null || functionCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Function not found");

        newManager.Password = Utils.HashText(newManager.Password);
        newManager.AccountId = requesterUser.AccountId;
        newManager.CreationDate = DateTime.Now;
        newManager.CreatorId = requesterUser.UserId;
        newManager.UpdaterId = null;
        newManager.UpdateDate = null;

        return await _managerRepository.InsertAsync(newManager);
    }

    public async Task<Manager> UpdateAsync(AuthenticatedUserObject requesterUser, int ownerId, Manager updatedManager)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);

        var managerCheck = _managerRepository.GetOneById(ownerId);
        if (managerCheck == null || managerCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Owner not found");

        var duplicatedEmail = _managerRepository.GetOneByEmail((string)updatedManager.Email);
        if (duplicatedEmail != null && duplicatedEmail.Id != ownerId)
            throw new UnauthorizedAccessException("This email has already used");

        var functionCheck = _functionRepository.GetOneById(updatedManager.FunctionId);
        if (functionCheck == null || functionCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Function not found");

        updatedManager.Id = ownerId;
        updatedManager.Password = managerCheck.Password;
        updatedManager.AccountId = requesterUser.AccountId;
        updatedManager.CreationDate = managerCheck.CreationDate;
        updatedManager.CreatorId = managerCheck.CreatorId;
        updatedManager.UpdaterId = requesterUser.UserId;
        updatedManager.UpdateDate = DateTime.Now;

        return await _managerRepository.UpdateAsync(updatedManager);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int managerId)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);

        var managerCheck = _managerRepository.GetOneById(managerId);
        if (managerCheck == null || managerCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Owner not found");

        managerCheck.Id = managerId;
        managerCheck.UpdaterId = requesterUser.UserId;
        managerCheck.UpdateDate = DateTime.Now;

        await _managerRepository.UpdateAsync(managerCheck);
        await _managerRepository.DeleteAsync(managerId);
    }
}
