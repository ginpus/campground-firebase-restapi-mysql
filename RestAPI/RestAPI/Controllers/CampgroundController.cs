using Contracts.RequestModels;
using Contracts.ResponseModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("campground")]
    public class CampgroundController : ControllerBase
    {
        private readonly ICampgroundsService _campgroundsService;
        private readonly IUserService _userService;

        public CampgroundController(ICampgroundsService campgroundsService, IUserService userService)
        {
            _campgroundsService = campgroundsService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CampgroundResponse>> CreateCampground(CampgroundCreateorUpdateRequest campground)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var newCampground = new CampgroundRequestModel
            {
                CampgroundId = Guid.NewGuid(),
                UserId = user.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = DateTime.Now
            };

            await _campgroundsService.CreateOrEditAsync(newCampground);

            return Ok(newCampground.AsDto());
        }

        [HttpGet]
        [Authorize]
        [Route("byUser")]
        public async Task<ActionResult<IEnumerable<CampgroundResponse>>> GetAllUserCampgrounds()
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var allItems = await _campgroundsService.GetAllUserItemsAsync(user.UserId);

            var allCampgrounds = new List<CampgroundResponse> { };

            foreach (var item in allItems)
            {
                var allImages = item.Images
                    .Select(image => image.AsDto());

                var campground = new CampgroundResponse
                {
                    CampgroundId = item.CampgroundId,
                    UserId = item.UserId,
                    Name = item.Name,
                    Price = item.Price,
                    Images = allImages,
                    Description = item.Description,
                    DateCreated = item.DateCreated
                };

                allCampgrounds.Add(campground);
            }

            return Ok(allCampgrounds);
        }

        [HttpGet]
        [Route("{campgroundId}")]
        public async Task<ActionResult<CampgroundResponse>> GetCampground(Guid campgroundId)
        {
            var item = await _campgroundsService.GetCampgroundAsync(campgroundId);

            var allImages = item.Images
                .Select(image => image.AsDto());

            var campground = new CampgroundResponse
            {
                CampgroundId = item.CampgroundId,
                UserId = item.UserId,
                Name = item.Name,
                Price = item.Price,
                Images = allImages,
                Description = item.Description,
                DateCreated = item.DateCreated
            };

            return Ok(campground);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShortCampgroundResponse>>> GetAllCampgrounds()
        {
            var allCampgrounds = (await _campgroundsService.GetAllItemsAsync())
                .Select(item => item.AsDto());

            return Ok(allCampgrounds);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<int>> DeleteAllCampgrounds()
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var entriesDeleted = await _campgroundsService.DeleteAllAsync(user.UserId);

            return entriesDeleted;
        }

        [HttpDelete]
        [Authorize]
        [Route("{campgroundId}")]
        public async Task<ActionResult<int>> DeleteCampground(Guid campgroundId)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var entryDeleted = await _campgroundsService.DeleteAsync(campgroundId, user.UserId);

            return entryDeleted;
        }

        [HttpPut]
        [Authorize]
        [Route("{campgroundId}")]
        public async Task<ActionResult<CampgroundResponse>> EditCampground(Guid campgroundId, CampgroundCreateorUpdateRequest campground)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var campgroundToUpdate = await _campgroundsService.GetCampgroundByIdAsync(campgroundId, user.UserId);

            if (campgroundToUpdate is null)
            {
                return NotFound($"Campground item with specified id : `{campgroundId}` does not exist");
            }

            campgroundToUpdate.Name = campground.Name;
            campgroundToUpdate.Price = campground.Price;
            campgroundToUpdate.Description = campground.Description;

            await _campgroundsService.CreateOrEditAsync(new CampgroundRequestModel
            {
                CampgroundId = campgroundToUpdate.CampgroundId,
                UserId = campgroundToUpdate.UserId,
                Name = campgroundToUpdate.Name,
                Price = campgroundToUpdate.Price,
                Description = campgroundToUpdate.Description,
                DateCreated = campgroundToUpdate.DateCreated
            });

            return campgroundToUpdate.AsDto();
        }
    }
}
