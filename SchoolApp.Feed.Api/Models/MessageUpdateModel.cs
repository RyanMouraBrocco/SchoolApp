using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Feed.Api.Models;

public class MessageUpdateModel
{
    [Required]
    public string Text { get; set; }
    [Required]
    public IList<MessageAllowedClassroomModel> AllowedClassrooms { get; set; }
    [Required]
    public IList<MessageAllowedStudentModel> AllowedStudents { get; set; }
}
