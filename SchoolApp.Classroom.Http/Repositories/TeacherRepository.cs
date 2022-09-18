using SchoolApp.Classroom.Application.Domain.Dtos;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Http.Settings;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace SchoolApp.Classroom.Http.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private IdentityProviderServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public TeacherRepository(HttpClient httpClient, IOptions<IdentityProviderServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<TeacherDto> GetOneByIdAsync(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Teachers/GetOneById/{id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TeacherDto>(responseBody);
    }
}
