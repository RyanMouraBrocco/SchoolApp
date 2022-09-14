using System;

namespace SchoolApp.Activity.Application.Domain.Entities.Answers;

public class ActivityAnswerVersion
{
    public string Id { get; set; }
    public string ActivityAnswerId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
}
