using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Http.Settings;

namespace SchoolApp.File.Http.Repositories;

public class ActivityRepository : IActivityRepository
{
    private ActivityServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public ActivityRepository(HttpClient httpClient, IOptions<ActivityServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<ActivityDto> GetOneByIdAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/Activities/GetOneById/{id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ActivityDto>(responseBody);
    }
}
