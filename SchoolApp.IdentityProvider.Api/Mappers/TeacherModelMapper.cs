using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Api.Mappers;

public static class TeacherModelMapper
{
    public static Teacher MapToTeacher(this TeacherCreateModel model)
    {
        return new Teacher()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            AcademicFormation = model.AcademicFormation,
            HiringDate = model.HiringDate,
            Salary = (decimal)model.Salary,
            Password = model.Password,
            ImageUrl = model.ImageUrl,
            Formations = model.Formations?.Select(x => x.MapToTeacherFormation()).ToList() ?? new List<TeacherFormation>()
        };
    }

    public static Teacher MapToTeacher(this TeacherUpdateModel model)
    {
        return new Teacher()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            AcademicFormation = model.AcademicFormation,
            HiringDate = model.HiringDate,
            Salary = (decimal)model.Salary,
            ImageUrl = model.ImageUrl,
            Formations = model.Formations?.Select(x => x.MapToTeacherFormation()).ToList() ?? new List<TeacherFormation>()
        };
    }

    private static TeacherFormation MapToTeacherFormation(this TeacherFormationModel model)
    {
        return new TeacherFormation()
        {
            AcademicFormation = model.AcademicFormation,
            UniversityDegree = model.UniversityDegree,
            UniversityDegreeDate = model.UniversityDegreeDate
        };
    }
}
