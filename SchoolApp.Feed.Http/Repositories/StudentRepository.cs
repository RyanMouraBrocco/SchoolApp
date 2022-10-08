using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.Http.Settings;

namespace SchoolApp.Feed.Http.Repositories;

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

    public async Task<StudentDto> GetOneByIdAsync(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Students/GetOneById/{id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StudentDto>(responseBody);
    }

    public async Task<IList<StudentDto>> GetAllByTeacherIdAsync(int teacherId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Students/GetAllByTeacherId/{teacherId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<StudentDto>>(responseBody);
    }
}
