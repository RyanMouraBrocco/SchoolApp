using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Activity.Api.Models.ActivitiesAnswers;

public class ActivityAnswerCreateReviewModel
{
    [Required]
    public string Text { get; set; }
}
