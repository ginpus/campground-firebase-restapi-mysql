using Contracts.RequestModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Domain.Client.Models.ResponseModels;
using System.Threading.Tasks;
using Contracts.ResponseModels;

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
            var newUser = await _userService.SignUpAsync(new UserRequestModel
            {
                Email = request.Email,
                Password = request.Password
            });

            return Ok(newUser);
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request)
        {
            var returnedUser = await _userService.SignInUserAsync(request);

            return Ok(returnedUser);
        }
    }
}