using SchoolApp.Feed.Application.Domain.Entities;

namespace SchoolApp.Feed.Application.Domain.Dtos;

public class MessageAllowedStudentDto
{
    public string MessageId { get; set; }
    public int StudentId { get; set; }
}
