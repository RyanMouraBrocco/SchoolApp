using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Classroom.Api.Models.Classrooms;

public class ClassroomStudentModel
{
    [Required]
    public int ClassroomId { get; set; }
}
