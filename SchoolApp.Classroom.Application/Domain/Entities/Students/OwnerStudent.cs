using SchoolApp.Classroom.Application.Domain.Entities.OwnerTypes;

namespace SchoolApp.Classroom.Application.Domain.Entities.Students;

public class OwnerStudent
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int OwnerTypeId { get; set; }
    public OwnerType OwnerType { get; set; }
}
