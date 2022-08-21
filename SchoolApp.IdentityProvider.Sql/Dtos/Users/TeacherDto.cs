using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Users;

[Table("Teacher")]
public class TeacherDto : UserDto
{
    public string AcademicFormation { get; set; }
    public decimal Salary { get; set; }
    public DateTime HiringDate { get; set; }
}
