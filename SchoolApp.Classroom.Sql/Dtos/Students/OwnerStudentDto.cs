using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.Classroom.Sql.Dtos.Students;

[Table("Owner_Student")]
public class OwnerStudentDto : IIdentityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public StudentDto Student { get; set; }
    public int OwnerTypeId { get; set; }
}
