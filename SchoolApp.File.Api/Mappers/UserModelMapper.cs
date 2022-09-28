using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Domain.Entities;

namespace SchoolApp.File.Api.Mappers;

public static class UserModelMapper
{
    public static UserFile MapToUserFile(this UserFileCreateModel model)
    {
        return new UserFile()
        {
            Base64Value = model.Base64Value,
            Extension = model.Extension
        };
    }

    public static UserFile MapToUserFile(this UserFileRemoveModel model)
    {
        return new UserFile()
        {
            FileName = model.FileName
        };
    }
}
