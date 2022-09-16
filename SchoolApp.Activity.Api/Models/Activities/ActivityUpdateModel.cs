using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Activity.Api.Models.Activities;

public class ActivityUpdateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int ClassroomId { get; set; }
    public DateTime? CloseDate { get; set; }
    [Required]
    public bool Closed { get; set; }
}
