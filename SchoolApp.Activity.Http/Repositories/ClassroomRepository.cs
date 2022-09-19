using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.Activity.Application.Domain.Dtos;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Http.Settings;

namespace SchoolApp.Activity.Http.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private ClassroomServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public ClassroomRepository(HttpClient httpClient, IOptions<ClassroomServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<ClassroomDto> GetOneByIdAsync(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Classrooms/GetOneById/{id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClassroomDto>(responseBody);
    }
}
