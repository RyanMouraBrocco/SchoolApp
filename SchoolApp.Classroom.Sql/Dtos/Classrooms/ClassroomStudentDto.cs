using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Classroom.Sql.Dtos.Classrooms;

[Table("Classroom_Student")]
public class ClassroomStudentDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ClassroomId { get; set; }
    public int StudentId { get; set; }
}
