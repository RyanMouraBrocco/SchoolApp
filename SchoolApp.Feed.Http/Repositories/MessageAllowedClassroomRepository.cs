using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.Http.Settings;

namespace SchoolApp.Feed.Http.Repositories;

public class MessageAllowedClassroomRepository : IMessageAllowedClassroomRepository
{
    private IdentityProviderServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public MessageAllowedClassroomRepository(HttpClient httpClient, IOptions<IdentityProviderServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<IList<MessageAllowedClassroomDto>> GetAllByMessageIdAsync(string messageId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/MessageAllowedClassrooms/GetAllByMessageId/{messageId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<MessageAllowedClassroomDto>>(responseBody);
    }
}
