using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;

namespace SchoolApp.Classroom.Application.Domain.Entities.Grades;

public class ClassroomStudentGrade : Grade
{
    public int ClassroomStudentId { get; set; }
    public ClassroomStudent ClassroomStudent { get; set; }
}
