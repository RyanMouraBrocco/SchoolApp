namespace SchoolApp.Classroom.Api.Models.Grades;

public class ClassroomStudentGradeCreateModel
{
    public int ClassroomStudentId { get; set; }
    public int StudentId { get; set; }
    public decimal Value { get; set; }
}
