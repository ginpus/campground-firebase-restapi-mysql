using Contracts.RequestModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestAPI.Client;
using RestAPI.Client.Models.ResponseModels;
using RestAPI.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthClient _authClient;
        private readonly ApiKeySettings _apiKeySettings;
        private readonly IUserService _userService;

        public AuthController(
            IAuthClient authClient,
            IOptions<ApiKeySettings> apiKeySettings,
            IUserService userService)
        {
            _authClient = authClient;
            _apiKeySettings = apiKeySettings.Value;
            _userService = userService;
        }

        [HttpPost]
        [Route("signUp")]

        public async Task<ActionResult<CreateUserResponse>> SignUp(SignUpRequest request)
        {
            var newUser = await _authClient.SignUpUserAsync(request.Email, request.Password);
            await _userService.SaveUserAsync(new UserRequestModel
            {
                Email = newUser.Email,
                LocalId = newUser.LocalId
            });
            return newUser;
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SignInUserResponse>> SignIn(SignInRequest request)
        {
            return await _authClient.SignInUserAsync(request.Email, request.Password);
        }
    }
}