using System.Collections.Generic;

namespace SchoolApp.Classroom.Api.Models.Classrooms;

public class ClassroomUpdateModel
{
    public string RoomNumber { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    public IList<ClassroomStudentModel> Students { get; set; }
}
