using SchoolApp.Classroom.Application.Domain.Entities.Students;

namespace SchoolApp.Classroom.Application.Domain.Entities.Grades;

public class Grade
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public decimal Value { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
}
