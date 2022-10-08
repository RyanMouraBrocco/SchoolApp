using System.Security.Principal;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Shared.Utils.Sql.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Sql.Dtos.MessageAllowedPermissions;

[Table("MessageAllowedClassroom")]
public class MessageAllowedClassroomDto : IIdentityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string MessageId { get; set; }
    public int ClassroomId { get; set; }
}
