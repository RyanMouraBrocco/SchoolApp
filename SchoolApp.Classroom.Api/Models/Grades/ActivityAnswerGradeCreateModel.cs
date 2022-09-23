namespace SchoolApp.Classroom.Api.Models.Grades;

public class ActivityAnswerGradeCreateModel
{
    public string ActivityAnswerId { get; set; }
    public int StudentId { get; set; }
    public decimal Value { get; set; }
}
