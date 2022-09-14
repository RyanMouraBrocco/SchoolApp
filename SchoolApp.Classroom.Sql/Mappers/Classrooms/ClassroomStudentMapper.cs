using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;

namespace SchoolApp.Classroom.Sql.Mappers.Classrooms;

public static class ClassroomStudentMapper
{
    public static ClassroomStudent MapToDomain(ClassroomStudentDto dto)
    {
        if (dto == null)
            return null;

        return new ClassroomStudent()
        {
            Id = dto.Id,
            ClassroomId = dto.ClassroomId,
            StudentId = dto.StudentId
        };
    }

    public static ClassroomStudentDto MapToDto(ClassroomStudent domain)
    {
        if (domain == null)
            return null;

        return new ClassroomStudentDto()
        {
            Id = domain.Id,
            ClassroomId = domain.ClassroomId,
            StudentId = domain.StudentId
        };
    }
}
