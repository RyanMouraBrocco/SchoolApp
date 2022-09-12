using System.Security.Principal;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Shared.Utils.Sql.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Classroom.Sql.Dtos.Classrooms;

[Table("Classroom")]
public class ClassroomDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string RoomNumber { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool Deleted { get; set; }
    public IList<ClassroomStudentDto> Students { get; set; }
}
