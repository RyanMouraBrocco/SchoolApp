using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Domain.Entities;

namespace SchoolApp.File.Api.Mappers;

public static class StudentFileModelMapper
{
    public static StudentFile MapToStudentFile(this StudentFileCreateModel model)
    {
        return new StudentFile()
        {
            StudentId = model.StudentId,
            Base64Value = model.Base64Value,
            Extension = model.Extension
        };
    }

    public static StudentFile MapToStudentFile(this StudentFileRemoveModel model)
    {
        return new StudentFile()
        {
            StudentId = model.StudentId,
            FileName = model.FileName
        };
    }
}
