using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.Classroom.Application.Domain.Dtos;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Http.Settings;

namespace SchoolApp.Classroom.Http.Repositories;

public class AcitivityAnswerRepository : IActivityAnswerRepository
{
    private ActivityServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public AcitivityAnswerRepository(HttpClient httpClient, IOptions<ActivityServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<ActivityAnswerDto> GetOneByIdIncludingActivityAsync(string activityAnswerId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}ActivitiesAnswers/GetOneByIdIncludingActivity/{activityAnswerId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ActivityAnswerDto>(responseBody);
    }
}
