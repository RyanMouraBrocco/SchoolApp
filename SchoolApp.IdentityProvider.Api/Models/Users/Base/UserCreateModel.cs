using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models.Users.Base;

public class UserCreateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public string DocumentId { get; set; }
}
