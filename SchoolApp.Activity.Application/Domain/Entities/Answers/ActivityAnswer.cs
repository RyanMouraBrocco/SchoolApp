using System;

namespace SchoolApp.Activity.Application.Domain.Entities.Answers;

public class ActivityAnswer
{
    public string Id { get; set; }
    public string ActivityId { get; set; }
    public int StudentId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
}
