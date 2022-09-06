namespace SchoolApp.Classroom.Application.Domain.Entities.OwnerTypes;

public class OwnerType
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
}
