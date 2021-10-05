﻿using Contracts.RequestModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Domain.Client.Models.ResponseModels;
using System.Threading.Tasks;
using Contracts.ResponseModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Contracts.GeneralModels;
using System.Text.Json;
using Domain.Models.ResponseModels;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("signUp")]

        public async Task<ActionResult<CreateUserResponse>> SignUp(SignUpRequest request)
        {
            var newUser = await _userService.SignUpAsync(request);

            return Ok(newUser);
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request)
        {
            var returnedUser = await _userService.SignInUserAsync(request);

            return Ok(returnedUser);
        }

        [HttpPost]
        [Route("changePassword")]
        [Authorize]

        public async Task<ActionResult<EditUserResponse>> ChangePassword(ChangePasswordRequest request)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var firebaseIdentity = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "firebase").Value;

            FirebaseIdentity firebaseIdentityJson = (FirebaseIdentity)JsonSerializer.Deserialize(firebaseIdentity, typeof(FirebaseIdentity));

            var userEmail = firebaseIdentityJson.Identities.Email.First();

            Request.Headers.TryGetValue("Authorization", out var idToken);

            //var idToken = this.GetHeaderData("Authorization");

            var idTokenValue = idToken.ToString().Remove(0, 7);

            System.Console.WriteLine($"Token from Request.Headers: {idTokenValue}");

            var response = await _userService.ChangePasswordAsync(new ChangePasswordRequestModel
            {
                IdToken = idTokenValue,
                NewPassword = request.NewPassword,
                ReturnSecureToken = true
            });

            return Ok(response);
        }

        [HttpGet("GetHeaderData")]
        public ActionResult<string> GetHeaderData(string headerKey)
        {
            Request.Headers.TryGetValue(headerKey, out var headerValue);
            return Ok(headerValue);
        }
    }
}