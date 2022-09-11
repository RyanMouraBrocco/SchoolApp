using System.Security.Principal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.Classroom.Sql.Dtos.Grades;

public class GradeDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int StudentId { get; set; }
    public decimal Value { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool Deleted { get; set; }
}
