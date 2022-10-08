using SchoolApp.Feed.Application.Domain.Entities;

namespace SchoolApp.Feed.Application.Domain.Dtos;

public class MessageAllowedClassroomDto
{
    public string MessageId { get; set; }
    public int ClassroomId { get; set; }
}
