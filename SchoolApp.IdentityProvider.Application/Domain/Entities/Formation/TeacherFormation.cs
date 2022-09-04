namespace SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;

public class TeacherFormation
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string AcademicFormation { get; set; }
    public string UniversityDegree { get; set; }
    public DateTime? UniversityDegreeDate { get; set; }
}
