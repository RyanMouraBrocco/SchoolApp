using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Users;

[Table("Owner")]
public class OwnerDto
{
    public bool IsMainOwner { get; set; }
}
