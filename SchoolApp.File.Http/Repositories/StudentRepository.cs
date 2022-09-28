using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Http.Settings;

namespace SchoolApp.File.Http.Repositories;

public class StudentRepository : IStudentRepository
{
    private ClassroomServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public StudentRepository(HttpClient httpClient, IOptions<ClassroomServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<IList<StudentDto>> GetAllByOwnerIdAsync(int ownerId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Students/GetAllByOwnerId/{ownerId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<StudentDto>>(responseBody);
    }
}
