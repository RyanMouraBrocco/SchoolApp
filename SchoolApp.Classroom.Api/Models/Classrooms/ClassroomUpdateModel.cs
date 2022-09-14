using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Classroom.Api.Models.Classrooms;

public class ClassroomUpdateModel
{
    [Required]
    public string RoomNumber { get; set; }
    [Required]
    public int TeacherId { get; set; }
    [Required]
    public int SubjectId { get; set; }
    public IList<ClassroomStudentModel> Students { get; set; }
}
