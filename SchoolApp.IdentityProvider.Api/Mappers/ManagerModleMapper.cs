using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Api.Mappers;

public static class ManagerModleMapper
{
    public static Manager MapToTeacher(this ManagerCreateModel model)
    {
        return new Manager()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            HiringDate = model.HiringDate,
            Salary = (decimal)model.Salary,
            Password = model.Password,
            ImageUrl = model.ImageUrl,
            FunctionId = (int)model.FunctionId
        };
    }

    public static Manager MapToTeacher(this ManagerUpdateModel model)
    {
        return new Manager()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            HiringDate = model.HiringDate,
            Salary = (decimal)model.Salary,
            ImageUrl = model.ImageUrl,
            FunctionId = (int)model.FunctionId
        };
    }
}
