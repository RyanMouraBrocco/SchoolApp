using System.ComponentModel.DataAnnotations;
using SchoolApp.IdentityProvider.Api.Models.Users.Base;

namespace SchoolApp.IdentityProvider.Api.Models.Users;

public class TeacherUpdateModel : UserUpdateModel
{
    [Required]
    public string AcademicFormation { get; set; }
    [Required]
    public decimal? Salary { get; set; }
    [Required]
    public DateTime HiringDate { get; set; }
    public IList<TeacherFormationModel> Formations { get; set; }
}
