using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.Classroom.Sql.Dtos.Classrooms;

[Table("Classroom_Student")]
public class ClassroomStudentDto : IIdentityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ClassroomId { get; set; }
    [ForeignKey("ClassroomId")]
    public ClassroomDto Classroom { get; set; }
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public StudentDto Student { get; set; }
}
