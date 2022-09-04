using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;
using SchoolApp.IdentityProvider.Sql.Dtos.Formation;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class TeacherFormationMapper
{
    public static TeacherFormation MapToDomain(TeacherFormationDto dto)
    {
        if (dto == null)
            return null;

        return new TeacherFormation()
        {
            Id = dto.Id,
            AcademicFormation = dto.AcademicFormation,
            TeacherId = dto.TeacherId,
            UniversityDegree = dto.UniversityDegree,
            UniversityDegreeDate = dto.UniversityDegreeDate
        };
    }

    public static TeacherFormationDto MapToDto(TeacherFormation domain)
    {
        if (domain == null)
            return null;

        return new TeacherFormationDto()
        {
            Id = domain.Id,
            AcademicFormation = domain.AcademicFormation,
            TeacherId = domain.TeacherId,
            UniversityDegree = domain.UniversityDegree,
            UniversityDegreeDate = domain.UniversityDegreeDate
        };
    }
}
