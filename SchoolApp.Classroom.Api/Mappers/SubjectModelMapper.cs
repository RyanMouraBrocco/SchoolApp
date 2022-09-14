using SchoolApp.Classroom.Api.Models.Subjects;
using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Sql.Dtos.Subjects;

namespace SchoolApp.Classroom.Api.Mappers;

public static class SubjectModelMapper
{
    public static Subject MapToSubject(this SubjectModel model)
    {
        return new Subject()
        {
            Name = model.Name
        };
    }
}
