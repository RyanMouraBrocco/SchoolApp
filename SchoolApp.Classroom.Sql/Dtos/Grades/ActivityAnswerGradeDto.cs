using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Classroom.Sql.Dtos.Grades;

[Table("Activity_Answer_Grade")]
public class ActivityAnswerGradeDto : GradeDto
{
    public string ActivityAnswerId { get; set; }
}
