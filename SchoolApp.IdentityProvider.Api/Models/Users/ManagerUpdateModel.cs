using System.ComponentModel.DataAnnotations;
using SchoolApp.IdentityProvider.Api.Models.Users.Base;

namespace SchoolApp.IdentityProvider.Api.Models.Users;

public class ManagerUpdateModel : UserUpdateModel
{
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
    [Required]
    public DateTime HiringDate { get; set; }
    [Required]
    public int FunctionId { get; set; }
}
