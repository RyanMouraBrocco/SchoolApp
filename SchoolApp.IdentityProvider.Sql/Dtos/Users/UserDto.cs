using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Users;

[Table("User")]
public class UserDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    public string DocumentId { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreateDate { get; set; }
}
