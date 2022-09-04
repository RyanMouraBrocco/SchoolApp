using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models.Users.Base;

public class UserUpdateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public string DocumentId { get; set; }
}
