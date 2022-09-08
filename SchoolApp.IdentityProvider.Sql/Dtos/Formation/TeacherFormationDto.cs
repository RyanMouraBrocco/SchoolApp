using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Formation;

[Table("Teacher_Formation")]
public class TeacherFormationDto : IIdentityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int TeacherId { get; set; }
    [ForeignKey("TeacherId")]
    public TeacherDto Teacher { get; set; }
    public string AcademicFormation { get; set; }
    public string UniversityDegree { get; set; }
    public DateTime? UniversityDegreeDate { get; set; }
}
