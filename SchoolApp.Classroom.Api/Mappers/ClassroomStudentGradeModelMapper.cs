using SchoolApp.Classroom.Api.Models.Grades;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;

namespace SchoolApp.Classroom.Api.Mappers;

public static class ClassroomStudentGradeModelMapper
{
    public static ClassroomStudentGrade MapToClassroomStudentGrade(this ClassroomStudentGradeCreateModel model)
    {
        return new ClassroomStudentGrade()
        {
            StudentId = model.StudentId,
            Value = model.Value,
            ClassroomStudentId = model.ClassroomStudentId
        };
    }

    public static ClassroomStudentGrade MapToClassroomStudentGrade(this ClassroomStudentGradeUpdateModel model)
    {
        return new ClassroomStudentGrade()
        {
            Value = model.Value
        };
    }
}
