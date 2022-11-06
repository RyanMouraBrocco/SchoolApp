using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.File.Application.Services;

public class ActivityAnswerVersionFileService : FileService<ActivityAnswerVersionFile>, IActivityAnswerVersionFileService
{
    private readonly IActivityAnswerVersionRepository _activityAnswerVersionRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IClassroomRepository _classroomRepository;

    public ActivityAnswerVersionFileService(IFileRepository<ActivityAnswerVersionFile> fileRepository,
                               IActivityAnswerVersionRepository activityAnswerVersionRepository,
                               IActivityRepository activityRepository,
                               IClassroomRepository classroomRepository) : base(fileRepository)
    {
        _activityAnswerVersionRepository = activityAnswerVersionRepository;
        _activityRepository = activityRepository;
        _classroomRepository = classroomRepository;
    }


    private async Task CheckActivityAnswerVersion(AuthenticatedUserObject requesterUser, string activityAnswerVersionId)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);

        var activityAnswerVersionCheck = await _activityAnswerVersionRepository.GetOneByIdAsync(activityAnswerVersionId);
        if (activityAnswerVersionCheck == null)
            throw new UnauthorizedAccessException("ActivityAnswerVersion not found");

        var activityCheck = await _activityRepository.GetOneByIdAsync(activityAnswerVersionCheck.ActivityId);
        if (activityCheck == null || activityCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("ActivityAnswerVersion not found");

        var allClassrooms = await _classroomRepository.GetAllByOwnerIdAsync(requesterUser.UserId);
        if (!allClassrooms.Select(x => x.Id).Contains(activityCheck.ClassroomId))
            throw new UnauthorizedAccessException("ActivityAnswerVersion not found");
    }

    public async Task AddAsync(AuthenticatedUserObject requesterUser, ActivityAnswerVersionFile file)
    {
        await CheckActivityAnswerVersion(requesterUser, file.ActivityAnswerVersionId);
        await AddAsync(GetFolderFullPath(file.ActivityAnswerVersionId), file);
    }

    public async Task<IList<ActivityAnswerVersionFile>> GetAllByActivityAnswerVersionIdAsync(AuthenticatedUserObject requesterUser, string activityAnswerVersionId)
    {
        await CheckActivityAnswerVersion(requesterUser, activityAnswerVersionId);
        return GetAllInPath(GetFolderFullPath(activityAnswerVersionId));
    }
    public async Task RemoveAsync(AuthenticatedUserObject requesterUser, ActivityAnswerVersionFile file)
    {
        await CheckActivityAnswerVersion(requesterUser, file.ActivityAnswerVersionId);
        await RemoveAsync(GetFolderFullPath(file.ActivityAnswerVersionId), file);
    }

    private string GetFolderFullPath(string activityAnswerVersionId)
    {
        return $"activityanswerversions/{activityAnswerVersionId}/";
    }
}
