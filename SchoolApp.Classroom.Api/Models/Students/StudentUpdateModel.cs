using System;
using SchoolApp.Classroom.Application.Domain.Enums;

namespace SchoolApp.Classroom.Api.Models.Students;

public class StudentUpdateModel
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string DocumentId { get; set; }
    public SexTypeEnum Sex { get; set; }
}
