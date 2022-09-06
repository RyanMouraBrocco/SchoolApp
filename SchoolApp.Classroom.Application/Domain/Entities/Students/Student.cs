using SchoolApp.Classroom.Application.Domain.Enums;

namespace SchoolApp.Classroom.Application.Domain.Entities.Students;

public class Student
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string DocumentId { get; set; }
    public SexType Sex { get; set; }
    public string ImageUrl { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
}
