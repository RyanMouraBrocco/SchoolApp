using SchoolApp.Classroom.Application.Domain.Dtos;

namespace SchoolApp.Classroom.Application.Interfaces.Repositories;

public interface IActivityAnswerRepository
{
    Task<ActivityAnswerDto> GetOneByIdIncludingActivityAsync(string activityAnswerId);
}
