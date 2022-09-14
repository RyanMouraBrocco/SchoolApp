using System;
using System.ComponentModel.DataAnnotations;
using SchoolApp.Classroom.Application.Domain.Enums;

namespace SchoolApp.Classroom.Api.Models.Students;

public class StudentCreateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public string DocumentId { get; set; }
    [Required]
    [Range(1, 3)]
    public SexTypeEnum Sex { get; set; }
}
