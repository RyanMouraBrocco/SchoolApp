using System.Runtime.InteropServices;
using SchoolApp.Activity.Api.Models.ActivitiesAnswers;
using SchoolApp.Activity.Application.Domain.Entities.Answers;

namespace SchoolApp.Activity.Api.Mappers;

public static class ActivityAnswerMapper
{
    public static ActivityAnswer MapToActivityAnswer(this ActivityAnswerCreateModel model)
    {
        return new ActivityAnswer()
        {
            ActivityId = model.ActivityId,
            LastReview = new ActivityAnswerVersion()
            {
                Text = model.Text
            }
        };
    }

    public static ActivityAnswerVersion MapToActivityAnswerVersion(this ActivityAnswerCreateReviewModel model)
    {
        return new ActivityAnswerVersion()
        {
            Text = model.Text
        };
    }
}
