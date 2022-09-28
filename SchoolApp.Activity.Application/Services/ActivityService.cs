using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Interfaces;
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
        newActivity.CloseDate = newActivity.Closed ? DateTime.Now : null;
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
        if (requesterUser.Type == UserTypeEnum.Manager)
        {
            return _activityRepository.GetAll(requesterUser.AccountId, top, skip);
        }
        else if (requesterUser.Type == UserTypeEnum.Owner)
        {
            var classrooms = _classroomRepository.GetAllByOwnerIdAsync(requesterUser.UserId).Result;
            return _activityRepository.GetAllByClassroomsIds(classrooms.Select(x => x.Id));
        }
        else if (requesterUser.Type == UserTypeEnum.Teacher)
        {
            var classrooms = _classroomRepository.GetAllByTeacherIdAsync(requesterUser.UserId).Result;
            return _activityRepository.GetAllByClassroomsIds(classrooms.Select(x => x.Id));
        }
        else
            throw new NotImplementedException("User type not valid");
    }

    public async Task<Domain.Entities.Activities.Activity> GetOneByIdAsync(AuthenticatedUserObject requesterUser, string id)
    {
        var activity = _activityRepository.GetOneById(id);
        if (activity == null || activity.AccountId != requesterUser.AccountId)
            return null;

        if (requesterUser.Type == UserTypeEnum.Manager)
        {
            return activity;
        }
        else if (requesterUser.Type == UserTypeEnum.Owner)
        {
            var classrooms = await _classroomRepository.GetAllByOwnerIdAsync(requesterUser.UserId);
            if (!classrooms.Select(x => x.Id).Contains(activity.ClassroomId))
                return null;

            return activity;
        }
        else if (requesterUser.Type == UserTypeEnum.Teacher)
        {
            var classroom = await _classroomRepository.GetOneByIdAsync(activity.ClassroomId);
            if (classroom.TeacherId != requesterUser.UserId)
                return null;

            return activity;
        }
        else
            throw new NotImplementedException("User type not valid");
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
        updatedAcitvity.Closed = activityCheck.Closed;
        updatedAcitvity.CloseDate = activityCheck.CloseDate;
        updatedAcitvity.UpdateDate = DateTime.Now;
        updatedAcitvity.UpdaterId = requesterUser.UserId;

        return await _activityRepository.UpdateAsync(updatedAcitvity);
    }

    public async Task<Domain.Entities.Activities.Activity> CloseAsync(AuthenticatedUserObject requesterUser, string activityId)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var activityCheck = await GetOneByIdAsync(requesterUser, activityId);
        if (activityCheck == null)
            throw new UnauthorizedAccessException("Activity not found");

        if (activityCheck.Closed)
            throw new UnauthorizedAccessException("This activity is already closed");

        activityCheck.Closed = true;
        activityCheck.CloseDate = DateTime.Now;

        return await _activityRepository.UpdateAsync(activityCheck);
    }

    public async Task<Domain.Entities.Activities.Activity> OpenAsync(AuthenticatedUserObject requesterUser, string activityId)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var activityCheck = await GetOneByIdAsync(requesterUser, activityId);
        if (activityCheck == null)
            throw new UnauthorizedAccessException("Activity not found");

        if (!activityCheck.Closed)
            throw new UnauthorizedAccessException("This activity is already opened");

        activityCheck.Closed = false;
        activityCheck.CloseDate = null;

        return await _activityRepository.UpdateAsync(activityCheck);
    }

    public Domain.Entities.Activities.Activity GetOneById(string id)
    {
        return _activityRepository.GetOneById(id);
    }
}
