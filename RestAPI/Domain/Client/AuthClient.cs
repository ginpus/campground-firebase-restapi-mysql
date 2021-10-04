using Microsoft.Extensions.Options;
using Domain.Client.Models.RequestModels;
using Domain.Client.Models.ResponseModels;
using Domain.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.Client.Models;
using Domain.Exceptions;

namespace Domain.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly FirebaseSettings _firebaseSettings;

        public AuthClient(HttpClient httpClient, IOptions<FirebaseSettings> apiKeySettings)
        {
            _httpClient = httpClient;
            _firebaseSettings = apiKeySettings.Value;
        }

        public async Task<CreateUserResponse> SignUpUserAsync(string email, string password)
        {
            var userCreds = new CreateUserRequest
            {
                Email = email,
                Password = password,
                ReturnSecureToken = true
            };
            var url = $"{_firebaseSettings.BaseAddress}/v1/accounts:signUp?key={_firebaseSettings.WebApiKey}";

            var response = await _httpClient.PostAsJsonAsync(url, userCreds);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            }

            var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
        }

        public async Task<SignInUserResponse> SignInUserAsync(string email, string password)
        {
            var userCreds = new SignInUserRequest
            {
                Email = email,
                Password = password,
                ReturnSecureToken = true
            };

            var url = $"{_firebaseSettings.BaseAddress}/v1/accounts:signInWithPassword?key={_firebaseSettings.WebApiKey}";

            var response = await _httpClient.PostAsJsonAsync(url, userCreds);

            if (response.IsSuccessStatusCode)
            {
                return await
                    response.Content.ReadFromJsonAsync<SignInUserResponse>();
            }

            var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
        }
    }
}
