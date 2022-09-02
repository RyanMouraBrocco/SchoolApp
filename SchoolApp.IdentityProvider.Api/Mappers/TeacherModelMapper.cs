using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Domain.Users;

namespace SchoolApp.IdentityProvider.Api.Mappers;

public static class TeacherModelMapper
{
    public static Teacher MapToTeacher(this TeacherModel model)
    {
        return new Teacher()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            AcademicFormation = model.AcademicFormation,
            HiringDate = model.HiringDate,
            Salary = model.Salary,
            Password = model.Password,
            ImageUrl = model.ImageUrl
        };
    }
}
