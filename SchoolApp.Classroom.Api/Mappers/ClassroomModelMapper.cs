using System.Linq;
using SchoolApp.Classroom.Api.Models.Classrooms;
using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;

namespace SchoolApp.Classroom.Api.Mappers;

public static class ClassroomModelMapper
{
    public static Application.Domain.Entities.Classrooms.Classroom MapToClassroom(this ClassroomCreateModel model)
    {
        return new Application.Domain.Entities.Classrooms.Classroom()
        {
            RoomNumber = model.RoomNumber,
            TeacherId = model.TeacherId,
            SubjectId = model.SubjectId,
            Students = model.Students.Select(x => new ClassroomStudent() { ClassroomId = x.ClassroomId }).ToList()
        };
    }

    public static Application.Domain.Entities.Classrooms.Classroom MapToClassroom(this ClassroomUpdateModel model)
    {
        return new Application.Domain.Entities.Classrooms.Classroom()
        {
            RoomNumber = model.RoomNumber,
            TeacherId = model.TeacherId,
            SubjectId = model.SubjectId,
            Students = model.Students.Select(x => new ClassroomStudent() { ClassroomId = x.ClassroomId }).ToList()
        };
    }

}
