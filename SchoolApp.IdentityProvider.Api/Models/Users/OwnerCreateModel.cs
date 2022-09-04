using System.ComponentModel.DataAnnotations;
using SchoolApp.IdentityProvider.Api.Models.Users.Base;

namespace SchoolApp.IdentityProvider.Api.Models.Users;

public class OwnerCreateModel : UserCreateModel
{
    [Required]
    public bool IsMainOwner { get; set; }
}
