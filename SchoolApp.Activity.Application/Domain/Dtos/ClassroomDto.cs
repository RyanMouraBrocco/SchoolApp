using System.Text.Json.Serialization;

namespace SchoolApp.Activity.Application.Domain.Dtos;

public class ClassroomDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("teacherId")]
    public int TeacherId { get; set; }
    [JsonPropertyName("accountId")]
    public int AccountId { get; set; }
}
