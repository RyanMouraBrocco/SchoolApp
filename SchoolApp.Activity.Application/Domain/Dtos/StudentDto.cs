using System.Text.Json.Serialization;

namespace SchoolApp.Activity.Application.Domain.Dtos;

public class StudentDto
{

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
