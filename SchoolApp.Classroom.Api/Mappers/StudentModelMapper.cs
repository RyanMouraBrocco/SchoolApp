using SchoolApp.Classroom.Api.Models.Students;
using SchoolApp.Classroom.Application.Domain.Entities.Students;

namespace SchoolApp.Classroom.Api.Mappers;

public static class StudentModelMapper
{
    public static Student MapToStudent(this StudentCreateModel model)
    {
        return new Student()
        {
            BirthDate = model.BirthDate,
            DocumentId = model.DocumentId,
            Name = model.Name,
            Sex = model.Sex
        };
    }

    public static Student MapToStudent(this StudentUpdateModel model)
    {
        return new Student()
        {
            BirthDate = model.BirthDate,
            DocumentId = model.DocumentId,
            Name = model.Name,
            Sex = model.Sex
        };
    }
}
