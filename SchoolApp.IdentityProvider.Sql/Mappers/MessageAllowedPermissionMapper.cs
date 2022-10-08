using SchoolApp.IdentityProvider.Sql.Dtos.MessageAllowedPermissions;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class MessageAllowedPermissionMapper
{
    public static Application.Domain.Dtos.MessageAllowedClassroomDto MapToDomain(MessageAllowedClassroomDto dto)
    {
        return new Application.Domain.Dtos.MessageAllowedClassroomDto()
        {
            MessageId = dto.MessageId,
            ClassroomId = dto.ClassroomId
        };
    }

    public static MessageAllowedClassroomDto MapToDto(Application.Domain.Dtos.MessageAllowedClassroomDto domain)
    {
        return new MessageAllowedClassroomDto()
        {
            MessageId = domain.MessageId,
            ClassroomId = domain.ClassroomId
        };
    }

    public static Application.Domain.Dtos.MessageAllowedStudentDto MapToDomain(MessageAllowedStudentDto dto)
    {
        return new Application.Domain.Dtos.MessageAllowedStudentDto()
        {
            MessageId = dto.MessageId,
            StudentId = dto.StudentId
        };
    }

    public static MessageAllowedStudentDto MapToDto(Application.Domain.Dtos.MessageAllowedStudentDto domain)
    {
        return new MessageAllowedStudentDto()
        {
            MessageId = domain.MessageId,
            StudentId = domain.StudentId
        };
    }
}
