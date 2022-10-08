using SchoolApp.Chat.Application.Domain.Dtos;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Chat.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<UserDto> GetOneByIdAsync(int userId, UserTypeEnum userType);
}
