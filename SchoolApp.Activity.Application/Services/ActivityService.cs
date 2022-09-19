using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Activity.Application.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IClassroomRepository _classroomRepository;
    public ActivityService(IActivityRepository activityRepository, IClassroomRepository classroomRepository)
    {
        _activityRepository = activityRepository;
        _classroomRepository = classroomRepository;
    }

    public async Task<Domain.Entities.Activities.Activity> CreateAsync(AuthenticatedUserObject requesterUser, Domain.Entities.Activities.Activity newActivity)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        if (string.IsNullOrEmpty(newActivity.Name?.Trim()))
            throw new FormatException("Name must not be null or empty");

        if (string.IsNullOrEmpty(newActivity.Description?.Trim()))
            throw new FormatException("Description must not be null or empty");

        var classroomCheck = await _classroomRepository.GetOneByIdAsync(newActivity.ClassroomId);
        if (classroomCheck == null || classroomCheck.TeacherId != requesterUser.UserId)
            throw new UnauthorizedAccessException("Classroom not found");

        newActivity.AccountId = requesterUser.AccountId;
        newActivity.CreatorId = requesterUser.UserId;
        newActivity.CreationDate = DateTime.Now;
        newActivity.UpdateDate = null;
        newActivity.UpdaterId = null;

        return await _activityRepository.InsertAsync(newActivity);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, string activityId)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var activityCheck = _activityRepository.GetOneById(activityId);
        if (activityCheck == null || activityCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Acitvity not found");

        activityCheck.UpdateDate = DateTime.Now;
        activityCheck.UpdaterId = requesterUser.UserId;

        await _activityRepository.UpdateAsync(activityCheck);
        await _activityRepository.DeleteAsync(activityId);
    }

    public IList<Domain.Entities.Activities.Activity> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager => new List<Domain.Entities.Activities.Activity>(),
            _ => throw new NotImplementedException()
        };
    }

    public Domain.Entities.Activities.Activity GetOneById(AuthenticatedUserObject requesterUser, string id)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager => new Domain.Entities.Activities.Activity(),
            UserTypeEnum.Owner => new Domain.Entities.Activities.Activity(),
            UserTypeEnum.Teacher => new Domain.Entities.Activities.Activity(),
            _ => throw new NotImplementedException()
        };
    }

    public async Task<Domain.Entities.Activities.Activity> UpdateAsync(AuthenticatedUserObject requesterUser, string activityId, Domain.Entities.Activities.Activity updatedAcitvity)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var activityCheck = _activityRepository.GetOneById(activityId);
        if (activityCheck == null || activityCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Acitvity not found");

        if (string.IsNullOrEmpty(updatedAcitvity.Name?.Trim()))
            throw new FormatException("Name must not be null or empty");

        if (string.IsNullOrEmpty(updatedAcitvity.Description?.Trim()))
            throw new FormatException("Description must not be null or empty");

        var classroomCheck = await _classroomRepository.GetOneByIdAsync(updatedAcitvity.ClassroomId);
        if (classroomCheck == null || classroomCheck.TeacherId != requesterUser.UserId)
            throw new UnauthorizedAccessException("Classroom not found");

        updatedAcitvity.AccountId = activityCheck.AccountId;
        updatedAcitvity.CreatorId = activityCheck.CreatorId;
        updatedAcitvity.CreationDate = activityCheck.CreationDate;
        updatedAcitvity.UpdateDate = DateTime.Now;
        updatedAcitvity.UpdaterId = requesterUser.UserId;

        return await _activityRepository.UpdateAsync(updatedAcitvity);
    }
}
