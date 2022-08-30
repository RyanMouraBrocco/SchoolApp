using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models;

public class AuthenticationLoginModel
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}
