using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.IdentityProvider.Sql.Dtos.MessageAllowedPermissions;

[Table("MessageAllowedStudent")]
public class MessageAllowedStudentDto : IIdentityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string MessageId { get; set; }
    public int StudentId { get; set; }
}
