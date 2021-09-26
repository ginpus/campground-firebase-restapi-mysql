using RestAPI.Client.Models.ResponseModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestAPI.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;

        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<CreateUserResponse> CreateUserAsync(string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<SignInUserResponse> SignInUserAsync(string email, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
