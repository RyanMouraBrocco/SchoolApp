using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Classroom.Sql.Dtos.Students;

[Table("Owner_Student")]
public class OwnerStudentDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int StudentId { get; set; }
    public int OwnerTypeId { get; set; }
}
