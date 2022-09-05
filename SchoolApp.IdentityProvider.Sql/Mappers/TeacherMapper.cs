using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class TeacherMapper
{
    public static Teacher MapToDomain(TeacherDto dto)
    {
        if (dto == null)
            return null;

        return new Teacher()
        {
            Id = dto.Id,
            Name = dto.Name,
            AccountId = dto.AccountId,
            DocumentId = dto.DocumentId,
            CreatorId = dto.CreatorId,
            CreationDate = dto.CreationDate,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate,
            Email = dto.Email,
            Password = dto.Password,
            AcademicFormation = dto.AcademicFormation,
            HiringDate = dto.HiringDate,
            Salary = dto.Salary,
            Formations = dto.Formations?.Select(x => TeacherFormationMapper.MapToDomain(x)).ToList() ?? new List<TeacherFormation>()
        };
    }

    public static TeacherDto MapToDto(Teacher domain)
    {
        if (domain == null)
            return null;

        return new TeacherDto()
        {
            Id = domain.Id,
            Name = domain.Name,
            AccountId = domain.AccountId,
            DocumentId = domain.DocumentId,
            CreatorId = domain.CreatorId,
            CreationDate = domain.CreationDate,
            Email = domain.Email,
            Password = domain.Password,
            AcademicFormation = domain.AcademicFormation,
            HiringDate = domain.HiringDate,
            Salary = domain.Salary,
            UpdateDate = domain.UpdateDate,
            UpdaterId = domain.UpdaterId
        };
    }
}
