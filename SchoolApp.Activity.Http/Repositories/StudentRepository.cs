using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.Activity.Application.Domain.Dtos;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Http.Settings;

namespace SchoolApp.Activity.Http.Repositories;

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
        return JsonSerializer.Deserialize<List<StudentDto>>(responseBody);
    }
}
