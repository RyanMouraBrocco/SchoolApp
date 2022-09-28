namespace SchoolApp.File.Api.Models;

public class StudentFileCreateModel
{
    public int StudentId { get; set; }
    public string Base64Value { get; set; }
    public string Extension { get; set; }
}
