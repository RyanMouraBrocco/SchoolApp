using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Domain.Entities;

namespace SchoolApp.File.Api.Mappers;

public static class ActivityAnswerVersionFileModelMapper
{
    public static ActivityAnswerVersionFile MapToActivityAnswerVersionFile(this ActivityAnswerVersionFileCreateModel model)
    {
        return new ActivityAnswerVersionFile()
        {
            ActivityAnswerVersionId = model.ActivityAnswerVersionId,
            Base64Value = model.Base64Value,
            Extension = model.Extension
        };
    }

    public static ActivityAnswerVersionFile MapToActivityAnswerVersionFile(this ActivityAnswerVersionFileRemoveModel model)
    {
        return new ActivityAnswerVersionFile()
        {
            ActivityAnswerVersionId = model.ActivityAnswerVersionId,
            FileName = model.FileName
        };
    }
}
