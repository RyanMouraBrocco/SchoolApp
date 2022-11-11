using System.Text.Json.Serialization;

namespace SchoolApp.Classroom.Application.Domain.Dtos;

public class TeacherDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("accountId")]
    public int AccountId { get; set; }
}
