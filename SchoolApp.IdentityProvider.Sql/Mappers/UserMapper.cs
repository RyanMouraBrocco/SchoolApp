using SchoolApp.IdentityProvider.Application.Domain.Users;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class UserMapper
{
    public static User MapToDomain(UserDto dto)
    {
        return new User()
        {
            Id = dto.Id,
            Name = dto.Name,
            DocumentId = dto.DocumentId,
            CreatorId = dto.CreatorId,
            CreationDate = dto.CreationDate,
            Email = dto.Email,
            Password = dto.Password
        };
    }

    public static UserDto MapToDto(User domain)
    {
        return new UserDto()
        {
            Id = domain.Id,
            Name = domain.Name,
            DocumentId = domain.DocumentId,
            CreatorId = domain.CreatorId,
            CreationDate = domain.CreationDate,
            Email = domain.Email,
            Password = domain.Password
        };
    }
}
