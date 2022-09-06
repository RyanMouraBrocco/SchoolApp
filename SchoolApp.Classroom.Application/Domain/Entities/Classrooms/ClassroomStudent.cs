using SchoolApp.Classroom.Application.Domain.Entities.Students;

namespace SchoolApp.Classroom.Application.Domain.Entities.Classrooms;

public class ClassroomStudent
{
    public int Id { get; set; }
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
}
