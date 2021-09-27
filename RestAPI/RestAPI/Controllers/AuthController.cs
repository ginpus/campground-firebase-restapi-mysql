﻿using Domain.Models.RequestModels;
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
        [Authorize] // needed to force authorization
        public async Task<ActionResult<CreateUserResponse>> CreateUser(string email, string password)
        {
            //------------------------ START OF AUTHORIZATION DATA COLLECTION --------------------------------
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            Console.WriteLine($"LocalID: {userId.Value}");
            Console.WriteLine($"UserID: {user.UserId}");

            //------------------------ END OF AUTHORIZATION DATA COLLECTION --------------------------------

            var newUser = await _authClient.CreateUserAsync(email, password);
            await _userService.SaveUserAsync(new UserRequestModel
            {
                Email = newUser.Email,
                LocalId = newUser.LocalId
            });
            return newUser;
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SignInUserResponse>> SignIn(string email, string password)
        {
            return await _authClient.SignInUserAsync(email, password);
        }

        /*        [HttpGet]
                public async Task<ActionResult<IEnumerable<ReturnUserResponse>>> ShowAllUsers()
                {

                }*/
    }
}