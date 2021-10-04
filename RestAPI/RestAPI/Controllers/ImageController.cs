using Contracts.ResponseModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("image")]
    public class ImageController : ControllerBase
    {
        private readonly ICampgroundsService _campgroundsService;
        private readonly IUserService _userService;

        public ImageController(ICampgroundsService campgroundsService, IUserService userService)
        {
            _campgroundsService = campgroundsService;
            _userService = userService;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ImageResponse>> InsertCampgroundImage(Guid campgroundId, string url)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var newImage = new ImageRequestModel
            {
                ImageId = Guid.NewGuid(),
                CampgroundId = campgroundId,
                Url = url
            };

            var insertedImage = await _campgroundsService.CreateOrEditImageAsync(newImage, user.UserId);

            return insertedImage.AsDto();
        }

        [HttpDelete]
        [Authorize]
        [Route("{imageId}")]
        public async Task<ActionResult<int>> DeleteCampgroundImage(Guid imageId)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var entryDeleted = await _campgroundsService.DeleteImageAsync(imageId, user.UserId);

            if (entryDeleted == 0)
            {
                return BadRequest("No such image found for your user");
            }

            return Ok(entryDeleted);
        }
    }
}
