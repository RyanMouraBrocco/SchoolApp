namespace SchoolApp.IdentityProvider.Application.Domain.Users;

public class Teacher : User
{
    public string AcademicFormation { get; set; }
    public decimal Salary { get; set; }
    public DateTime HiringDate { get; set; }
}
