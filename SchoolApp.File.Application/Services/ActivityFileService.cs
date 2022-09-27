using System.Diagnostics.Contracts;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.File.Application.Services;

public class ActivityFileService : FileService<ActivityFile>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IClassroomRepository _classroomRepository;

    public ActivityFileService(IFileRepository<ActivityFile> fileRepository,
                               IActivityRepository activityRepository,
                               IClassroomRepository classroomRepository) : base(fileRepository)
    {
        _activityRepository = activityRepository;
        _classroomRepository = classroomRepository;
    }


    private async Task CheckActivity(AuthenticatedUserObject requesterUser, string activityId)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var activityCheck = await _activityRepository.GetOneByIdAsync(activityId);
        if (activityCheck == null || activityCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Activity not found");

        var classroomCheck = await _classroomRepository.GetOneByIdAsync(activityCheck.ClassroomId);
        if (classroomCheck == null || classroomCheck.AccountId != requesterUser.AccountId || (requesterUser.Type == UserTypeEnum.Teacher && classroomCheck.TeacherId != requesterUser.UserId))
            throw new UnauthorizedAccessException("Activity not found");
    }

    public async Task Add(AuthenticatedUserObject requesterUser, ActivityFile file)
    {
        await CheckActivity(requesterUser, file.ActivityId);
        Add(GetFolderFullPath(file.ActivityId), file);
    }

    public async Task<IList<ActivityFile>> GetAllByActivityIdAsync(AuthenticatedUserObject requesterUser, string activityId)
    {
        await CheckActivity(requesterUser, activityId);
        return GetAllInPath(GetFolderFullPath(activityId));
    }
    public async Task RemoveAsync(AuthenticatedUserObject requesterUser, string folderPath, ActivityFile file)
    {
        await CheckActivity(requesterUser, file.ActivityId);
        Remove(GetFolderFullPath(file.ActivityId), file);
    }

    private string GetFolderFullPath(string activityId)
    {
        return $"Activity/{activityId}/";
    }
}
