using System;

namespace SchoolApp.Activity.Application.Domain.Entities.Activities;

public class Activity
{
    public string Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClassroomId { get; set; }
    public DateTime? CloseDate { get; set; }
    public bool Closed { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
}
