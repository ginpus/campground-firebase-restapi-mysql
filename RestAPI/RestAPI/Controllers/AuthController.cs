using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestAPI.Client;
using RestAPI.Client.Models.ResponseModels;
using RestAPI.Options;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthClient _authClient;
        private readonly ApiKeySettings _apiKeySettings;

        public AuthController(IAuthClient authClient, IOptions<ApiKeySettings> apiKeySettings)
        {
            _authClient = authClient;
            _apiKeySettings = apiKeySettings.Value;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<ActionResult<CreateUserResponse>> CreateUser(string email, string password)
        {
            return await _authClient.CreateUserAsync(email, password);
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SignInUserResponse>> SignIn(string email, string password)
        {
            return await _authClient.SignInUserAsync(email, password);
        }
    }
}