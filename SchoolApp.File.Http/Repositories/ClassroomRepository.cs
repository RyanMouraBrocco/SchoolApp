using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Http.Settings;

namespace SchoolApp.File.Http.Repositories;

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

    public async Task<IList<ClassroomDto>> GetAllByOwnerIdAsync(int ownerId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Classrooms/GetAllByOwnerId/{ownerId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<ClassroomDto>>(responseBody);
    }
}
