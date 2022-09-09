using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Application.Validations;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.IdentityProvider.Application.Services;

public class FunctionService : IFunctionService
{
    private readonly IFunctionRepository _functionRepository;
    public FunctionService(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public IList<Function> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return _functionRepository.GetAll(requesterUser.AccountId, top, skip);
    }

    public async Task<Function> CreateAsync(AuthenticatedUserObject requesterUser, Function newFunction)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        newFunction.AccountId = requesterUser.AccountId;
        newFunction.CreationDate = DateTime.Now;
        newFunction.CreatorId = requesterUser.UserId;
        newFunction.UpdaterId = null;
        newFunction.UpdateDate = null;

        return await _functionRepository.InsertAsync(newFunction);
    }

    public async Task<Function> UpdateAsync(AuthenticatedUserObject requesterUser, int functionId, Function updatedFunction)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var functionCheck = _functionRepository.GetOneById(functionId);
        if (functionCheck == null || functionCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Function not found");

        updatedFunction.Id = functionId;
        updatedFunction.AccountId = requesterUser.AccountId;
        updatedFunction.CreationDate = DateTime.Now;
        updatedFunction.CreatorId = requesterUser.UserId;
        updatedFunction.UpdaterId = null;
        updatedFunction.UpdateDate = null;

        return await _functionRepository.UpdateAsync(updatedFunction);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int functionId)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var functionCheck = _functionRepository.GetOneById(functionId);
        if (functionCheck == null || functionCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Owner not found");

        functionCheck.Id = functionId;
        functionCheck.UpdaterId = requesterUser.UserId;
        functionCheck.UpdateDate = DateTime.Now;

        await _functionRepository.UpdateAsync(functionCheck);
        await _functionRepository.DeleteAsync(functionId);
    }
}
