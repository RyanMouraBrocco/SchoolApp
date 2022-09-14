using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Sql.Dtos.Students;

namespace SchoolApp.Classroom.Sql.Mappers.Students;

public static class OwnerStudentMapper
{
    public static OwnerStudent MapToDomain(OwnerStudentDto dto)
    {
        if (dto == null)
            return null;

        return new OwnerStudent()
        {
            Id = dto.Id,
            OwnerId = dto.OwnerId,
            OwnerTypeId = dto.OwnerTypeId,
            StudentId = dto.StudentId
        };
    }

    public static OwnerStudentDto MapToDto(OwnerStudent domain)
    {
        if (domain == null)
            return null;

        return new OwnerStudentDto()
        {
            Id = domain.Id,
            OwnerId = domain.OwnerId,
            OwnerTypeId = domain.OwnerTypeId,
            StudentId = domain.StudentId
        };
    }
}
