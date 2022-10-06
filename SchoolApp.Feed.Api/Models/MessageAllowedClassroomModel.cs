using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Feed.Api.Models;

public class MessageAllowedClassroomModel
{
    [Required]
    public int ClassroomId { get; set; }
}
