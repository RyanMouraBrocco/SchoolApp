using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models.Users;

public class TeacherFormationModel
{
    [Required]
    public string AcademicFormation { get; set; }
    [Required]
    public string UniversityDegree { get; set; }
    public DateTime? UniversityDegreeDate { get; set; }
}
