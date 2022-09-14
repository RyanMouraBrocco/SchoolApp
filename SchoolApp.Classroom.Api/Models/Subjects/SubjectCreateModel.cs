using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Classroom.Api.Models.Subjects;

public class SubjectModel
{
    [Required]
    public string Name { get; set; }
}
