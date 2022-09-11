using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Sql.Dtos.Grades;

namespace SchoolApp.Classroom.Sql.Mappers.Grades;

public class ClassroomStudentGradeMapper
{
    public static ClassroomStudentGrade MapToDomain(ClassroomStudentGradeDto dto)
    {
        return new ClassroomStudentGrade()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            ClassroomStudentId = dto.ClassroomStudentId,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            StudentId = dto.StudentId,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate
        };
    }

    public static ClassroomStudentGradeDto MapToDto(ClassroomStudentGrade domain)
    {
        return new ClassroomStudentGradeDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            ClassroomStudentId = domain.ClassroomStudentId,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            StudentId = domain.StudentId,
            UpdaterId = domain.UpdaterId,
            UpdateDate = domain.UpdateDate
        };
    }
}
