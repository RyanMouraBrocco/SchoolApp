using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models.Users;

public class TeacherModel
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
    [Required]
    public string AcademicFormation { get; set; }
    [Required]
    public decimal Salary { get; set; }
    [Required]
    public DateTime HiringDate { get; set; }
}
