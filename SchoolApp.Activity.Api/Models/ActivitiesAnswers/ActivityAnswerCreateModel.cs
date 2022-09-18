using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Activity.Api.Models.ActivitiesAnswers;

public class ActivityAnswerCreateModel
{
    [Required]
    public string ActivityId { get; set; }
    [Required]
    public string Text { get; set; }
}
