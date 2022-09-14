using System.Xml;
using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Domain.Enums;
using SchoolApp.Classroom.Sql.Dtos.Students;

namespace SchoolApp.Classroom.Sql.Mappers.Students;

public static class StudentMapper
{
    public static Student MapToDomain(StudentDto dto)
    {
        if (dto == null)
            return null;

        return new Student()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            BirthDate = dto.BirthDate,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            DocumentId = dto.DocumentId,
            Name = dto.Name,
            Sex = (SexTypeEnum)dto.Sex,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate
        };
    }

    public static StudentDto MapToDto(Student domain)
    {
        if (domain == null)
            return null;

        return new StudentDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            BirthDate = domain.BirthDate,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            DocumentId = domain.DocumentId,
            Name = domain.Name,
            Sex = (int)domain.Sex,
            UpdaterId = domain.UpdaterId,
            UpdateDate = domain.UpdateDate
        };
    }
}
