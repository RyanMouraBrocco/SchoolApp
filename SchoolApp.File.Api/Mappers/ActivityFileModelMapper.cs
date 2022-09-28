using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Domain.Entities;

namespace SchoolApp.File.Api.Mappers;

public static class ActivityFileModelMapper
{
    public static ActivityFile MapToActivityFile(this ActivityFileCreateModel model)
    {
        return new ActivityFile()
        {
            ActivityId = model.ActivityId,
            Base64Value = model.Base64Value,
            Extension = model.Extension
        };
    }

    public static ActivityFile MapToActivityFile(this ActivityFileRemoveModel model)
    {
        return new ActivityFile()
        {
            ActivityId = model.ActivityId,
            FileName = model.FileName
        };
    }
}
