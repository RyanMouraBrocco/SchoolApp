using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Classroom.Sql.Dtos.Grades;

[Table("Classroom_Student_Grade")]
public class ClassroomStudentGradeDto : GradeDto
{
    public int ClassroomStudentId { get; set; }
}
