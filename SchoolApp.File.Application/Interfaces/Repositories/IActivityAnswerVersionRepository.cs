using SchoolApp.File.Application.Domain.Dtos;

namespace SchoolApp.File.Application.Interfaces.Repositories;

public interface IActivityAnswerVersionRepository
{
    Task<ActivityAnswerVersionDto> GetOneByIdAsync(string activityAnswerVersionId);
}
