using SchoolApp.File.Application.Domain.Dtos;

namespace SchoolApp.File.Application.Interfaces.Repositories;

public interface IActivityRepository
{
    Task<ActivityDto> GetOneByIdAsync(string activityId);
}
