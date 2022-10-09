using System.Text.Json;
using Microsoft.Extensions.Options;
using SchoolApp.Chat.Application.Domain.Dtos;
using SchoolApp.Chat.Application.Interfaces.Repositories;
using SchoolApp.Chat.Http.Settings;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Chat.Http.Repositories;

public class UserRepository : IUserRepository
{
    private IdentityProviderServiceApiSettings Settigns { get; set; }
    private readonly HttpClient _httpClient;
    public UserRepository(HttpClient httpClient, IOptions<IdentityProviderServiceApiSettings> settings)
    {
        _httpClient = httpClient;
        Settigns = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Internal-Key", Settigns.Key);
    }

    public async Task<UserDto> GetOneByIdAsync(int id, UserTypeEnum type)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{Settigns.Url}/{GetEndpointByUserType(type)}/GetOneById/{id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserDto>(responseBody);
    }

    private string GetEndpointByUserType(UserTypeEnum type)
    {
        return type switch
        {
            UserTypeEnum.Manager => "Managers",
            UserTypeEnum.Owner => "Owners",
            UserTypeEnum.Teacher => "Teachers",
            _ => throw new NotImplementedException("User type not valid")
        };
    }
}
