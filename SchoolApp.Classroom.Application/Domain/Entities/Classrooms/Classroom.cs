using SchoolApp.Classroom.Application.Domain.Entities.Subjects;

namespace SchoolApp.Classroom.Application.Domain.Entities.Classrooms;

public class Classroom
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string RoomNumber { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public IList<ClassroomStudent> Students { get; set; }
}
