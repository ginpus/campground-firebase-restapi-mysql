using Contracts.RequestModels;
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

            var response = await _userService.ChangePasswordAsync(new ChangePasswordRequestModel
            {
                IdToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjM1MDM0MmIwMjU1MDAyYWI3NWUwNTM0YzU4MmVjYzY2Y2YwZTE3ZDIiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vY2FtcGdyb3VuZGFwcC1naW5wdXMiLCJhdWQiOiJjYW1wZ3JvdW5kYXBwLWdpbnB1cyIsImF1dGhfdGltZSI6MTYzMzQyODUxOSwidXNlcl9pZCI6IkNqd3lQNlB5TjdQb1pkdXNSY3BLbHFBcU53eTIiLCJzdWIiOiJDand5UDZQeU43UG9aZHVzUmNwS2xxQXFOd3kyIiwiaWF0IjoxNjMzNDI4NTE5LCJleHAiOjE2MzM0MzIxMTksImVtYWlsIjoidGVzdEB0ZXN0LmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJ0ZXN0QHRlc3QuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.iAWyU5R33TbhBNsBQLhucPmbQsj2L_NRWy3MAmvN3KwJt_xFI9j1CBKCMErHGijyNWKsPJkuxski4m9lIpFbfM5528qM7fHfZP8g9HHq3sdaK1cbaSmhN4m61vYPygvs5QZXJSSNY-rVs6GzfsMS1lxmsoAbCEEoPqMvJYbiLazt5FWxdQaHAtnOY7TOXbv7zqyPx3Eo7mwhkFMvkZS7GfrDew7IaJaCbeo4bsCk5WAc6wnPWfmQ85t7n7iun21LPdArzQXDAbxhDt5_mlZw0N1Kfvfl3nY7LdJq8Xd3Qdng-wYHXP8p4iUClU23c-4N3D55RGPoq8s95vurtBRvBg",
                NewPassword = request.NewPassword,
                ReturnSecureToken = true
            });

            return Ok(response);
        }
    }
}