using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Users;

[Table("Owner")]
public class OwnerDto : UserDto
{
    public bool IsMainOwner { get; set; }
}
