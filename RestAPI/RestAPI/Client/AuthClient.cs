using Microsoft.Extensions.Options;
using RestAPI.Client.Models.RequestModels;
using RestAPI.Client.Models.ResponseModels;
using RestAPI.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RestAPI.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly ApiKeySettings _apiKeySettings;

        public AuthClient(HttpClient httpClient, IOptions<ApiKeySettings> apiKeySettings)
        {
            _httpClient = httpClient;
            _apiKeySettings = apiKeySettings.Value;
        }

        public async Task<CreateUserResponse> SignUpUserAsync(string email, string password)
        {
            var userCreds = new CreateUserRequest
            {
                Email = email,
                Password = password,
                ReturnSecureToken = true
            };
            var url = $"/v1/accounts:signUp?key={_apiKeySettings.WebApiKey}";
            var response = await _httpClient.PostAsJsonAsync(url, userCreds);
            return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
        }

        public async Task<SignInUserResponse> SignInUserAsync(string email, string password)
        {
            var userCreds = new SignInUserRequest
            {
                Email = email,
                Password = password,
                ReturnSecureToken = true
            };
            var url = $"/v1/accounts:signInWithPassword?key={_apiKeySettings.WebApiKey}";
            var response = await _httpClient.PostAsJsonAsync(url, userCreds);
            return await response.Content.ReadFromJsonAsync<SignInUserResponse>();
        }
    }
}
