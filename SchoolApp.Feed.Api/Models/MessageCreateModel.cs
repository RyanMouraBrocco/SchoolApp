using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Feed.Api.Models;

public class MessageCreateModel
{
    public string MessageId { get; set; }
    [Required]
    public string Text { get; set; }
    [Required]
    public IList<MessageAllowedClassroomModel> AllowedClassrooms { get; set; }
    [Required]
    public IList<MessageAllowedStudentModel> AllowedStudents { get; set; }
}
