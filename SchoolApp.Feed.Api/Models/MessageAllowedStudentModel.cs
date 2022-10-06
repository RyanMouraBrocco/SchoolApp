using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Feed.Api.Models;

public class MessageAllowedStudentModel
{
    [Required]
    public int StudentId { get; set; }
}
