using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Activity.Api.Models.Activities;

public class ActivityCreateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int ClassroomId { get; set; }
    [Required]
    public bool Closed { get; set; }
}
