using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;

namespace SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

public class Teacher : User
{
    public string AcademicFormation { get; set; }
    public decimal Salary { get; set; }
    public DateTime HiringDate { get; set; }
    public IList<TeacherFormation> Formations { get; set; }

    public Teacher()
    {
        Formations = new List<TeacherFormation>();
    }
}
